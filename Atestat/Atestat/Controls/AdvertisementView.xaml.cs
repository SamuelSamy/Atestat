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
using System.IO;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using Atestat.Classes;

namespace Atestat.Controls
{
    public partial class AdvertisementView : UserControl
    {
        #region Variables
        public Advertisement ad;
        public User adOwner;
        public User adReporter;

        public string searchBarText;
        public int page = -1, currentImage = 0;
        public bool isAdmin = false;
        public DateTime data;

        #endregion

        public AdvertisementView(int id, int page, string searchBarText, bool isFromAdminPanel = false)
        {
            InitializeComponent();

            this.page = page;
            this.searchBarText = searchBarText;
            ad = new Advertisement(id);
            adOwner = ad.GetAdOwner();

            this.isAdmin = isFromAdminPanel;

            SetUp();
        }

        #region ControlsEvents
        private void PrevImage_Click(object sender, RoutedEventArgs e)
        {
            if (currentImage > 0)
            {
                currentImage--;
            }

            PrevImage.Visibility = Visibility.Visible;
            NextImage.Visibility = Visibility.Visible;

            if (currentImage == 0)
            {
                PrevImage.Visibility = Visibility.Hidden;
            }


            UpdateImage();
        }

        private void NextImage_Click(object sender, RoutedEventArgs e)
        {
            if (currentImage < ad.images)
            {
                currentImage++;
            }

            PrevImage.Visibility = Visibility.Visible;
            NextImage.Visibility = Visibility.Visible;

            if (currentImage == ad.images - 1)
            {
                NextImage.Visibility = Visibility.Hidden;
            }
            UpdateImage();
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            ReportIcon.RowDefinitions[0].Height = new GridLength(.1, GridUnitType.Star);
            ReportIcon.RowDefinitions[1].Height = new GridLength(.8, GridUnitType.Star);
            ReportIcon.RowDefinitions[2].Height = new GridLength(.1, GridUnitType.Star);
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            ReportIcon.RowDefinitions[0].Height = new GridLength(.2, GridUnitType.Star);
            ReportIcon.RowDefinitions[1].Height = new GridLength(.6, GridUnitType.Star);
            ReportIcon.RowDefinitions[2].Height = new GridLength(.2, GridUnitType.Star);
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /// open the report menu

            ImageView.Visibility = Visibility.Hidden;
            InfoGrid.Visibility = Visibility.Hidden;
            ReportGrid.Visibility = Visibility.Visible;

            AnimateBlurEffect(2, 0, 20, 0, 1);
        }

        private void Info_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /// open the info report menu

            ImageView.Visibility = Visibility.Hidden;
            ReportGrid.Visibility = Visibility.Hidden;
            InfoGrid.Visibility = Visibility.Visible;

            AnimateBlurEffect(2, 0, 20, 0, 1);

            Panel.SetZIndex(PopupGrid, 2);
        }


