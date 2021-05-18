using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;

namespace Atestat.Classes
{
    public class Advertisement
    {
        public string title, desc;
        public int id, idUser, price, views, images, idCat;
        public DateTime date;

        public string descriptionR;
        public int idR, idReporter;
        public DateTime dateR;
        public bool verified;

        public Advertisement(int id)
        {
            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Variables.conn;
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

                    r.Close();

                    MySqlCommand cmd2 = new MySqlCommand();
                    cmd2.CommandText = "select * from reclamatii where idAnunt = @id and verificat = 0";
                    cmd2.Connection = Variables.conn;

                    cmd2.Parameters.AddWithValue("id", id);

                    r = cmd2.ExecuteReader();

                    if (r.Read())
                    {
                        this.descriptionR = r["descriere"].ToString();
                        this.idReporter = int.Parse(r["idReclamant"].ToString());
                        this.idR = int.Parse(r["id"].ToString());
                        this.verified = Convert.ToBoolean(r["verificat"].ToString());
                        this.dateR = DateTime.Parse(r["data"].ToString());
                    }
                    else
                    {
                        this.descriptionR = "";
                        this.idReporter = 0;
                        this.idR = 0;
                        this.verified = false;
                        this.dateR = DateTime.Now;
                    }

                }

                r.Close();
                Variables.conn.Close();
            }
            catch (Exception ex)
            {
                if (Variables.conn.State == ConnectionState.Open)
                {
                    Variables.conn.Close();
                }

                MessageBox.Show("A aparut o eroare! Daca problema persista va rugam sa contactati un administrator!");
            }
        }

        public bool DeleteFromDataBase()
        {
            bool succes = false;

            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "delete from anunturi where id = @id";
                cmd.Connection = Variables.conn;

                cmd.Parameters.AddWithValue("id", id);

                if (cmd.ExecuteNonQuery() != 0)
                {
                    succes = true;
                }

                Variables.conn.Close();
            }
            catch
            {
                if (Variables.conn.State == System.Data.ConnectionState.Open)
                {
                    Variables.conn.Close();
                }

                MessageBox.Show("Eroare");
            }

            return succes;
        }

        public User GetAdOwner()
        {
            User user = new User(id);

            return user;
        }

        public User GetReporter()
        {
            User user = new User(idReporter);

            return user;
        }
    }
}
