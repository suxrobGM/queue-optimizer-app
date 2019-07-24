using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Prism.Commands;
using MedicalClinicQueue.Models;
using MedicalClinicQueue.Views;
using MedicalClinicQueue.Data;

namespace MedicalClinicQueue.ViewModels
{
    public class MainWindowViewModel
    {
        private ApplicationDbContext _db;

        public ObservableCollection<QueueItemControl> ServiceItemControls { get; set; }
        public DelegateCommand AddServiceItemCommand { get; set; }
        public DelegateCommand ResetAllQueueCountsCommand { get; set; }
        public DelegateCommand OpenSettingsCommand { get; set; }

        public MainWindowViewModel()
        {
            ServiceItemControls = new ObservableCollection<QueueItemControl>();           

            using (_db = new ApplicationDbContext())
            {
                _db.Database.EnsureCreated();
                var serviceItems = _db.ServiceItems;
                foreach (var item in serviceItems)
                {
                    AddServiceItem(item);
                }                
            }

            AddServiceItemCommand = new DelegateCommand(() =>
            {
                using (_db = new ApplicationDbContext())
                {
                    var serviceItem = new ServiceItem() { Name = "ИМЯ_ДОКТОРА" };
                    AddServiceItem(serviceItem);

                    _db.Database.EnsureCreated();
                    _db.ServiceItems.Add(serviceItem);
                    _db.SaveChanges();
                }
            });

            ResetAllQueueCountsCommand = new DelegateCommand(() =>
            {
                using (_db = new ApplicationDbContext())
                {
                    foreach (var itemControl in ServiceItemControls)
                    {
                        itemControl.ServiceItem.QueueCount = 0;
                        var serviceItems = _db.ServiceItems;
                        foreach (var item in serviceItems)
                        {
                            item.QueueCount = 0;
                        }
                        _db.SaveChanges();
                    }
                }                
            });

            OpenSettingsCommand = new DelegateCommand(() =>
            {
                var settingsWindow = new SettingsWindow();
                settingsWindow.ShowDialog();
            });
        }
       
        private void AddServiceItem(ServiceItem serviceItem)
        {
            var queueItemControl = new QueueItemControl() { DataContext = serviceItem };
            queueItemControl.removeFromQueueBtn.Click += RemoveItemFromQueue;
            ServiceItemControls.Add(queueItemControl);
        }

        private void RemoveItemFromQueue(object sender, RoutedEventArgs e)
        {
            using (_db = new ApplicationDbContext())
            {
                var itemControl = ((sender as Button).Parent as Grid).Parent as QueueItemControl;
                ServiceItemControls.Remove(itemControl);

                var serviceItem = _db.ServiceItems.Where(i => i.Id == itemControl.ServiceItem.Id).First();
                _db.ServiceItems.Remove(serviceItem);
                _db.SaveChanges();
            }         
        }
    }
}
