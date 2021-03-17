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
using MySql.Data.MySqlClient;
using System.Data;

namespace Atestat
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        public LoginControl()
        {
            InitializeComponent();
        }

        bool Connected = false;

        /// <summary>
        /// Checks if the mail address already exists in the database and if the password is correct    - true  -> save user's data
        ///                                                                                             - false -> MessageBox.Show("The mail address is incorect");
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Vars.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select * from utilizatori where mail = @mail";
                cmd.Connection = Vars.conn;

                cmd.Parameters.AddWithValue("mail", txtMail.Text);

                MySqlDataReader r = cmd.ExecuteReader();

                if (r.Read() && r["parola"].ToString() == passBox.Password.ToString())
                {
                    User.id = int.Parse(r["id"].ToString());
                    User.mail = r["mail"].ToString();
                    User.name = r["nume"].ToString();
                    User.registerDate = DateTime.Parse(r["dataI"].ToString());
                    User.password = r["parola"].ToString();
                    User.phone = r["nrTelefon"].ToString();
                    User.type = r["tipCont"].ToString();
                    User.loggedIn = true;

                    MessageBox.Show("Ati fost autentificat cu succes!");


                    Connected = true;
                }
                else
                {
                    txtMail.Text = "";
                    passBox.Password = "";
                    MessageBox.Show("Datele introduse nu sunt corecte!");
                }

                r.Close();

                Vars.conn.Close();
            }
            catch (Exception ex)
            {
                if (Vars.conn.State == ConnectionState.Open)
                    Vars.conn.Close();

                txtMail.Text = "";
                passBox.Password = "";
                MessageBox.Show("Eroare in procesul de conectare! Daca problema persista, contactati un administrator!\n" + ex.ToString());
            }

            if (Connected)
            {
                this.Content = new Controls.MainLogin();
            }
        }



        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            /*BlurEffect b = new BlurEffect();
            this.Effect = b;

            DoubleAnimation anim = new DoubleAnimation();
            anim.From = 0;
            anim.To = 20;
            anim.Duration = (Duration)TimeSpan.FromSeconds(.05);
            anim.AutoReverse = true;

            b.BeginAnimation(BlurEffect.RadiusProperty, anim);*/

            this.Content = new RegisterControl();
     
           
        }

        private void ControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
             Functions.ControlResize(sender, e);
        }

        private void Button_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e);
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Content = new ForgotPasswordControl();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new MainControl();
        }
    }
}
