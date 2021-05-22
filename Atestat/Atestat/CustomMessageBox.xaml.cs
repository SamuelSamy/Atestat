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
using System.Windows.Shapes;

using Atestat.Classes;

namespace Atestat
{
    public partial class CustomMessageBox : Window
    {
        #region Variables

        public float AnimationTime = .10f;

        public Color color = Color.FromRgb(100, 100, 100);

        public string message = "";
        public SolidColorBrush colorBrush = new SolidColorBrush();

        Window owner;

        #endregion
        public CustomMessageBox(int ColorType, string Message, UserControl ControlParent, MessageBoxButton button)
        {
            InitializeComponent();

            if (button == MessageBoxButton.YesNo)
            {
                YesNoButtons.Visibility = Visibility.Visible;
                OkButton.Visibility = Visibility.Hidden;
            }
            else if (button == MessageBoxButton.OK)
            {
                YesNoButtons.Visibility = Visibility.Hidden;
                OkButton.Visibility = Visibility.Visible;
            }



            switch (ColorType)
            {
                case (int)MessageBoxColorTypes.green:   
                    color.G = 255;

                    Icon.Source = (Resources["Info"] as ImageSource);
                    break;

                case (int)MessageBoxColorTypes.red:     
                    color.R = 255;

                    break;

                case (int)MessageBoxColorTypes.yellow:  
                    color.R = 255;
                    color.G = 255;

                    Icon.Source = (Resources["Qustion"] as ImageSource);
                    break;
            }

            colorBrush = new SolidColorBrush(color);

            textBlock.Text = Message;

            Object e = ControlParent;

            while (!(e is MainWindow))
            {
                e = (e as FrameworkElement).Parent;
            }

            owner = e as MainWindow;

            DoubleAnimation anim = new DoubleAnimation();
            anim.From = 0;
            anim.To = .95;
            anim.Duration = (Duration)TimeSpan.FromSeconds(AnimationTime);

            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        #region CloseFunctions

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        { 
            Border.Background = colorBrush;

            this.Height = owner.ActualHeight / 1.5;
            this.Width = owner.ActualWidth / 1.5;

            this.Left = owner.Left + (owner.ActualWidth - this.ActualWidth) / 2;
            this.Top = owner.Top + (owner.ActualHeight - this.ActualHeight) / 2;


            BlurEffect b = new BlurEffect();
            owner.Effect = b;

            DoubleAnimation anim = new DoubleAnimation();
            anim.From = 0;
            anim.To = 20;
            anim.Duration = (Duration)TimeSpan.FromSeconds(AnimationTime);

            b.BeginAnimation(BlurEffect.RadiusProperty, anim);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            BlurEffect b = new BlurEffect();
            owner.Effect = b;

            DoubleAnimation anim = new DoubleAnimation();
            anim.From = 20;
            anim.To = 0;
            anim.Duration = (Duration)TimeSpan.FromSeconds(AnimationTime);

            b.BeginAnimation(BlurEffect.RadiusProperty, anim);
        }

        private void ControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }
    }
}
