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
using System.Data;
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Reflection;
using Atestat.Classes;

namespace Atestat
{
    public partial class RegisterControl : UserControl
    {
        public RegisterControl()
        {
            InitializeComponent();
        }

        public string password1 = "", password2 = "";

        #region ControlsEvents

        #region PasswordBoxEvents
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
                    int index = txtPass.SelectionStart;
                    password = password.Remove(index, txtPass.SelectionLength);
                    txtPass.Text = txtPass.Text.Remove(index, txtPass.SelectionLength);
                    txtPass.CaretIndex = index;
                }
                else if (caretPos != 0)
                {
                    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    {
                        password = password.Remove(0, caretPos);
                        txtPass.Text = txtPass.Text.Remove(0, caretPos);
                        txtPass.CaretIndex = 0;
                    }
                    else
                    {
                        password = password.Remove(caretPos - 1, 1);
                        txtPass.Text = txtPass.Text.Remove(caretPos - 1, 1);
                        txtPass.CaretIndex = caretPos - 1;
                    }
                }

                e.Handled = true;

            }

            if (e.Key == Key.Delete && caretPos != password.Length)
            {
                if (txtPass.SelectionLength != 0)
                {
                    int index = txtPass.SelectionStart;
                    password = password.Remove(index, txtPass.SelectionLength);
                    txtPass.Text = txtPass.Text.Remove(index, txtPass.SelectionLength);
                    txtPass.CaretIndex = index;
                }
                else if (caretPos != password.Length)
                {
                    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    {
                        password = password.Remove(caretPos, password.Length - caretPos);
                        txtPass.Text = txtPass.Text.Remove(caretPos, txtPass.Text.Length - caretPos);
                        txtPass.CaretIndex = 0;
                    }
                    else
                    {
                        password = password.Remove(caretPos, 1);
                        txtPass.Text = txtPass.Text.Remove(caretPos, 1);
                        txtPass.CaretIndex = caretPos;
                    }
                }
                e.Handled = true;

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
            string s1  = txtPass1.Text;
            string s2 = txtPass2.Text;
            int value = 0;

            if ((value = IsValidPassword(password)) < (int)PasswordRequirements.All)
            {
                txtPass.Style = (Resources["CustomTextBoxWrong"] as Style);

                if (txtPass.Name.Substring(txtPass.Name.Length - 1, 1) == "1")
                {
                    PasswordToolTip1.Visibility = Visibility.Visible;
                }
                else
                {
                    PasswordToolTip2.Visibility = Visibility.Visible;
                }

            }
            else
            {
                PasswordToolTip1.Visibility = Visibility.Hidden;
                PasswordToolTip2.Visibility = Visibility.Hidden;

                txtPass1.Style = (Resources["CustomTextBox"] as Style);
                txtPass2.Style = (Resources["CustomTextBox"] as Style);
            }

            txtPass1.Text = s1;
            txtPass2.Text = s2;
            txtPass.CaretIndex = caretPos;
        }

        #endregion
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!AllAreCompleted())
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "Va rugam sa completati toate casutele pentru text!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
                return;
            }

            if (txtName.Text.Trim().Length < 3)
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "Va rugam sa introduceti un nume valid!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
                return;
            }

            if (!IsValidAddress(txtMail.Text))
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "Va rugam sa introduceti o adresa de mail valida!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
                return;
            }

            if (Functions.AdressAlreadyInDataBase(txtMail.Text) != 0)
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "Adresa de mail introdusa exista deja!", this, MessageBoxButton.OK);
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
                cmd.CommandText = "insert into utilizatori (nume, mail, dataI, nrTelefon, parola, tipCont) values (@nume, @mail, @dataI, @nrTelefon, @parola, @tipCont)";

                cmd.Parameters.AddWithValue("nume", txtName.Text);
                cmd.Parameters.AddWithValue("mail", txtMail.Text);
                cmd.Parameters.AddWithValue("dataI", DateTime.Now);
                cmd.Parameters.AddWithValue("nrTelefon", "-");
                cmd.Parameters.AddWithValue("parola", Functions.MD5Hash(password1));
                cmd.Parameters.AddWithValue("tipCont", "utilizator");

                cmd.Parameters["nume"].DbType = DbType.String;
                cmd.Parameters["mail"].DbType = DbType.String;
                cmd.Parameters["dataI"].DbType = DbType.Date;
                cmd.Parameters["nrTelefon"].DbType = DbType.String;
                cmd.Parameters["parola"].DbType = DbType.String;
                cmd.Parameters["tipCont"].DbType = DbType.String;

                if (cmd.ExecuteNonQuery() != 0)
                {
                    CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.green, "Contul dumneavoastra a fost inregistrat cu succes!", this, MessageBoxButton.OK);
                    cmb.ShowDialog();
                    ConnectedUser.loggedIn = true;
                    ConnectedUser.mail = txtMail.Text;
                    ConnectedUser.password = password1;
                    ConnectedUser.name = txtName.Text;
                    ConnectedUser.registerDate = DateTime.Now;

                    cmd.CommandText = "select @@Identity";
                    ulong id = (ulong)cmd.ExecuteScalar();

                    ConnectedUser.id = (int)id;

                    this.Content = new MainLogin();
                }
                else
                {
                    CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o problema la creearea contului! Va rugam sa incercati din nou!", this, MessageBoxButton.OK);
                    cmb.ShowDialog();
                }

                Variables.conn.Close();
            }
            catch (Exception ex)
            {
                if (Variables.conn.State == ConnectionState.Open)
                {
                    Variables.conn.Close();
                }

                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o problema la creearea contului! Va rugam sa incercati din nou!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
            }
        }

        private void AlreadyHasAccount_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Content = new LoginControl();
        }

        #region TextNameEvents
        private void txtName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char last = e.Text[e.Text.Length - 1];

            if (!Char.IsLetter(last) && !last.Equals(' '))
            {
                e.Handled = true;
            }
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            int caretPos = txtName.CaretIndex;
            string text = txtName.Text;

            if (txtName.Text.Trim().Length < 3)
            {        
                txtName.Style = (Resources["CustomTextBoxWrong"] as Style);
                NameToolTip.Visibility = Visibility.Visible;
            }
            else
            {
                txtName.Style = (Resources["CustomTextBox"] as Style);
                NameToolTip.Visibility = Visibility.Hidden;
            }

            txtName.Text = text;
            txtName.CaretIndex = caretPos;

        }

        #endregion

        #region TextMailEvents
        private void txtMail_TextChanged(object sender, TextChangedEventArgs e)
        {
            int caretPos = txtMail.CaretIndex;
            string mail = txtMail.Text;

            if (txtMail.Text.Length <= 1 || !IsValidAddress(txtMail.Text))
            {
                txtMail.Style = (Resources["CustomTextBoxWrong"] as Style);
                MailToolTip.Visibility = Visibility.Visible;
            }
            else
            {
                txtMail.Style = (Resources["CustomTextBox"] as Style);
                MailToolTip.Visibility = Visibility.Hidden;
            }

            txtMail.Text = mail;
            txtMail.CaretIndex = caretPos;
        }

        #endregion


        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new MainControl();
        }

        #endregion

        #region CustomFunctions

        private void SetSelection(PasswordBox passwordBox, int start, int length)
        {
            passwordBox.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(passwordBox, new object[] { start, length });
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
        private int IsValidPassword(string password, bool match = true)
        {
            int Value = (int)PasswordRequirements.None;

            if (password1 == password2)
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

        public bool AllAreCompleted()
        {
            return (!String.IsNullOrEmpty(txtName.Text) && !String.IsNullOrEmpty(txtMail.Text) && !String.IsNullOrEmpty(password1) && !String.IsNullOrEmpty(password2));
        }

        private void ControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e, this);
        }

        #endregion

        
    }
}
