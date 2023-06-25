using MahApps.Metro.Controls;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace RealEstateAgency
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : MetroWindow
    {
        public AuthWindow()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            string login = tbLogin.Text.Trim();
            string pass = pbPassword.Password.Trim();

            if (login.Length < 6)
            {
                tbLogin.ToolTip = "Поле не может содержать менее 6 символов!";
                tbLogin.Background = Brushes.LightCoral;
            }
            else if (pass.Length < 6)
            {
                pbPassword.ToolTip = "Поле не может содержать менее 6 символов!";
                pbPassword.Background = Brushes.LightCoral;
            }
            else
            {
                tbLogin.ToolTip = "";
                tbLogin.Background = Brushes.Transparent;
                pbPassword.ToolTip = "";
                pbPassword.Background = Brushes.Transparent;

                User authUser = null;
                using (RealEstateAgencyEntities context = new RealEstateAgencyEntities())
                {
                    authUser = context.User.Where(x => x.Login == login && x.Password == pass).FirstOrDefault();
                }

                if (authUser != null && authUser.Permission.Equals("admin"))
                {
                    UserWindow userWindow = new UserWindow();
                    
                    userWindow.user = authUser;
                    userWindow.Show();
                    this.Hide();
                }
                else if (authUser != null && authUser.Permission.Equals("manager"))
                {
                    UserWindow userWindow = new UserWindow();
                    userWindow.btnOperacii.Visibility = Visibility.Hidden;
                    userWindow.btnReportBuilderSales.Visibility = Visibility.Hidden;
                    userWindow.user = authUser;
                    userWindow.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Данные не корректны!");
                }
            }
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }

        private void btnSpravka_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"avorizaciya.pdf");
        }
    }
}
