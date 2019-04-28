using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace SlideShow_1._0
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {

            StorageFolder Folder = KnownFolders.PicturesLibrary;
            if (Folder != null)
            {
                var query = CommonFileQuery.OrderByDate;
                var queryOptions = new QueryOptions(query, new[] { ".png", ".jpg" });
                queryOptions.FolderDepth = FolderDepth.Shallow;
                var queryResult = Folder.CreateFileQueryWithOptions(queryOptions);
                var files = await queryResult.GetFilesAsync();
                if (files != null)
                {
                    while (true)
                    {
                        foreach (var file in files)
                        {
                            using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                            {
                                // Set the image source to the selected bitmap 
                                BitmapImage bitmapImage = new BitmapImage();
                                bitmapImage.DecodePixelWidth = 1920; //match the target Image.Width, not shown
                                bitmapImage.DecodePixelHeight = 1080; //match the target Image.Width, not shown

                                await bitmapImage.SetSourceAsync(fileStream);
                                im.Source = bitmapImage;
                                await Task.Delay(20000);

                            }
                        }
                    }
                }
            }
        }



    }
}
