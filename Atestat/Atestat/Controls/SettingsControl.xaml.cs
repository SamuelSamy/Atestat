using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Atestat.Classes;
using MySql.Data.MySqlClient;

namespace Atestat.Controls
{
    public partial class SettingsControl : UserControl
    {
        #region Variables

        bool btnReportsEnabled = true, btnCatEnabled = true, btnAdminsEnabled = true;

        List<Admin> admins = new List<Admin>();
        List<Category> categories = new List<Category>();

        public List<int> ads = new List<int>();
        int currentPage = 1, len = 0, pages = 0;

        #endregion
        public SettingsControl(bool DisplayReports = false, int currentPage = 1)
        {
            InitializeComponent();

            SetUp(DisplayReports, currentPage);
        }

        #region ControlsEvents
        private void btnAdmini_Click(object sender, RoutedEventArgs e)
        {
            if (btnAdminsEnabled)
            {
                ColorAnimation c = new ColorAnimation();

                c.From = (Color)Resources["LightBlue"];
                c.To = (Color)Resources["DarkBlue"];
                c.Duration = new Duration(TimeSpan.FromSeconds(.15));

                ColorAnimation normalC = new ColorAnimation();

                normalC.From = (Color)Resources["DarkBlue"];
                normalC.To = (Color)Resources["LightBlue"];
                normalC.Duration = new Duration(TimeSpan.FromSeconds(.15));

                if (!btnCatEnabled)
                {
                    btnCats.Background = new SolidColorBrush((Color)Resources["DarkBlue"]);
                    btnCats.Background.BeginAnimation(SolidColorBrush.ColorProperty, normalC);
                }

                if (!btnReportsEnabled)
                {
                    btnReports.Background = new SolidColorBrush((Color)Resources["DarkBlue"]);
                    btnReports.Background.BeginAnimation(SolidColorBrush.ColorProperty, normalC);
                }

                btnAdmins.Background = new SolidColorBrush((Color)Resources["LightBlue"]);
                btnAdmins.Background.BeginAnimation(SolidColorBrush.ColorProperty, c);

                btnAdminsEnabled = false;
                btnReportsEnabled = true;
                btnCatEnabled = true;
                gridLogo.Visibility = Visibility.Hidden;

                gridLogo.Visibility = Visibility.Hidden;
                Admins.Visibility = Visibility.Visible;
                AddAdmin.Visibility = Visibility.Hidden;
                gridAds.Visibility = Visibility.Hidden;
                gridCats.Visibility = Visibility.Hidden;
                gridAddCategory.Visibility = Visibility.Hidden;
            }
        }

        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            if (btnReportsEnabled)
            {
                ColorAnimation c = new ColorAnimation();

                c.From = (Color)Resources["LightBlue"];
                c.To = (Color)Resources["DarkBlue"];
                c.Duration = new Duration(TimeSpan.FromSeconds(.15));

                ColorAnimation normalC = new ColorAnimation();

                normalC.From = (Color)Resources["DarkBlue"];
                normalC.To = (Color)Resources["LightBlue"];
                normalC.Duration = new Duration(TimeSpan.FromSeconds(.15));

                if (!btnAdminsEnabled)
                {
                    btnAdmins.Background = new SolidColorBrush((Color)Resources["DarkBlue"]);
                    btnAdmins.Background.BeginAnimation(SolidColorBrush.ColorProperty, normalC);
                }

                if (!btnCatEnabled)
                {
                    btnCats.Background = new SolidColorBrush((Color)Resources["DarkBlue"]);
                    btnCats.Background.BeginAnimation(SolidColorBrush.ColorProperty, normalC);
                }

                btnReports.Background = new SolidColorBrush((Color)Resources["LightBlue"]);
                btnReports.Background.BeginAnimation(SolidColorBrush.ColorProperty, c);

                btnReportsEnabled = false;
                btnAdminsEnabled = true;
                btnCatEnabled = true;
                gridLogo.Visibility = Visibility.Hidden;

                gridLogo.Visibility = Visibility.Hidden;
                Admins.Visibility = Visibility.Hidden;
                AddAdmin.Visibility = Visibility.Hidden;
                gridAds.Visibility = Visibility.Visible;
                gridCats.Visibility = Visibility.Hidden;
                gridAddCategory.Visibility = Visibility.Hidden;

                SelectAds();
                currentPage = 1;
                UpdateAds();
            }
        }

