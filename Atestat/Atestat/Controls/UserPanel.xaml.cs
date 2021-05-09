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

namespace Atestat.Controls
{
    /// <summary>
    /// Interaction logic for UserPanel.xaml
    /// </summary>
    public partial class UserPanel : UserControl
    {
        public List<int> ads = new List<int>();
        public int len, pages = 0, currentPage = 1;
        public bool btnProfileEnabled = true, btnAdsEnabled = true;

        public const string key = "This Ad Is From The User Page And It Should Redirect Him To The User Panel ▬↓-:`┘y";

        public UserPanel(int adPage = -1)
        {
            InitializeComponent();


            Profile.Visibility = Visibility.Hidden;
            Ads.Visibility = Visibility.Hidden;

            if (adPage != -1)
            {
                gridLogo.Visibility = Visibility.Hidden;

                /// do some things here 
                SelectAds();
                btnAdsEnabled = false;
                btnAds.Background = new SolidColorBrush((Color)Resources["DarkBlue"]);
                Ads.Visibility = Visibility.Visible;

                int len = ads.Count;
                pages = len / 3 + Convert.ToInt32(len % 3 != 0);

                txtPage.Text = "Pagina " + adPage.ToString() + " / " + pages.ToString();
                currentPage = adPage;

                for (int c = 1, i = (currentPage - 1) * 3; i <= Math.Min(len - 1, currentPage * 3 - 1); i++, c += 2)
                {
                    DisplayAdControlH newAd = new DisplayAdControlH(ads[i], key, currentPage);

                    Ads.Children.Add(newAd);
                    Grid.SetRow(newAd, c);
                    Grid.SetColumn(newAd, 1);
                }

                btnPrevPage.Visibility = Visibility.Visible;
                btnNextPage.Visibility = Visibility.Visible;

                if (currentPage == 1)
                {
                    btnPrevPage.Visibility = Visibility.Hidden;
                }

                if (currentPage == pages)
                {
                    btnNextPage.Visibility = Visibility.Hidden;
                }
            }
        }
        private void Button_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e);
        }


        public void SelectAds()
        {
            ads.Clear();

            try
            {
                Vars.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Vars.conn;
                cmd.CommandText = "select * from anunturi where idUtilizator = @idUtilizator";

                cmd.Parameters.AddWithValue("idUtilizator", User.id);

                MySqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    ads.Add(int.Parse(r["id"].ToString()));
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

                MessageBox.Show("A aparut o eroare! Daca problema persista va rugam contactati un administrator");
            }
        }
        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            ///Show personal data
            
            if (btnProfileEnabled)
            {
                Ads.Visibility = Visibility.Hidden;
                Profile.Visibility = Visibility.Visible;
                gridLogo.Visibility = Visibility.Hidden;

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


                name.Content = User.name;
                mail.Content = User.mail;
                date.Content = User.registerDate;
                phoneNo.Content = User.phone;
                adsCount.Content = 0;
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

                if (ads.Count > 0)
                {
                    int len = ads.Count;
                    pages = len / 3 + Convert.ToInt32(len % 3 != 0);

                    for (int c = 1, i = (currentPage - 1) * 3; i <= Math.Min(len - 1, currentPage * 3 - 1); i++, c += 2)
                    {
                        DisplayAdControlH newAd = new DisplayAdControlH(ads[i], key, currentPage);

                        Ads.Children.Add(newAd);
                        Grid.SetRow(newAd, c);
                        Grid.SetColumn(newAd, 1);
                    }

                    txtPage.Text = "Pagina " + currentPage + " / " + pages.ToString();
                }

                btnPrevPage.Visibility = Visibility.Visible;
                btnNextPage.Visibility = Visibility.Visible;

                if (currentPage == 1)
                {
                    btnPrevPage.Visibility = Visibility.Hidden;
                }

                if (currentPage == pages)
                {
                    btnNextPage.Visibility = Visibility.Hidden;
                }

                btnProfileEnabled = true;
                btnAdsEnabled = false;
            }

           
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {


            User.id = -1;
            User.name = User.mail = User.password = User.phone = User.type = "";
            User.loggedIn = false;

            MessageBox.Show("Ati fost deconectat cu succes!");

            this.Content = new MainControl();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new MainLogin();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

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

        public void UpdateAds()
        {
            for (int i = 0; i < Ads.Children.Count; i++)
            {
                if (Ads.Children[i].GetType() == typeof(DisplayAdControlH))
                {
                    Ads.Children.RemoveAt(i);
                    i--;
                }
            }

            len = ads.Count;
            
            for (int c = 1, i = (currentPage - 1) * 3; i <= Math.Min(len - 1, currentPage * 3 - 1); i++, c += 2)
            {
                DisplayAdControlH newAd = new DisplayAdControlH(ads[i], key, currentPage);

                Ads.Children.Add(newAd);
                Grid.SetRow(newAd, c);
                Grid.SetColumn(newAd, 1);
            }

            txtPage.Text = "Pagina " + currentPage.ToString() + " / " + pages.ToString();
        }
    }
}
