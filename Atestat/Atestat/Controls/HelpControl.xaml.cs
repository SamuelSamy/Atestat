﻿using System;
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

namespace Atestat.Controls
{
    /// <summary>
    /// Interaction logic for HelpControl.xaml
    /// </summary>
    public partial class HelpControl : UserControl
    {
        public HelpControl()
        {
            InitializeComponent();
        }

        #region ControlEvents

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (ConnectedUser.loggedIn)
            {
                this.Content = new MainLogin();
            }
            else
            {
                this.Content = new MainControl();
            }
        }

        private void btnTutorial_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnContact_Click(object sender, RoutedEventArgs e)
        {
            CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.green, "Something to test because i wanna see how this goes", this);
            cmb.Show();
        }
        private void Button_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e);
        }

        #endregion
    }
}
