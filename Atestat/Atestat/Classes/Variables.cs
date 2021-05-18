using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace Atestat.Classes
{
    class Variables
    {
        public static string debugPath = AppDomain.CurrentDomain.BaseDirectory;

        public static string MailAdress = "emarketatestat@gmail.com";
        public static string MailPass   = "fsgpv2407";

        public static MySqlConnection conn = new MySqlConnection(@"SERVER=localhost; DATABASE=atestat; UID=root; PASSWORD=; Allow User Variables=True");

    }
}
