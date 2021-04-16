using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using Atestat.Classes;

namespace Atestat
{
    class Functions
    {
      
        public static void ControlResize(object sender, SizeChangedEventArgs e)
        {
            //double scale = (e.NewSize.Width * e.NewSize.Width + e.NewSize.Height * e.NewSize.Height) / (e.PreviousSize.Width * e.PreviousSize.Width + e.PreviousSize.Height * e.PreviousSize.Height);
            double scale = e.NewSize.Width / e.PreviousSize.Width;

            if (scale != double.PositiveInfinity && scale != 0)
            {
                switch (sender.GetType().Name)
                {
                    case "TextBox":
                        (sender as TextBox).FontSize   *= scale;
                        break;

                    case "Button":
                        (sender as Button).FontSize    *= scale;
                        break;

                    case "TextBlock":
                        (sender as TextBlock).FontSize *= scale;
                        break;
                }
            }
        }

        public static bool AdressAlreadyInDataBase(string mail)
        {
            try
            {
                Vars.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Vars.conn;
                cmd.CommandText = "select * from utilizatori where mail = @mail";

                cmd.Parameters.AddWithValue("mail", mail);

                cmd.Parameters["mail"].DbType = DbType.String;

                MySqlDataReader r = cmd.ExecuteReader();

                if (r.Read())
                {
                    r.Close();
                    Vars.conn.Close();
                    return true;
                }
                else
                {
                    r.Close();
                    Vars.conn.Close();
                }

                return false;

            }
            catch
            {
                if (Vars.conn.State == ConnectionState.Open)
                    Vars.conn.Close();

                MessageBox.Show("Eroare!");
                return true;
            }
        }
    }
}
