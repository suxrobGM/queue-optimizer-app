using MedicalClinicQueue.Data;
using MedicalClinicQueue.Models;
using System.Drawing;
using System.Drawing.Printing;

namespace MedicalClinicQueue.Services
{
    public class PrinterService
    {
        private readonly PrintDocument _printDocument;
        private readonly ServiceItem _serviceItem;
        private readonly Company _company;

        public PrinterService(ServiceItem serviceItem)
        {
            _printDocument = new PrintDocument();
            _printDocument.PrintPage += PrintDocument_PrintPage;
            _serviceItem = serviceItem;
            _company = Company.GetValidatedCompany(Constants.SettingsConfigFile);
        }
        
        public void Print()
        {
            _printDocument.PrinterSettings.PrinterName = _company.Printer;
            _printDocument.Print();
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            using (var graphics = e.Graphics)
            {
                var regular = new Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Regular);
                var regularSmaller = new Font(FontFamily.GenericSansSerif, 8.0f, FontStyle.Regular);
                var bold = new Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Bold);
                var boldHigher = new Font(FontFamily.GenericSansSerif, 50.0f, FontStyle.Bold);
                var stringFormat = new StringFormat()
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center
                };

                // Company name
                graphics.DrawString(_company.Name, regular, Brushes.Black, 20, 10);
                graphics.DrawLine(Pens.Black, 10, 30, 210, 30);

                // Number and time
                graphics.DrawString($"Время: {_serviceItem.LastTimestamp.ToString("dd.MM.yyyy hh:mm")}", bold, Brushes.Black, 20, 50);
                graphics.DrawString(_serviceItem.QueueCount.ToString(), boldHigher, Brushes.Black, 55, 65);

                // Doctor name section
                var doctorNameSF = graphics.MeasureString(_serviceItem.Name, bold, 200, stringFormat);
                graphics.DrawRectangle(Pens.Black, 10, 150, 200, 70);
                //graphics.DrawString(_serviceItem.Name, bold, Brushes.Black, 20, 162);
                graphics.DrawString(_serviceItem.Name, bold, Brushes.Black, new RectangleF(new PointF(20, 160), doctorNameSF), stringFormat);

                // Contacts section        
                //graphics.DrawString($"Контакты:", regular, Brushes.Black, 10, 210);
                //graphics.DrawRectangle(Pens.Black, 10, 225, 200, 120);
                var contactsSF = graphics.MeasureString(_company.Contacts, regularSmaller, 200);
                graphics.DrawString(_company.Contacts, regularSmaller, Brushes.Black, new RectangleF(new Point(13, 250), contactsSF), StringFormat.GenericTypographic);
                graphics.DrawLine(Pens.Black, 10, 300, 210, 300);
            }
        }        
    }
}
