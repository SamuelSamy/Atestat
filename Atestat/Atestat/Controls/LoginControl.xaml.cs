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
using Atestat.Classes;

namespace Atestat
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        #region Variables
        
        bool Connected = false;

        #endregion

        public LoginControl()
        {
            InitializeComponent();
        }

        #region ControlsEvents


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select * from utilizatori where mail = @mail";
                cmd.Connection = Variables.conn;

                cmd.Parameters.AddWithValue("mail", txtMail.Text);

                MySqlDataReader r = cmd.ExecuteReader();

                if (r.Read() && r["parola"].ToString() == Functions.MD5Hash(passBox.Password.ToString()))
                {
                    ConnectedUser.id = int.Parse(r["id"].ToString());
                    ConnectedUser.mail = r["mail"].ToString();
                    ConnectedUser.name = r["nume"].ToString();
                    ConnectedUser.registerDate = DateTime.Parse(r["dataI"].ToString());
                    ConnectedUser.password = passBox.Password.ToString();
                    ConnectedUser.phone = r["nrTelefon"].ToString();
                    ConnectedUser.loggedIn = true;
                    ConnectedUser.type = int.Parse(r["tipCont"].ToString());

                    CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.green, "Ati fost autentificat cu succes!", this, MessageBoxButton.OK);
                    cmb.ShowDialog();

                    Connected = true;
                }
                else
                {
                    txtMail.Text = "";
                    passBox.Password = "";
                    CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "Datele introduse nu sunt corecte!", this, MessageBoxButton.OK);
                    cmb.ShowDialog();
                }

                r.Close();

                Variables.conn.Close();
            }
            catch (Exception ex)
            {
                if (Variables.conn.State == ConnectionState.Open)
                    Variables.conn.Close();

                txtMail.Text = "";
                passBox.Password = "";
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
            }

            if (Connected)
            {
                this.Content = new MainLogin();
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new RegisterControl(); 
        }

        private void ControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
             Functions.ControlResize(sender, e, this);
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Content = new ForgotPasswordControl();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new MainControl();
        }

        #endregion
    }
}
