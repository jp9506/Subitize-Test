using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subitize_Test
{
    public class Database
    {
        public static User GetUserByAuthCode(string authcode) { }
        public static bool ValidateAuthCode(string authcode) { }
        public static Settings GetSettings() { }
        public static Test GetTestByID(int id) { }
        public static bool SaveUser(User user) { }
    }
}
