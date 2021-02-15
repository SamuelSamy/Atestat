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

namespace Atestat
{
    /// <summary>
    /// Interaction logic for RegisterControl.xaml
    /// </summary>
    public partial class RegisterControl : UserControl
    {
        public RegisterControl()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AlreadyHasAccount_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BlurEffect b = new BlurEffect();
            this.Effect = b;

            DoubleAnimation anim = new DoubleAnimation();
            anim.From = 0;
            anim.To = 20;
            anim.Duration = (Duration)TimeSpan.FromSeconds(.05);
            anim.AutoReverse = true;

            b.BeginAnimation(BlurEffect.RadiusProperty, anim);

            this.Content = new LoginControl();
        }
    }
}
