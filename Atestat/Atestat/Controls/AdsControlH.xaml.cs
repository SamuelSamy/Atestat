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
using Atestat.Classes;
using System.IO;
using MySql.Data.MySqlClient;
using System.Windows.Threading;
using System.Globalization;

namespace Atestat.Controls
{
    /// <summary>
    /// Interaction logic for DisplayAdControlH.xaml
    /// </summary>
    public partial class AdsControlH : UserControl
    {
        public AdsControlH()
        {
            InitializeComponent();    
        }

        public const string watermark = "Cauta un anunt...";

        public int[] ids = new int[1024];
        public string[] titluri = new string[1024];
        public int pages = 0, currentPage = 1, len = 0, lenCats = 0;
        public int[] catIds = new int[64];

        public int getNo()
        {
            int len = 0;

            try
            {
                Vars.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select id from anunturi";
                cmd.Connection = Vars.conn;

                MySqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    ids[++len] = int.Parse(r["id"].ToString());
                }

                r.Close();
                Vars.conn.Close();
            }
            catch
            {
                if (Vars.conn.State == System.Data.ConnectionState.Open)
                {
                    Vars.conn.Close();
                }
                MessageBox.Show("A aparut o eroare! Daca problema persista va rugam sa contactati un administrator!");
            }

            return len;
        }

        public string[] SelectCats()
        {
            string[] cats = new string[1024];

            try
            {
                Vars.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select * from categorii";
                cmd.Connection = Vars.conn;

                MySqlDataReader r = cmd.ExecuteReader();

                int len = 1;

                catIds[0] = -1;
                cats[0] = "-";

                while (r.Read())
                {
                    catIds[len] = int.Parse(r["id"].ToString());
                    cats[len] = r["nume"].ToString();
                    len++;
                }

                lenCats = len;

                r.Close();
                Vars.conn.Close();
            }
            catch
            {
                if (Vars.conn.State == System.Data.ConnectionState.Open)
                {
                    Vars.conn.Close();
                }
                MessageBox.Show("A aparut o eroare!");
            }

            return cats;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            len = getNo();
            pages = len / 3 + Convert.ToInt32(len % 3 != 0);

            for (int i = 1; i <= Math.Min(3, len); i++)
            {
                DisplayAdControlH newAd = new DisplayAdControlH(ids[i]);

                Ads.Children.Add(newAd);
                Grid.SetRow(newAd, i * 2 + 1);
                Grid.SetColumn(newAd, 1);
            }

            txtPage.Text = "Pagina 1 / " + pages.ToString();

            string[] s = SelectCats();

            for (int i = 0; i <= lenCats; i++)
            {
                cmbCats.Items.Add(s[i]);
            }

            if (User.loggedIn == false)
            {
                btnAdd.Visibility = Visibility.Hidden;
            }

            btnPrevPage.Visibility = Visibility.Hidden;
        }

