using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PreyCore
{
    static class Constants
    {
        #if(DEBUG)
        public const string PanelUrl = "http://unstable.share.cl";
        #else
        public const string PanelUrl = "http://panel.preyproject.com";
        #endif


    }
}
