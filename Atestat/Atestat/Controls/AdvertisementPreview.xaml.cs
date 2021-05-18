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
using System.Windows.Media.Animation;
using MySql.Data.MySqlClient;

namespace Atestat.Controls
{
    public partial class AdvertisementPreview : UserControl
    {
        #region Variables

        public int adID = 0, page, fromControl;

        public string searchBarText;

        public bool ButtonPressed = false, isFav = false;

        Advertisement ad;

        #endregion

        public AdvertisementPreview(int adID, string searchBarText, int page, int fromCotnrol)
        {
            InitializeComponent();

            this.searchBarText = searchBarText;
            this.adID = adID;
            this.page = page;
            this.fromControl = fromCotnrol;

            ad = new Advertisement(adID);

            LoadImagePreview();
            ToggleRemoveButton();
            ToggleFavoriteButton();

            if (fromControl == (int)ControlTypes.AdminPanel)
            {
                RemoveImage.Visibility = Visibility.Hidden;
                Favorite.Visibility = Visibility.Hidden;
            }
        }

        #region ControlsEvents
        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            Color newColor = (Color)Resources["White"];
            newColor.R *= 2;
            newColor.G *= 2;
            newColor.B *= 2;

            ColorAnimation animation;
            animation = new ColorAnimation();
            animation.From = (Color)Resources["White"];
            animation.To = newColor;
            animation.Duration = new Duration(TimeSpan.FromSeconds(.2));
            this.Border.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }
        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            Color newColor = (Color)Resources["White"];
            newColor.R *= 2;
            newColor.G *= 2;
            newColor.B *= 2;

            ColorAnimation animation;
            animation = new ColorAnimation();
            animation.From = newColor;
            animation.To = (Color)Resources["White"];
            animation.Duration = new Duration(TimeSpan.FromSeconds(.2));
            this.Border.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            DeleteGrid.RowDefinitions[1].Height = new GridLength(10, GridUnitType.Star);
            DeleteGrid.ColumnDefinitions[1].Width = new GridLength(10, GridUnitType.Star);

            ButtonEffect.ShadowDepth = 5;
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            DeleteGrid.RowDefinitions[1].Height = new GridLength(.7, GridUnitType.Star);
            DeleteGrid.ColumnDefinitions[1].Width = new GridLength(.7, GridUnitType.Star);

            ButtonEffect.ShadowDepth = 2;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ButtonPressed = true;

            /// delete from database and update list

