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
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Essence.Elements
{
    /// <summary>
    /// Interação lógica para Resellers.xam
    /// </summary>
    public partial class Resellers : UserControl
    {
        public Resellers()
        {
            InitializeComponent();
        }

        internal string urll = "";

        internal ImageSource gray;
        internal ImageSource normal;


        private async void ResselerControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            await Task.Delay(300);
            if(urll != "")
                Process.Start(urll);
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            AnimateStarColor(star1, Colors.Gold);
            AnimateStarColor(star2, Colors.Gold);
            AnimateStarColor(star3, Colors.Gold);
            AnimateStarColor(star4, Colors.Gold);
            AnimateStarColor(star5, Colors.Gold);

            AnimateTextColor(dffdafdffda, Color.FromRgb(255, 255, 255));
            AnimateTextColor(dffdafda, Color.FromRgb(200, 200, 200));
            Imgggg.Source = normal;
            Imgggg.Opacity = 0.9;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            AnimateStarColor(star1, Color.FromRgb(100, 100, 100)); // Cor original (cinza)
            AnimateStarColor(star2, Color.FromRgb(100, 100, 100)); // Cor original (cinza)
            AnimateStarColor(star3, Color.FromRgb(100, 100, 100)); // Cor original (cinza)
            AnimateStarColor(star4, Color.FromRgb(100, 100, 100)); // Cor original (cinza)
            AnimateStarColor(star5, Color.FromRgb(100, 100, 100)); // Cor original (cinza)

            AnimateTextColor(dffdafdffda, Color.FromRgb(200, 200, 200));
            AnimateTextColor(dffdafda, Color.FromRgb(100, 100, 100));
            Imgggg.Source = gray;
            Imgggg.Opacity = 0.7;
        }

        private void AnimateStarColor(Path star, Color targetColor)
        {
            ColorAnimation colorAnimation = new ColorAnimation
            {
                To = targetColor,
                Duration = new Duration(TimeSpan.FromSeconds(0.3)) // Animação de 0.5 segundos
            };
            star.Fill.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }

        private void AnimateTextColor(TextBlock textBlock, Color targetColor)
        {
            ColorAnimation colorAnimation = new ColorAnimation
            {
                To = targetColor,
                Duration = new Duration(TimeSpan.FromSeconds(0.3)) // Animação de 0.5 segundos
            };
            textBlock.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }
    }
}
