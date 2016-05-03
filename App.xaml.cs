using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfTest.Model;
using WpfTest.ViewModel;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Wenskaart mijnWenskaart = new Wenskaart();
            WenskaartVM vm = new WenskaartVM(mijnWenskaart);
            MainWindow mijnMainWindow = new MainWindow();

            mijnMainWindow.DataContext = vm;
            mijnMainWindow.Show();
        }
    }
}