            if (fromControl == (int)ControlTypes.UserPanel)
            {
                UserControl uc = ((((this.Parent as Grid).Parent as Grid).Parent as Grid).Parent as UserControl);
                UserPanel up = (uc as UserPanel);

                if (ad.DeleteFromDataBase())
                {
                    up.ads.RemoveAt(up.ads.FindIndex(x => x == adID));
                    up.ResetAds();

                    MessageBox.Show("Anunt sters cu succes!");
                }
                else
                {
                    MessageBox.Show("A aparut o eroare");
                }

            }
            else if (fromControl == (int)ControlTypes.AdvertisementsPage)
            {
                UserControl uc = (((this.Parent as Grid).Parent as Grid).Parent as UserControl);
                AdvertisementsPage ac = (uc as AdvertisementsPage);

                if (ad.DeleteFromDataBase())
                {
                    ac.adIds.RemoveAt(ac.adIds.FindIndex(x => x == adID));
                    ac.ResetAds();
                    MessageBox.Show("Anunt sters cu succes!");
                }
                else
                {
                    MessageBox.Show("A aparut o eroare");
                }
            }

        }



        private void Favorite_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ButtonPressed = true;

            if (isFav)
            {
                /// delete from database

                try
                {
                    Variables.conn.Open();

                    MySqlCommand cmd = new MySqlCommand();


                    cmd.CommandText = "delete from anunturi_favorite where idUtilizator = @idUtilizator and idAnunt = @idAnunt";
                    cmd.Connection = Variables.conn;

                    cmd.Parameters.AddWithValue("idUtilizator", ConnectedUser.id);
                    cmd.Parameters.AddWithValue("idAnunt", adID);

                    if (cmd.ExecuteNonQuery() != 0)
                    {
                        Variables.conn.Close();

                        if (fromControl == (int)ControlTypes.AdvertisementsPage)
                        {
                            UserControl uc = ((this.Parent as Grid).Parent as Grid).Parent as UserControl;
                            AdvertisementsPage ap = uc as AdvertisementsPage;


                            if (ap.cmbCats.SelectedIndex == (int)Categories.Favorite && ConnectedUser.loggedIn)
                            {
                                ap.adIds.RemoveAt(ap.adIds.FindIndex(x => x == adID));
                                ap.ResetAds();
                            }
                        }



                        Favorite.Source = (Resources["NotFavorite"] as ImageSource);

                        isFav = false;
                    }

                }
                catch
                {
                    if (Variables.conn.State == System.Data.ConnectionState.Open)
                    {
                        Variables.conn.Close();
                    }

                    MessageBox.Show("Eroare");
                }
            }
            else
            {
                /// isert  into database

                try
                {
                    Variables.conn.Open();

                    MySqlCommand cmd = new MySqlCommand();


                    cmd.CommandText = "insert into anunturi_favorite (idUtilizator, idAnunt) values (@idUtilizator, @idAnunt)";
                    cmd.Connection = Variables.conn;

                    cmd.Parameters.AddWithValue("idUtilizator", ConnectedUser.id);
                    cmd.Parameters.AddWithValue("idAnunt", adID);

                    if (cmd.ExecuteNonQuery() != 0)
                    {
                        Variables.conn.Close();

                        isFav = true;
                        Favorite.Source = (Resources["Favorite"] as ImageSource);
                    }

                }
                catch (Exception ex)
                {
                    if (Variables.conn.State == System.Data.ConnectionState.Open)
                    {
                        Variables.conn.Close();
                    }

                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void Favorite_MouseEnter(object sender, MouseEventArgs e)
        {

            FavGrid.RowDefinitions[1].Height = new GridLength(10, GridUnitType.Star);
            FavGrid.ColumnDefinitions[1].Width = new GridLength(10, GridUnitType.Star);

            if (isFav)
            {
                Favorite.Source = (Resources["NotFavorite"] as ImageSource);

            }
            else
            {
                Favorite.Source = (Resources["Favorite"] as ImageSource);
            }

        }

        private void Favorite_MouseLeave(object sender, MouseEventArgs e)
        {
            FavGrid.RowDefinitions[1].Height = new GridLength(.7, GridUnitType.Star);
            FavGrid.ColumnDefinitions[1].Width = new GridLength(.7, GridUnitType.Star);

            if (isFav)
            {
                Favorite.Source = (Resources["Favorite"] as ImageSource);

            }
            else
            {
                Favorite.Source = (Resources["NotFavorite"] as ImageSource);
            }
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ButtonPressed)
            {
                ButtonPressed = false;
                return;
            }

            if (fromControl == (int)ControlTypes.UserPanel)
            {
                /// Show Editable ad

                ((((this.Parent as Grid).Parent as Grid).Parent as Grid).Parent as UserControl).Content = new AddAdvertisementPage(true, adID, page);
            }
            else if (fromControl == (int)ControlTypes.AdvertisementsPage)
            {
                /// Show ViewOnly ad for normal user

                (((this.Parent as Grid).Parent as Grid).Parent as AdvertisementsPage).Content = new AdvertisementView(adID, page, searchBarText);
            }
            else if (fromControl == (int)ControlTypes.AdminPanel)
            {
                /// Show ViewOnly ad for admin

                ((((this.Parent as Grid).Parent as Grid).Parent as Grid).Parent as UserControl).Content = new AdvertisementView(adID, page, searchBarText, true);

            }
        }
        #endregion

        #region CustomFunctions
        public void ToggleRemoveButton()
        {
            if (ad.idUser == ConnectedUser.id || ConnectedUser.type >= (int)UserTypes.Admin)
            {
                RemoveImage.Visibility = Visibility.Visible;
            }
        }        

        public void ToggleFavoriteButton()
        {
            if (ConnectedUser.loggedIn)
            {
                Favorite.Visibility = Visibility.Visible;

                if (isFav = IsMarkedAsFavorite())
                {
                    Favorite.Source = (Resources["Favorite"] as ImageSource);
                }
                else
                {
                    Favorite.Source = (Resources["NotFavorite"] as ImageSource);
                }
            }
        }
        public void LoadImagePreview()
        {
            string path = Variables.debugPath + "Assets\\Images\\" + adID.ToString() + "_0";

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

            AdImage.Source = im;
            AdTitle.Text = ad.title;
            AdPrice.Text = ad.price.ToString() + " Lei";
        }

        public bool IsMarkedAsFavorite()
        {
            bool isFav = false;

            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();


                cmd.CommandText = "select * from anunturi_favorite where idUtilizator = @idUtilizator and idAnunt = @idAnunt";
                cmd.Connection = Variables.conn;

                cmd.Parameters.AddWithValue("idUtilizator", ConnectedUser.id);
                cmd.Parameters.AddWithValue("idAnunt", adID);

                MySqlDataReader r = cmd.ExecuteReader();

                if (r.Read())
                {
                    isFav = true;
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

                MessageBox.Show("Eroare");
            }

            return isFav;
        }

        #endregion
    }
}
