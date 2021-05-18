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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Atestat.Classes;

namespace Atestat
{
    public partial class CustomMessageBox : Window
    {
        #region Variables

        public Color color = Color.FromRgb(100, 100, 100);

        public string message = "";
        public SolidColorBrush colorBrush = new SolidColorBrush();

        UserControl ControlParent;

        #endregion
        public CustomMessageBox(int ColorType, string Message, UserControl ControlParent)
        {
            InitializeComponent();

            switch (ColorType)
            {
                case (int)MessageBoxColorTypes.green: color.G = 100;
                    return;

                case (int)MessageBoxColorTypes.red: color.R = 100;
                    return;

                case (int)MessageBoxColorTypes.yellow: color.B = 100;
                    return;
            }

            colorBrush = new SolidColorBrush(color);

            TextBlock.Text = Message;

            this.ControlParent = ControlParent;
        }

        #region SelectAnswer

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

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        { 
            Border.Background = colorBrush;
            TextBlock.Text = message;

            this.Height = ControlParent.ActualHeight / 1.5;
            this.Width = ControlParent.ActualWidth / 1.5;

            this.Left = Owner.Left + (ControlParent.ActualWidth - this.ActualWidth) / 2;
            this.Top = Owner.Top + (ControlParent.ActualHeight - this.ActualHeight) / 2;

            DoubleAnimation anim = new DoubleAnimation();
            anim.From = 0;
            anim.To = 0.75;
            anim.Duration = (Duration)TimeSpan.FromSeconds(.3);

            this.BeginAnimation(UIElement.OpacityProperty, anim);            
        }

        #region ResizeFunctions

        private void btnYes_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Size n = e.NewSize;
            Size o = e.PreviousSize;

            double l = n.Width / o.Width;

            if (l != double.PositiveInfinity)
            {
                btnYes.FontSize *= l;
            }
        }

        private void btnNo_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Size n = e.NewSize;
            Size o = e.PreviousSize;

            double l = n.Width / o.Width;

            if (l != double.PositiveInfinity)
            {
                btnNo.FontSize *= l;
            }
        }

        private void TextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Size n = e.NewSize;
            Size o = e.PreviousSize;

            double l = n.Width / o.Width;

            if (l != double.PositiveInfinity)
            {
                TextBlock.FontSize *= l;
            }
        }

        #endregion
    }
}
