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
    /// Логика взаимодействия для ReportAllSales.xaml
    /// </summary>
    public partial class ReportAllSales : Window
    {
        public ReportAllSales()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(lol);
        }
        
        private void lol(object sender, RoutedEventArgs e)
        {
            this.Report.ReportPath = System.IO.Path.Combine(Environment.CurrentDirectory, "sales.rdl");
            this.Report.RefreshReport();
        }
    }
}
