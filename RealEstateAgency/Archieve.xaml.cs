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
using System.Windows.Shapes;

namespace RealEstateAgency
{
    /// <summary>
    /// Логика взаимодействия для Archieve.xaml
    /// </summary>
    public partial class Archieve : Window
    {
        RealEstateAgencyEntities db = new RealEstateAgencyEntities();

        public Archieve()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            db = new RealEstateAgencyEntities();
            InitializeComponent();
            Load_data();
        }

        List<Archive> GetArchieves() { return db.Archive.ToList(); }

        public void Load_data()
        {
            var archieve =
                from Archi in GetArchieves()

                select new
                {
                    ДатаПродажи = Archi.date_sale.ToShortDateString(),
                    Продавец = db.Owners.Where(x => x.id == Archi.idOwner).FirstOrDefault().Name + " " + db.Owners.Where(x => x.id == Archi.idOwner).FirstOrDefault().Surname,
                    Покупатель = db.Client.Where(x => x.id == Archi.idClient).FirstOrDefault().Surname + " " + db.Client.Where(x => x.id == Archi.idClient).FirstOrDefault().Name,
                    Наименование = db.Apartments.Where(x => x.id == Archi.idApartment).FirstOrDefault().Title,
                    Адрес = db.Apartments.Where(x => x.id == Archi.idApartment).FirstOrDefault().Address,
                    Район = db.Apartments.Where(x => x.id == Archi.idApartment).FirstOrDefault().Region.TItile,
                    Комнаты = db.Apartments.Where(x => x.id == Archi.idApartment).FirstOrDefault().Rooms,
                    Площадь = db.Apartments.Where(x => x.id == Archi.idApartment).FirstOrDefault().Area,
                    Этаж = db.Apartments.Where(x => x.id == Archi.idApartment).FirstOrDefault().Floor,
                    Цена = db.Apartments.Where(x => x.id == Archi.idApartment).FirstOrDefault().Price
                };

            dgArchieve.ItemsSource = archieve;
        }
    }
}