        private void btnCats_click(object sender, RoutedEventArgs e)
        {
            if (btnCatEnabled)
            {
                ColorAnimation c = new ColorAnimation();

                c.From = (Color)Resources["LightBlue"];
                c.To = (Color)Resources["DarkBlue"];
                c.Duration = new Duration(TimeSpan.FromSeconds(.15));

                ColorAnimation normalC = new ColorAnimation();

                normalC.From = (Color)Resources["DarkBlue"];
                normalC.To = (Color)Resources["LightBlue"];
                normalC.Duration = new Duration(TimeSpan.FromSeconds(.15));

                if (!btnAdminsEnabled)
                {
                    btnAdmins.Background = new SolidColorBrush((Color)Resources["DarkBlue"]);
                    btnAdmins.Background.BeginAnimation(SolidColorBrush.ColorProperty, normalC);
                }

                if (!btnReportsEnabled)
                {
                    btnReports.Background = new SolidColorBrush((Color)Resources["DarkBlue"]);
                    btnReports.Background.BeginAnimation(SolidColorBrush.ColorProperty, normalC);
                }

                btnCats.Background = new SolidColorBrush((Color)Resources["LightBlue"]);
                btnCats.Background.BeginAnimation(SolidColorBrush.ColorProperty, c);

                btnCatEnabled = false;
                btnAdminsEnabled = true;
                btnReportsEnabled = true;

                gridLogo.Visibility = Visibility.Hidden;
                Admins.Visibility = Visibility.Hidden;
                AddAdmin.Visibility = Visibility.Hidden;
                gridAds.Visibility = Visibility.Hidden;
                gridCats.Visibility = Visibility.Visible;
                gridAddCategory.Visibility = Visibility.Hidden;


            }
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new MainLogin();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (ConnectedUser.type != 255)
            {
                btnAdd.Visibility = btnDelete.Visibility = Visibility.Hidden;
            }

            AddItemsIntoDataGrid();
            AddCategoriesIntoDataGrid();
        }

        private void DeleteAdmin_Click(object sender, RoutedEventArgs e)
        {
            if (gridAdmins.SelectedIndex != -1)
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.yellow, "Sunteti sigur ca doriti sa eliminati utilizatorul selectat?", this, MessageBoxButton.YesNo);
                

