using System;
using System.Net.Http;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Essence.Elements
{
    public partial class SpecialScripts : UserControl
    {
        private static readonly HttpClient client = new HttpClient();

        public string ImageURL = "";
        public SpecialScripts() => this.InitializeComponent();

        public bool imagefailed = false;
        public string script = "";
        public string image = "";
        public SpecialScripts(string title = "", string script = "", string image = "")
        {
            SnapsToDevicePixels = true;
            UseLayoutRounding = true;
            this.InitializeComponent();

            ImageBrush background404 = (ImageBrush)this.BorderImg.Background;

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.DownloadFailed += (EventHandler<ExceptionEventArgs>)((sender, e) =>
            {
                imagefailed = true;
                BorderImg.Background = background404;
            });

            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(image);
            bitmapImage.EndInit();
            this.BorderImg.Background = new ImageBrush()
            {
                ImageSource = (ImageSource)bitmapImage,
                Opacity = 0.9,
                Stretch = Stretch.Uniform
            };

            this.ScriptTitle.Content = title;
            //this.Credit.Content = credit;
            this.script = script;
            this.image = image;
        }
    }
}
