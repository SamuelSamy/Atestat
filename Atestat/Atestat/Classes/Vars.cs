using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace Atestat
{
    class Vars
    {
        public static string debugPath = AppDomain.CurrentDomain.BaseDirectory;

        public static MySqlConnection conn = new MySqlConnection(@"SERVER=localhost; DATABASE=atestat; UID=root; PASSWORD=; Allow User Variables=True");
    }
}
