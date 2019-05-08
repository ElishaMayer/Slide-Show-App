using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class ChangeTime : ContentDialog
    {
        public int IntervalNum { set; get; }
        public bool Shuffle{set;get;}
        public bool Update = false;
        public event EventHandler<EventArgs> DialogClosed;
        public ChangeTime(int defaultNum =1,bool shuffle=false)
        {
            this.InitializeComponent();
            Interval.ItemsSource = Enumerable.Range(1, 200).ToList();
            Interval.SelectedItem = defaultNum ;
            this.ShuffleCheck.IsChecked = shuffle;
            Shuffle = shuffle;

        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Update = true;
            this.Hide();
            DialogClosed?.Invoke(this, new EventArgs());

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Update = false;
            this.Hide();
            DialogClosed?.Invoke(this, new EventArgs());

        }

        private void Interval_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IntervalNum = (int)Interval.SelectedItem;
        }

        private void ShuffleCheck_Checked(object sender, RoutedEventArgs e)
        {
            Shuffle = true;
        }

        private void ShuffleCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            Shuffle = false;
        }
    }
}
