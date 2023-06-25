using Aspose.Cells.Drawing;
using BoldReports.Processing.ObjectModel;
using Syncfusion.Windows.Shared;
using Syncfusion.XlsIO.Implementation.PivotAnalysis;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace RealEstateAgency
{
    /// <summary>
    /// Логика взаимодействия для Choose.xaml
    /// </summary>
    public partial class Choose : Window
    {
        RealEstateAgencyEntities db = new RealEstateAgencyEntities();
        Apartments apart = new Apartments();
        public User user = new User();
        public int idTempOwner = 0;
        public int idTempClient = 0;

        public Choose(Apartments apartments)
        {
            InitializeComponent();
            db = new RealEstateAgencyEntities();
            apart = apartments;
        }

        private void cbPokypatel_Loaded(object sender, RoutedEventArgs e)
        {
            cbPokypatel.Items.Clear();
            var list = db.Client.ToList();
            foreach (var item in list)
            {
                cbPokypatel.Items.Add(item.Surname + " " + item.Name + " " + item.lastName);
            }
        }

        private void cbOwner_Loaded(object sender, RoutedEventArgs e)
        {
            cbOwner.Items.Clear();
            var list = db.Owners.ToList();
            foreach (var item in list)
            {
                cbOwner.Items.Add(item.Name + " " + item.Surname + " " + item.lastName);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cbOwner.SelectedItem != null && cbPokypatel.SelectedItem != null)
            {
                var pokyp = cbPokypatel.Text.Split(' ')[0];
                var ow = cbOwner.Text.Split(' ')[0];
                var pokypatel = db.Client.Where(x => x.Surname == pokyp).FirstOrDefault();
                var owner = db.Owners.Where(x => x.Name == ow).FirstOrDefault();
                var ap = db.Apartments
                            .Where(c => c.id == apart.id)
                            .FirstOrDefault();
                idTempClient = pokypatel.id;
                idTempOwner = owner.id;

                if (ap.Status == "Сдается")   
                {
                    try
                    {
                        DateTime certainDate;

                        if (!DateTime.TryParse(tbDate.Text, out certainDate))
                        {
                            tbDate.ToolTip = "Поле введено не корректно!";
                            tbDate.Background = Brushes.LightCoral;
                            return;
                        }
                        else if (certainDate < DateTime.Now)
                        {
                            tbDate.ToolTip = "Поле введено не корректно!";
                            tbDate.Background = Brushes.LightCoral;
                            return;
                        }
                        else
                        {
                            tbDate.ToolTip = "";
                            tbDate.Background = Brushes.Transparent;
                        }

                        Sales s = new Sales();
                        s.idClient = pokypatel.id;
                        s.idApartment = apart.id;
                        s.idOwner = owner.id;
                        s.date_sale = DateTime.Now;
                        s.CertainDate = certainDate;
                        s.idUser = user.id;

                        Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
                        app.Visible = true;
                        Microsoft.Office.Interop.Word.Document doc = app.Documents.Open(@"C:\Users\Kisliy\source\repos\RealEstateAgency\RealEstateAgency\bin\Debug\dogArendi.docx");
                        doc.Bookmarks["Дата"].Range.Text = s.date_sale.ToShortDateString();
                        doc.Bookmarks["Адрес"].Range.Text = apart.Address;
                        doc.Bookmarks["Площадь"].Range.Text = apart.Area.ToString();
                        doc.Bookmarks["КоличествоКомнат"].Range.Text = apart.Rooms.ToString();
                        doc.Bookmarks["Владелец"].Range.Text = owner.Surname + " " + owner.Name + " " + owner.lastName;
                        doc.Bookmarks["Покупатель"].Range.Text = pokypatel.Surname + " " + pokypatel.Name + " " + pokypatel.lastName;
                        doc.Bookmarks["Срок"].Range.Text = certainDate.ToShortDateString();
                        doc.Bookmarks["Этаж"].Range.Text = apart.Floor.ToString();

                        doc.SaveAs2(@"C:\Users\Kisliy\source\repos\RealEstateAgency\RealEstateAgency\dog.doc");

                        doc = app.Documents.Open(@"C:\Users\Kisliy\source\repos\RealEstateAgency\RealEstateAgency\bin\Debug\Prilojenie.doc");
                        doc.Bookmarks["emailПокупатель"].Range.Text = pokypatel.Email;
                        doc.Bookmarks["emailПродавец"].Range.Text = owner.email;
                        doc.Bookmarks["Дата"].Range.Text = s.date_sale.ToShortDateString();
                        doc.Bookmarks["Дата1"].Range.Text = s.date_sale.ToShortDateString();
                        doc.Bookmarks["НазваниеКвартиры"].Range.Text = apart.Title;
                        doc.Bookmarks["Адрес"].Range.Text = apart.Address;
                        doc.Bookmarks["Площадь"].Range.Text = apart.Area.ToString();
                        doc.Bookmarks["КоличествоКомнат"].Range.Text = apart.Rooms.ToString();
                        doc.Bookmarks["Этаж"].Range.Text = apart.Floor.ToString();
                        doc.Bookmarks["Район"].Range.Text = apart.Region.TItile;
                        doc.Bookmarks["Продавец"].Range.Text = owner.Surname + " " + owner.Name + " " + owner.lastName;
                        doc.Bookmarks["Продавец1"].Range.Text = owner.Surname + " " + owner.Name + " " + owner.lastName;
                        doc.Bookmarks["Покупатель"].Range.Text = pokypatel.Surname + " " + pokypatel.Name + " " + pokypatel.lastName;
                        doc.Bookmarks["Покупатель1"].Range.Text = pokypatel.Surname + " " + pokypatel.Name + " " + pokypatel.lastName;
                        doc.Bookmarks["ТелефонПокупатель"].Range.Text = pokypatel.Phone;
                        doc.Bookmarks["ТелефонПродавец"].Range.Text = owner.Phone;
                        doc.Bookmarks["ПаспортПокупатель"].Range.Text = pokypatel.Passport;
                        doc.Bookmarks["ПаспортПродавец"].Range.Text = owner.PassportNumber;
                        doc.Bookmarks["Цена"].Range.Text = apart.Price.ToString();

                        doc.SaveAs2(@"C:\Users\Kisliy\source\repos\RealEstateAgency\RealEstateAgency\Приложение.doc");

                        Archive archive = new Archive();
                        archive.date_sale = DateTime.Now;
                        archive.idOwner = owner.id;
                        archive.idClient = pokypatel.id;
                        archive.idApartment = apart.id;
                        archive.CertainDate = Convert.ToDateTime(tbDate.Text);

                        ap.Status = "Закрыто";
                        db.Sales.Add(s);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                else if (ap.Status == "Продается")
                {
                    try
                    {
                        Sales s = new Sales();
                        s.idClient = pokypatel.id;
                        s.idApartment = apart.id;
                        s.idOwner = owner.id;
                        s.date_sale = DateTime.Now;
                        s.idUser = user.id;

                        Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
                        app.Visible = true;
                        Microsoft.Office.Interop.Word.Document doc = app.Documents.Open(@"C:\Users\Kisliy\source\repos\RealEstateAgency\RealEstateAgency\bin\Debug\dogovor.docx");
                        doc.Bookmarks["Дата"].Range.Text = s.date_sale.ToShortDateString();
                        doc.Bookmarks["Адрес"].Range.Text = apart.Address;
                        doc.Bookmarks["Площадь"].Range.Text = apart.Area.ToString();
                        doc.Bookmarks["КоличествоКомнат"].Range.Text = apart.Rooms.ToString();
                        doc.Bookmarks["Продавец"].Range.Text = owner.Surname + " " + owner.Name + " " + owner.lastName;
                        doc.Bookmarks["Продавец1"].Range.Text = owner.Surname + " " + owner.Name + " " + owner.lastName;
                        doc.Bookmarks["Покупатель"].Range.Text = pokypatel.Surname + " " + pokypatel.Name + " " + pokypatel.lastName;
                        doc.Bookmarks["Покупатель1"].Range.Text = pokypatel.Surname + " " + pokypatel.Name + " " + pokypatel.lastName;
                        doc.Bookmarks["ПокупательПасспорт"].Range.Text = pokypatel.Passport;
                        doc.Bookmarks["ПродавецПасспорт"].Range.Text = owner.PassportNumber;
                        doc.Bookmarks["Цена"].Range.Text = apart.Price.ToString();
                        doc.Bookmarks["Цена1"].Range.Text = apart.Price.ToString();

                        doc.SaveAs2(@"C:\Users\Kisliy\source\repos\RealEstateAgency\RealEstateAgency\dog.doc");

                        doc = app.Documents.Open(@"C:\Users\Kisliy\source\repos\RealEstateAgency\RealEstateAgency\bin\Debug\Prilojenie.doc");
                        doc.Bookmarks["emailПокупатель"].Range.Text = pokypatel.Email;
                        doc.Bookmarks["emailПродавец"].Range.Text = owner.email;
                        doc.Bookmarks["Дата"].Range.Text = s.date_sale.ToShortDateString();
                        doc.Bookmarks["Дата1"].Range.Text = s.date_sale.ToShortDateString();
                        doc.Bookmarks["НазваниеКвартиры"].Range.Text = apart.Title;
                        doc.Bookmarks["Адрес"].Range.Text = apart.Address;
                        doc.Bookmarks["Площадь"].Range.Text = apart.Area.ToString();
                        doc.Bookmarks["КоличествоКомнат"].Range.Text = apart.Rooms.ToString();
                        doc.Bookmarks["Этаж"].Range.Text = apart.Floor.ToString();
                        doc.Bookmarks["Район"].Range.Text = apart.Region.TItile;
                        doc.Bookmarks["Продавец"].Range.Text = owner.Surname + " " + owner.Name + " " + owner.lastName;
                        doc.Bookmarks["Продавец1"].Range.Text = owner.Surname + " " + owner.Name + " " + owner.lastName;
                        doc.Bookmarks["Покупатель"].Range.Text = pokypatel.Surname + " " + pokypatel.Name + " " + pokypatel.lastName;
                        doc.Bookmarks["Покупатель1"].Range.Text = pokypatel.Surname + " " + pokypatel.Name + " " + pokypatel.lastName;
                        doc.Bookmarks["ТелефонПокупатель"].Range.Text = pokypatel.Phone;
                        doc.Bookmarks["ТелефонПродавец"].Range.Text = owner.Phone;
                        doc.Bookmarks["ПаспортПокупатель"].Range.Text = pokypatel.Passport;
                        doc.Bookmarks["ПаспортПродавец"].Range.Text = owner.PassportNumber;
                        doc.Bookmarks["Цена"].Range.Text = apart.Price.ToString();

                        doc.SaveAs2(@"C:\Users\Kisliy\source\repos\RealEstateAgency\RealEstateAgency\Приложение.doc");
                        
                        Archive archive = new Archive();
                        archive.date_sale = DateTime.Now;
                        archive.idOwner = owner.id;
                        archive.idClient = pokypatel.id;
                        archive.idApartment = apart.id;

                        ap.Status = "Закрыто";
                        db.Sales.Add(s);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Квартира не продается/сдается!");
                }
            }
            else
            {
                MessageBox.Show("Выберите покупателя и продавца!");
            }
        }
    }
}
