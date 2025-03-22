using ElectricStore.ApplicationData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ElectricStore
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }
        public static int CurrentClientId { get; set; }
        public static int CurrentClientRole { get; set; }
    }
}