                if (cmb.ShowDialog() == true) // yes pressed
                {
                    DataGridCellInfo cellInfo = gridAdmins.SelectedCells[0];

                    int id = int.Parse((cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text);

                    if (DeleteUser(id))
                    {
                        CustomMessageBox c = new CustomMessageBox((int)MessageBoxColorTypes.green, "Utilziatorul a fost eliminat cu succes!", this, MessageBoxButton.OK);
                        c.ShowDialog();
                    }
                    else
                    {
                        CustomMessageBox c = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                        c.ShowDialog();
                    }
                }
            }
            else
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "Va rugam sa selectati un admin!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
            }
        }

        private void ShowAddAdminPanel_Click(object sender, RoutedEventArgs e)
        {
            Admins.Visibility = Visibility.Hidden;
            AddAdmin.Visibility = Visibility.Visible;
            txtMail.Text = "";
        }

        private void btnBackToAdminGrid_Click(object sender, RoutedEventArgs e)
        {
            AddAdmin.Visibility = Visibility.Hidden;
            Admins.Visibility = Visibility.Visible;
        }

        private void btnAddAdmin_Click(object sender, RoutedEventArgs e)
        {

            Admin admin = GetUser(txtMail.Text);

            if (admin.id == 0)
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "Utilizatorul nu exista!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
                return;
            }

            if (IsAdminAlready(admin))
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "Acest utilizator are deja statutul de administrator!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
            }
            else
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.yellow, "Sunteti sigur ca doriti sa-i acordati utilizatorului cu numele " + admin.name + " statutul de administrator?", this, MessageBoxButton.YesNo);
                
                if (cmb.ShowDialog() == true)
                {
                    if (AddUsser(admin))
                    {

                        admins.Add(admin);
                        gridAdmins.Items.Refresh();
                        AddAdmin.Visibility = Visibility.Hidden;
                        Admins.Visibility = Visibility.Visible;
                        CustomMessageBox c = new CustomMessageBox((int)MessageBoxColorTypes.green, "Utilziatorul " + admin.name +" a primit statutul de administrator!", this, MessageBoxButton.OK);
                        c.ShowDialog();
                    }
                    else
                    {
                        CustomMessageBox c = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                        c.ShowDialog();
                    }
                }
            }
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesDataGrid.SelectedIndex != -1)
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.yellow, "Sunteti sigur ca doriti sa eliminati categoria selectata?", this, MessageBoxButton.YesNo);
                
                if (cmb.ShowDialog() == true)
                {
                    DataGridCellInfo cellInfo = CategoriesDataGrid.SelectedCells[0];

                    int id = int.Parse((cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text);

                    if (DeleteCategory(id))
                    {
                        CustomMessageBox c = new CustomMessageBox((int)MessageBoxColorTypes.green, "Categoria a fost eliminata cu succes!", this, MessageBoxButton.OK);
                        c.ShowDialog();
                    }
                    else
                    {
                        CustomMessageBox c = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                        c.ShowDialog();
                    }
                }
            }
            else
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "Va rugam sa selectati o categorie!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
            }
        }

        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            if (IsACategoryAlready(txtCategory.Text))
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "Aceasta categorie exista deja!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
            }
            else
            {
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.yellow, "Sunteti sigur ca doriti sa adaugati categoria: " + txtCategory.Text + "?", this, MessageBoxButton.YesNo);
                if (cmb.ShowDialog() == true)
                {
                    int idCategory = AddCategory(txtCategory.Text);
                    if (idCategory != 0)
                    {
                        Category c = new Category();
                        c.addedBy = ConnectedUser.mail;
                        c.idAdder = ConnectedUser.id;
                        c.category = txtCategory.Text;
                        c.id = idCategory;

                        categories.Add(c);
                        CategoriesDataGrid.Items.Refresh();
                        gridAddCategory.Visibility = Visibility.Hidden;
                        gridCats.Visibility = Visibility.Visible;
                        CustomMessageBox cb = new CustomMessageBox((int)MessageBoxColorTypes.green, "Categoria a fost adaugata cu succes!", this, MessageBoxButton.OK);
                        cb.ShowDialog();
                    }
                    else
                    {
                        CustomMessageBox c = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                        c.ShowDialog();
                    }
                }
            }
        }

        private void btnBackToCategoryGrid_Click(object sender, RoutedEventArgs e)
        {
            gridCats.Visibility = Visibility.Visible;
            gridAddCategory.Visibility = Visibility.Hidden;
        }

        private void ShowAddCategoryPanel_Click(object sender, RoutedEventArgs e)
        {
            gridCats.Visibility = Visibility.Hidden;
            gridAddCategory.Visibility = Visibility.Visible;
            txtCategory.Text = "";
        }


        private void ControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Functions.ControlResize(sender, e, this);
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

        #endregion

        #region CustomFunctions
        public void SetUp(bool DisplayReports, int currentPage)
        {
            if (DisplayReports)
            {
                btnReports.Background = new SolidColorBrush((Color)Resources["DarkBlue"]);

                btnReportsEnabled = false;
                btnAdminsEnabled = true;
                btnCatEnabled = true;
                gridLogo.Visibility = Visibility.Hidden;

                gridLogo.Visibility = Visibility.Hidden;
                Admins.Visibility = Visibility.Hidden;
                AddAdmin.Visibility = Visibility.Hidden;
                gridAds.Visibility = Visibility.Visible;
                gridCats.Visibility = Visibility.Hidden;

                SelectAds();
                this.currentPage = currentPage;
                UpdateAds();
            }
        }

        public void AddItemsIntoDataGrid()
        {
            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Variables.conn;
                cmd.CommandText = "select * from utilizatori where tipCont = @tip";

                cmd.Parameters.AddWithValue("tip", (int)UserTypes.Admin);

                MySqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    Admin a = new Admin();
                    a.id = int.Parse(r["id"].ToString());
                    a.name = r["nume"].ToString();
                    a.mail = r["mail"].ToString();
                    a.registerDate = DateTime.Parse(r["dataI"].ToString()).ToShortDateString();
                    a.phone = r["nrTelefon"].ToString();

                    admins.Add(a);

                }

                gridAdmins.ItemsSource = admins;
                r.Close();
                Variables.conn.Close();
            }
            catch
            {
                if (Variables.conn.State == System.Data.ConnectionState.Open)
                {
                    Variables.conn.Close();
                }
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
            }
        }

        public void SelectAds()
        {
            ads.Clear();

            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Variables.conn;
                cmd.CommandText = "select * from anunturi inner join reclamatii on anunturi.id = reclamatii.idAnunt where verificat = 0";

                MySqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    ads.Add(int.Parse(r["id"].ToString()));
                }

                r.Close();
                Variables.conn.Close();
            }
            catch (Exception ex)
            {
                if (Variables.conn.State == ConnectionState.Open)
                {
                    Variables.conn.Close();
                }
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
            }
        }

        public void UpdateAds()
        {
            for (int i = 0; i < gridAds.Children.Count; i++)
            {
                if (gridAds.Children[i].GetType() == typeof(AdvertisementPreview))
                {
                    gridAds.Children.RemoveAt(i);
                    i--;
                }
            }

            len = ads.Count;
            pages = len / 3 + Convert.ToInt32(len % 3 != 0);

            if (len == 0)
            {
                currentPage = 0;
                txtPage.Text = "Nu am gasit niciun anunt reclamat!";
                gridLogo.Visibility = Visibility.Visible;
            }
            else
            {
                for (int c = 1, i = (currentPage - 1) * 3; i <= Math.Min(len - 1, currentPage * 3 - 1); i++, c += 2)
                {
                    AdvertisementPreview newAd = new AdvertisementPreview(ads[i], "", currentPage, (int)ControlTypes.AdminPanel); ;

                    gridAds.Children.Add(newAd);
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

        public void AddCategoriesIntoDataGrid()
        {
            try
            {
                Variables.conn.Open();


                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Variables.conn;
                cmd.CommandText = "select * from categorii inner join utilizatori on categorii.idAdaugator = utilizatori.id";


                MySqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    Category c = new Category();
                    c.id = int.Parse(r["id"].ToString());
                    c.addedBy = r["mail"].ToString();
                    c.category = r["nume"].ToString();
                    c.idAdder = int.Parse(r["idAdaugator"].ToString());

                    categories.Add(c);
                }

                CategoriesDataGrid.ItemsSource = categories;

                r.Close();
                Variables.conn.Close();
            }
            catch
            {
                if (Variables.conn.State == System.Data.ConnectionState.Open)
                {
                    Variables.conn.Close();
                }
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
            }
        }
        private bool DeleteUser(int id)
        {
            bool deleted = false;

            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Variables.conn;
                cmd.CommandText = "update utilizatori set tipCont = @tipCont where id = @id";

                cmd.Parameters.AddWithValue("tipCont", (int)UserTypes.Normal);
                cmd.Parameters.AddWithValue("id", id);

                if (cmd.ExecuteNonQuery() != 0)
                {
                    int index = admins.FindIndex(x => x.id == id);
                    admins.RemoveAt(index);
                    gridAdmins.Items.Refresh();
                    deleted = true;
                }

                Variables.conn.Close();

            }
            catch (Exception ex)
            {
                if (Variables.conn.State == System.Data.ConnectionState.Open)
                {
                    Variables.conn.Close();
                }
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
            }

            return deleted;
        }

        public bool AddUsser(Admin admin)
        {
            bool added = false;

            try
            {
                Variables.conn.Open();


                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Variables.conn;
                cmd.CommandText = "update utilizatori set tipCont = @tipCont where id = @id";

                cmd.Parameters.AddWithValue("tipCont", (int)UserTypes.Admin);
                cmd.Parameters.AddWithValue("id", admin.id);

                if (cmd.ExecuteNonQuery() != 0)
                {
                    added = true;
                }

                Variables.conn.Close();

            }
            catch (Exception ex)
            {
                if (Variables.conn.State == System.Data.ConnectionState.Open)
                {
                    Variables.conn.Close();
                }
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
            }

            return added;
        }



        public bool IsAdminAlready(Admin admin)
        {
            bool isAdmin = false;

            try
            {
                Variables.conn.Open();


                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Variables.conn;
                cmd.CommandText = "select * from utilizatori where id = @id";

                cmd.Parameters.AddWithValue("id", admin.id);

                MySqlDataReader r = cmd.ExecuteReader();

                if (r.Read() && int.Parse(r["tipCont"].ToString()) >= (int)UserTypes.Admin)
                {
                    isAdmin = true;
                }

                r.Close();
                Variables.conn.Close();

            }
            catch (Exception ex)
            {
                if (Variables.conn.State == System.Data.ConnectionState.Open)
                {
                    Variables.conn.Close();
                }
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
            }

            return isAdmin;
        }

        public Admin GetUser(string mail)
        {
            Admin admin = new Admin();

            try
            {
                Variables.conn.Open();


                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Variables.conn;
                cmd.CommandText = "select * from utilizatori where mail = @mail";

                cmd.Parameters.AddWithValue("mail", mail);

                MySqlDataReader r = cmd.ExecuteReader();

                if (r.Read())
                {
                    admin.name = r["nume"].ToString();
                    admin.id = int.Parse(r["id"].ToString());
                    admin.phone = r["nrTelefon"].ToString();
                    admin.registerDate = DateTime.Parse(r["dataI"].ToString()).ToShortDateString();
                    admin.mail = mail;
                }

                r.Close();
                Variables.conn.Close();

            }
            catch (Exception ex)
            {
                if (Variables.conn.State == System.Data.ConnectionState.Open)
                {
                    Variables.conn.Close();
                }
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
            }
            return admin;
        }

        public bool DeleteCategory(int id)
        {
            bool deleted = false;

            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Variables.conn;
                cmd.CommandText = "delete from categorii where id = @id";

                cmd.Parameters.AddWithValue("id", id);

                if (cmd.ExecuteNonQuery() != 0)
                {
                    int index = categories.FindIndex(x => x.id == id);
                    categories.RemoveAt(index);
                    CategoriesDataGrid.Items.Refresh();
                    deleted = true;

                }

                Variables.conn.Close();

            }
            catch (Exception ex)
            {
                if (Variables.conn.State == System.Data.ConnectionState.Open)
                {
                    Variables.conn.Close();
                }
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
            }

            return deleted;
        }

        public int AddCategory(string name)
        {
            int id = 0;

            try
            {
                Variables.conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Variables.conn;
                cmd.CommandText = "insert into categorii (nume, idAdaugator) values (@nume, @idAdaugator)";

                cmd.Parameters.AddWithValue("nume", name);
                cmd.Parameters.AddWithValue("idAdaugator", ConnectedUser.id);

                if (cmd.ExecuteNonQuery() != 0)
                {
                    cmd.CommandText = "select @@Identity";
                    id = (int)(ulong)cmd.ExecuteScalar();
                }

                CategoriesDataGrid.ItemsSource = categories;
                Variables.conn.Close();
            }
            catch
            {
                if (Variables.conn.State == System.Data.ConnectionState.Open)
                {
                    Variables.conn.Close();
                }
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
            }

            return id;
        }

        public bool IsACategoryAlready(string category)
        {
            bool isCategory = false;

            try
            {
                Variables.conn.Open();


                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Variables.conn;
                cmd.CommandText = "select * from categorii where nume = @nume";

                cmd.Parameters.AddWithValue("nume", category);

                MySqlDataReader r = cmd.ExecuteReader();


                if (r.Read())
                {
                    isCategory = true;
                }

                r.Close();
                Variables.conn.Close();
            }
            catch
            {
                if (Variables.conn.State == System.Data.ConnectionState.Open)
                {
                    Variables.conn.Close();
                }
                CustomMessageBox cmb = new CustomMessageBox((int)MessageBoxColorTypes.red, "A aparut o eroare, daca problema persista va rugam sa contactati un administrator!", this, MessageBoxButton.OK);
                cmb.ShowDialog();
            }

            return isCategory;
        }
        #endregion
    }
}
