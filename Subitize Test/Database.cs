using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private static bool SelectUser(ref User user) { }
        private static bool UpdateUser(ref User user) { }
        private static bool UpdateUserResult(string authcode, int testid, ref ImageArray result)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            D.Database db = D.DatabaseFactory.CreateDatabase();
            return UpdateUserResult(authcode, testid, ref result, ref db);
        }
        private static bool UpdateUserResult(string authcode, int testid, ref ImageArray result, ref D.Database db) { }
        private static bool SelectTest(ref Test test) { }
        private static bool SelectSettings(ref Settings settings) { }

    }
}
