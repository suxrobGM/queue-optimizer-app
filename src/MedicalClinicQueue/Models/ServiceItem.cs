using MedicalClinicQueue.Data;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalClinicQueue.Models
{
    public class ServiceItem : BindableBase
    {
        private string _name;
        private int _queueCount;
        private DateTime _lastTimestamp;

        public ServiceItem()
        {
            Id = Guid.NewGuid().ToString().Replace("-", "");
            LastTimestamp = DateTime.Now;
        }

        public string Id { get; set; }
        public string Name { get => _name; set { SetProperty(ref _name, value); } }
        public int QueueCount { get => _queueCount; set { SetProperty(ref _queueCount, value); }}
        public DateTime LastTimestamp { get => _lastTimestamp; set { SetProperty(ref _lastTimestamp, value); } }
    }
}
