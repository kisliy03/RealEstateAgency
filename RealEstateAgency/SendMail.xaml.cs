using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
    /// Логика взаимодействия для SendMail.xaml
    /// </summary>
    public partial class SendMail : Window
    {
        RealEstateAgencyEntities db = new RealEstateAgencyEntities();
        string path = "";
        int idTempClient = 0;
        int idTempOwner = 0;
        public SendMail(string path, int idCl, int idOw)
        {
            idTempClient = idCl;
            idTempOwner = idOw;
            this.path = path;
            db = new RealEstateAgencyEntities();
            InitializeComponent();
        }

        public static System.Net.Mail.MailMessage CreateMail(string name, string emailFrom, string emailTo, string subject, string body)
        {
            var from = new MailAddress(emailFrom, name);
            var to = new MailAddress(emailTo);
            var mail = new System.Net.Mail.MailMessage(from, to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            return mail;
        }

        public static void SendEmail(string host, int snptPort, string emailFrom, string pass, System.Net.Mail.MailMessage mail)
        {
            SmtpClient smtp = new SmtpClient(host, snptPort);
            smtp.Credentials = new NetworkCredential(emailFrom, pass);
            smtp.EnableSsl = true;

            smtp.Send(mail);
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbClient.IsChecked == true && cbOwner.IsChecked == true)
                {
                    var emTo = db.Client.Where(x => x.id == idTempClient).FirstOrDefault();

                    var mail = CreateMail("Менеджер", "yaparser1208@gmail.com", emTo.Email, "Договор", "Договор");
                    mail.Attachments.Add(new Attachment(path));
                    SendEmail("smtp.gmail.com", 587, "yaparser1208@gmail.com", "zybwpilqxozerseg", mail);

                    var emTo1 = db.Owners.Where(x => x.id == idTempOwner).FirstOrDefault();

                    mail = CreateMail("Менеджер", "yaparser1208@gmail.com", emTo.Email, "Договор", "Договор");
                    mail.Attachments.Add(new Attachment(path));
                    SendEmail("smtp.gmail.com", 587, "yaparser1208@gmail.com", "zybwpilqxozerseg", mail);

                }
                else if (cbOwner.IsChecked == true)
                {
                    var emTo = db.Owners.Where(x => x.id == idTempOwner).FirstOrDefault();

                    //var mail = CreateMail("Менеджер", "yaparser1208@gmail.com", emTo.Email, "Договор", "Договор");
                    //mail.Attachments.Add(new Attachment(path));
                    //SendEmail("smtp.gmail.com", 587, "yaparser1208@gmail.com", "zybwpilqxozerseg", mail);
                }
                else
                {
                    var emTo = db.Client.Where(x => x.id == idTempClient).FirstOrDefault();
                    var mail = CreateMail("Менеджер", "yaparser1208@gmail.com", emTo.Email, "Договор", "Договор");
                    mail.Attachments.Add(new Attachment(path));
                    SendEmail("smtp.gmail.com", 587, "yaparser1208@gmail.com", "zybwpilqxozerseg", mail);
                }


                MessageBox.Show("Успешно отпрвлено");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
