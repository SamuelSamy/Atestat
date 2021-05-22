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
    public partial class AdvertisementsPage : UserControl
    {
        #region Variables
        public const string watermark = "Cauta un anunt...";

        public static string[] titluri = new string[1024];

        public int pages = 0, currentPage, len = 0, lenCats = 0;

        public int[] catIds = new int[64];

        public bool hasText = false;

        public List<int> adIds = new List<int>();

        #endregion
        public AdvertisementsPage(int page, string searchBarText = watermark)
        {
            InitializeComponent();
            currentPage = page;

            if (searchBarText != watermark)
            {
                searchBar.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                searchBar.FontWeight = FontWeights.UltraBold;
            }

            searchBar.Text = searchBarText;

            GetAdvertisements();
        }

        #region ControlsEvents
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (currentPage == -1)
            {
                currentPage = 1;
            }

            UpdateAds();

            string[] s = SelectCategories();

            for (int i = 0; i <= lenCats; i++)
            {
                cmbCats.Items.Add(s[i]);
            }

            if (ConnectedUser.loggedIn == false)
            {
                btnAdd.Visibility = Visibility.Hidden;
            }
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
            if (ConnectedUser.loggedIn == true)
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
            this.Content = new AddAdvertisementPage(false);
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

        private void cmbCats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (runOnce)
            {
                searchBar.Text = watermark;
                searchBar.Foreground = new SolidColorBrush(Color.FromRgb(70, 70, 70));
                searchBar.FontWeight = FontWeights.Normal;
                ChangeAds();
                currentPage = 1;
                UpdateAds();
            }

            runOnce = true;

        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            adIds.Clear();

            if (searchBar.Text == watermark)
            {
                hasText = false;
                try
                {
                    Variables.conn.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = Variables.conn;

                    if (cmbCats.SelectedIndex == (int)Categories.None || cmbCats.SelectedIndex == (int)Categories.All)
                    {
                        cmd.CommandText = "select * from anunturi";
                    }
                    else if (cmbCats.SelectedIndex == (int)Categories.Favorite)
                    {
                        cmd.CommandText = "select * from anunturi inner join anunturi_favorite on anunturi.id = anunturi_favorite.idAnunt where anunturi_favorite.idUtilizator = @idUtilizator";
                        cmd.Parameters.AddWithValue("idUtilizator", ConnectedUser.id);
                    }
                    else
                    {
                        cmd.CommandText = "select * from anunturi where idCategorie = @idCategorie";
                        cmd.Parameters.AddWithValue("idCategorie", catIds[cmbCats.SelectedIndex]);
                    }

                    MySqlDataReader r = cmd.ExecuteReader();

                    while (r.Read())
                    {
                        adIds.Add(int.Parse(r["id"].ToString()));

                        if ((r["titlu"].ToString()).ToLower() == searchBar.Text.ToLower())
                        {
                            int tempId = adIds[adIds.Count - 1];
                            adIds[adIds.Count - 1] = adIds[0];
                            adIds[0] = tempId;
                        }
                    }

                    r.Close();
                    Variables.conn.Close();
                }
                catch (Exception ex)
                {
                    if (Variables.conn.State == System.Data.ConnectionState.Open)
                    {
                        Variables.conn.Close();
                    }
                    CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                    cmb.ShowDialog();
                }
            }
            else
            {
                hasText = true;
                try
                {
                    Variables.conn.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = Variables.conn;

                    if (cmbCats.SelectedIndex == (int)Categories.None || cmbCats.SelectedIndex == (int)Categories.All)
                    {
                        cmd.CommandText = "select * from anunturi where lower(titlu) like lower(@titlu)";
                    }
                    else if (cmbCats.SelectedIndex == (int)Categories.Favorite)
                    {
                        cmd.CommandText = "select * from anunturi inner join anunturi_favorite on anunturi_favorite.idAnunt = anunturi.id where lower(anunturi.titlu) like lower(@titlu) and anunturi_favorite.idUtilizator = @idUtilizator";
                        cmd.Parameters.AddWithValue("idUtilizator", ConnectedUser.id);
                    }
                    else
                    {
                        cmd.CommandText = "select * from anunturi where lower(titlu) like lower(@titlu) and idCategorie = @idCategorie";
                        cmd.Parameters.AddWithValue("idCategorie", catIds[cmbCats.SelectedIndex]);

                    }

                    cmd.Parameters.AddWithValue("titlu", "%" + searchBar.Text + "%");

                    MySqlDataReader r = cmd.ExecuteReader();

                    while (r.Read())
                    {
                        adIds.Add(int.Parse(r["id"].ToString()));

                        if ((r["titlu"].ToString()).ToLower() == searchBar.Text.ToLower())
                        {
                            int tempId = adIds[adIds.Count - 1];
                            adIds[adIds.Count - 1] = adIds[0];
                            adIds[0] = tempId;
                        }

                        len++;
                    }


                    r.Close();
                    Variables.conn.Close();
                }
                catch (Exception ex)
                {
                    if (Variables.conn.State == System.Data.ConnectionState.Open)
                    {
                        Variables.conn.Close();
                    }
                    CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                    cmb.ShowDialog();
                }
            }

            currentPage = 1;
            UpdateAds();

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

        #endregion

        #region CustomFunctions
        public void GetAdvertisements()
        {
            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select id from anunturi";
                cmd.Connection = Variables.conn;

                MySqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    adIds.Add(int.Parse(r["id"].ToString()));
                }

                r.Close();
                Variables.conn.Close();
            }
            catch (Exception ex)
            {
                if (Variables.conn.State == System.Data.ConnectionState.Open)
                {
                    Variables.conn.Close();
                }
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
            }
        }

        public string[] SelectCategories()
        {
            string[] cats = new string[1024];

            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select * from categorii";
                cmd.Connection = Variables.conn;

                MySqlDataReader r = cmd.ExecuteReader();

                int len = 0;

                catIds[len] = -1;
                cats[len] = "Toate categoriile";

                len++;

                if (ConnectedUser.loggedIn)
                {
                    cats[len] = "Anunturi favorite";
                    catIds[len] = 0;
                    len++;
                }


                while (r.Read())
                {
                    catIds[len] = int.Parse(r["id"].ToString());
                    cats[len] = r["nume"].ToString();
                    len++;
                }

                lenCats = len;

                r.Close();
                Variables.conn.Close();
            }
            catch
            {
                if (Variables.conn.State == System.Data.ConnectionState.Open)
                {
                    Variables.conn.Close();
                }
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
            }

            return cats;
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

            len = adIds.Count;
            pages = len / 3 + Convert.ToInt32(len % 3 != 0);

            if (len == 0)
            {
                currentPage = 0;
                txtPage.Text = "Nu am gasit niciun anunt publicat!";
                btnNextPage.Visibility = btnPrevPage.Visibility = Visibility.Hidden;
            }
            else
            {
                for (int c = 3, i = (currentPage - 1) * 3; i <= Math.Min(len - 1, currentPage * 3 - 1); i++, c += 2)
                {
                    string text = searchBar.Text;

                    if (hasText == false)
                    {
                        text = watermark;
                    }

                    AdvertisementPreview newAd = new AdvertisementPreview(adIds[i], text, currentPage, (int)ControlTypes.AdvertisementsPage);

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




        public void ChangeAds()
        {
            int comboBoxId = cmbCats.SelectedIndex;

            if (comboBoxId == (int)Categories.None)
            {
                return;
            }
            else if (comboBoxId == (int)Categories.All)
            {
                try
                {
                    Variables.conn.Open();

                    adIds.Clear();

                    MySqlCommand cmd = new MySqlCommand();


                    cmd.CommandText = "select * from anunturi";
                    cmd.Connection = Variables.conn;

                    MySqlDataReader r = cmd.ExecuteReader();

                    while (r.Read())
                    {
                        adIds.Add(int.Parse(r["id"].ToString()));
                    }

                    r.Close();
                    Variables.conn.Close();
                }
                catch
                {
                    if (Variables.conn.State == System.Data.ConnectionState.Open)
                    {
                        Variables.conn.Close();
                    }
                    CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                    cmb.ShowDialog();
                }
            }
            else if (ConnectedUser.loggedIn && comboBoxId == (int)Categories.Favorite)
            {
                try
                {
                    Variables.conn.Open();

                    adIds.Clear();

                    MySqlCommand cmd = new MySqlCommand();


                    cmd.CommandText = "select * from anunturi inner join anunturi_favorite on anunturi.id = anunturi_favorite.idAnunt where anunturi_favorite.idUtilizator = @idUtilizator";
                    cmd.Connection = Variables.conn;

                    cmd.Parameters.AddWithValue("idUtilizator", ConnectedUser.id);

                    MySqlDataReader r = cmd.ExecuteReader();

                    while (r.Read())
                    {
                        adIds.Add(int.Parse(r["id"].ToString()));
                    }

                    r.Close();
                    Variables.conn.Close();
                }
                catch (Exception ex)
                {
                    if (Variables.conn.State == System.Data.ConnectionState.Open)
                    {
                        Variables.conn.Close();
                    }
                    CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                    cmb.ShowDialog();
                }
            }
            else
            {
                try
                {
                    Variables.conn.Open();

                    adIds.Clear();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = "select * from anunturi where idCategorie = @idCategorie";
                    cmd.Connection = Variables.conn;


                    cmd.Parameters.AddWithValue("idCategorie", catIds[comboBoxId]);

                    MySqlDataReader r = cmd.ExecuteReader();

                    len = 0;

                    while (r.Read())
                    {
                        adIds.Add(int.Parse(r["id"].ToString()));
                    }

                    r.Close();
                    Variables.conn.Close();
                }
                catch
                {
                    if (Variables.conn.State == System.Data.ConnectionState.Open)
                    {
                        Variables.conn.Close();
                    }
                    CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                    cmb.ShowDialog();
                }
            }
        }

        bool runOnce = false;


        public void ResetAds()
        {
            len = adIds.Count;
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
        private void ControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e, this);
        }

        #endregion
    }
}