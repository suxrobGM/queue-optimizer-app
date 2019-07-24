using System.Windows;
using Prism.Ioc;
using Prism.Unity;
using MedicalClinicQueue.Views;

namespace MedicalClinicQueue
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<MainWindow>();
        }
    }
}
