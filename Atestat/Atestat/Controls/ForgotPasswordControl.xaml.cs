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
        public string password1 = "", password2 = "";

        #endregion
        public ForgotPasswordControl()
        {
            InitializeComponent();
        }

        #region ControlsEvents

        private void txtPass_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox txtPass = sender as TextBox;

            int caretPos = txtPass.CaretIndex;

            string password = "", textbox = "0";

            if ((textbox = txtPass.Name.Substring(txtPass.Name.Length - 1, 1)) == "1")
            {
                password = password1;
            }
            else
            {
                password = password2;
            }

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
            }

            if (e.Key == Key.Back)
            {
                if (txtPass.SelectionLength != 0)
                {
                    password = password.Remove(txtPass.SelectionStart, txtPass.SelectionLength);
                }
                else if (caretPos != 0)
                {
                    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    {
                        password = password.Remove(0, caretPos);
                    }
                    else
                    {
                        password = password.Remove(caretPos - 1, 1);
                    }
                }
            }

            if (e.Key == Key.Delete && caretPos != password.Length)
            {
                if (txtPass.SelectionLength != 0)
                {
                    password = password.Remove(txtPass.SelectionStart, txtPass.SelectionLength);
                }
                else if (caretPos != password.Length)
                {
                    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    {
                        password = password.Remove(caretPos, password.Length - caretPos);
                    }
                    else
                    {
                        password = password.Remove(caretPos, 1);
                    }
                }

            }

            if (e.Key == Key.Space)
            {
                password = password.Insert(caretPos, " ");
                txtPass.Text = txtPass.Text.Insert(caretPos, "•");
                txtPass.CaretIndex = caretPos + 1;
                e.Handled = true;
            }

            if (textbox == "1")
            {
                password1 = password;
            }
            else
            {
                password2 = password;
            }

            CheckPasswords(password, txtPass, txtPass.CaretIndex);
        }

        private void txtPass_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox txtPass = sender as TextBox;

            int caretPos = txtPass.CaretIndex;

            string key = e.Text;
            string password = "";

            if (txtPass.Name.Substring(txtPass.Name.Length - 1, 1) == "1")
            {
                password1 = password1.Insert(caretPos, key);
                password = password1;

            }
            else
            {
                password2 = password2.Insert(caretPos, key);
                password = password2;
            }

            e.Handled = true;
            txtPass.Text = txtPass.Text.Insert(caretPos, "•");
            txtPass.CaretIndex = caretPos + 1;

            CheckPasswords(password, txtPass, txtPass.CaretIndex);

        }

        public void CheckPasswords(string password, TextBox txtPass, int caretPos)
        {
            string s1 = txtPass1.Text;
            string s2 = txtPass2.Text;

            if (IsValidPassword(password) < (int)PasswordRequirements.All)
            {
                txtPass.Style = (Resources["CustomTextBoxWrong"] as Style);
            }
            else
            {
                txtPass1.Style = (Resources["CustomTextBox"] as Style);
                txtPass2.Style = (Resources["CustomTextBox"] as Style);
            }

            txtPass1.Text = s1;
            txtPass2.Text = s2;
            txtPass.CaretIndex = caretPos;


        }
        private void ControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e, this);
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
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "Va rugam sa completati toate casutele pentru text!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
                return;
            }

            if (IsValidPassword(password1) != (int)PasswordRequirements.All)
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "Parola introdusa nu respecta criteriile!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
                return;
            }

            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Variables.conn;
                cmd.CommandText = "update utilizatori set parola = @parola where mail = @mail";

                cmd.Parameters.AddWithValue("parola", Functions.MD5Hash(password1));
                cmd.Parameters.AddWithValue("mail", mailAdress);

                if (cmd.ExecuteNonQuery() != 0)
                {
                    CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.green, "Parola dumneavoastra a fost modificata cu succes!", this, MessageBoxButton.OK);
                    cmb.ShowDialog();
                    this.Content = new MainControl();
                }
                else
                {
                    CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                    cmb.ShowDialog();
                }
                Variables.conn.Close();
            }
            catch
            {
                if (Variables.conn.State == ConnectionState.Open)
                    Variables.conn.Close();
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (Functions.AdressAlreadyInDataBase(txtMail.Text) == 0)
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "Adresa de mail introdusa nu exista!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
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
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
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
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "Codul introdus nu corespunde cu cel trimis la adresa dumneavoastra!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
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
            return (!String.IsNullOrEmpty(password1) && !String.IsNullOrEmpty(password2));
        }

        private int IsValidPassword(string password, bool match = true)
        {
            int Value = (int)PasswordRequirements.None;

            if (password1 == password2 && match)
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
