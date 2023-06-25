using Microsoft.Office.Interop.Word;
using Syncfusion.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Window = System.Windows.Window;

namespace RealEstateAgency
{
    /// <summary>
    /// Логика взаимодействия для ObjectCard.xaml
    /// </summary>
    public partial class ObjectCard : Window
    {
        Apartments ap = new Apartments();
        public User user = new User();
        int idClientt = 0;
        int idOwnerr = 0;
        public ObjectCard(Apartments apart)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ap = apart;

            if (apart.img != null)
            {
                Uri uri = new Uri(apart.img, UriKind.Absolute);
                ImageSource imgSource = new BitmapImage(uri);
                imageObj.Source = imgSource;
            }

            tbObjName.Text = apart.Title;
            tbMetro.Text = apart.Metro.Name;
            tbCountRooms.Text = Convert.ToString(apart.Rooms);
            tbAddress.Text = apart.Address;
            tbArea.Text = Convert.ToString(apart.Area);
            tbFloor.Text = Convert.ToString(apart.Floor);
            tbPrice.Text = Convert.ToString(apart.Price);
            tbComent.Text = apart.Comment;
        }

        private void btnCreateDog_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show($"Вы точно хотите совершить операцию!", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {

                Choose choose = new Choose(ap);
                choose.user = user;
                if (ap.Status != "Сдается")
                    choose.tbDate.Visibility = Visibility.Hidden;

                choose.ShowDialog();
                idClientt = choose.idTempClient;
                idOwnerr = choose.idTempOwner;
                btnCreateDog.IsEnabled = false;
            }

            btnSendDog.IsEnabled = true;
        }

        private void btnSendDog_Click(object sender, RoutedEventArgs e)
        {
            SendMail sm = new SendMail("C:\\Users\\Kisliy\\source\\repos\\RealEstateAgency\\RealEstateAgency\\dog.doc", idClientt, idOwnerr);
            sm.ShowDialog();
        }

        private void btnSendCard_Click(object sender, RoutedEventArgs e)
        {
            var bounds = Screen.GetBounds(System.Drawing.Point.Empty);
            using (var bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(System.Drawing.Point.Empty, System.Drawing.Point.Empty, bounds.Size);
                }
                bitmap.Save("C:\\Users\\Kisliy\\source\\repos\\RealEstateAgency\\RealEstateAgency\\img.png", System.Drawing.Imaging.ImageFormat.Png);
            }

            SendMail sm = new SendMail("C:\\Users\\Kisliy\\source\\repos\\RealEstateAgency\\RealEstateAgency\\img.png", idClientt, idOwnerr);
            sm.ShowDialog();
        }

        private void hlLink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(ap.Link);    
        }

        private void btnSpravka_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"objectCard.pdf");
        }
    }
}
