using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Atestat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            sideBarLogin.Visibility = Visibility.Visible;
            sideBarMain.Visibility = Visibility.Hidden;

            gridLogo.Visibility = Visibility.Hidden;
            gridLogin.Visibility = Visibility.Visible;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            sideBarLogin.Visibility = Visibility.Hidden;
            sideBarMain.Visibility = Visibility.Visible;

            gridLogo.Visibility = Visibility.Visible;
            gridLogin.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Checks if the mail address already exists in the database and if the password is correct    - true  -> save user's data
        ///                                                                                             - false -> MessageBox.Show("The mail address is incorect");
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoginReg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Vars.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select * from utilizatori where mail = @mail";
                cmd.Connection = Vars.conn;

                cmd.Parameters.AddWithValue("mail", txtMail.Text);

                MySqlDataReader r = cmd.ExecuteReader();

                if (r.Read() && r["parola"].ToString() == "salut")
                {
                    User.id = int.Parse(r["id"].ToString());
                    User.mail = r["mail"].ToString();
                    User.name = r["nume"].ToString();
                    User.registerDate = DateTime.Parse(r["dataInregistrare"].ToString());
                    User.password = r["parola"].ToString();
                    User.phone = r["nrTelefon"].ToString();
                    User.type = r["tipCont"].ToString();

                    MessageBox.Show("Ati fost autentificat cu succes!");
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
                Vars.conn.Close();
                txtMail.Text = "";
                passBox.Password = "";
                MessageBox.Show("Eroare in procesul de conectare! Daca problema persista, contactati un administrator!\n" + ex.ToString());
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
