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
using System.Windows.Media.Animation;

using Atestat.Classes;

namespace Atestat.Controls
{
    public partial class UserPanel : UserControl
    {
        #region Variables
        public List<int> ads = new List<int>();

        public int len, pages = 0, currentPage = 1;
        public bool btnProfileEnabled = true, btnAdsEnabled = true;

        public const string key = "This Ad Is From The User Page And It Should Redirect Him To The User Panel ▬↓-:`┘y";

        #endregion
        public UserPanel(int adPage = -1)
        {
            InitializeComponent();


            Profile.Visibility = Visibility.Hidden;
            Ads.Visibility = Visibility.Hidden;

            if (adPage != -1)
            {
                gridLogo.Visibility = Visibility.Hidden;

                btnAdsEnabled = false;
                btnAds.Background = new SolidColorBrush((Color)Resources["DarkBlue"]);
                Ads.Visibility = Visibility.Visible;

                SelectAds();
                currentPage = adPage;
                UpdateAds();
            }
        }

        #region ControlsEvents
        private void Button_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e);
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            ///Show personal data

            if (btnProfileEnabled)
            {
                Ads.Visibility = Visibility.Hidden;
                Profile.Visibility = Visibility.Visible;
                gridLogo.Visibility = Visibility.Hidden;
                ChangeProfile.Visibility = Visibility.Hidden;

                ColorAnimation c = new ColorAnimation();

                c.From = (Color)Resources["LightBlue"];
                c.To = (Color)Resources["DarkBlue"];
                c.Duration = new Duration(TimeSpan.FromSeconds(.15));

                ColorAnimation normalC = new ColorAnimation();

                normalC.From = (Color)Resources["DarkBlue"];
                normalC.To = (Color)Resources["LightBlue"];
                normalC.Duration = new Duration(TimeSpan.FromSeconds(.15));

                if (!btnAdsEnabled)
                {
                    btnAds.Background = new SolidColorBrush((Color)Resources["DarkBlue"]);
                    btnAds.Background.BeginAnimation(SolidColorBrush.ColorProperty, normalC);
                }

                btnProfile.Background = new SolidColorBrush((Color)Resources["LightBlue"]);
                btnProfile.Background.BeginAnimation(SolidColorBrush.ColorProperty, c);

                btnAdsEnabled = true;
                btnProfileEnabled = false;

                name.Content = ConnectedUser.name;
                mail.Content = ConnectedUser.mail;
                date.Content = ConnectedUser.registerDate;
                phoneNo.Content = ConnectedUser.phone;

                int ads = getAds();

                if (ads == -1)
                {
                    adsCount.Content = "404 Ads not found";
                }
                else
                {
                    adsCount.Content = ads;
                }
            }
        }

        private void btnAnunturi_Click(object sender, RoutedEventArgs e)
        {
            ///Load personal ads

            if (btnAdsEnabled)
            {
                Ads.Visibility = Visibility.Visible;
                Profile.Visibility = Visibility.Hidden;
                gridLogo.Visibility = Visibility.Hidden;
                ChangeProfile.Visibility = Visibility.Hidden;

                ColorAnimation ca = new ColorAnimation();

                ca.From = (Color)Resources["LightBlue"];
                ca.To = (Color)Resources["DarkBlue"];
                ca.Duration = new Duration(TimeSpan.FromSeconds(.15));

                ColorAnimation normalC = new ColorAnimation();

                normalC.From = (Color)Resources["DarkBlue"];
                normalC.To = (Color)Resources["LightBlue"];
                normalC.Duration = new Duration(TimeSpan.FromSeconds(.15));

                if (!btnProfileEnabled)
                {
                    btnProfile.Background = new SolidColorBrush((Color)Resources["DarkBlue"]);
                    btnProfile.Background.BeginAnimation(SolidColorBrush.ColorProperty, normalC);
                }

                btnAds.Background = new SolidColorBrush((Color)Resources["LightBlue"]);
                btnAds.Background.BeginAnimation(SolidColorBrush.ColorProperty, ca);



                SelectAds();
                currentPage = 1;
                UpdateAds();

                btnProfileEnabled = true;
                btnAdsEnabled = false;
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            ConnectedUser.id = -1;
            ConnectedUser.name = ConnectedUser.mail = ConnectedUser.password = ConnectedUser.phone = "";
            ConnectedUser.type = 0;
            ConnectedUser.loggedIn = false;

            MessageBox.Show("Ati fost deconectat cu succes!");

            this.Content = new MainControl();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new MainLogin();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /// change ui

            txtName.Text = ConnectedUser.name;
            txtMail.Text = ConnectedUser.mail;
            txtNumber.Text = ConnectedUser.phone;
            txtPassword.Text = ConnectedUser.password;

            ChangeProfile.Visibility = Visibility.Visible;
            Profile.Visibility = Visibility.Hidden;
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < pages)
            {
                currentPage++;
                UpdateAds();
            }

            btnPrevPage.Visibility = Visibility.Visible;

            if (currentPage == pages)
            {
                btnNextPage.Visibility = Visibility.Hidden;
            }
            else
            {
                btnNextPage.Visibility = Visibility.Visible;
            }
        }

        private void UpdateProfile(object sender, RoutedEventArgs e)
        {
            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Variables.conn;
                cmd.CommandText = "update utilizatori set nume = @nume, mail = @mail, nrTelefon = @nrTelefon, parola = @parola where id = @id";

                cmd.Parameters.AddWithValue("nume", txtName.Text);
                cmd.Parameters.AddWithValue("mail", txtMail.Text);
                cmd.Parameters.AddWithValue("nrTelefon", txtNumber.Text);
                cmd.Parameters.AddWithValue("parola", txtPassword.Text);
                cmd.Parameters.AddWithValue("id", ConnectedUser.id);

                if (cmd.ExecuteNonQuery() != 0)
                {
                    ConnectedUser.name = txtName.Text;
                    ConnectedUser.mail = txtMail.Text;
                    ConnectedUser.phone = txtNumber.Text;
                    ConnectedUser.password = txtPassword.Text;
                    MessageBox.Show("Datele au fost modificate");
                    ChangeProfile.Visibility = Visibility.Hidden;
                    Profile.Visibility = Visibility.Visible;

                    name.Content = ConnectedUser.name;
                    mail.Content = ConnectedUser.mail;
                    phoneNo.Content = ConnectedUser.phone;
                }

                Variables.conn.Close();
            }
            catch
            {
                if (Variables.conn.State == ConnectionState.Open)
                {
                    Variables.conn.Close();
                }

                MessageBox.Show("A aparut o eroare! Daca problema persista va rugam contactati un administrator");
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            Profile.Visibility = Visibility.Visible;
            ChangeProfile.Visibility = Visibility.Hidden;

            name.Content = ConnectedUser.name;
            mail.Content = ConnectedUser.mail;
            phoneNo.Content = ConnectedUser.phone;
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                UpdateAds();
            }

            btnNextPage.Visibility = Visibility.Visible;

            if (currentPage == 1)
            {
                btnPrevPage.Visibility = Visibility.Hidden;
            }
            else
            {
                btnPrevPage.Visibility = Visibility.Visible;
            }
        }

        private void ControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e);
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Char.IsDigit((char)((int)e.Key + 14)) && e.Key != Key.Back)
            {
                e.Handled = true;
            }
        }

        #endregion

        #region CustomFunctions
        public void SelectAds()
        {
            ads.Clear();

            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Variables.conn;
                cmd.CommandText = "select * from anunturi where idUtilizator = @idUtilizator";

                cmd.Parameters.AddWithValue("idUtilizator", ConnectedUser.id);

                MySqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    ads.Add(int.Parse(r["id"].ToString()));
                }

                r.Close();
                Variables.conn.Close();
            }
            catch
            {
                if (Variables.conn.State == ConnectionState.Open)
                {
                    Variables.conn.Close();
                }

                MessageBox.Show("A aparut o eroare! Daca problema persista va rugam contactati un administrator");
            }
        }

        public int getAds()
        {
            int ads = 0;

            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Variables.conn;
                cmd.CommandText = "select * from anunturi where idUtilizator = @idUtilizator";

                cmd.Parameters.AddWithValue("idUtilizator", ConnectedUser.id);

                MySqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    ads++;
                }

                r.Close();
                Variables.conn.Close();
            }
            catch
            {
                if (Variables.conn.State == ConnectionState.Open)
                {
                    Variables.conn.Close();
                }

                ads = -1;
            }

            return ads;
        }

        public void UpdateAds()
        {
            for (int i = 0; i < Ads.Children.Count; i++)
            {
                if (Ads.Children[i].GetType() == typeof(AdvertisementPreview))
                {
                    Ads.Children.RemoveAt(i);
                    i--;
                }
            }

            len = ads.Count;
            pages = len / 3 + Convert.ToInt32(len % 3 != 0);

            if (len == 0)
            {
                currentPage = 0;
                txtPage.Text = "Nu am gasit niciun anunt publicat!";
                gridLogo.Visibility = Visibility.Visible;
            }
            else
            {
                for (int c = 1, i = (currentPage - 1) * 3; i <= Math.Min(len - 1, currentPage * 3 - 1); i++, c += 2)
                {
                    AdvertisementPreview newAd = new AdvertisementPreview(ads[i], key, currentPage, (int)ControlTypes.UserPanel);

                    Ads.Children.Add(newAd);
                    Grid.SetRow(newAd, c);
                    Grid.SetColumn(newAd, 1);
                }

                txtPage.Text = "Pagina " + currentPage.ToString() + " / " + pages.ToString();
            }

            btnPrevPage.Visibility = Visibility.Visible;
            btnNextPage.Visibility = Visibility.Visible;

            if (currentPage == 1)
            {
                btnPrevPage.Visibility = Visibility.Hidden;
            }
            if (currentPage == pages && pages > 0)
            {
                btnNextPage.Visibility = Visibility.Hidden;
            }
            if (currentPage == 0)
            {
                btnPrevPage.Visibility = Visibility.Hidden;
                btnNextPage.Visibility = Visibility.Hidden;
            }

        }

        public void ResetAds()
        {
            len = ads.Count;
            pages = len / 3 + Convert.ToInt32(len % 3 != 0);

            if (len % 3 == 0 && currentPage == pages + 1)
            {
                currentPage--;
            }

            UpdateAds();


            btnNextPage.Visibility = Visibility.Hidden;

            if (pages > 1)
            {
                btnNextPage.Visibility = Visibility.Visible;
            }

            if (len != 0)
            {
                txtPage.Text = "Pagina " + currentPage.ToString() + " / " + pages.ToString();
            }
        }

        #endregion
    }
}
