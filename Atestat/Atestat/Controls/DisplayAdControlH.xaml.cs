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

namespace Atestat.Controls
{
    /// <summary>
    /// Interaction logic for DisplayAdControlH.xaml
    /// </summary>
    public partial class DisplayAdControlH : UserControl
    {
        public int adID = 0, page;
        public string searchBarText;

        public const string key = "This Ad Is From The User Page And It Should Redirect Him To The User Panel ▬↓-:`┘y";
        public DisplayAdControlH(int adID, string searchBarText, int adPage = -1)
        {
            InitializeComponent();

            this.searchBarText = searchBarText;

            Ad newAd = new Ad(adID);


            string path = Vars.debugPath + "\\Assets\\Images\\" + adID.ToString() + "_0";

            if (File.Exists(path + ".png"))
            {
                path += ".png";
            }
            else
            {
                path += ".jpg";
            }

            AdImage.Source = new BitmapImage(new Uri(path));
            AdTitle.Text = newAd.title;
            AdPrice.Text = newAd.price.ToString() + " Lei";

            this.adID = adID;

            if (searchBarText == key)
            {
                page = adPage;
            }
            else
            {
                int len = Vars.adIds.FindIndex(x => x == adID);
                page = len / 3 + 1;
            }
        }

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

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (searchBarText == key)
            {
                /// Show Editable ad

                ((((this.Parent as Grid).Parent as Grid).Parent as Grid).Parent as UserControl).Content = new AddAnAd(true, adID, page);
            }
            else
            {
                /// Show ViewOnly ad
                
                (((this.Parent as Grid).Parent as Grid).Parent as AdsControlH).Content = new SingleAdView(adID, page, searchBarText);
            }
        }
    }
}
