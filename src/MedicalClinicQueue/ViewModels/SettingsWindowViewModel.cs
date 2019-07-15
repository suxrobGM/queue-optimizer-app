using System.IO;
using System.Windows;
using Newtonsoft.Json;
using Prism.Commands;
using MedicalClinicQueue.Models;
using System.Drawing.Printing;
using System.Collections.ObjectModel;
using MedicalClinicQueue.Data;

namespace MedicalClinicQueue.ViewModels
{
    public class SettingsWindowViewModel
    {
        public ObservableCollection<string> PrintersList { get; set; }
        public Company Company { get; set; }
        public DelegateCommand SaveCommand { get; set; }

        public SettingsWindowViewModel()
        {
            PrintersList = new ObservableCollection<string>();
            Company = Company.GetValidatedCompany(Constants.SettingsConfigFile);

            SaveCommand = new DelegateCommand(() =>
            {
                var jsonData = JsonConvert.SerializeObject(Company, Formatting.Indented);
                File.WriteAllText("Settings.json", jsonData);
                MessageBox.Show("Настройки успешно сохранено", "Настройки", MessageBoxButton.OK, MessageBoxImage.Information);
            });

            foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
            {
                PrintersList.Add(installedPrinter);
            }
        }
    }
}
