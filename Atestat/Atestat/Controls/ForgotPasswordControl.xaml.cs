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
using System.Net;
using System.IO;
using System.Data;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using Atestat.Classes;

namespace Atestat
{

    public partial class ForgotPasswordControl : UserControl
    {
        #region Variables

        public string mailAdress = "";
        public string code = "";

        #endregion
        public ForgotPasswordControl()
        {
            InitializeComponent();
        }

        #region ControlsEvents
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

        private void passBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            if (!AllAreCompleted())
            {
                MessageBox.Show("Va rugam sa completati toate casutele!");
                return;
            }

            if (IsValidPassword(passBox.Password) != (int)PasswordRequirements.All)
            {
                MessageBox.Show("Parola introdusa nu respecta criteriile!");
                return;
            }

            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Variables.conn;
                cmd.CommandText = "update utilizatori set parola = @parola where mail = @mail";

                cmd.Parameters.AddWithValue("parola", passBox.Password);
                cmd.Parameters.AddWithValue("mail", mailAdress);

                if (cmd.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("Parola dumneavostra a fost schimbata cu succes!");
                    this.Content = new MainControl();
                }
                else
                {
                    MessageBox.Show("A aparut o eroare! Daca problema persista contactati un administrator!");
                }
                Variables.conn.Close();
            }
            catch
            {
                if (Variables.conn.State == ConnectionState.Open)
                    Variables.conn.Close();

                MessageBox.Show("A aparut o eroare! Daca problema persista contactati un administrator!");
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (Functions.AdressAlreadyInDataBase(txtMail.Text) == false)
            {
                MessageBox.Show("Aceasta adresa nu exista!");
                return;
            }

            mailAdress = txtMail.Text;

            if (SendMail() == true)
            {
                grid1.Visibility = Visibility.Hidden;
                grid2.Visibility = Visibility.Visible;
            }
            else
            {
                mailAdress = txtMail.Text = "";
                MessageBox.Show("A aparut o eroare! Daca problema persista contactati un administrator!");
            }
        }

        private void btnNext2_Click(object sender, RoutedEventArgs e)
        {
            if (txtCode.Text == code)
            {
                grid2.Visibility = Visibility.Hidden;
                grid3.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Codul introdus nu este valid!");
            }

        }

        #endregion

        #region CustomFunctions
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

        
        public bool AllAreCompleted()
        {
            return (!String.IsNullOrEmpty(passBox.Password) && !String.IsNullOrEmpty(passBoxConf.Password));
        }

        private int IsValidPassword(string password, bool match = true)
        {
            int Value = (int)PasswordRequirements.None;

            if (password == passBoxConf.Password && match)
            {
                Value |= (int)PasswordRequirements.Match;
            }

            if (password.Length > 7)
            {
                Value |= (int)PasswordRequirements.Length;
            }

            Regex r = new Regex(@"[0-9]");

            if (r.Matches(password).Count > 0)
            {
                Value |= (int)PasswordRequirements.Digit;
            }

            r = new Regex(@"[A-Z]");

            if (r.Matches(password).Count > 0)
            {
                Value |= (int)PasswordRequirements.Capital;
            }

            return Value;
        }

        public string GenerateCode()
        {
            string code = "";

            Random r = new Random();

            for (int i = 0; i < 8; i++)
            {
                code += r.Next(0, 10).ToString();
            }

            return code;
        }


        public bool SendMail()
        { 
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(Variables.MailAdress);
                mail.To.Add(mailAdress);
                mail.Subject = "Recuperare parola";
                code = GenerateCode();
                mail.Body = "Codul pentru recuperarea parolei dumneavostra este: " + code;


                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(Variables.MailAdress, Variables.MailPass);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
