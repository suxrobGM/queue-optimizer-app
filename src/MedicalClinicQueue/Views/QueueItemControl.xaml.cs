using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using MedicalClinicQueue.Data;
using MedicalClinicQueue.Models;
using MedicalClinicQueue.Services;

namespace MedicalClinicQueue.Views
{
    /// <summary>
    /// Interaction logic for QueueItemControl.xaml
    /// </summary>
    public partial class QueueItemControl : UserControl, INotifyPropertyChanged
    {
        private ServiceItem _serviceItem;
        public ServiceItem ServiceItem { get => _serviceItem; set { _serviceItem = value; RaisePropertyChanged(); } }

        public QueueItemControl()
        {
            InitializeComponent();
            ServiceItem = new ServiceItem();

            DataContextChanged += (s, e) =>
            {
                if (DataContext is ServiceItem)
                {
                    ServiceItem = DataContext as ServiceItem;
                }
            };
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private void AddToQueueBtn_Click(object sender, RoutedEventArgs e)
        {
            var mbResult = MessageBox.Show($"Напечатить очередь № {ServiceItem.QueueCount + 1} ?", "Печать на принтер", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            if (mbResult == MessageBoxResult.Cancel)
                return;

            using (var db = new ApplicationDbContext())
            {              
                var item = db.ServiceItems.Where(i => i.Id == ServiceItem.Id).FirstOrDefault();
                if (item != null)
                {
                    item.QueueCount = ++ServiceItem.QueueCount;
                    ServiceItem.LastTimestamp = DateTime.Now;
                    item.LastTimestamp = ServiceItem.LastTimestamp;                  
                }

                try
                {
                    var printerService = new PrinterService(ServiceItem);
                    printerService.Print();
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }                
            }         
        }        

        private void ServiceNameTB_LostFocus(object sender, RoutedEventArgs e)
        {
            using (var db = new ApplicationDbContext())
            {
                var item = db.ServiceItems.Where(i => i.Id == ServiceItem.Id).FirstOrDefault();
                if (item != null)
                {
                    ServiceItem.Name = serviceNameTB.Text;
                    item.Name = ServiceItem.Name;
                    db.SaveChanges();
                }
            }
        }
    }
}
