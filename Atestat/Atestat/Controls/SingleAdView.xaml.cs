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

namespace Atestat.Controls
{
    /// <summary>
    /// Interaction logic for SingleAdView.xaml
    /// </summary>
    public partial class SingleAdView : UserControl
    {

        public string titlu, descriere, searchBarText;
        public int idVanzator, pret, nrImagini, idCategorie, idAnunt, page = -1, currentImage = 0;
        public DateTime data;

        public SingleAdView(int id, int page, string searchBarText)
        {
            InitializeComponent();

            this.page = page;
            this.searchBarText = searchBarText;
            SelectAd(id);
        }

        public void SelectAd(int id)
        {
            try
            {
                Vars.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select * from anunturi where id = @id";
                cmd.Connection = Vars.conn;

                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters["id"].DbType = DbType.Int32;

                MySqlDataReader r = cmd.ExecuteReader();
                
                if (r.Read())
                {
                    titlu = r["titlu"].ToString();
                    descriere = r["descriere"].ToString();
                    idVanzator = int.Parse(r["idUtilizator"].ToString());
                    pret = int.Parse(r["pret"].ToString());
                    nrImagini = int.Parse(r["nrImagini"].ToString());
                    idCategorie = int.Parse(r["idCategorie"].ToString());
                    idAnunt = id;

                    
                }
                else
                {
                    MessageBox.Show("Nu a fost gasit anuntul!");
                }

                r.Close();
                Vars.conn.Close();

                Complete();
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

        public void Complete()
        {
            txtTtile.Text = titlu;
            txtDesc.Text = descriere;
            txtPrice.Text = pret.ToString();
            txtCategory.Text = SelectCategory();
            SelectUser();
            UploadPhotos();
        }

        public void UpdateImage()
        {
            string path = Vars.debugPath + "\\Assets\\Images\\" + idAnunt.ToString() + "_" + currentImage.ToString();

            if (File.Exists(path + ".png"))
            {
                path += ".png";
            }
            else
            {
                path += ".jpg";
            }

            BigImage.Source = new BitmapImage(new Uri(path));
        }


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
            if (currentImage < nrImagini)
            {
                currentImage++; 
            }

            PrevImage.Visibility = Visibility.Visible;
            NextImage.Visibility = Visibility.Visible;

            if (currentImage == nrImagini - 1)
            {
                NextImage.Visibility = Visibility.Hidden;
            }
            UpdateImage();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect b = new BlurEffect();
            MainView.Effect = b;

            DoubleAnimation anim = new DoubleAnimation();
            anim.From = 20;
            anim.To = 0;
            anim.Duration = (Duration)TimeSpan.FromSeconds(.1);

            DoubleAnimation opacityAnimation = new DoubleAnimation();

            opacityAnimation.From = 1;
            opacityAnimation.To = 0;
            opacityAnimation.Duration = (Duration)TimeSpan.FromSeconds(.1);

            ImageView.BeginAnimation(OpacityProperty, opacityAnimation);
            b.BeginAnimation(BlurEffect.RadiusProperty, anim);

            Panel.SetZIndex(ImageView, 0);
        }

        private void Image1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Panel.SetZIndex(ImageView, 2);
            
            BlurEffect b = new BlurEffect();
            MainView.Effect = b;

            DoubleAnimation anim = new DoubleAnimation();
            anim.From = 0;
            anim.To = 20;
            anim.Duration = (Duration)TimeSpan.FromSeconds(.1);

            DoubleAnimation opacityAnimation = new DoubleAnimation();

            opacityAnimation.From = 0;
            opacityAnimation.To = 1;
            opacityAnimation.Duration = (Duration)TimeSpan.FromSeconds(.1);
            
            ImageView.BeginAnimation(OpacityProperty, opacityAnimation);
            b.BeginAnimation(BlurEffect.RadiusProperty, anim);

            string path = Vars.debugPath + "\\Assets\\Images\\" + idAnunt.ToString() + "_0";

            if (File.Exists(path + ".png"))
            {
                path += ".png";
            }
            else
            {
                path += ".jpg";
            }

            PrevImage.Visibility = Visibility.Hidden;

            if (nrImagini == 1)
            {
                NextImage.Visibility = Visibility.Hidden;
            }

            BigImage.Source = new BitmapImage(new Uri(path));
        }
        
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new AdsControlH(page, searchBarText);
        }

        public string SelectCategory()
        {
            string category = "";

            try
            {
                Vars.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select * from categorii where id = @id";
                cmd.Connection = Vars.conn;

                cmd.Parameters.AddWithValue("id", idCategorie);

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
                Vars.conn.Close();
            }
            catch (Exception ex)
            {
                if (Vars.conn.State == ConnectionState.Open)
                {
                    Vars.conn.Close();
                }
                MessageBox.Show(ex.ToString());
            }

            return category;
        }

        public void SelectUser()
        {
            try
            {
                Vars.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select * from utilizatori where id = @id";
                cmd.Connection = Vars.conn;

                cmd.Parameters.AddWithValue("id", idVanzator);

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
                Vars.conn.Close();
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

        public void UploadPhotos()
        {
            for (int i = 0; i < nrImagini; i++)
            {
                string path = Vars.debugPath + "\\Assets\\Images\\" + idAnunt.ToString() + "_" + i.ToString();

                if (File.Exists(path + ".png"))
                {
                    path += ".png";
                }
                else
                {
                    path += ".jpg";
                }

                (Images.Children[i] as Image).Source = new BitmapImage(new Uri(path));
            }
            
        }

        #region ResizeFunctions
        private void ControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e);
        }

        
        private void Button_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e);
        }
        #endregion
    }
}
