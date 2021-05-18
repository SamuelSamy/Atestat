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
using Atestat.Controls;
using Atestat.Classes;

namespace Atestat
{
    /// <summary>
    /// Interaction logic for MainLogin.xaml
    /// </summary>
    public partial class MainLogin : UserControl
    {
        public MainLogin()
        {
            InitializeComponent();

            if (ConnectedUser.type >= (int)UserTypes.Admin)
            {
                btnHelp.Content = "Setari";
            }
        }

        #region ControlsEvents

        private void Button_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e);
        }

        private void btnAnunturi_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new AdvertisementsPage(-1);
        }

        private void btnAccount_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new UserPanel();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            if (ConnectedUser.type >= (int)UserTypes.Admin)
            {
                this.Content = new SettingsControl();
            }
            else
            {
                this.Content = new HelpControl();
            }
        }

        #endregion
    }
}
