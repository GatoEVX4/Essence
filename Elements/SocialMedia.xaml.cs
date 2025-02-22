using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Essence.Elements
{
    /// <summary>
    /// Interação lógica para SocialMedia.xam
    /// </summary>
    public partial class SocialMedia : UserControl
    {
        public SocialMedia()
        {
            InitializeComponent();
        }

        private void Path_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://discord.gg/Ku5HGekNQw");
        }

        private void Path_PreviewMouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://www.youtube.com/channel/UCiftSp7kEWmct3RKboe1d4Q");
        }

        private void Path_PreviewMouseDown_2(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://getessence.discloud.app/");
        }
    }
}
