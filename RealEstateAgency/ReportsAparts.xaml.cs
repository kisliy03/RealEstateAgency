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
    /// Логика взаимодействия для ReportsAparts.xaml
    /// </summary>
    public partial class ReportsAparts : Window
    {
        public ReportsAparts()
        {
            InitializeComponent();
        }

        private void btnReportSales_Click(object sender, RoutedEventArgs e)
        {
            ReportSales reportSales = new ReportSales();
            reportSales.ShowDialog();
        }

        private void btnReportRent_Click(object sender, RoutedEventArgs e)
        {
            ReportRent reportRent = new ReportRent();
            reportRent.ShowDialog();
        }

        private void btnReportClosed_Click(object sender, RoutedEventArgs e)
        {
            ReportClosed reportClosed = new ReportClosed();
            reportClosed.ShowDialog();
        }
    }
}
