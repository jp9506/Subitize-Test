using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
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
        public static Test GetTestByID(int id)
        {
            Test test = new Test() { ID = id };
            if (SelectTest(ref test))
                return test;
            else
                return null;
        }
        public static bool SaveUser(User user) => UpdateUser(ref user);
        public static int GetMaxTestID()
        {
            try
            {
                int result = 0;
                SqlDatabase db = new SqlDatabase(Properties.Settings.Default.connString);
                using (DbCommand cmd = db.GetStoredProcCommand("SelectMaxTest"))
                {
                    int? o = db.ExecuteScalar(cmd) as int?;
                    if (o == null)
                        result = 0;
                    else
                        result = o.Value;
                    return result;
                }
            }
            catch
            {
                return 0;
            }
        }

        private static bool IsUser(string authcode, ref bool result)
        {
            try
            {
                SqlDatabase db = new SqlDatabase(Properties.Settings.Default.connString);
                using (DbCommand cmd = db.GetStoredProcCommand("IsUser"))
                {
                    db.AddInParameter(cmd, "authcode", DbType.String, authcode);
                    int? o = db.ExecuteScalar(cmd) as int?;
                    if (o == null)
                        result = false;
                    else
                        result = (o.Value == 1);
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
                SqlDatabase db = new SqlDatabase(Properties.Settings.Default.connString);
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
                                user.Age = dr["age"] as string;
                                user.AuthCode = dr["authcode"] as string;
                                user.Gender = dr["gender"] as string;
                                first = false;
                            }
                            int? tid = dr["testid"] as int?;
                            if (tid != null)
                            {
                                Test t = null;
                                if (user.Tests.ContainsKey(tid.Value))
                                    t = user.Tests[tid.Value];
                                else
                                {
                                    t = new Test()
                                    {
                                        ID = tid.Value,
                                        TimeEst = (int)dr["timeest"],
                                        MaxArraySize = (int)dr["maxarraysize"],
                                        DelayPeriod = (int)dr["delayperiod"]
                                    };
                                    user.Tests.Add(tid.Value, t);
                                }
                                t.ImageArrays.Add(new ImageArray()
                                {
                                    Index = (int)dr["index"],
                                    ImagesDisplayed = (int)dr["imagesdisplayed"],
                                    UserInput = (int)dr["userinput"],
                                    ImageFile = (string)dr["imagefile"]
                                });
                            }
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
                SqlDatabase db = new SqlDatabase(Properties.Settings.Default.connString);
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
                SqlDatabase db = new SqlDatabase(Properties.Settings.Default.connString);
                return UpdateUserResult(authcode, testid, result, ref db);
            }
            catch
            {
                return false;
            }
        }
        private static bool UpdateUserResult(string authcode, int testid, ImageArray result, ref SqlDatabase db)
        {
            try
            {
                using (DbCommand cmd = db.GetStoredProcCommand("UpdateUserResult"))
                {
                    db.AddInParameter(cmd, "authcode", DbType.String, authcode);
                    db.AddInParameter(cmd, "testid", DbType.Int32, testid);
                    db.AddInParameter(cmd, "imagesdisplayed", DbType.Int32, result.ImagesDisplayed);
                    db.AddInParameter(cmd, "userinput", DbType.Int32, result.UserInput);
                    db.AddInParameter(cmd, "imagefile", DbType.String, result.ImageFile);
                    db.AddInParameter(cmd, "index", DbType.Int32, result.Index);
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
                SqlDatabase db = new SqlDatabase(Properties.Settings.Default.connString);
                using (DbCommand cmd = db.GetStoredProcCommand("SelectTest"))
                {
                    db.AddInParameter(cmd, "id", DbType.String, test.ID);
                    using (IDataReader dr = db.ExecuteReader(cmd))
                    {
                        bool first = true;
                        while (dr.Read())
                        {
                            if (first)
                            {
                                test.TimeEst = (int)dr["timeest"];
                                test.MaxArraySize = (int)dr["maxarraysize"];
                                test.DelayPeriod = (int)dr["delayperiod"];
                                first = false;
                            }
                            test.SubTests.Add(new SubTest()
                            {
                                ImageFile = dr["imagefile"] as string,
                                TestID = test.ID
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
    }
}
