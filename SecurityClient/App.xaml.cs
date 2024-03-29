﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using ChipSecurityCore;

namespace SecurityClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var view = new SecurityClientView();
            var vm = new SecurityClientViewModel(new AccessService());
            view.DataContext = vm;
            view.Show();
        }
    }
}
