using System;
using System.Collections.Generic;
using System.Net.Mail;
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
    /// Interaction logic for ForogtPasswordControl.xaml
    /// </summary>
    public partial class ForgotPasswordControl : UserControl
    {
        public ForgotPasswordControl()
        {
            InitializeComponent();
        }

        private void Button_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e);
        }

        private void ControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e);
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new MainControl();
        }

        private bool IsValidAddress(string mail)
        {
            try
            {
                MailAddress address = new MailAddress(mail);
                return address.Address == mail;
            }
            catch
            {
                return false;
            }
        }

        private void txtMail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtMail.Text.Length <= 1 || !IsValidAddress(txtMail.Text))
            {
                string mail = txtMail.Text;
                txtMail.Style = (Resources["CustomTextBoxWrong"] as Style);
                txtMail.Text = mail;
                txtMail.CaretIndex = txtMail.Text.Length;
            }
            else
            {
                string mail = txtMail.Text;
                txtMail.Style = (Resources["CustomTextBox"] as Style);
                txtMail.Text = mail;
                txtMail.CaretIndex = txtMail.Text.Length;
            }
        }
    }
}
