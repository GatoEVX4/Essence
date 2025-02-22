using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace Essence.Elements
{
    /// <summary>
    /// Interação lógica para User.xam
    /// </summary>
    public partial class User : UserControl
    {
        public string _id = "";
        public string _name = "";
        public string _imagelink = "";
        //public string _tokenlocal = "";
        public User(string id, string name, string imagelink)
        {
            _id = id;
            _name = name;
            _imagelink = imagelink;
            //_tokenlocal = tokenlocal;

            InitializeComponent();
            Name.Text = name;
            ImageBehavior.SetAnimatedSource(AnimatedImage1, new BitmapImage(new Uri(imagelink, UriKind.RelativeOrAbsolute)));
            ImageBehavior.SetAnimatedSource(AnimatedImage2, new BitmapImage(new Uri(imagelink, UriKind.RelativeOrAbsolute)));
        }
    }
}
