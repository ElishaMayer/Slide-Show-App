using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using SlideShow_1._1;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media;

namespace SlideShow_1._0
{
    /// <summary>
    /// The main page ( Shows the picturs ).
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //Time to wait between pictures in seconds
        private int WaitTime = 20;

        StorageFile[] pictures;
        int index = -1;

        bool pasue = false;

        bool Shuffle = true;
        Random rand = new Random();
        int prew = 0;

        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;

            // load a setting that is local to the device
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            bool? Value1 = localSettings.Values["ShowWelocme"] as bool?;
            if (Value1 == null)
            {
                Welcome dialog = new Welcome();
                dialog.DialogClosed += Dialog_DialogClosed1;
                dialog.ShowAsync();            
            }

            // load a setting that is local to the device
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            int? Value = localSettings.Values["interval"] as int?;
            if (Value != null)
                WaitTime = (int)Value;
            else
            {
                // Save a setting locally on the device
                localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["interval"] = WaitTime;
            }

            // load a setting that is local to the device
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            bool? Value2 = localSettings.Values["shuffle"] as bool?;
            if (Value2 != null)
                Shuffle = (bool)Value2;
            else
            {
                // Save a setting locally on the device
                localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["shuffle"] = Shuffle;
            }

        }

        private void Dialog_DialogClosed1(object sender, EventArgs e)
        {
            try
            {
                Welcome welcome = sender as Welcome;
                if (welcome.ShowNext)
                {
                    // Save a setting locally on the device
                    ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                    localSettings.Values["ShowWelocme"] = true;
                }
            }
            catch { }
        }



        /// <summary>
        /// When Page is Loaded then load pictures
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                var folder = KnownFolders.PicturesLibrary;
                if (folder != null)
                {
                    //Get all the images (jpg , png) in the pictures folder
                    var queryOptions = new QueryOptions(CommonFileQuery.OrderByDate, new[] { ".png", ".jpg" })
                    {
                        FolderDepth = FolderDepth.Shallow
                    };
                    var queryResult = folder.CreateFileQueryWithOptions(queryOptions);
                    IReadOnlyList<StorageFile> files = await queryResult.GetFilesAsync();
                    pictures = files.ToArray();
                }

                 
                var UsbPictures = await KnownFolders.RemovableDevices.GetAllFilesAsync();
                //var Usb = Usbs.FirstOrDefault();
                //var pic = await Removebledrives.GetAllFilesAsync();
                pictures = pictures.Union(UsbPictures.ToArray()).ToArray();
                //var folders = await Usb.GetFoldersAsync();
                //folder = folders.FirstOrDefault();

                //if (folder != null)
                //{
                //    //Get all the images (jpg , png) in the pictures folder
                //    var queryOptions = new QueryOptions(CommonFileQuery.OrderByDate, new[] { ".png", ".jpg" })
                //    {
                //        FolderDepth = FolderDepth.Shallow
                //    };
                //    var queryResult = folder.CreateFileQueryWithOptions(queryOptions);
                //    IReadOnlyList<StorageFile> files = await queryResult.GetFilesAsync();
                //    pictures = files.ToArray();
                //}
                //// GetAllFiles(folder,aaa);



                ring.Visibility = Visibility.Collapsed;
                ring.IsActive = false;
                this.Focus(FocusState.Keyboard);
                //main loop
                while (true)
                {
                    if(!pasue)
                        ShowNextPicture();
                    await Task.Delay(WaitTime * 1000);
                }
            }
            catch (Exception ex)
            {
                //Show exception message
                image.Visibility = Visibility.Collapsed;
                errorM.Visibility = Visibility.Visible;
                errorM.Text = ex.Message;
            }
        }

      
        private StorageFile GetNextPicture()
        {
            try
            {
                if (!Shuffle)
                {
                    if (index == pictures.Length - 1)
                        index = -1;
                    return pictures[++index];
                }
                else
                {
                    int num;
                    do
                    {
                        num = rand.Next(0, pictures.Length - 1);
                    } while (num == prew);
                    return pictures[num];
                }
            }
            catch { return null; }
        }

        private StorageFile GetPrewPicture()
        {
            try
            {
                if (index == 0)
                    index = pictures.Length;
                return pictures[--index];
            }
            catch { return null; }

        }


        private async void ShowNextPicture()
        {
            try
            {
                using (IRandomAccessStream fileStream = await GetNextPicture().OpenAsync(FileAccessMode.Read))
                {
                    // Set the image source to the selected bitmap 
                    BitmapImage bitmapImage = new BitmapImage();
                    //Optional set width and height
                    bitmapImage.DecodePixelWidth = 1920;
                    //  bitmapImage.DecodePixelHeight = 1080;
                    await bitmapImage.SetSourceAsync(fileStream);
                    image.Source = bitmapImage;
                }
            }
            catch { }
        }

        private async void ShowPrewPicture()
        {
            try
            {
                using (IRandomAccessStream fileStream = await GetPrewPicture().OpenAsync(FileAccessMode.Read))
                {
                    // Set the image source to the selected bitmap 
                    BitmapImage bitmapImage = new BitmapImage();
                    //Optional set width and height
                    bitmapImage.DecodePixelWidth = 1920;
                    //  bitmapImage.DecodePixelHeight = 1080;
                    await bitmapImage.SetSourceAsync(fileStream);
                    image.Source = bitmapImage;
                }
            }
            catch { }
        }


        private async void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            try
            {
                if (args.VirtualKey == Windows.System.VirtualKey.Right)
                {
                    ShowNextPicture();
                }
                else if (args.VirtualKey == Windows.System.VirtualKey.Left)
                {
                    ShowPrewPicture();
                }
                else if (args.VirtualKey == Windows.System.VirtualKey.F1)
                {
                    var popups = VisualTreeHelper.GetOpenPopups(Window.Current);
                    foreach (var popup in popups)
                    {
                        if (popup.Child is ContentDialog)
                        {
                            return;
                        }
                    }
                    ChangeTime dialog = new ChangeTime(WaitTime, Shuffle);
                    dialog.DialogClosed += Dialog_DialogClosed;
                    dialog.ShowAsync();

                }
                else if (args.VirtualKey == Windows.System.VirtualKey.F11)
                {
                    var view = ApplicationView.GetForCurrentView();
                    if (view.IsFullScreenMode)
                    {
                        view.ExitFullScreenMode();
                    }
                    else
                    {
                        var succeeded = view.TryEnterFullScreenMode();
                    }
                }
                else if (args.VirtualKey == Windows.System.VirtualKey.Space)
                {
                    var popups = VisualTreeHelper.GetOpenPopups(Window.Current);
                    foreach (var popup in popups)
                    {
                        if (popup.Child is ContentDialog)
                        {
                            return;
                        }
                    }
                    if (pasue == false)
                    {
                        pasue = true;
                        MessageBox.Show("Paused", 1000);
                    }
                    else
                    {
                        pasue = false;
                        MessageBox.Show("Continue", 1000);
                    }
                }
                else if (args.VirtualKey == Windows.System.VirtualKey.F2)
                {
                    var picker = new Windows.Storage.Pickers.FileOpenPicker();
                    picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                    picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
                    picker.FileTypeFilter.Add(".jpg");
                    picker.FileTypeFilter.Add(".jpeg");
                    picker.FileTypeFilter.Add(".png");
                    var files = await picker.PickMultipleFilesAsync();
                    if (files != null && files.ToArray().Count() != 0)
                    {

                        pictures = files.ToArray();
                        index = 0;
                        ShowNextPicture();
                        this.Focus(FocusState.Keyboard);
                    }
                }
            }
            catch { }
        }

        private void Dialog_DialogClosed(object sender, EventArgs e)
        {
            try
            {
                ChangeTime dialog = sender as ChangeTime;
                if (dialog.Update)
                {
                    WaitTime = dialog.IntervalNum;
                    Shuffle = dialog.Shuffle;
                    MessageBox.Show("New Settings saved.", 1000);
                }

                // Save a setting locally on the device
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["interval"] = WaitTime;
                // Save a setting locally on the device
                localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["shuffle"] = Shuffle;
            }
            catch { }
        }
    }

    static class Extention
    {
        public static async Task<IEnumerable<StorageFile>> GetAllFilesAsync(this StorageFolder folder)
        {
            IEnumerable<StorageFile> files = await folder.GetFilesAsync();
            IEnumerable<StorageFolder> folders = await folder.GetFoldersAsync();
            foreach (StorageFolder subfolder in folders)
                files = files.Concat(await subfolder.GetAllFilesAsync());
            return files;
        }

    }
}
