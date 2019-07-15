using MedicalClinicQueue.Data;
using MedicalClinicQueue.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                var boldHigher = new Font(FontFamily.GenericSansSerif, 30.0f, FontStyle.Bold);

                graphics.DrawString(_company.Name, regular, Brushes.Black, 30, 10);
                graphics.DrawLine(Pens.Black, 10, 30, 200, 30);
                graphics.DrawString($"Время: {_serviceItem.LastTimestamp.ToShortTimeString()}", bold, Brushes.Black, 20, 50);
                graphics.DrawString(_serviceItem.QueueCount.ToString(), boldHigher, Brushes.Black, 70, 80);

                //Doctor name section
                graphics.DrawRectangle(Pens.Black, 10, 150, 200, 40);
                graphics.DrawString(_serviceItem.Name, bold, Brushes.Black, 20, 160);

                //Contacts section        
                graphics.DrawString($"Контакты:", regular, Brushes.Black, 10, 210);
                graphics.DrawRectangle(Pens.Black, 10, 225, 200, 120);
                var sf = graphics.MeasureString(_company.Contacts, regularSmaller, 200);
                graphics.DrawString(_company.Contacts, regularSmaller, Brushes.Black, new RectangleF(new Point(13, 230), sf), StringFormat.GenericTypographic);
            }
        }        
    }
}
