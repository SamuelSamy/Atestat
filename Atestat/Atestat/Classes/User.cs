using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Atestat.Classes
{
    public class User
    {
        public int id { get; }
        public string name { get; }
        public string mail { get; }
        public string phone { get; }
        public DateTime registerDate { get; }
        public int type { get; }


        public User(int id)
        {
            this.id = id;

            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select * from utilizatori where id = @id";
                cmd.Connection = Variables.conn;

                cmd.Parameters.AddWithValue("id", id);

                MySqlDataReader r = cmd.ExecuteReader();

                if (r.Read())
                {
                    mail = r["mail"].ToString();
                    name = r["nume"].ToString();
                    registerDate = DateTime.Parse(r["dataI"].ToString());
                    phone = r["nrTelefon"].ToString();
                    type = int.Parse(r["tipCont"].ToString());
                }

                r.Close();
                Variables.conn.Close();
            }
            catch (Exception e)
            {
                if (Variables.conn.State == System.Data.ConnectionState.Open)
                {
                    Variables.conn.Close();
                }

            }
        }
    }
}
