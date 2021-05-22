using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using Atestat.Classes;
using System.Security.Cryptography;

namespace Atestat
{
    class Functions
    {
      
        public static void ControlResize(object sender, SizeChangedEventArgs e, UserControl control)
        {
            Object obj = control;

            while (!(obj is MainWindow))
            {
                obj = (obj as FrameworkElement).Parent;
            }

            Window owner = obj as MainWindow;

            double scale = Math.Min(owner.Width / Variables.WindowW, owner.Height / Variables.WindowH);


            if (scale != double.PositiveInfinity && scale != 0)
            {
                switch (sender.GetType().Name)
                {
                    case "TextBox":
                        (sender as TextBox).FontSize = double.Parse((sender as TextBox).Tag.ToString()) * scale;
                        break;

                    case "Button":
                        (sender as Button).FontSize = double.Parse((sender as Button).Tag.ToString()) * scale;
                        break;

                    case "PasswordBox":
                        (sender as PasswordBox).FontSize = double.Parse((sender as PasswordBox).Tag.ToString()) * scale;
                        break;

                    case "Label":
                        (sender as Label).FontSize = double.Parse((sender as Label).Tag.ToString()) * scale;
                        break;

                    case "ComboBox":
                        (sender as ComboBox).FontSize = double.Parse((sender as ComboBox).Tag.ToString()) * scale;
                        break;
                }
            }
        }

        public static bool AdressAlreadyInDataBase(string mail)
        {
            try
            {
                bool ret = false;
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Variables.conn;
                cmd.CommandText = "select * from utilizatori where mail = @mail";

                cmd.Parameters.AddWithValue("mail", mail);

                cmd.Parameters["mail"].DbType = DbType.String;

                MySqlDataReader r = cmd.ExecuteReader();

                if (r.Read())
                {
                    ret = true;
                }
                
                r.Close();
                Variables.conn.Close();
               
                return ret;

            }
            catch
            {
                if (Variables.conn.State == ConnectionState.Open)
                {
                    Variables.conn.Close();
                }

                return true;
            }
        }

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}
