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
using Atestat.Classes;

namespace Atestat.Controls
{
    /// <summary>
    /// Interaction logic for AddAnAd.xaml
    /// </summary>
    public partial class AddAnAd : UserControl
    {
        public int adId = -1, adPage = -1;
        public bool EditView = false;
        public Ad ad;
        public AddAnAd(bool EditView = false, int adId = -1, int adPage = -1)
        {
            InitializeComponent();

            if (EditView)
            {
                this.EditView = true;
                this.adId = adId;
                this.adPage = adPage;

                btnAdd.Content = "Modifica";
                btnHome.Content = "Inapoi";
                lblAction.Text = "Modificare anunt";
                /// Get The Ad Info
                ad = new Ad(adId);
            }
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

                categorii[0] = "Selectati o categorie";
                ids[0] = -1;
                cmbCats.Items.Add(categorii[0]);

                while (r.Read())
                {
                    categorii[i] = r["nume"].ToString();
                    ids[i] = int.Parse(r["id"].ToString());
                    cmbCats.Items.Add(categorii[i]);

                    if (EditView && ids[i] == ad.idCat)
                    {
                        cmbCats.SelectedIndex = i;
                    }


                    i++;
                }

                r.Close();

                if (EditView)
                {
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
                }
                else
                {
                    nrTel = txtNumber.Text = User.phone;
                }

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

            if (EditView)
            {
                txtDesc.Text = ad.desc;
                txtTitle.Text = ad.title;
                txtPrice.Text = ad.price.ToString();
                nrImg = ad.images;

                for (int i = 0; i < ad.images; i++)
                {

                    Image img = (Images.FindName("Image" + (i + 1).ToString()) as Image);

                    string path = Vars.debugPath + "\\Assets\\Images\\" + ad.id.ToString() + "_" + i.ToString();

                    if (File.Exists(path + ".png"))
                    {
                        path += ".png";
                    }
                    else
                    {
                        path += ".jpg";
                    }

                    /*
                     Gunoi pe care l am incercat dar evident ca nu a mers
                    
                    using (Uri sourceImg = new Uri(path))
                    {
                        Image clonedImg = new Bitmap(sourceImg.Width, sourceImg.Height, PixelFormat.Format32bppArgb);

                        using (var copy = Graphics.FromImage(clonedImg))
                        {
                            copy.DrawImage(sourceImg, 0, 0);
                        }

                        pbImg.InitialImage = null;
                        pbImg.Image = clonedImg;
                    }

                    using (BitmapImage im = new BitmapImage())
                    {
                        im.BeginInit();
                        im.CacheOption = BitmapCacheOption.OnLoad;
                        im.UriSource = new Uri(path);
                        im.EndInit();

                        img.Source = im;
                    }*/

                    BitmapImage im = new BitmapImage();
                    im.BeginInit();
                    im.CacheOption = BitmapCacheOption.OnLoad;
                    im.UriSource = new Uri(path);
                    im.EndInit();

                    img.Source = im;


                    ((Images.FindName("G" + img.Name) as Grid).FindName("R" + img.Name) as Image).Visibility = Visibility.Visible;
                }

            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!Completed())
            {
                return;
            }

            if (btnAdd.Content.ToString() == "Modifica")
            {
                /// Change The Ad ; I'm going to cry :/ I don't want to do this anymore
                try
                {
                    Vars.conn.Open();

                    CheckPhoneNo();
                    bool changed = ChangeAdd();

                    Vars.conn.Close();

                    if (changed)
                    {
                        this.Content = new UserPanel(adPage);
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
            else
            {
                /// Add The Ad
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

        public bool ChangeAdd()
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "update anunturi set idUtilizator = @idUtilizator,  titlu = @titlu,  pret = @pret,  descriere = @descriere,  dataIncarcare = @dataIncarcare, " +
                                                   "nrImagini = @nrImagini,  idCategorie = @idCategorie where id = @id";
            cmd.Connection = Vars.conn;

            cmd.Parameters.AddWithValue("idUtilizator", User.id);
            cmd.Parameters.AddWithValue("titlu", txtTitle.Text);
            cmd.Parameters.AddWithValue("pret", int.Parse(txtPrice.Text));
            cmd.Parameters.AddWithValue("descriere", txtDesc.Text);
            cmd.Parameters.AddWithValue("dataIncarcare", DateTime.Now.Date);
            cmd.Parameters.AddWithValue("nrImagini", nrImg);
            cmd.Parameters.AddWithValue("idCategorie", ids[cmbCats.SelectedIndex]);
            cmd.Parameters.AddWithValue("id", ad.id);

            cmd.Parameters["idUtilizator"].DbType = DbType.Int32;
            cmd.Parameters["pret"].DbType = DbType.Int32;
            cmd.Parameters["dataIncarcare"].DbType = DbType.Date;
            cmd.Parameters["nrImagini"].DbType = DbType.Int32;
            cmd.Parameters["idCategorie"].DbType = DbType.Int32;
            cmd.Parameters["id"].DbType = DbType.Int32;

            if (cmd.ExecuteNonQuery() != 0)
            {
                cmd.CommandText = "select @@Identity";
                ulong id = (ulong)cmd.ExecuteScalar();


                string[] s= new string[8];

                for (int i = 0; i < 4; i++)
                {
                    s[i] = (Images.FindName("Image" + (i + 1).ToString()) as Image).Source.ToString();

                    (Images.FindName("Image" + (i + 1).ToString()) as Image).Source = (ImageSource)Resources["NoImage"];
                    (Images.FindName("RImage" + (i + 1).ToString()) as Image).Source = null;
                }

                int c = 0;

                for (int i = 0; i < 4; i++)
                {
                    if (s[i] != ((ImageSource)Resources["NoImage"]).ToString())
                    {
                        string img = s[i];

                        string path = Vars.debugPath + "\\Assets\\Images\\" + ad.id.ToString() + "_" + c.ToString();



                        if (File.Exists(path + ".png"))
                        {
                            path += ".png";
                            File.Replace(img.Substring(8, img.Length - 8), path, null);
                        }
                        else if (File.Exists(path + ".jpg"))
                        {
                            path += ".jpg";
                            File.Replace(img.Substring(8, img.Length - 8), path, null);
                        }
                        else
                        {
                            File.Copy(img.Substring(8, img.Length - 8), Vars.debugPath + "\\Assets\\Images\\" + id.ToString() + "_" + c.ToString() + img.Substring(img.Length - 4, 4));
                        }

                        c++;
                    }
                   
                }

                MessageBox.Show("anunt modificat!");
                return true;
            }
            else
            {
                MessageBox.Show("Eroare");
                return false;
            }
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

                for (int i = 0; i < 4; i++)
                {
                    if ((Images.FindName("Image" + (i + 1).ToString()) as Image).Source.ToString() != ((ImageSource)Resources["NoImage"]).ToString())
                    {
                        string img = (Images.FindName("Image" + (i + 1).ToString()) as Image).Source.ToString();

                        File.Copy(img.Substring(8, img.Length - 8), Vars.debugPath + "\\Assets\\Images\\" + id.ToString() + "_" + c.ToString() + img.Substring(img.Length - 4, 4));

                        c++;
                    }
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
            if (EditView)
            {
                this.Content = new UserPanel(adPage);
            }
            else
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
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.UriSource = new Uri(ofd.FileName);
                img.EndInit();

                (sender as Image).Source = img;
                img = new BitmapImage();
                nrImg++;
                ((Images.FindName("G" + (sender as Image).Name) as Grid).FindName("R" + (sender as Image).Name) as Image).Visibility = Visibility.Visible;

            }
        }

        private void RemoveImage(object sender, MouseButtonEventArgs e)
        {
            string name = (sender as Image).Name.Substring(1, 6);
            (Images.FindName(name) as Image).Source = (ImageSource)Resources["NoImage"];
            (sender as Image).Visibility = Visibility.Hidden;
            nrImg--;
        }
    }
}
