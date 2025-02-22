using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Essence.Elements
{
    public partial class ScriptThingy : UserControl
    {
        private static readonly HttpClient client = new HttpClient();

        public bool Salvo = false;
        public string ImageURL = "";

        public ScriptObject scriptObject;
        public ScriptThingy() => this.InitializeComponent();

        public bool imagefailed = false;
        public string script = "";
        public string _id = "";
        public string image = "";
        public string views = "";
        public ScriptThingy(ScriptObject scriptObj = null, string title = "", string script = "", string views = "", string credit = "", string image = "", string id = "")
        {
            if (scriptObj == null)
            {
                SnapsToDevicePixels = true;
                UseLayoutRounding = true;
                this.InitializeComponent();

                if (image != "")
                {
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
                        Opacity = 0.65,
                        Stretch = Stretch.UniformToFill
                    };
                }

                this.ScriptTitle.Text = title.Length > 30 ? title.Substring(0, 30) + "..." : title;
                this.Credit.Text = credit;
                this.Views.Text = "Views: " + views;
                this.script = script;
                this._id = id;
                this.image = image;
                this.views = views;
            }
            else
            {
                ScriptThingy scriptResult = this;

                this.scriptObject = scriptObj;
                scriptObj.Correct();

                SnapsToDevicePixels = true;
                UseLayoutRounding = true;
                this.InitializeComponent();
                ImageBrush background404 = (ImageBrush)this.BorderImg.Background;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.DownloadFailed += (EventHandler<ExceptionEventArgs>)((sender, e) =>
                {
                    imagefailed = true;
                    scriptResult.BorderImg.Background = background404;
                });

                image = scriptObj.game.imageUrl;

                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(scriptObj.game.imageUrl);
                bitmapImage.EndInit();
                this.BorderImg.Background = new ImageBrush()
                {
                    ImageSource = (ImageSource)bitmapImage,
                    Opacity = 0.65
                };

                this.ScriptTitle.Text = scriptObj.title.Length > 30 ? scriptObj.title.Substring(0, 30) + "..." : scriptObj.title;
                this.Credit.Text = scriptObj.owner != null ? scriptObj.owner.username : scriptObj.game.name;
                this.Views.Text = "Views: " + scriptObj.views;
            }
        }
    }
}