        private void btnSendReport_Click(object sender, RoutedEventArgs e)
        {
            /// send the report
            if (String.IsNullOrEmpty(txtReportDesc.Text))
            {
                MessageBox.Show("Va rugam sa adaugati un motiv!");
                return;
            }
            try
            {
                if (AlreadyReported())
                {
                    MessageBox.Show("Mesajul a fost reclamat cu succes!");

                    AnimateBlurEffect(0, 20, 0, 1, 0);
                    return;
                }
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "insert into reclamatii (idReclamant, idAnunt, data, descriere, verificat) values (@idReclamant, @idAnunt, @data, @descriere, @verificat)";
                cmd.Connection = Variables.conn;

                cmd.Parameters.AddWithValue("idReclamant", ConnectedUser.id);
                cmd.Parameters.AddWithValue("idAnunt", ad.id);
                cmd.Parameters.AddWithValue("data", DateTime.Now);
                cmd.Parameters.AddWithValue("descriere", txtReportDesc.Text);
                cmd.Parameters.AddWithValue("verificat", false);

                if (cmd.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("Mesajul a fost reclamat cu succes!");
                    AnimateBlurEffect(0, 20, 0, 1, 0);
                }
                else
                {
                    MessageBox.Show("Eroare la trimiterea mesajului");
                }

                Variables.conn.Close();
            }
            catch (Exception ex)
            {
                if (Variables.conn.State == ConnectionState.Open)
                {
                    Variables.conn.Close();
                }

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            /// clsoe the reprot menu
            AnimateBlurEffect(0, 20, 0, 1, 0);
        }

        private void btnBackInfo_Click(object sender, RoutedEventArgs e)
        {
            /// close the info report menu
            AnimateBlurEffect(0, 20, 0, 1, 0);
        }

        private void btnDeleteAdd_Click(object sender, RoutedEventArgs e)
        {
            /// delete add from data base and mark the report as verified
            ad.DeleteFromDataBase();
            MarkAsVerified();
            this.Content = new SettingsControl(true, 1);
        }

        private void btnVerify_Click(object sender, RoutedEventArgs e)
        {
            /// mark the report as verified, close the ad
            MarkAsVerified();
            this.Content = new SettingsControl(true, 1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /// close the image menu
            AnimateBlurEffect(0, 20, 0, 1, 0);
        }

        private void Image1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /// open the image menu

            ImageView.Visibility = Visibility.Visible;
            ReportGrid.Visibility = Visibility.Hidden;
            InfoGrid.Visibility = Visibility.Hidden;

            AnimateBlurEffect(2, 0, 20, 0, 1);

            string path = Variables.debugPath + "Assets\\Images\\" + ad.id.ToString() + "_0";

            if (File.Exists(path + ".png"))
            {
                path += ".png";
            }
            else
            {
                path += ".jpg";
            }

            PrevImage.Visibility = Visibility.Hidden;

            if (ad.images == 1)
            {
                NextImage.Visibility = Visibility.Hidden;
            }

            BitmapImage im = new BitmapImage();
            im.BeginInit();
            im.CacheOption = BitmapCacheOption.OnLoad;
            im.UriSource = new Uri(path);
            im.EndInit();

            BigImage.Source = im;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (isAdmin)
            {
                /// back to admin panel
                this.Content = new SettingsControl(true, page);
            }
            else
            {
                /// back to ad page

                this.Content = new AdvertisementsPage(page, searchBarText);
            }
        }

        private void ControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e);
        }


        private void Button_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e);
        }

        #endregion

        #region CustomFunctions
        public void SetUp()
        {
            if (ConnectedUser.loggedIn)
            {
                ReportButton.Visibility = Visibility.Visible;
            }

            if (isAdmin)
            {
                adReporter = ad.GetReporter();
                ReportButton.Source = (Resources["Info"] as ImageSource);
                ReportButton.MouseDown += Info_MouseDown;
            }
            else
            {
                ReportButton.MouseDown += Image_MouseDown;
            }

            Complete();
        }


        public void Complete()
        {
            txtTtile.Text = ad.title;
            txtDesc.Text = ad.desc;
            txtPrice.Text = ad.price.ToString();
            txtCategory.Text = SelectCategory();
            SelectUser();
            UploadPhotos();

            if (isAdmin)
            {

                txtInfoDesc.Text = ad.descriptionR;
                txtDetailsInfo.Text = "Anunt reclamat de: " + adReporter.mail + "\nIn data de: " + ad.dateR.Date.ToString().Substring(0, 10) + "\nMotiv:";
            }

        }

        public void UpdateImage()
        {
            string path = Variables.debugPath + "Assets\\Images\\" + ad.id.ToString() + "_" + currentImage.ToString();

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

            BigImage.Source = im;
        }




        

        public bool AlreadyReported()
        {
            bool reported = false;

            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select * from reclamatii where idAnunt = @id and verificat = 0";
                cmd.Connection = Variables.conn;

                cmd.Parameters.AddWithValue("id", ad.id);

                MySqlDataReader r = cmd.ExecuteReader();

                if (r.Read())
                {
                    reported = true;
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
                MessageBox.Show(ex.ToString());
            }

            return reported;
        }

        

        public void MarkAsVerified()
        {
            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "update reclamatii set verificat = 1 where idAnunt = @idAnunt";
                cmd.Connection = Variables.conn;

                cmd.Parameters.AddWithValue("idAnunt", ad.id);

                if (cmd.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("Reclamatie sterarsa cu succes!");
                }
                else
                {
                    MessageBox.Show("eroare");
                }

                Variables.conn.Close();
            }
            catch (Exception ex)
            {
                if (Variables.conn.State == ConnectionState.Open)
                {
                    Variables.conn.Close();
                }
                MessageBox.Show(ex.ToString());
            }
        }

       
        public string SelectCategory()
        {
            string category = "";

            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select * from categorii where id = @id";
                cmd.Connection = Variables.conn;

                cmd.Parameters.AddWithValue("id", ad.idCat);

                MySqlDataReader r = cmd.ExecuteReader();

                if (r.Read())
                {
                    category = r["nume"].ToString();
                }
                else
                {
                    MessageBox.Show("Nu a fost gasita categoria");
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
                MessageBox.Show(ex.ToString());
            }

            return category;
        }

        public void SelectUser()
        {
            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select * from utilizatori where id = @id";
                cmd.Connection = Variables.conn;

                cmd.Parameters.AddWithValue("id", ad.idUser);

                MySqlDataReader r = cmd.ExecuteReader();

                if (r.Read())
                {
                    txtNumber.Text = r["nrTelefon"].ToString();
                    txtSeller.Text = r["nume"].ToString();
                }
                else
                {
                    MessageBox.Show("Nu a fost gasit utilizatorul");
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

                MessageBox.Show(ex.ToString());
            }
        }

        public void UploadPhotos()
        {
            for (int i = 0; i < ad.images; i++)
            {
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

                (Images.Children[i] as Image).Source = im;
            }

        }

        public void AnimateBlurEffect(int zIndex, int from, int to, int opacityFrom, int opacityTo)
        {
            Panel.SetZIndex(PopupGrid, zIndex);

            BlurEffect b = new BlurEffect();
            MainView.Effect = b;

            DoubleAnimation anim = new DoubleAnimation();
            anim.From = from;
            anim.To = to;
            anim.Duration = (Duration)TimeSpan.FromSeconds(.1);

            DoubleAnimation opacityAnimation = new DoubleAnimation();

            opacityAnimation.From = opacityFrom;
            opacityAnimation.To = opacityTo;
            opacityAnimation.Duration = (Duration)TimeSpan.FromSeconds(.1);

            PopupGrid.BeginAnimation(OpacityProperty, opacityAnimation);
            b.BeginAnimation(BlurEffect.RadiusProperty, anim);
        }

        #endregion
    }
}
