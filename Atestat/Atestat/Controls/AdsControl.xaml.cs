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

namespace Atestat
{
    /// <summary>
    /// Interaction logic for AdsControl.xaml
    /// </summary>
    public partial class AdsControl : UserControl
    {
        public AdsControl()
        {
            InitializeComponent();
        }

        private void Button_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e);
        }
    }
}
