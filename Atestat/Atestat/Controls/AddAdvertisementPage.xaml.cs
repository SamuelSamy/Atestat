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
    public partial class AddAdvertisementPage : UserControl
    {
        #region Variables

        public Advertisement ad;

        public int adId = -1, adPage = -1, nrImg = 0;

        public string nrTel;

        public string[] categorii = new string[64];

        public int[] ids = new int[64];

        public bool EditView = false;

        #endregion

        public AddAdvertisementPage(bool EditView, int adId = -1, int adPage = -1)
        {
            InitializeComponent();

            btnHome.Content = "Inapoi";


            if (EditView)
            {
                this.EditView = true;
                this.adId = adId;
                this.adPage = adPage;

                btnAdd.Content = "Modifica";
                lblAction.Text = "Modificare anunt";
                
                ad = new Advertisement(adId);
            }
        }

        #region ControlsEvents
        private void ControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e, this);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select * from categorii";
                cmd.Connection = Variables.conn;

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
                    selectCmd.Connection = Variables.conn;

                    selectCmd.Parameters.AddWithValue("id", ConnectedUser.id);

                    r = selectCmd.ExecuteReader();

                    if (r.Read())
                    {
                        nrTel = txtNumber.Text = r["nrTelefon"].ToString();
                    }

                    r.Close();
                }
                else
                {
                    nrTel = txtNumber.Text = ConnectedUser.phone;
                }

                Variables.conn.Close();
            }
            catch
            {
                if (Variables.conn.State == ConnectionState.Open)
                {
                    Variables.conn.Close();
                }

                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                cmb.ShowDialog();

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



                    string path = Variables.debugPath + "Assets\\Images\\" + ad.id.ToString() + "_" + i.ToString();

                    if (File.Exists(path + ".png"))
                    {
                        path += ".png";
                    }
                    else
                    {
                        path += ".jpg";
                    }

                    BitmapImage im = new BitmapImage();
                    im.BeginInit();
                    im.CacheOption = BitmapCacheOption.OnLoad;
                    im.UriSource = new Uri(path);
                    im.EndInit();

                    img.Source = im;

                    im.UriSource = null;

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
                    Variables.conn.Open();

                    CheckPhoneNo();
                    bool changed = ChangeAdd();

                    Variables.conn.Close();

                    if (changed)
                    {
                        this.Content = new UserPanel(adPage);
                    }
                }
                catch (Exception ex)
                {
                    if (Variables.conn.State == ConnectionState.Open)
                    {
                        Variables.conn.Close();
                    }

                    CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                    cmb.ShowDialog();
                }
            }
            else
            {
                /// Add The Ad
                try
                {
                    Variables.conn.Open();

                    CheckPhoneNo();
                    bool added = InsertAdd();

                    Variables.conn.Close();

                    if (added)
                    {
                        this.Content = new MainLogin();
                    }
                }
                catch (Exception ex)
                {
                    if (Variables.conn.State == ConnectionState.Open)
                    {
                        Variables.conn.Close();
                    }

                    CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                    cmb.ShowDialog();
                }
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
                this.Content = new AdvertisementsPage(adPage);
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
                Uri imageUri = new Uri((sender as Image).Source.ToString());


                if ((sender as Image).Source.ToString() == (Resources["NoImage"] as ImageSource).ToString())
                {
                    nrImg++;
                }


                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.UriSource = new Uri(ofd.FileName);
                img.EndInit();

                (sender as Image).Source = img;

                img.UriSource = null;
                img = new BitmapImage();

                ((Images.FindName("G" + (sender as Image).Name) as Grid).FindName("R" + (sender as Image).Name) as Image).Visibility = Visibility.Visible;

            }
        }

        #endregion

        #region CustomFunctions
        public bool Completed()
        {
            if (txtTitle.Text == "")
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "Va rugam sa adaugati un titlu!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
             
                return false;
            }

            if (txtDesc.Text == "")
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "Va rugam sa adaugati descrierea produsului!", this, MessageBoxButton.OK);
                cmb.ShowDialog();

                return false;
            }

            if (nrImg == 0)
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "Va rugam sa adaugati cel putin o imagine!", this, MessageBoxButton.OK);
                cmb.ShowDialog();

                return false;
            }

            if (cmbCats.SelectedIndex == -1 || cmbCats.SelectedIndex == 0)
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "Va rugam sa selectati o categorie!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
                return false;
            }

            if (txtPrice.Text == "")
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "Va rugam sa adaugati un pret produsului!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
          
                return false;
            }

            if (txtNumber.Text == "")
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "Va rugam sa adaugati un numar de telefon!", this, MessageBoxButton.OK);
                cmb.ShowDialog();

                return false;
            }


            return true;
        }

        public bool ChangeAdd()
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "update anunturi set idUtilizator = @idUtilizator,  titlu = @titlu,  pret = @pret,  descriere = @descriere,  dataIncarcare = @dataIncarcare, " +
                                                   "nrImagini = @nrImagini,  idCategorie = @idCategorie where id = @id";
            cmd.Connection = Variables.conn;

            cmd.Parameters.AddWithValue("idUtilizator", ConnectedUser.id);
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


                string[] s = new string[8];

                for (int i = 0; i < 4; i++)
                {
                    s[i] = (Images.FindName("Image" + (i + 1).ToString()) as Image).Source.ToString();

                    (Images.FindName("Image" + (i + 1).ToString()) as Image).Source = (Resources["NoImage"] as ImageSource);
                    (Images.FindName("RImage" + (i + 1).ToString()) as Image).Source = null;
                }

                int c = 0;

                for (int i = 0; i < 4; i++)
                {
                    if (s[i] != ((Resources["NoImage"] as ImageSource)).ToString())
                    {
                        string img = s[i];

                        string path = Variables.debugPath + "Assets\\Images\\" + ad.id.ToString() + "_" + c.ToString();

                        string oldPath = img.Substring(8, img.Length - 8);

                        if (File.Exists(path + ".png"))
                        {
                            path += ".png";
                          
                        }
                        else
                        {
                            path += ".jpg";
                        }
                        

                        Uri oldUri = new Uri(oldPath);
                        Uri uri = new Uri(path);

                        if (oldUri != uri)
                        {
                            File.Delete(path);
                            File.Copy(oldPath, path);
                        }

                        c++;
                    }
                   
                }

                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.green, "Anuntul a fost modificat cu succes!", this, MessageBoxButton.OK);
                cmb.ShowDialog();

                return true;
            }
            else
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
                return false;
            }
        }

        public bool InsertAdd()
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "insert into anunturi ( idUtilizator,  titlu,  pret,  descriere,  dataIncarcare,  nrImagini,  idCategorie) values " +
                                                   "(@idUtilizator, @titlu, @pret, @descriere, @dataIncarcare, @nrImagini, @idCategorie)";
            cmd.Connection = Variables.conn;

            cmd.Parameters.AddWithValue("idUtilizator", ConnectedUser.id);
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
                    if ((Images.FindName("Image" + (i + 1).ToString()) as Image).Source.ToString() != ((Resources["NoImage"] as ImageSource)).ToString())
                    {
                        string img = (Images.FindName("Image" + (i + 1).ToString()) as Image).Source.ToString();

                        File.Copy(img.Substring(8, img.Length - 8), Variables.debugPath + "Assets\\Images\\" + id.ToString() + "_" + c.ToString() + img.Substring(img.Length - 4, 4));

                        c++;
                    }
                }

                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.green, "Anuntul a fost publicat cu succes!", this, MessageBoxButton.OK);
                cmb.ShowDialog();

                return true;
            }
            else
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
                return false;
            }
        }

        public void CheckPhoneNo()
        {
            if (nrTel != txtNumber.Text)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "update utilizatori set nrTelefon = @nrTelefon where id = @id";
                cmd.Connection = Variables.conn;

                cmd.Parameters.AddWithValue("nrTelefon", txtNumber.Text);
                cmd.Parameters.AddWithValue("id", ConnectedUser.id);

                cmd.Parameters["id"].DbType = DbType.Int32;

                cmd.ExecuteNonQuery();
            }
        }

        

        private void RemoveImage(object sender, MouseButtonEventArgs e)
        {
            string name = (sender as Image).Name.Substring(1, 6);
            (Images.FindName(name) as Image).Source = (Resources["NoImage"] as ImageSource);
            (sender as Image).Visibility = Visibility.Hidden;
            nrImg--;
        }
        #endregion
    }
}
