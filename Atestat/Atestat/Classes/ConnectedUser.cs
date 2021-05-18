using System;
using System.Collections.Generic;
using System.Text;

namespace Atestat.Classes
{
    class ConnectedUser
    {
        public static int id = -1;
        public static string name, mail, phone, password;
        public static DateTime registerDate;
        public static bool loggedIn = false;
        public static int type = (int)UserTypes.Normal;

    }
}