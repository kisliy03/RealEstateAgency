using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace RealEstateAgency
{
    public partial class MainWindow : MetroWindow
    {
        RealEstateAgencyEntities db = new RealEstateAgencyEntities();

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            db = new RealEstateAgencyEntities();
        }

        private void btn_Registation_Click(object sender, RoutedEventArgs e)
        {
            string name = tbName.Text.Trim();
            string login = tbLogin.Text.Trim();
            string pass1 = pbPassword.Password.Trim();
            string pass2 = pbRePassword.Password.Trim();
            string email = tbEmail.Text.Trim().ToLower();

            if (login.Length < 6)
            {
                tbLogin.ToolTip = "Поле не может содержать менее 6 символов!";
                tbLogin.Background = Brushes.LightCoral;
            } else if (pass1.Length < 6)
            {
                pbPassword.ToolTip = "Поле не может содержать менее 6 символов!";
                pbPassword.Background = Brushes.LightCoral;
            } else if (pass1 != pass2)
            {
                pbRePassword.ToolTip = "Пароли не совпадают";
                pbRePassword.Background = Brushes.LightCoral;
            } else if (!Regex.IsMatch(tbEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                tbEmail.ToolTip = "Поле введено не корректно!";
                tbEmail.Background = Brushes.LightCoral;
            } else
            {
                tbLogin.ToolTip = "";
                tbLogin.Background = Brushes.Transparent;
                pbPassword.ToolTip = "";
                pbPassword.Background = Brushes.Transparent;
                pbRePassword.ToolTip = "";
                pbRePassword.Background = Brushes.Transparent;
                tbEmail.ToolTip = "";
                tbEmail.Background = Brushes.Transparent;

                User user = new User(name, login, pass1, email);

                db.User.Add(user);
                db.SaveChanges();

                MessageBox.Show("Регистрация прошла успешно!");

                Button_Auth_Click(sender, e);
            }
        }

        private void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();
            this.Hide();
        }
    }
}
