using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SlideShow_1._1
{
    public sealed partial class MessageBox : ContentDialog
    {
        private int time = 0;
        public async static void Show(string message, int time)
        {
            MessageBox ms = new MessageBox(message, time);
            await ms.ShowAsync();
        }
        public MessageBox(string message,int time)
        {
            this.InitializeComponent();
            this.time = time;
            this.text.Text = message;
        }

        private async void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay( time);
            this.Hide();
        }
    }
}
