using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;

namespace Atestat.Classes
{
    public class Ad
    {
        public string title, desc;
        public int id, idUser, price, views, images, idCat;
        public DateTime date;
        public Ad(int id)
        {
            Vars.conn.Open();

            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Vars.conn;
                cmd.CommandText = "select * from anunturi where id = @id";

                cmd.Parameters.AddWithValue("id", id);

                MySqlDataReader r = cmd.ExecuteReader();

                if (r.Read())
                {
                    this.id = id;
                    this.title = r["titlu"].ToString();
                    this.desc = r["descriere"].ToString();
                    this.idUser = int.Parse(r["idUtilizator"].ToString());
                    this.price = int.Parse(r["pret"].ToString());
                    this.views = int.Parse(r["vizualizari"].ToString());
                    this.images = int.Parse(r["nrImagini"].ToString());
                    this.idCat = int.Parse(r["idCategorie"].ToString());
                }

                r.Close();
                Vars.conn.Close();
            }
            catch
            {
                if (Vars.conn.State == ConnectionState.Open)
                {
                    Vars.conn.Close();
                }

                MessageBox.Show("A aparut o eroare! Daca problema persista va rugam sa contactati un administrator!");
            }
        }
    }
}
