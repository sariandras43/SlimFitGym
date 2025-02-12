using SlimFitGym_Mobile.Components.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace SlimFitGym_Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            Routing.RegisterRoute("QrScanner", typeof(QrScanner));

            Items.Add(new ShellContent
            {
                Content = new MainPage()
            });
        }
    }
}
