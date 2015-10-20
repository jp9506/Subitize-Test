using System.Web.Services;
using System.Collections.Generic;
using System.Linq;

namespace Subitize_Test
{
    /// <summary>
    /// Summary description for webservices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class webservices : System.Web.Services.WebService
    {
        [WebMethod]
        public bool ValidateAuthCode(string authcode)
        {
            return Database.ValidateAuthCode(authcode);
        }
        [WebMethod]
        public string GetUser(string authcode)
        {
            User u = Database.GetUserByAuthCode(authcode);
            return JSON<User>.Parse(u);
        }
        [WebMethod]
        public string SetUser(string authcode, string user)
        {
            User u = JSON<User>.Parse(user);
            u.AuthCode = authcode;
            if (Database.SaveUser(u))
            {
                u = Database.GetUserByAuthCode(u.AuthCode);
                return JSON<User>.Parse(u);
            } else
            {
                return JSON<User>.Parse((User)null);
            }
        }
        [WebMethod]
        public string GetTest(int id)
        {
            Test t = Database.GetTestByID(id);
            return JSON<Test>.Parse(t);
        }
        [WebMethod]
        public string TakeTest(string authcode)
        {
            User u = Database.GetUserByAuthCode(authcode);
            User testuser = new User()
            {
                AuthCode = authcode,
                Gender = u.Gender,
                Age = u.Age
            };
            int mid = Database.GetMaxTestID();
            List<int> lid = new List<int>();
            for (int i = 1; i <= mid; i++)
            {
                lid.Add(i);
            }
            int tid = lid.Where(x => !u.Tests.ContainsKey(x)).Min();
            Test t = Database.GetTestByID(tid);
            t.GenerateArrays();
            testuser.Tests.Add(t.ID, t);
            return JSON<User>.Parse(testuser);
        }
    }
}
