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
    /// The main page ( Shows the picturs ).
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //Time to wait between pictures in seconds
        private int WaitTime = 20;
        public MainPage()
        {
            this.InitializeComponent();
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
                //get pictures folder
                StorageFolder Folder = KnownFolders.PicturesLibrary;

                if (Folder != null)
                {
                    //Get all the images (jpg , png) in the pictures folder
                    var query = CommonFileQuery.OrderByDate;
                    var queryOptions = new QueryOptions(query, new[] { ".png", ".jpg" });
                    queryOptions.FolderDepth = FolderDepth.Shallow;
                    var queryResult = Folder.CreateFileQueryWithOptions(queryOptions);
                    var files = await queryResult.GetFilesAsync();


                    if (files != null)
                    {
                        //main loop
                        while (true)
                        {
                            //go over the pictures
                            foreach (var file in files)
                            {
                                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                                {
                                    // Set the image source to the selected bitmap 
                                    BitmapImage bitmapImage = new BitmapImage();
                                    //Optional set width and height
                                    bitmapImage.DecodePixelWidth = 1920; 
                                    bitmapImage.DecodePixelHeight = 1080; 
                                    await bitmapImage.SetSourceAsync(fileStream);
                                    image.Source = bitmapImage;
                                    
                                    //wait 20 seconds
                                    await Task.Delay(WaitTime*1000);

                                }
                            }
                        }
                    }
                }
            }catch(Exception ex)
            {
                //Show exception message
                image.Visibility = Visibility.Collapsed;
                errorM.Visibility = Visibility.Visible;
                errorM.Text = ex.Message;
            }
        }



    }
}
