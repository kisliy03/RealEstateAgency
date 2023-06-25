using Aspose.Cells;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;
using System.Globalization;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data;
using Style = System.Windows.Style;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder.Spatial;
using Syncfusion.Windows.Shared;
using System.Windows.Forms;
using System.Runtime.Remoting.Contexts;
using MessageBox = System.Windows.MessageBox;
using System.Web.UI.WebControls;
using ControlzEx.Standard;
using System.Threading.Tasks;
using Aspose.Cells.Drawing;
using System.Diagnostics;
using System.Net;

namespace RealEstateAgency
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : MetroWindow
    {
        Dictionary<object, Brush> rowBackgrounds = new Dictionary<object, Brush>();
        RealEstateAgencyEntities db = new RealEstateAgencyEntities();
        public User user = new User();
        public UserWindow()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            db = new RealEstateAgencyEntities();

            cbRajon.SelectedIndex = 0;
            cbMetro.SelectedIndex = 0;
            cbSort.SelectedIndex = 0;
            //ComboBox Metro
            cbMetro.Items.Add("Без выбора");
            foreach (var metro in db.Metro.ToList())
            {
                cbMetro.Items.Add(metro.Name);
            }

            Load_data();
        }
        List<Apartments> GetApartmenrs() { return db.Apartments.ToList(); }

        //Функция заполнения DataGrid
        public void Load_data()
        {
            var apartments =
                from Apart in GetApartmenrs()
                where Apart.Status == "Продается" || Apart.Status == "Сдается"
                select new
                {
                    Заголовок = Apart.Title,
                    Адрес = Apart.Address,
                    Метро = Apart.Metro.Name,
                    Район = Apart.Region.TItile,
                    Комнаты = Apart.Rooms,
                    Площадь = Apart.Area + " " + "м²",
                    Этаж = Apart.Floor,
                    Цена = Apart.Price + " " + "BYN",
                    Статус = Apart.Status
                };

            ListOfAparts.ItemsSource = apartments;
        }
        //Функция заполнения DataGrid
        public void Load_data_Search(List<Apartments> dataSr)
        {
            var apartments =
                from Apart in dataSr
                where Apart.Status == "Продается" || Apart.Status == "Сдается"
                select new
                {
                    Заголовок = Apart.Title,
                    Адрес = Apart.Address,
                    Метро = Apart.Metro.Name,
                    Район = Apart.Region.TItile,
                    Комнаты = Apart.Rooms,
                    Площадь = Apart.Area + " " + "м²",
                    Этаж = Apart.Floor,
                    Цена = Apart.Price + " " + "BYN",
                    Статус = Apart.Status
                };

            ListOfAparts.ItemsSource = apartments;
        }

        //Функция заполнения DataGrid
        private void Load_data_Search(IOrderedEnumerable<Apartments> dataSr)
        {
            var apartments =
                from Apart in dataSr
                where Apart.Status == "Продается" || Apart.Status == "Сдается"
                select new
                {
                    Заголовок = Apart.Title,
                    Адрес = Apart.Address,
                    Метро = Apart.Metro.Name,
                    Район = Apart.Region.TItile,
                    Комнаты = Apart.Rooms,
                    Площадь = Apart.Area + " " + "м²",
                    Этаж = Apart.Floor,
                    Цена = Apart.Price + " " + "BYN",
                    Статус = Apart.Status
                };

            ListOfAparts.ItemsSource = apartments;
        }

        //Функция проверки на существующую недвижимость в бд
        public bool CheckApartments(string title)
        {
            return db.Apartments.Any(x => x.Title == title);
        }

        //Функция запуска парсера на Python
        private void ParseObjects() { System.Diagnostics.Process.Start(@"C:\Users\Kisliy\PycharmProjects\testproj\parser.py"); }

        //Функция парсинга объекта
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Workbook wb = new Workbook("output.xlsx");

            // Получить все рабочие листы
            WorksheetCollection collection = wb.Worksheets;

            // Получить рабочий лист, используя его индекс
            Worksheet worksheet = collection[0];

            // Печать имени рабочего листа
            Console.WriteLine("Worksheet: " + worksheet.Name);

            // Получить количество строк и столбцов
            int rows = worksheet.Cells.MaxDataRow;

            if (CheckApartments(worksheet.Cells[1, 3].Value.ToString()))
            {
                MessageBox.Show("Такой объект уже существует!");
                btnAdd.IsEnabled = false;
                return;
            }

            string img = worksheet.Cells[1, 1].Value.ToString();
            string Link = worksheet.Cells[1, 2].Value.ToString();
            string Title = worksheet.Cells[1, 3].Value.ToString();
            string Address = worksheet.Cells[1, 4].Value.ToString();
            string Region = worksheet.Cells[1, 5].Value.ToString();
            string Metro = worksheet.Cells[1, 6].Value.ToString();
            int Rooms = Convert.ToInt32(worksheet.Cells[1, 7].Value.ToString());
            int Floor = 3;
            try
            {
                Floor = Convert.ToInt32(worksheet.Cells[1, 9].Value.ToString().Split('/')[0].Trim());
            }
            catch (Exception ex)
            {
                Floor = 3;
            }
            string[] Price = worksheet.Cells[1, 10].Value.ToString().Split(' ');
            string Comment = worksheet.Cells[1, 11].Value.ToString();
            string ResultPrice = "";
            CultureInfo temp_culture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            double Area = Convert.ToDouble(worksheet.Cells[1, 8].Value.ToString().Split(' ')[0]);
            Thread.CurrentThread.CurrentCulture = temp_culture;
            string status = "";

            if (worksheet.Cells[1, 12].Value.ToString().Split(' ')[0] == "Срок")
            {
                status = "Сдается";
            }
            else { status = "Продается"; }

            foreach (string str in Price)
            {
                if (str != "р." && str != "р./мес.")
                    ResultPrice += str;
            }

            List<Metro> metros = db.Metro.ToList();
            int idMetro = 33;
            foreach (Metro m in metros)
            {
                if (Metro == m.Name)
                {
                    idMetro = m.id;
                }
            }

            List<Region> regions = db.Region.ToList();
            int idRegion = 1;
            foreach (Region m in regions)
            {
                if (Region == m.TItile)
                {
                    idRegion = m.id;
                }
            }

            Apartments apartments = new Apartments(idMetro, idRegion, img, Link, Title, Address, Rooms, Area, Floor, Convert.ToInt32(ResultPrice), Comment, status);
            MessageBox.Show(apartments.Title);
            db.Apartments.Add(apartments);
            db.SaveChanges();
            Load_data();
            btnAdd.IsEnabled = false;
        }
        //Фунция поиска по названию
        private void SearchEntity(string s)
        {
            try
            {
                var data = db.Apartments;

                List<Apartments> dataSr = new List<Apartments>();
                foreach (var obj in data)
                {
                    if (obj.Title.StartsWith(s) || obj.Address.StartsWith(s) || obj.Metro.Name.StartsWith(s) || obj.Rooms.ToString().StartsWith(s) || obj.Area.ToString().StartsWith(s) || obj.Floor.ToString().StartsWith(s) || obj.Price.ToString().StartsWith(s))
                    {
                        dataSr.Add(obj);
                    }

                    obj.Title.OrderBy(p => p);
                }
                ListOfAparts.ItemsSource = dataSr;
                Load_data_Search(dataSr);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
        //Функция обработки текста с поля поиска
        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            char[] k = tbSearch.Text.ToCharArray();
            string s;

            try
            {
                s = tbSearch.Text;
                SearchEntity(s);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
        //Функция сортировки
        private void btnSort_Click(object sender, RoutedEventArgs e)
        {
            string soort = cbSort.Text;
            switch (soort)
            {
                case "Название":
                    Load_data_Search(db.Apartments.ToList().OrderBy(x => x.Title));
                    break;
                case "Адрес":
                    Load_data_Search(db.Apartments.ToList().OrderBy(x => x.Address));
                    break;
                case "Метро":
                    Load_data_Search(db.Apartments.ToList().OrderBy(x => x.Metro));
                    break;
                case "Комнаты":
                    Load_data_Search(db.Apartments.ToList().OrderBy(x => x.Rooms));
                    break;
                case "Площадь":
                    Load_data_Search(db.Apartments.ToList().OrderBy(x => x.Area));
                    break;
                case "Этаж":
                    Load_data_Search(db.Apartments.ToList().OrderBy(x => x.Floor));
                    break;
                case "Цена":
                    Load_data_Search(db.Apartments.ToList().OrderBy(x => x.Price));
                    break;
                case "Без сортировки":
                    Load_data();
                    break;
            }
        }
        //Функция формирования карточки объекта
        private void ListOfAparts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var index = ListOfAparts.SelectedIndex;
                var lst = db.Apartments.ToList();


                string title = ListOfAparts.Items[index].ToString().Split('=')[1].Split(new String[] { "Адрес" }, System.StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                title = title.Remove(title.Length - 1);
                var CloseCount = db.Apartments.Where(w => w.Title == title).FirstOrDefault();

                ObjectCard objectCard = new ObjectCard(CloseCount);
                objectCard.user = user;
                objectCard.ShowDialog();
                Load_data();
            }
            catch(ArgumentOutOfRangeException)
            {
                Console.WriteLine("GG");
            }
        }
        //Функция поиска по заданным критериям
        public List<Apartments> SearchFlats(string address = null, int rooms = 0, int floor = 0, string region = null, string metro = null,
            int priceOT = 0, int priceDO = 0, int areaOT = 0, int areaDO = 0)
        {
            var query = db.Apartments.AsQueryable();

            // добавляем условия для поиска по критериям, если они не null
            if (!string.IsNullOrEmpty(address))
            {
                query = query.Where(f => f.Address.Contains(address));
            }

            if (rooms != 0)
            {
                query = query.Where(f => f.Rooms == rooms);
            }

            if (floor != 0)
            {
                query = query.Where(f => f.Floor == floor);
            }

            if (priceDO != 0)
            {
                query = query.Where(f => f.Price <= priceDO);
            }

            if (priceOT != 0)
            {
                query = query.Where(f => f.Price >= priceOT);
            }

            if (areaDO != 0)
            {
                query = query.Where(f => f.Area <= areaDO);
            }

            if (areaOT != 0)
            {
                query = query.Where(f => f.Area >= areaOT);
            }

            if (region != "Район")
            {
                query = query.Where(f => f.Region.TItile.Contains(region));
            }

            if (metro != "Без выбора")
            {
                query = query.Where(f => f.Metro.Name.Contains(metro));
            }

            return query.ToList();
        }
        //Функция обработки кнопки поиска
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            string adres = tbAddress.Text;
            string region = cbRajon.Text;
            string metro = cbMetro.SelectedItem.ToString();
            int room = Convert.ToInt32(sRooms.Value);
            int floor = Convert.ToInt32(sFloors.Value);
            int priceOT;
            int priceDO;
            int areaOT;
            int areaDO;
            int.TryParse(tbPriceOT.Text, out priceOT);
            int.TryParse(tbPriceDO.Text, out priceDO);
            int.TryParse(tbAreaOT.Text, out areaOT);
            int.TryParse(tbAreaDO.Text, out areaDO);

            List<Apartments> ds = SearchFlats(adres, room, floor, region, metro, priceOT, priceDO, areaOT, areaDO);
            Load_data_Search(ds);
        }
        //Функция обработки кнопки отмена
        private void btnDecline_Click(object sender, RoutedEventArgs e)
        {
            Load_data();
        }
        //Функция обработки кнопки операции с бд
        private void btnOperacii_Click(object sender, RoutedEventArgs e)
        {
            AgentWindow agent = new AgentWindow();
            agent.ShowDialog();
            Load_data();
        }
        //Функция обработки кнопки отчетов
        private void btnReportSales_Click(object sender, RoutedEventArgs e)
        {
            ReportData reportData = new ReportData();
            reportData.user = user;
            reportData.ShowDialog();
        }
        //Функция обработки кнопки отчетов
        private void btnReportBuilderSales_Click(object sender, RoutedEventArgs e)
        {
            ReportAllSales reportAllSales = new ReportAllSales();
            reportAllSales.ShowDialog();
        }
        //Функция обработки кнопки Архива
        private void btnArchive_Click(object sender, RoutedEventArgs e)
        {
            Archieve archieve = new Archieve();
            archieve.ShowDialog();
        }
        //Функция обработки кнопки отчетов
        private void btnReportsAparts_Click(object sender, RoutedEventArgs e)
        {
            ReportsAparts reportsAparts = new ReportsAparts();
            reportsAparts.ShowDialog();
        }
        //Функция обработки кнопки добавить недвижимость
        private void btnAddObject_Click(object sender, RoutedEventArgs e)
        {
            ParseObjects();
            btnAdd.IsEnabled = true;
        }

        private void btnSpravka_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"UserWindows.pdf");
        }

        private void btnReportsAparts_Click_1(object sender, RoutedEventArgs e)
        {
            ReportsAparts apart = new ReportsAparts();
            apart.ShowDialog();
        }
    }
}
