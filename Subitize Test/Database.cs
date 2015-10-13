using D = Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace Subitize_Test
{
    public class Database
    {
        public static User GetUserByAuthCode(string authcode)
        {
            User user = new User() { AuthCode = authcode };
            if (SelectUser(ref user))
                return user;
            else
                return null;
        }
        public static bool ValidateAuthCode(string authcode)
        {
            bool res = false;
            if (IsUser(authcode, ref res))
                return res;
            else
                return false;
        }
        public static Settings GetSettings()
        {
            Settings settings = new Settings();
            if (SelectSettings(ref settings))
                return settings;
            else
                return null;
        }
        public static Test GetTestByID(int id)
        {
            Test test = new Test() { ID = id };
            if (SelectTest(ref test))
                return test;
            else
                return null;
        }
        public static bool SaveUser(User user) => UpdateUser(ref user);

        private static bool IsUser(string authcode, ref bool result)
        {
            try
            {
                D.Database db = D.DatabaseFactory.CreateDatabase();
                using (DbCommand cmd = db.GetStoredProcCommand("IsUser"))
                {
                    db.AddInParameter(cmd, "authcode", DbType.String, authcode);
                    object o = db.ExecuteScalar(cmd);
                    if (o == null)
                        result = false;
                    else
                        result = (bool)o;
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        private static bool SelectUser(ref User user)
        {
            try
            {
                D.Database db = D.DatabaseFactory.CreateDatabase();
                using (DbCommand cmd = db.GetStoredProcCommand("SelectUser"))
                {
                    db.AddInParameter(cmd, "authcode", DbType.String, user.AuthCode);
                    using (IDataReader dr = db.ExecuteReader(cmd))
                    {
                        bool first = true;
                        while (dr.Read())
                        {
                            if (first)
                            {
                                user.Age = (string)dr["age"];
                                user.AuthCode = (string)dr["authcode"];
                                user.Gender = (string)dr["gender"];
                                first = false;
                            }
                            int tid = (int)dr["testid"];
                            Test t = null;
                            if (user.Tests.ContainsKey(tid))
                                t = user.Tests[tid];
                            else
                            {
                                t = new Test()
                                {
                                    ID = tid,
                                    MaxArraySize = (int)dr["maxarraysize"],
                                    ArraysPerSize = (int)dr["arrayspersize"],
                                    DelayPeriod = (int)dr["delayperiod"],
                                    TimeEst = (int)dr["timeest"]
                            };
                                user.Tests.Add(tid, t);
                            }
                            t.ImageArrays.Add(new ImageArray()
                            {
                                ImagesDisplayed = (int)dr["imagesdisplayed"],
                                UserInput = (int)dr["userinput"]
                            });
                        }
                    }
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        private static bool UpdateUser(ref User user)
        {
            try
            {
                D.Database db = D.DatabaseFactory.CreateDatabase();
                using (DbCommand cmd = db.GetStoredProcCommand("UpdateUser"))
                {
                    db.AddInParameter(cmd, "authcode", DbType.String, user.AuthCode);
                    db.AddInParameter(cmd, "gender", DbType.String, user.Gender);
                    db.AddInParameter(cmd, "age", DbType.String, user.Age);
                    db.ExecuteNonQuery(cmd);
                    bool res = true;
                    foreach (Test test in user.TestResults)
                    {
                        foreach (ImageArray result in test.Arrays)
                        {
                            res = UpdateUserResult(user.AuthCode, test.ID, result, ref db) && res;
                        }
                    }
                    return res;
                }
            }
            catch
            {
                return false;
            }
        }
        private static bool UpdateUserResult(string authcode, int testid, ImageArray result)
        {
            try
            {
                D.Database db = D.DatabaseFactory.CreateDatabase();
                return UpdateUserResult(authcode, testid, result, ref db);
            }
            catch
            {
                return false;
            }
        }
        private static bool UpdateUserResult(string authcode, int testid, ImageArray result, ref D.Database db)
        {
            try
            {
                using (DbCommand cmd = db.GetStoredProcCommand("UpdateUserResult"))
                {
                    db.AddInParameter(cmd, "authcode", DbType.String, authcode);
                    db.AddInParameter(cmd, "testid", DbType.Int32, testid);
                    db.AddInParameter(cmd, "imagesdisplayed", DbType.Int32, result.ImagesDisplayed);
                    db.AddInParameter(cmd, "userinput", DbType.Int32, result.UserInput);
                    db.ExecuteNonQuery(cmd);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        private static bool SelectTest(ref Test test)
        {
            try
            {
                D.Database db = D.DatabaseFactory.CreateDatabase();
                using (DbCommand cmd = db.GetStoredProcCommand("SelectTest"))
                {
                    db.AddInParameter(cmd, "id", DbType.String, test.ID);
                    using (IDataReader dr = db.ExecuteReader(cmd))
                    {
                        if (dr.Read())
                        {
                            test.MaxArraySize = (int)dr["maxarraysize"];
                            test.ArraysPerSize = (int)dr["arrayspersize"];
                            test.DelayPeriod = (int)dr["delayperiod"];
                            test.TimeEst = (int)dr["timeest"];
                        }
                    }
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        private static bool SelectSettings(ref Settings settings)
        {
            try
            {
                D.Database db = D.DatabaseFactory.CreateDatabase();
                using (DbCommand cmd = db.GetStoredProcCommand("SelectSettings"))
                {
                    using (IDataReader dr = db.ExecuteReader(cmd))
                    {
                        if (dr.Read())
                        {
                            settings.MaxTests = (int)dr["maxtests"];
                        }
                    }
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}
