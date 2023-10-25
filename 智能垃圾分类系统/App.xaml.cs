using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using 智能垃圾分类系统.View;

namespace 智能垃圾分类系统
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (new LoginView().ShowDialog() == true)
            {
                new MainView().ShowDialog();
            }
            Application.Current.Shutdown();
        }
    }
}
