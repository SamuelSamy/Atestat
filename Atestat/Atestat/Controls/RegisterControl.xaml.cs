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

namespace Atestat
{
    /// <summary>
    /// Interaction logic for RegisterControl.xaml
    /// </summary>
    /// 

    [Flags]
    public enum PasswordRequirements
    {
        None    = 0b_0000_0000, /// None
        Match   = 0b_0000_0001, /// The passwords match
        Length  = 0b_0000_0010, /// The password has at least 8 characters
        Digit   = 0b_0000_0100, /// The passwrod has at least one digit
        Capital = 0b_0000_1000, /// The password has at least on capital letter
        All     = 0b_0000_1111  /// All the requirements 
    }    
    public partial class RegisterControl : UserControl
    {
        public RegisterControl()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!AllAreCompleted())
            {
                MessageBox.Show("Va rugam sa completati toate casutele!");
                return;
            }

            if (!IsValidAddress(txtMail.Text))
            {
                MessageBox.Show("Adresa de mail introdusa nu este valida!");
                return;
            }

            if (Functions.AdressAlreadyInDataBase(txtMail.Text))
            {
                MessageBox.Show("Adresa de mail exista deja!");
                return;
            }
            
            if (IsValidPassword(passBox.Password) != (int)PasswordRequirements.All)
            {
                MessageBox.Show("Parola introdusa nu respecta criteriile!");
                return;
            }

            try
            {
                Vars.conn.Open();


                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Vars.conn;
                cmd.CommandText = "insert into utilizatori (nume, mail, dataI, nrTelefon, parola, tipCont) values (@nume, @mail, @dataI, @nrTelefon, @parola, @tipCont)";

                cmd.Parameters.AddWithValue("nume", txtName.Text);
                cmd.Parameters.AddWithValue("mail", txtMail.Text);
                cmd.Parameters.AddWithValue("dataI", DateTime.Now);
                cmd.Parameters.AddWithValue("nrTelefon", "-");
                cmd.Parameters.AddWithValue("parola", passBox.Password);
                cmd.Parameters.AddWithValue("tipCont", "utilizator");

                cmd.Parameters["nume"].DbType       = DbType.String;
                cmd.Parameters["mail"].DbType       = DbType.String;
                cmd.Parameters["dataI"].DbType      = DbType.Date;
                cmd.Parameters["nrTelefon"].DbType  = DbType.String;
                cmd.Parameters["parola"].DbType     = DbType.String;
                cmd.Parameters["tipCont"].DbType    = DbType.String;

                if (cmd.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("Datele au fost adaugate cu succes!");
                    User.loggedIn = true;
                    this.Content = new MainLogin();
                }
                else
                {
                    MessageBox.Show("A aparut o eroare la crearea contului!");
                }

                Vars.conn.Close();
            }
            catch (Exception ex)
            {
                if (Vars.conn.State == ConnectionState.Open)
                {
                    Vars.conn.Close();
                }

                MessageBox.Show(ex.ToString());
            }
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



        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns>
        ///    nothing, 8+ chars, digit, capital
        ///    0000, 0001, ..., 1111
        ///    first bit    -> passwords match
        ///    second bit   -> 8+ chars
        ///    third bit    -> digit
        ///    fourth bit   -> capital
        /// </returns>
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

        /// <summary>
        /// Checks if all the TextBoxes and PasswordBoxes are filled
        /// </summary>
        /// <returns></returns>
        public bool AllAreCompleted()
        {
            return (!String.IsNullOrEmpty(txtName.Text) && !String.IsNullOrEmpty(txtMail.Text) && !String.IsNullOrEmpty(passBox.Password));            
        }

        private void AlreadyHasAccount_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /*BlurEffect b = new BlurEffect();
            this.Effect = b;

            DoubleAnimation anim = new DoubleAnimation();
            anim.From = 0;
            anim.To = 20;
            anim.Duration = (Duration)TimeSpan.FromSeconds(.05);
            anim.AutoReverse = true;

            b.BeginAnimation(BlurEffect.RadiusProperty, anim);*/

            this.Content = new LoginControl();
        }

        private void btnRegister_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e);
        }

        private void txtName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char last = e.Text[e.Text.Length - 1];

            if (!Char.IsLetter(last) && !last.Equals(' '))
            {
                e.Handled = true;
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

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtName.Text.Trim().Length < 3)
            {
                string text = txtName.Text;
                txtName.Style = (Resources["CustomTextBoxWrong"] as Style);
                txtName.Text = text;
                txtName.CaretIndex = text.Length;
            }
            else
            {
                string text = txtName.Text;
                txtName.Style = (Resources["CustomTextBox"] as Style);
                txtName.Text = text;
                txtName.CaretIndex = text.Length;
            }
        }


        private void passBox_KeyDown(object sender, KeyEventArgs e)
        {
            /*if (IsValidPassword(passBox.Password.ToString(), false) < (int)PasswordRequirements.Capital + (int)PasswordRequirements.Digit + (int)PasswordRequirements.Length)
            {
                string text = passBox.Password.ToString();
                passBox.Style = (Resources["CustomPasswordBoxWrong"] as Style);
                passBox.Password = text;
  

            }
            else
            {
                string text = passBox.Password.ToString();
                passBox.Style = (Resources["CustomPasswordBox"] as Style);
                passBox.Password = text;
            }*/
        }

        private void Button_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e);
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new MainControl();
        }
    }
}
