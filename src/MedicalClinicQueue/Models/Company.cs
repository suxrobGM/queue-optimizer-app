using System.IO;
using System.Drawing.Printing;
using Newtonsoft.Json;
using Prism.Mvvm;
using MedicalClinicQueue.Extensions;

namespace MedicalClinicQueue.Models
{
    public class Company : BindableBase
    {
        private string _name;
        private string _contacts;
        private string _printer;

        public string Name { get => _name; set { SetProperty(ref _name, value); } }
        public string Contacts { get => _contacts; set { SetProperty(ref _contacts, value); } }
        public string Printer { get => _printer; set { SetProperty(ref _printer, value); } }

        public static Company GetValidatedCompany(string settingsJsonFile)
        {
            Company company;

            if (!File.Exists(settingsJsonFile) || string.IsNullOrWhiteSpace(File.ReadAllText(settingsJsonFile)))
            {
                company = new Company()
                {
                    Name = "ИМЯ_КОМПАНИИ",
                    Contacts = "КОНТАКТЫ_КОМПАНИИ",
                    Printer = PrinterSettings.InstalledPrinters[0]
                };

                var jsonData = JsonConvert.SerializeObject(company, Formatting.Indented);
                File.WriteAllText(settingsJsonFile, jsonData);
            }
            else
            {
                company = JsonConvert.DeserializeObject<Company>(File.ReadAllText(settingsJsonFile));
            }

            return company;
        }
    }
}
