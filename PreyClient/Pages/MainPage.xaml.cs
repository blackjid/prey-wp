using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using PreyCore;
using System.Threading.Tasks;
using Microsoft.Phone.Scheduler;
using System.Device.Location;
using PreyClient.Utilities;

namespace PreyClient
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            CreateAccountButton.Click += (o, e) =>
            {
                //App.Current.Prey.CreateAccount("Juan Ignacio Donoso", "jidonoso@gmail.com", "123456789", "123456789", "");
                App.Current.Prey.GetAccount("jidonoso@gmail.com", "s8egVyj9");
            };
        }

        
    }
}