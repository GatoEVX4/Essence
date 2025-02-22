using System;
using System.Net.Http;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Essence.Elements
{
    public partial class GameThingy : UserControl
    {
        private static readonly HttpClient client = new HttpClient();
        public string ImageURL = "";
        public GameThingy() => this.InitializeComponent();

        public bool imagefailed = false;
        public string _id = "";
        public string image = "";
        public GameThingy(string title = "", string image = "", string id = "")
        {
            _id = id;
            SnapsToDevicePixels = true;
            UseLayoutRounding = true;
            this.InitializeComponent();
            ImageBrush background404 = (ImageBrush)this.BorderImg.Background;
            BitmapImage bitmapImage = new BitmapImage();

            try
            {
                bitmapImage.BeginInit();
                bitmapImage.DownloadFailed += (EventHandler<ExceptionEventArgs>)((sender, e) =>
                {
                    imagefailed = true;
                    this.BorderImg.Background = background404;
                });
                bitmapImage.UriSource = new Uri(image);
                bitmapImage.EndInit();

                this.BorderImg.Background = new ImageBrush()
                {
                    ImageSource = (ImageSource)bitmapImage,
                    Opacity = 0.9,
                    Stretch = Stretch.UniformToFill
                };
            }
            catch
            {
                this.BorderImg.Background = background404;
            }
        }
    }
}
