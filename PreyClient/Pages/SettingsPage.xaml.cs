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
using PreyClient.Utilities;

namespace PreyClient
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            ExpirationWarnFast.Checked += (e, o) => { ExpirationWarnFast.Content = "On"; };
            ExpirationWarnFast.Unchecked += (e, o) => { ExpirationWarnFast.Content = "Off"; };

            MissingSwitch.Checked += (e, o) => { MissingSwitch.Content = "On"; };
            MissingSwitch.Unchecked += (e, o) => { MissingSwitch.Content = "Off"; };

            DetachDevice.Click += (e, o) => {  };

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string pagechoicestring;
            if (NavigationContext.QueryString.TryGetValue("page", out pagechoicestring))
            {
                Loaded += delegate
                {
                    Settings.SelectedIndex = int.Parse(pagechoicestring);
                };
            }
        }
    }
}