using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Win32;
using System.IO;

namespace Atestat.Controls
{
    /// <summary>
    /// Interaction logic for AddAnAd.xaml
    /// </summary>
    public partial class AddAnAd : UserControl
    {
        public AddAnAd()
        {
            InitializeComponent();
        }

        public string[] categorii = new string[64];
        public int[] ids = new int[64];
        public int nrImg = 0;
        public string nrTel;
        private void ControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e);
        }

        private void Button_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Vars.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select * from categorii";
                cmd.Connection = Vars.conn;

                MySqlDataReader r = cmd.ExecuteReader();

                int i = 1;

                categorii[0] = "-";
                ids[0] = -1;
                cmbCats.Items.Add(categorii[0]);

                while (r.Read())
                {
                    categorii[i] = r["nume"].ToString();
                    ids[i] = int.Parse(r["id"].ToString());
                    cmbCats.Items.Add(categorii[i]);

                    i++;
                }

                r.Close();

                MySqlCommand selectCmd = new MySqlCommand();
                selectCmd.CommandText = "select nrTelefon from utilizatori where id = @id";
                selectCmd.Connection = Vars.conn;

                selectCmd.Parameters.AddWithValue("id", User.id);

                r = selectCmd.ExecuteReader();

                if (r.Read())
                {
                    nrTel = txtNumber.Text = r["nrTelefon"].ToString();
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

                MessageBox.Show("A aparut o eroare, daca problema persista va rugam sa contactati un administrator!");
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!Completed())
            {
                return;
            }


            try
            {
                Vars.conn.Open();

                CheckPhoneNo();
                bool added = InsertAdd();

                Vars.conn.Close();

                if (added)
                {
                    this.Content = new MainLogin();
                }
            }
            catch (Exception ex)
            {
                if (Vars.conn.State == ConnectionState.Open)
                {
                    Vars.conn.Close();
                }

                MessageBox.Show(ex.ToString());
            }
        }

        public bool Completed()
        {
            if (txtTitle.Text == "")
            {
                MessageBox.Show("Va rugam sa adaugati un titlu!");
                return false;
            }

            if (txtDesc.Text == "")
            {
                MessageBox.Show("Va rugam sa adaugati descrierea produsului!");
                return false;
            }

            if (nrImg == 0)
            {
                MessageBox.Show("Va rugam sa adaugati cel putin o imagine!");
                return false;
            }

            if (cmbCats.SelectedIndex == -1 || cmbCats.SelectedIndex == 0)
            {
                MessageBox.Show("Va rugam sa selectati categoria in care doriti sa publicati anuntul!");
                return false;
            }

            if (txtPrice.Text == "")
            {
                MessageBox.Show("Va rugam sa adaugati un pret produsului!");
                return false;
            }

            if (txtNumber.Text == "")
            {
                MessageBox.Show("Va rugam sa adaugati un numar de telefon!");
                return false;
            }


            return true;
        }

        public bool InsertAdd()
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "insert into anunturi ( idUtilizator,  titlu,  pret,  descriere,  dataIncarcare,  nrImagini,  idCategorie) values " +
                                                   "(@idUtilizator, @titlu, @pret, @descriere, @dataIncarcare, @nrImagini, @idCategorie)";
            cmd.Connection = Vars.conn;

            cmd.Parameters.AddWithValue("idUtilizator", User.id);
            cmd.Parameters.AddWithValue("titlu", txtTitle.Text);
            cmd.Parameters.AddWithValue("pret", int.Parse(txtPrice.Text));
            cmd.Parameters.AddWithValue("descriere", txtDesc.Text);
            cmd.Parameters.AddWithValue("dataIncarcare", DateTime.Now.Date);
            cmd.Parameters.AddWithValue("nrImagini", nrImg);
            cmd.Parameters.AddWithValue("idCategorie", ids[cmbCats.SelectedIndex]);

            cmd.Parameters["idUtilizator"].DbType = DbType.Int32;
            cmd.Parameters["pret"].DbType = DbType.Int32;
            cmd.Parameters["dataIncarcare"].DbType = DbType.Date;
            cmd.Parameters["nrImagini"].DbType = DbType.Int32;
            cmd.Parameters["idCategorie"].DbType = DbType.Int32;

            if (cmd.ExecuteNonQuery() != 0)
            {
                cmd.CommandText = "select @@Identity";
                ulong id = (ulong)cmd.ExecuteScalar();

                int c = 0;

                if ((Images.FindName("Image1") as Image).Source.ToString() != ((ImageSource)Resources["NoImage"]).ToString())
                {
                    string img = (Images.FindName("Image1") as Image).Source.ToString();

                    File.Copy(img.Substring(8, img.Length - 8), Vars.debugPath + "\\Assets\\Images\\" + id.ToString() + "_" + c.ToString() + img.Substring(img.Length - 4, 4));
                    c++;
                }

                if ((Images.FindName("Image2") as Image).Source.ToString() != ((ImageSource)Resources["NoImage"]).ToString())
                {
                    string img = (Images.FindName("Image2") as Image).Source.ToString();

                    File.Copy(img.Substring(8, img.Length - 8), Vars.debugPath + "\\Assets\\Images\\" + id.ToString() + "_" + c.ToString() + img.Substring(img.Length - 4, 4));
                    c++;
                }

                if ((Images.FindName("Image3") as Image).Source.ToString() != ((ImageSource)Resources["NoImage"]).ToString())
                {
                    string img = (Images.FindName("Image3") as Image).Source.ToString();

                    File.Copy(img.Substring(8, img.Length - 8), Vars.debugPath + "\\Assets\\Images\\" + id.ToString() + "_" + c.ToString() + img.Substring(img.Length - 4, 4));
                    c++;
                }

                if ((Images.FindName("Image4") as Image).Source.ToString() != ((ImageSource)Resources["NoImage"]).ToString())
                {
                    string img = (Images.FindName("Image4") as Image).Source.ToString();

                    File.Copy(img.Substring(8, img.Length - 8), Vars.debugPath + "\\Assets\\Images\\" + id.ToString() + "_" + c.ToString() + img.Substring(img.Length - 4, 4));
                    c++;
                }


                MessageBox.Show("anunt publicat!");
                return true;
            }
            else
            {

                MessageBox.Show("Eroare");
                return false;
            }
        }

        public void CheckPhoneNo()
        {
            if (nrTel != txtNumber.Text)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "update utilizatori set nrTelefon = @nrTelefon where id = @id";
                cmd.Connection = Vars.conn;

                cmd.Parameters.AddWithValue("nrTelefon", txtNumber.Text);
                cmd.Parameters.AddWithValue("id", User.id);

                cmd.Parameters["id"].DbType = DbType.Int32;

                cmd.ExecuteNonQuery();
            }
        }
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            if (User.loggedIn == true)
            {
                this.Content = new MainLogin();
            }
            else
            {
                this.Content = new MainControl();
            }
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Char.IsDigit((char)((int)e.Key + 14)) && e.Key != Key.Back)
            {
                e.Handled = true;
            }
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                (sender as Image).Source = new BitmapImage(new Uri(ofd.FileName));
                nrImg++;
                ((Images.FindName("G" + (sender as Image).Name) as Grid).FindName("R" + (sender as Image).Name) as Image).Visibility = Visibility.Visible;

            }
        }

        private void RemoveImage(object sender, MouseButtonEventArgs e)
        {
            string name = (sender as Image).Name.Substring(1, 6);
            (Images.FindName(name) as Image).Source = (ImageSource)Resources["NoImage"];
            (sender as Image).Visibility = Visibility.Hidden;
        }
    }
}
