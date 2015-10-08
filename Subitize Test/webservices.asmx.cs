using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Subitize_Test
{
    /// <summary>
    /// Summary description for webservices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
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
        public string GetSettings()
        {
            Settings set = Database.GetSettings();
            return JSON<Settings>.Parse(set);
        }
        [WebMethod]
        public string GetTest(int id)
        {
            Test t = Database.GetTestByID(id);
            return JSON<Test>.Parse(t);
        }
    }
}
