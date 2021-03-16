using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Atestat
{
    class Functions
    {
      
        public static void ControlResize(object sender, SizeChangedEventArgs e)
        {
            //double scale = (e.NewSize.Width * e.NewSize.Width + e.NewSize.Height * e.NewSize.Height) / (e.PreviousSize.Width * e.PreviousSize.Width + e.PreviousSize.Height * e.PreviousSize.Height);
            double scale = e.NewSize.Width / e.PreviousSize.Width;

            if (scale != double.PositiveInfinity && scale != 0)
            {
                switch (sender.GetType().Name)
                {
                    case "TextBox":
                        (sender as TextBox).FontSize   *= scale;
                        break;

                    case "Button":
                        (sender as Button).FontSize    *= scale;
                        break;

                    case "TextBlock":
                        (sender as TextBlock).FontSize *= scale;
                        break;
                }
            }
        }
    }
}
