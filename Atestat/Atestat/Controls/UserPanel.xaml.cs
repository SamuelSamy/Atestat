using System;
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
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Media.Animation;

using Atestat.Classes;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Atestat.Controls
{
    public partial class UserPanel : UserControl
    {
        #region Variables
        public List<int> ads = new List<int>();

        public int len, pages = 0, currentPage = 1;
        public bool btnProfileEnabled = true, btnAdsEnabled = true;

        public const string key = "This Ad Is From The User Page And It Should Redirect Him To The User Panel ▬↓-:`┘y";

        string password1 = "";

        #endregion
        public UserPanel(int adPage = -1)
        {
            InitializeComponent();


            Profile.Visibility = Visibility.Hidden;
            Ads.Visibility = Visibility.Hidden;

            if (adPage != -1)
            {
                gridLogo.Visibility = Visibility.Hidden;

                btnAdsEnabled = false;
                btnAds.Background = new SolidColorBrush((Color)Resources["DarkBlue"]);
                Ads.Visibility = Visibility.Visible;

                SelectAds();
                currentPage = adPage;
                UpdateAds();
            }
        }

        #region ControlsEvents

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            ///Show personal data

            if (btnProfileEnabled)
            {
                Ads.Visibility = Visibility.Hidden;
                Profile.Visibility = Visibility.Visible;
                gridLogo.Visibility = Visibility.Hidden;
                ChangeProfile.Visibility = Visibility.Hidden;

                ColorAnimation c = new ColorAnimation();

                c.From = (Color)Resources["LightBlue"];
                c.To = (Color)Resources["DarkBlue"];
                c.Duration = new Duration(TimeSpan.FromSeconds(.15));

                ColorAnimation normalC = new ColorAnimation();

                normalC.From = (Color)Resources["DarkBlue"];
                normalC.To = (Color)Resources["LightBlue"];
                normalC.Duration = new Duration(TimeSpan.FromSeconds(.15));

                if (!btnAdsEnabled)
                {
                    btnAds.Background = new SolidColorBrush((Color)Resources["DarkBlue"]);
                    btnAds.Background.BeginAnimation(SolidColorBrush.ColorProperty, normalC);
                }

                btnProfile.Background = new SolidColorBrush((Color)Resources["LightBlue"]);
                btnProfile.Background.BeginAnimation(SolidColorBrush.ColorProperty, c);

                btnAdsEnabled = true;
                btnProfileEnabled = false;

                name.Content = ConnectedUser.name;
                mail.Content = ConnectedUser.mail;
                date.Content = ConnectedUser.registerDate;
                phoneNo.Content = ConnectedUser.phone;
                password1 = ConnectedUser.password;

                int ads = getAds();

                if (ads == -1)
                {
                    adsCount.Content = "404 Ads not found";
                }
                else
                {
                    adsCount.Content = ads;
                }
            }
        }

        private void btnAnunturi_Click(object sender, RoutedEventArgs e)
        {
            ///Load personal ads

            if (btnAdsEnabled)
            {
                Ads.Visibility = Visibility.Visible;
                Profile.Visibility = Visibility.Hidden;
                gridLogo.Visibility = Visibility.Hidden;
                ChangeProfile.Visibility = Visibility.Hidden;

                ColorAnimation ca = new ColorAnimation();

                ca.From = (Color)Resources["LightBlue"];
                ca.To = (Color)Resources["DarkBlue"];
                ca.Duration = new Duration(TimeSpan.FromSeconds(.15));

                ColorAnimation normalC = new ColorAnimation();

                normalC.From = (Color)Resources["DarkBlue"];
                normalC.To = (Color)Resources["LightBlue"];
                normalC.Duration = new Duration(TimeSpan.FromSeconds(.15));

                if (!btnProfileEnabled)
                {
                    btnProfile.Background = new SolidColorBrush((Color)Resources["DarkBlue"]);
                    btnProfile.Background.BeginAnimation(SolidColorBrush.ColorProperty, normalC);
                }

                btnAds.Background = new SolidColorBrush((Color)Resources["LightBlue"]);
                btnAds.Background.BeginAnimation(SolidColorBrush.ColorProperty, ca);



                SelectAds();
                currentPage = 1;
                UpdateAds();

                btnProfileEnabled = true;
                btnAdsEnabled = false;
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            ConnectedUser.id = -1;
            ConnectedUser.name = ConnectedUser.mail = ConnectedUser.password = ConnectedUser.phone = "";
            ConnectedUser.type = 0;
            ConnectedUser.loggedIn = false;

            CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.green, "Ati fost deconectat cu succes!", this, MessageBoxButton.OK);
            cmb.ShowDialog();

            this.Content = new MainControl();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new MainLogin();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /// change ui

            txtName.Text = ConnectedUser.name;
            txtMail.Text = ConnectedUser.mail;
            txtNumber.Text = ConnectedUser.phone;
            txtPassword.Text = ConnectedUser.password;

            ChangeProfile.Visibility = Visibility.Visible;
            Profile.Visibility = Visibility.Hidden;
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < pages)
            {
                currentPage++;
                UpdateAds();
            }

            btnPrevPage.Visibility = Visibility.Visible;

            if (currentPage == pages)
            {
                btnNextPage.Visibility = Visibility.Hidden;
            }
            else
            {
                btnNextPage.Visibility = Visibility.Visible;
            }
        }

        private void UpdateProfile(object sender, RoutedEventArgs e)
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

            int addresID = Functions.AdressAlreadyInDataBase(txtMail.Text);

            if (addresID != 0 && addresID != ConnectedUser.id)
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
                cmd.CommandText = "update utilizatori set nume = @nume, mail = @mail, nrTelefon = @nrTelefon, parola = @parola where id = @id";

                cmd.Parameters.AddWithValue("nume", txtName.Text);
                cmd.Parameters.AddWithValue("mail", txtMail.Text);
                cmd.Parameters.AddWithValue("nrTelefon", txtNumber.Text);
                cmd.Parameters.AddWithValue("parola", Functions.MD5Hash(txtPassword.Text));
                cmd.Parameters.AddWithValue("id", ConnectedUser.id);

                if (cmd.ExecuteNonQuery() != 0)
                {
                    ConnectedUser.name = txtName.Text;
                    ConnectedUser.mail = txtMail.Text;
                    ConnectedUser.phone = txtNumber.Text;
                    ConnectedUser.password = txtPassword.Text;

                    CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.green, "Datele dumneavoastra au fost modificate cu succes!", this, MessageBoxButton.OK);
                    cmb.ShowDialog();

                    ChangeProfile.Visibility = Visibility.Hidden;
                    Profile.Visibility = Visibility.Visible;

                    name.Content = ConnectedUser.name;
                    mail.Content = ConnectedUser.mail;
                    phoneNo.Content = ConnectedUser.phone;
                }

                Variables.conn.Close();
            }
            catch
            {
                if (Variables.conn.State == ConnectionState.Open)
                {
                    Variables.conn.Close();
                }

                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            Profile.Visibility = Visibility.Visible;
            ChangeProfile.Visibility = Visibility.Hidden;

            name.Content = ConnectedUser.name;
            mail.Content = ConnectedUser.mail;
            phoneNo.Content = ConnectedUser.phone;
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                UpdateAds();
            }

            btnNextPage.Visibility = Visibility.Visible;

            if (currentPage == 1)
            {
                btnPrevPage.Visibility = Visibility.Hidden;
            }
            else
            {
                btnPrevPage.Visibility = Visibility.Visible;
            }
        }

        private void ControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e, this);
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Char.IsDigit((char)((int)e.Key + 14)) && e.Key != Key.Back)
            {
                e.Handled = true;
            }
        }

        #endregion

        #region CustomFunctions
        public void SelectAds()
        {
            ads.Clear();

            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Variables.conn;
                cmd.CommandText = "select * from anunturi where idUtilizator = @idUtilizator";

                cmd.Parameters.AddWithValue("idUtilizator", ConnectedUser.id);

                MySqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    ads.Add(int.Parse(r["id"].ToString()));
                }

                r.Close();
                Variables.conn.Close();
            }
            catch
            {
                if (Variables.conn.State == ConnectionState.Open)
                {
                    Variables.conn.Close();
                }
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
            }
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            int caretPos = txtName.CaretIndex;
            string text = txtName.Text;

            if (txtName.Text.Trim().Length < 3)
            {
                txtName.Style = (Resources["CustomTextBoxWrong"] as Style);
            }
            else
            {
                txtName.Style = (Resources["CustomTextBox"] as Style);
            }

            txtName.Text = text;
            txtName.CaretIndex = caretPos;

        }

        private void txtMail_TextChanged(object sender, TextChangedEventArgs e)
        {
            int caretPos = txtMail.CaretIndex;
            string mail = txtMail.Text;

            if (txtMail.Text.Length <= 1 || !IsValidAddress(txtMail.Text))
            {
                txtMail.Style = (Resources["CustomTextBoxWrong"] as Style);
            }
            else
            {
                txtMail.Style = (Resources["CustomTextBox"] as Style);
            }

            txtMail.Text = mail;
            txtMail.CaretIndex = caretPos;
        }

        private void txtPass_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox txtPass = sender as TextBox;

            int caretPos = txtPass.CaretIndex;

            string password = password1;

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

            password1 = password;

            CheckPasswords(password, txtPass, txtPass.CaretIndex);
        }

        private void txtPass_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox txtPass = sender as TextBox;

            int caretPos = txtPass.CaretIndex;

            string key = e.Text;
            string password = "";

            password1 = password1.Insert(caretPos, key);
            password = password1;

            e.Handled = true;
            txtPass.Text = txtPass.Text.Insert(caretPos, key);
            txtPass.CaretIndex = caretPos + 1;

            CheckPasswords(password, txtPass, txtPass.CaretIndex);

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

        public void CheckPasswords(string password, TextBox txtPass, int caretPos)
        {
            string s1 = txtPassword.Text;

            if (IsValidPassword(password) < (int)PasswordRequirements.All)
            {
                txtPass.Style = (Resources["CustomTextBoxWrong"] as Style);
            }
            else
            {
                txtPass.Style = (Resources["CustomTextBox"] as Style);
            }

            txtPass.Text = s1;
            txtPass.CaretIndex = caretPos;
        }

        public int getAds()
        {
            int ads = 0;

            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Variables.conn;
                cmd.CommandText = "select * from anunturi where idUtilizator = @idUtilizator";

                cmd.Parameters.AddWithValue("idUtilizator", ConnectedUser.id);

                MySqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    ads++;
                }

                r.Close();
                Variables.conn.Close();
            }
            catch
            {
                if (Variables.conn.State == ConnectionState.Open)
                {
                    Variables.conn.Close();
                }

                ads = -1;
            }

            return ads;
        }

        private int IsValidPassword(string password, bool match = true)
        {
            int Value = (int)PasswordRequirements.None;

            Value |= (int)PasswordRequirements.Match;

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

        public void UpdateAds()
        {
            for (int i = 0; i < Ads.Children.Count; i++)
            {
                if (Ads.Children[i].GetType() == typeof(AdvertisementPreview))
                {
                    Ads.Children.RemoveAt(i);
                    i--;
                }
            }

            len = ads.Count;
            pages = len / 3 + Convert.ToInt32(len % 3 != 0);

            if (len == 0)
            {
                currentPage = 0;
                txtPage.Text = "Nu am gasit niciun anunt publicat!";
                gridLogo.Visibility = Visibility.Visible;
            }
            else
            {
                for (int c = 1, i = (currentPage - 1) * 3; i <= Math.Min(len - 1, currentPage * 3 - 1); i++, c += 2)
                {
                    AdvertisementPreview newAd = new AdvertisementPreview(ads[i], key, currentPage, (int)ControlTypes.UserPanel);

                    Ads.Children.Add(newAd);
                    Grid.SetRow(newAd, c);
                    Grid.SetColumn(newAd, 1);
                }

                txtPage.Text = "Pagina " + currentPage.ToString() + " / " + pages.ToString();
            }

            btnPrevPage.Visibility = Visibility.Visible;
            btnNextPage.Visibility = Visibility.Visible;

            if (currentPage == 1)
            {
                btnPrevPage.Visibility = Visibility.Hidden;
            }
            if (currentPage == pages && pages > 0)
            {
                btnNextPage.Visibility = Visibility.Hidden;
            }
            if (currentPage == 0)
            {
                btnPrevPage.Visibility = Visibility.Hidden;
                btnNextPage.Visibility = Visibility.Hidden;
            }

        }

        public void ResetAds()
        {
            len = ads.Count;
            pages = len / 3 + Convert.ToInt32(len % 3 != 0);

            if (len % 3 == 0 && currentPage == pages + 1)
            {
                currentPage--;
            }

            UpdateAds();


            btnNextPage.Visibility = Visibility.Hidden;

            if (pages > 1)
            {
                btnNextPage.Visibility = Visibility.Visible;
            }

            if (len != 0)
            {
                txtPage.Text = "Pagina " + currentPage.ToString() + " / " + pages.ToString();
            }
        }

        public bool AllAreCompleted()
        {
            return (!String.IsNullOrEmpty(txtName.Text) && !String.IsNullOrEmpty(txtMail.Text) && !String.IsNullOrEmpty(password1) && !String.IsNullOrEmpty(phoneNo.Content.ToString()));
        }

        #endregion
    }
}