        private void TextBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e);
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new AddAnAd();
        }

        public void UpdateAds()
        {
            for (int i = 0; i < Ads.Children.Count; i++)
            {
               if (Ads.Children[i].GetType().ToString() == "Atestat.Controls.DisplayAdControlH")
                {
                    Ads.Children.RemoveAt(i);
                    i--;
                }
            }

            for (int c = 3, i = (currentPage - 1) * 3 + 1; i <= Math.Min(len, currentPage * 3); i++, c += 2)
            {
                
                DisplayAdControlH newAd = new DisplayAdControlH(ids[i]);

                Ads.Children.Add(newAd);
                Grid.SetRow(newAd, c);
                Grid.SetColumn(newAd, 1);
            }

            txtPage.Text = "Pagina " + currentPage.ToString() + " / " + pages.ToString();
        }

        private void searchBar_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (searchBar.Text == watermark)
            {
                searchBar.Text = "";
                searchBar.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                searchBar.FontWeight = FontWeights.UltraBold;
            }
        }

        private void searchBar_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (searchBar.Text == watermark)
            {
                searchBar.Dispatcher.BeginInvoke(new Action(() => searchBar.CaretIndex = 0), DispatcherPriority.Background);
            }
        }


        public void ChangeAds()
        {
            int id = cmbCats.SelectedIndex;

            if (id == -1)
            {
                return;
            }
            else if (id == 0)
            {
                try
                {
                    Vars.conn.Open();

                    ids = new int[1024];

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = "select * from anunturi";
                    cmd.Connection = Vars.conn;

                    MySqlDataReader r = cmd.ExecuteReader();

                    len = 0;

                    while (r.Read())
                    {
                        ids[++len] = int.Parse(r["id"].ToString());
                    }

                    r.Close();
                    Vars.conn.Close();
                }
                catch
                {
                    if (Vars.conn.State == System.Data.ConnectionState.Open)
                    {
                        Vars.conn.Close();
                    }
                    MessageBox.Show("Eroare");
                }
            }
            else
            {
                try
                {
                    Vars.conn.Open();

                    ids = new int[1024];

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = "select * from anunturi where idCategorie = @idCategorie";
                    cmd.Connection = Vars.conn;

                    cmd.Parameters.AddWithValue("idCategorie", id);

                    MySqlDataReader r = cmd.ExecuteReader();

                    len = 0;

                    while (r.Read())
                    {
                        ids[++len] = int.Parse(r["id"].ToString());
                    }

                    r.Close();
                    Vars.conn.Close();
                }
                catch
                {
                    if (Vars.conn.State == System.Data.ConnectionState.Open)
                    {
                        Vars.conn.Close();
                    }
                    MessageBox.Show("Eroare");
                }
            }

            for (int i = 0; i < Ads.Children.Count; i++)
            {
                if (Ads.Children[i].GetType().ToString() == "Atestat.Controls.DisplayAdControlH")
                {
                    Ads.Children.RemoveAt(i);
                    i--;
                }
            }

            pages = len / 3 + Convert.ToInt32(len % 3 != 0);

            for (int i = 1; i <= Math.Min(3, len); i++)
            {
                DisplayAdControlH newAd = new DisplayAdControlH(ids[i]);

                Ads.Children.Add(newAd);
                Grid.SetRow(newAd, i * 2 + 1);
                Grid.SetColumn(newAd, 1);
            }

            btnPrevPage.Visibility = Visibility.Hidden;
            btnNextPage.Visibility = Visibility.Hidden;

            if (pages > 0)
            {
                txtPage.Text = "Pagina 1 / " + pages.ToString();
            }
            else
            {
                txtPage.Text = "Ne pare rau, dar nu a fost gasit niciun anunt!";
            }

            if (pages > 1)
            {
                btnNextPage.Visibility = Visibility.Visible;
            }

        }

        bool runOnce = false;

        private void cmbCats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (runOnce)
            {
                ChangeAds();
            }

            runOnce = true;

        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            ids = new int[1024];

            try
            {
                len = 1;

                Vars.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Vars.conn;
                
                cmd.CommandText = "select * from anunturi where lower(titlu) like lower(@titlu)";
                cmd.Parameters.AddWithValue("titlu", "%" +  searchBar.Text + "%");
                
                MySqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    ids[len] = int.Parse(r["id"].ToString());

                    if ((r["titlu"].ToString()).ToLower() == searchBar.Text.ToLower())
                    {
                        int tempId = ids[len];
                        ids[len] = ids[1];
                        ids[1] = tempId;
                    }

                    len++;
                }

                len--;

                r.Close();
                Vars.conn.Close();
            }
            catch (Exception ex)
            {
                if (Vars.conn.State == System.Data.ConnectionState.Open)
                {
                    Vars.conn.Close();
                }
                //MessageBox.Show("A aparut o eroare! Daca problema persista va rugam sa contactati un administrator!");
                MessageBox.Show(ex.ToString());
            }

            for (int i = 0; i < Ads.Children.Count; i++)
            {
                if (Ads.Children[i].GetType().ToString() == "Atestat.Controls.DisplayAdControlH")
                {
                    Ads.Children.RemoveAt(i);
                    i--;
                }
            }

            pages = len / 3 + Convert.ToInt32(len % 3 != 0);

            for (int i = 1; i <= Math.Min(3, len); i++)
            {
                DisplayAdControlH newAd = new DisplayAdControlH(ids[i]);

                Ads.Children.Add(newAd);
                Grid.SetRow(newAd, i * 2 + 1);
                Grid.SetColumn(newAd, 1);
            }

            btnPrevPage.Visibility = Visibility.Hidden;
            btnNextPage.Visibility = Visibility.Hidden;

            if (pages > 0)
            {
                txtPage.Text = "Pagina 1 / " + pages.ToString();
            }
            else
            {
                txtPage.Text = "Ne pare rau, dar nu a fost gasit niciun anunt!";
            }

            if (pages > 1)
            {
                btnNextPage.Visibility = Visibility.Visible;
            }

        }

        

        private void searchBar_LostFocus(object sender, RoutedEventArgs e)
        {
            if (searchBar.Text == "")
            {
                searchBar.Text = watermark;
                searchBar.Foreground = new SolidColorBrush(Color.FromRgb(70, 70, 70));
                searchBar.FontWeight = FontWeights.Normal;
            }
        }

        private void Button_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e);
        }


    }
}