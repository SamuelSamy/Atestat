using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Atestat.Controls;

namespace Atestat
{
    public partial class MainControl : UserControl
    {
        public MainControl()
        {
            InitializeComponent();
        }

        #region ControlsEvents


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new LoginControl();
        }

        private void btnAnunturi_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new AdvertisementsPage(-1);
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new HelpControl();
        }

        #endregion

        #region CustomFunctions
        private void ControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e, this);
        }
        #endregion
    }
}
