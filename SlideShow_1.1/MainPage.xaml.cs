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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

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

            //string faToken = ApplicationData.Current.LocalSettings.Values["faToken"] as string;
            //StorageFolder Folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(faToken);
            //  StorageFile file = null;
            StorageFolder Folder = KnownFolders.PicturesLibrary;
            if (Folder != null)
            {
                var query = CommonFileQuery.OrderByDate;
                var queryOptions = new QueryOptions(query, new[] { ".png", ".jpg" });
                queryOptions.FolderDepth = FolderDepth.Shallow;
                var queryResult = Folder.CreateFileQueryWithOptions(queryOptions);
                var files = await queryResult.GetFilesAsync();
                //  file = await GetFileAsync(Folder, @"IMG_0586.jpg");
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



        public async Task<StorageFile> GetFileAsync(StorageFolder folder, string filename)
        {
            StorageFile file = null;
            if (folder != null)
            {
                file = await folder.GetFileAsync(filename);
            }
            return file;
        }
    }
}
