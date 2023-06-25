using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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

namespace RealEstateAgency
{
    /// <summary>
    /// Логика взаимодействия для ReportData.xaml
    /// </summary>
    public partial class ReportData : Window
    {
        public User user = new User();
        RealEstateAgencyEntities db = new RealEstateAgencyEntities();
        public ReportData()
        {
            InitializeComponent();
            db = new RealEstateAgencyEntities();
        }

        private void tbConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Workbook workbookWithDataAndFormula = new Workbook("ReportSales.xlsx");

                DateTime startDate = Convert.ToDateTime(tbStartDate.Text);
                DateTime endDate = Convert.ToDateTime(tbEndDate.Text);

                Cell cellWithData = workbookWithDataAndFormula.Worksheets[0].Cells["D2"];
                cellWithData.Value = startDate.ToShortDateString();
                cellWithData = workbookWithDataAndFormula.Worksheets[0].Cells["F2"];
                cellWithData.Value = endDate.ToShortDateString();

                var owner = db.Sales.Where(x => x.idUser == user.id)
                .Where(x => x.date_sale >= startDate)
                .Where(x => x.date_sale <= endDate).ToList();

                int size = 5;
                if (owner.Count < 5)
                {
                    size = owner.Count();
                }

                for (int i = 0; i < size; i++)
                {
                    cellWithData = workbookWithDataAndFormula.Worksheets[0].Cells["B" + (i + 5).ToString()];
                    cellWithData.Value = owner[i].Apartments.Title;
                    cellWithData = workbookWithDataAndFormula.Worksheets[0].Cells["C" + (i + 5).ToString()];
                    cellWithData.Value = owner[i].date_sale.ToShortDateString();
                    cellWithData = workbookWithDataAndFormula.Worksheets[0].Cells["D" + (i + 5).ToString()];
                    //cellWithData.Value = owner[i].Apartments.region;
                    cellWithData = workbookWithDataAndFormula.Worksheets[0].Cells["E" + (i + 5).ToString()];
                    cellWithData.Value = owner[i].Apartments.Price;
                }
                cellWithData = workbookWithDataAndFormula.Worksheets[0].Cells["D" + (owner.Count() + 5).ToString()];
                cellWithData.Value = "Итого:";
                cellWithData = workbookWithDataAndFormula.Worksheets[0].Cells["B3"];
                cellWithData.Value = user.Name;
                Cell cellWithFormula = workbookWithDataAndFormula.Worksheets[0].Cells["E" + (owner.Count() + 5).ToString()];
                cellWithFormula.Formula = "=Sum(E5:E" + (owner.Count() + 4).ToString() + ")";
                workbookWithDataAndFormula.CalculateFormula();

                // Save the output workbook
                workbookWithDataAndFormula.Save("ReportSalesNew.xlsx");

                System.Windows.MessageBox.Show("Отчет сформирован");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Не верно введена дата.", "Ошибка", MessageBoxButton.OK, (MessageBoxImage)MessageBoxIcon.Warning);
            }
        }
    }
}
