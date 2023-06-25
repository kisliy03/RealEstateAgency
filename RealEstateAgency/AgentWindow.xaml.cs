using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Office.Interop.Word;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace RealEstateAgency
{
    /// <summary>
    /// Логика взаимодействия для AgentWindow.xaml
    /// </summary>
    public partial class AgentWindow : MetroWindow
    {
        RealEstateAgencyEntities db = new RealEstateAgencyEntities();
        public AgentWindow()
        {
            InitializeComponent();
            db = new RealEstateAgencyEntities();
            Load_data();
        }
        List<Apartments> GetApartmenrs() { return db.Apartments.ToList(); }
        List<Client> GetClients() { return db.Client.ToList(); }
        List<Owners> GetOwners() { return db.Owners.ToList(); }
        List<User> GetUsers() { return db.User.ToList(); }
        List<Sales> GetSales() { return db.Sales.ToList(); }
        public void Load_data()
        {
            var apartments =
                from Apart in GetApartmenrs()

                select new
                {
                    Заголовок = Apart.Title,
                    Адрес = Apart.Address,
                    Район = Apart.Region.TItile,
                    Метро = Apart.Metro.Name,
                    Комнаты = Apart.Rooms,
                    Площадь = Apart.Area + "м²",
                    Этаж = Apart.Floor,
                    Цена = Apart.Price + " BYN",
                    Комментарий = Apart.Comment,
                    Статус = Apart.Status,
                };

            dgAparts.ItemsSource = apartments;

            var owners =
                from owner in GetOwners()

                select new
                {
                    Имя = owner.Name,
                    Фамилия = owner.Surname,
                    Отчетсво = owner.lastName,
                    ДатаРождения = owner.Birthday.ToShortDateString(),
                    Телефон = owner.Phone,
                    Адрес = owner.Address,
                    Пасспорт = owner.PassportNumber,
                    ЛичныйНомер = owner.PersonalPassport,
                };

            dgOwners.ItemsSource = owners;

            var clients =
                from client in GetClients()

                select new
                {
                    Имя = client.Name,
                    Фамилия = client.Surname,
                    Отчество = client.lastName,
                    email = client.Email,
                    день_рождения = client.Birthday.ToShortDateString(),
                    Телефон = client.Phone,
                    Адрес = client.Address,
                    Пасспорт = client.Passport,
                    ЛичныйНомер = client.PersonalPassport,
                };

            dgClient.ItemsSource = clients;

            var users =
                from user in GetUsers()

                select new
                {
                    Имя = user.Name,
                    Логин = user.Login,
                    Пароль = user.Password,
                    Email = user.Email,
                    Доступ = user.Permission,
                };

            dgUsers.ItemsSource = users;

            var sales =
                from sale in GetSales()

                select new
                {
                    Продавец = sale.Owners.Surname + " " + sale.Owners.Name + " " + sale.Owners.lastName,
                    Покупатель = sale.Client.Surname + " " + sale.Client.Name + " " + sale.Client.lastName,
                    Риелтор = sale.User.Name,
                    Квартира = sale.Apartments.Title,
                    ДатаПродажи = sale.date_sale.ToShortDateString(),
                    Продолжительность = sale.CertainDate,
                };

            dgSales.ItemsSource = sales;
        }

        private void btnAddAparts_Click(object sender, RoutedEventArgs e)
        {
            int rooms, floor, price;
            double area;

            if (!int.TryParse(tbRooms.Text, out rooms))
            {
                tbRooms.ToolTip = "Поле введено не корректно!";
                tbRooms.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbRooms.ToolTip = "";
                tbRooms.Background = Brushes.Transparent;
            }

            if (rooms <= 0)
            {
                tbRooms.ToolTip = "Поле введено не корректно!";
                tbRooms.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbRooms.ToolTip = "";
                tbRooms.Background = Brushes.Transparent;
            }

            if (!double.TryParse(tbArea.Text, out area))
            {
                tbArea.ToolTip = "Поле введено не корректно!";
                tbArea.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbArea.ToolTip = "";
                tbArea.Background = Brushes.Transparent;
            }

            if (area <= 0)
            {
                tbArea.ToolTip = "Поле введено не корректно!";
                tbArea.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbArea.ToolTip = "";
                tbArea.Background = Brushes.Transparent;
            }

            if (!int.TryParse(tbFloor.Text, out floor))
            {
                tbFloor.ToolTip = "Поле введено не корректно!";
                tbFloor.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbFloor.ToolTip = "";
                tbFloor.Background = Brushes.Transparent;
            }

            if (floor <= 0)
            {
                tbFloor.ToolTip = "Поле введено не корректно!";
                tbFloor.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbFloor.ToolTip = "";
                tbFloor.Background = Brushes.Transparent;
            }

            if (!int.TryParse(tbPrice.Text, out price))
            {
                tbPrice.ToolTip = "Поле введено не корректно!";
                tbPrice.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbPrice.ToolTip = "";
                tbPrice.Background = Brushes.Transparent;
            }

            if (price <= 0)
            {
                tbPrice.ToolTip = "Поле введено не корректно!";
                tbPrice.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbPrice.ToolTip = "";
                tbPrice.Background = Brushes.Transparent;
            }

            if (db.Apartments.Any(x => x.Title == tbTitle.Text))
            {
                tbTitle.ToolTip = "Такой объект уже существует!";
                tbTitle.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbTitle.ToolTip = "";
                tbTitle.Background = Brushes.Transparent;
            }

            Apartments apartments = new Apartments();
            var metro = db.Metro.Where(x => x.Name == cbMetro.Text).FirstOrDefault();
            var region = db.Region.Where(x => x.TItile == cbRegion.Text).FirstOrDefault();
            apartments.id_Metro = metro.id;
            apartments.id_Region = region.id;
            apartments.Title = tbTitle.Text;
            apartments.Address = tbAddress.Text;
            apartments.Rooms = rooms;
            apartments.Area = area;
            apartments.Floor = floor;
            apartments.Price = Convert.ToInt32(tbPrice.Text);
            apartments.Comment = tbComment.Text;
            apartments.Status = cbStatus.Text;

            db.Apartments.Add(apartments);
            db.SaveChanges();
            Load_data();
        }

        private void btnDeleteAparts_Click(object sender, RoutedEventArgs e)
        {
            var ObjRemoving = dgAparts.SelectedIndex;
            var lst = db.Apartments.ToList();
            if (MessageBox.Show($"Вы точно хотите удалить этот элемент?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                try
                {
                    db.Apartments.Remove(lst[ObjRemoving]);
                    db.SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    Load_data();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
        }

        private void btnAddClient_Click(object sender, RoutedEventArgs e)
        {
            DateTime birthday;

            if (!DateTime.TryParse(tbBirthday.Text, out birthday))
            {
                tbBirthday.ToolTip = "Поле введено не корректно!";
                tbBirthday.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbBirthday.ToolTip = "";
                tbBirthday.Background = Brushes.Transparent;
            }

            if (!Regex.IsMatch(tbPhone.Text, @"^\+375\d{9}$"))
            {
                tbPhone.ToolTip = "Поле введено не корректно!";
                tbPhone.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbPhone.ToolTip = "";
                tbPhone.Background = Brushes.Transparent;
            }

            if (!Regex.IsMatch(tbEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                tbEmail.ToolTip = "Поле введено не корректно!";
                tbEmail.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbEmail.ToolTip = "";
                tbEmail.Background = Brushes.Transparent;
            }

            if (!Regex.IsMatch(tbPassportClient.Text, @"^[A-Z]{2}\d{7}$"))
            {
                tbPassportClient.ToolTip = "Поле введено не корректно!";
                tbPassportClient.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbPassportClient.ToolTip = "";
                tbPassportClient.Background = Brushes.Transparent;
            }

            if (!Regex.IsMatch(tbPersonalNumber.Text, @"^\d{7}[A-Z]\d{3}[A-Z][A-Z]\d$"))
            {
                tbPersonalNumber.ToolTip = "Поле введено не корректно!";
                tbPersonalNumber.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbPersonalNumber.ToolTip = "";
                tbPersonalNumber.Background = Brushes.Transparent;
            }

            if (db.Client.Any(x => x.Passport == tbPassportClient.Text))
            {
                tbPassportClient.ToolTip = "Такой клиент уже существует!";
                tbPassportClient.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbPassportClient.ToolTip = "";
                tbPassportClient.Background = Brushes.Transparent;
            }

            Client client = new Client();
            client.Name = tbName.Text;
            client.Surname = tbSurname.Text;
            client.lastName = tbLastName.Text;
            client.Birthday = birthday;
            client.Phone = tbPhone.Text;
            client.Email = tbEmail.Text;
            client.Address = tbAddressClient.Text;
            client.Passport = tbPassportClient.Text;
            client.PersonalPassport = tbPersonalNumber.Text;

            db.Client.Add(client);
            db.SaveChanges();
            Load_data();
        }

        private void btnDeleteClient_Click(object sender, RoutedEventArgs e)
        {
            var ObjRemoving = dgClient.SelectedIndex;
            var lst = db.Client.ToList();
            if (MessageBox.Show($"Вы точно хотите удалить этот элемент?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                try
                {
                    db.Client.Remove(lst[ObjRemoving]);
                    db.SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    Load_data();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }

        }

        private void btnAddOwner_Click(object sender, RoutedEventArgs e)
        {
            DateTime birthday;

            if (!DateTime.TryParse(tbBirthdayOwner.Text, out birthday))
            {
                tbBirthdayOwner.ToolTip = "Поле введено не корректно!";
                tbBirthdayOwner.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbBirthdayOwner.ToolTip = "";
                tbBirthdayOwner.Background = Brushes.Transparent;
            }

            if (!Regex.IsMatch(tbPhoneOwner.Text, @"^\+375\d{9}$"))
            {
                tbPhoneOwner.ToolTip = "Поле введено не корректно!";
                tbPhoneOwner.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbPhoneOwner.ToolTip = "";
                tbPhoneOwner.Background = Brushes.Transparent;
            }

            if (!Regex.IsMatch(tbEmailOwner.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                tbEmailOwner.ToolTip = "Поле введено не корректно!";
                tbEmailOwner.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbEmailOwner.ToolTip = "";
                tbEmailOwner.Background = Brushes.Transparent;
            }

            if (!Regex.IsMatch(tbPassport.Text, @"^[A-Z]{2}\d{7}$"))
            {
                tbPassport.ToolTip = "Поле введено не корректно!";
                tbPassport.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbPassport.ToolTip = "";
                tbPassport.Background = Brushes.Transparent;
            }

            if (!Regex.IsMatch(tbPersonalNumberOwner.Text, @"^\d{7}[A-Z]\d{3}[A-Z][A-Z]\d$"))
            {
                tbPersonalNumberOwner.ToolTip = "Поле введено не корректно!";
                tbPersonalNumberOwner.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbPersonalNumberOwner.ToolTip = "";
                tbPersonalNumberOwner.Background = Brushes.Transparent;
            }

            if (db.Owners.Any(x => x.PassportNumber == tbPassport.Text))
            {
                tbPassport.ToolTip = "Такой владелец уже существует!";
                tbPassport.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbPassport.ToolTip = "";
                tbPassport.Background = Brushes.Transparent;
            }

            Owners owners = new Owners();
            owners.Name = tbNameOwner.Text;
            owners.Surname = tbSurnameOwner.Text;
            owners.lastName = tbLastNameOwner.Text;
            owners.Birthday = birthday;
            owners.Phone = tbPhoneOwner.Text;
            owners.Address = tbAddres.Text;
            owners.email = tbEmailOwner.Text;
            owners.PassportNumber = tbPassport.Text;
            owners.PersonalPassport = tbPersonalNumberOwner.Text;

            db.Owners.Add(owners);
            db.SaveChanges();
            Load_data();
        }

        private void btnDeleteOwner_Click(object sender, RoutedEventArgs e)
        {
            var ObjRemoving = dgOwners.SelectedIndex;
            var lst = db.Owners.ToList();
            if (MessageBox.Show($"Вы точно хотите удалить этот элемент?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                try
                {
                    db.Owners.Remove(lst[ObjRemoving]);
                    db.SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    Load_data();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            if (tbLogin.Text.Length < 6)
            {
                tbLogin.ToolTip = "Поле не может содержать менее 6 символов!";
                tbLogin.Background = Brushes.LightCoral;
            }
            else
            {
                tbLogin.ToolTip = "";
                tbLogin.Background = Brushes.Transparent;
            }

            if (tbPassword.Text.Length < 6)
            {
                tbPassword.ToolTip = "Поле не может содержать менее 6 символов!";
                tbPassword.Background = Brushes.LightCoral;
            }
            else
            {
                tbPassword.ToolTip = "";
                tbPassword.Background = Brushes.Transparent;
            }

            if (!Regex.IsMatch(tbEmailUser.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                tbEmailUser.ToolTip = "Поле введено не корректно!";
                tbEmailUser.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbEmailUser.ToolTip = "";
                tbEmailUser.Background = Brushes.Transparent;
            }

            if (db.User.Any(x => x.Login == tbLogin.Text))
            {
                tbLogin.ToolTip = "Такой пользователь уже существует!";
                tbLogin.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbLogin.ToolTip = "";
                tbLogin.Background = Brushes.Transparent;
            }

            User users = new User();
            users.Name = tbNameUser.Text;
            users.Login = tbLogin.Text;
            users.Password = tbPassword.Text;
            users.Email = tbEmailUser.Text;
            users.Permission = cbPermission.Text;

            db.User.Add(users);
            db.SaveChanges();
            Load_data();
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var ObjRemoving = dgUsers.SelectedIndex;
            var lst = db.User.ToList();
            if (MessageBox.Show($"Вы точно хотите удалить этот элемент?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                try
                {
                    db.User.Remove(lst[ObjRemoving]);
                    db.SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    Load_data();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
        }

        private void cbIDOwner_Loaded(object sender, RoutedEventArgs e)
        {
            cbIDOwner.Items.Clear();
            var list = db.Owners.ToList();
            foreach (var item in list)
            {
                cbIDOwner.Items.Add(item.Name);
            }
        }

        private void cbIDClient_Loaded(object sender, RoutedEventArgs e)
        {
            cbIDClient.Items.Clear();
            var list = db.Client.ToList();
            foreach (var item in list)
            {
                cbIDClient.Items.Add(item.Surname);
            }
        }

        private void cbIDApart_Loaded(object sender, RoutedEventArgs e)
        {
            cbIDApart.Items.Clear();
            var list = db.Apartments.ToList();
            foreach (var item in list)
            {
                cbIDApart.Items.Add(item.Title);
            }
        }

        private void btnAddSales_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateSale, certainDate;

            if (!DateTime.TryParse(tbDateSale.Text, out dateSale))
            {
                tbDateSale.ToolTip = "Поле введено не корректно!";
                tbDateSale.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbDateSale.ToolTip = "";
                tbDateSale.Background = Brushes.Transparent;
            }

            if (!DateTime.TryParse(tbCertainDate.Text, out certainDate))
            {
                tbCertainDate.ToolTip = "Поле введено не корректно!";
                tbCertainDate.Background = Brushes.LightCoral;
                return;
            }
            else
            {
                tbCertainDate.ToolTip = "";
                tbCertainDate.Background = Brushes.Transparent;
            }

            Sales sales = new Sales();
            var owner = db.Owners.Where(x => x.Name == cbIDOwner.Text).FirstOrDefault();
            var client = db.Client.Where(x => x.Surname == cbIDClient.Text).FirstOrDefault();
            var apart = db.Apartments.Where(x => x.Title == cbIDApart.Text).FirstOrDefault();
            var user = db.User.Where(x => x.Name == cbIDUser.Text).FirstOrDefault();
            sales.date_sale = dateSale;
            sales.CertainDate = certainDate;
            sales.idClient = client.id;
            sales.idApartment = apart.id;
            sales.idOwner = owner.id;
            sales.idUser = user.id;


            db.Sales.Add(sales);
            db.SaveChanges();
            Load_data();
        }

        private void btnDeleteSales_Click(object sender, RoutedEventArgs e)
        {
            var ObjRemoving = dgSales.SelectedIndex;
            var lst = db.Sales.ToList();
            if (MessageBox.Show($"Вы точно хотите удалить этот элемент?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                try
                {
                    db.Sales.Remove(lst[ObjRemoving]);
                    db.SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    Load_data();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
        }

        private void cbIDUser_Loaded(object sender, RoutedEventArgs e)
        {
            cbIDUser.Items.Clear();
            var list = db.User.ToList();
            foreach (var item in list)
            {
                cbIDUser.Items.Add(item.Name);
            }
        }

        private void cbRegion_Loaded(object sender, RoutedEventArgs e)
        {
            cbRegion.Items.Clear();
            var list = db.Region.ToList();
            foreach (var item in list)
            {
                cbRegion.Items.Add(item.TItile);
            }
        }

        private void cbRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbMetro.Items.Clear();
            var region = db.Region.Where(x => x.TItile == cbRegion.SelectedItem.ToString()).FirstOrDefault();
            var list = db.Metro.Where(x => x.idRegion == region.id).ToList();
            cbMetro.Items.Add("Без метро");
            foreach (var item in list)
            {
                cbMetro.Items.Add(item.Name);
            }
        }

        private void btnSpravka_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"operacii.pdf");
        }
    }
}
