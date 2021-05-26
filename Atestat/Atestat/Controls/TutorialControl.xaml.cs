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
using System.IO;

using Atestat.Classes;

namespace Atestat.Controls
{
    /// <summary>
    /// Interaction logic for TutorialControl.xaml
    /// </summary>
    public partial class TutorialControl : UserControl
    {
        int currentPage = 0, totalPages = 0;
        string[] Descs = new string[64];
        string[] Titles = new string[64];
        string DirectoryPath = Variables.debugPath + "\\Assets\\Tutorial\\";
        public TutorialControl()
        {
            InitializeComponent();

            Descs = File.ReadAllLines(DirectoryPath + "Descriere.txt");
            Titles = File.ReadAllLines(DirectoryPath + "Titluri.txt");
            totalPages = Descs.Length;
            
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new HelpControl();
        }

        private void ControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e, this);
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPages - 1)
            {
                currentPage++;
            }
            UpdateImgAndDesc();
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 0)
            {
                currentPage--;
            }
            UpdateImgAndDesc();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateImgAndDesc();
        }

        public void UpdateImgAndDesc()
        {
            CurrentDescription.Text = Descs[currentPage];
            WindowName.Text = Titles[currentPage];

            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            string ImagePath = DirectoryPath + currentPage.ToString();
            img.UriSource = new Uri(ImagePath + GetExtenstion(ImagePath));
            img.EndInit();

            CurrentImage.ImageSource = img;

            img.UriSource = null;
            img = new BitmapImage();


            btnPrevPage.Visibility = Visibility.Visible;
            btnNextPage.Visibility = Visibility.Visible;

            if (currentPage == 0)
            {
                btnPrevPage.Visibility = Visibility.Hidden;
            }

            if (currentPage == totalPages - 1 && totalPages > 0)
            {
                btnNextPage.Visibility = Visibility.Hidden;
            }
        }

        public string GetExtenstion(string path)
        {
            string extenstion = "";

            if (File.Exists(path + ".png"))
            {
                extenstion = ".png";
            }
            else
            {
                extenstion  = ".jpg";
            }

            return extenstion;
        }
    }


}
