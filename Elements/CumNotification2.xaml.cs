using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Essence.Elements
{
    /// <summary>
    /// Interação lógica para CumNotification2.xam
    /// </summary>
    public partial class CumNotification2 : UserControl
    {
        private TaskCompletionSource<bool> _tcs;

        public CumNotification2(string Title, string Text)
        {
            InitializeComponent();

            titulo.Text = Title;
            textox.Text = Text;

            Fade(BlackMamba, 0, 0.7, 0.3);
            BlackMamba.Visibility = Visibility.Visible;

            Move(CumNotification, new Thickness(0, 0, 0, -600), new Thickness(0, 0, 0, 0), 0.6);
            CumNotification.Visibility = Visibility.Visible;
        }

        public Task<bool> ShowAsync()
        {
            _tcs = new TaskCompletionSource<bool>();
            // Exibe o controle
            this.Visibility = Visibility.Visible;

            // Retorna a Task para esperar o resultado
            return _tcs.Task;
        }


        public static void Fade(DependencyObject ElementName, double Start, double End, double Time)
        {
            DoubleAnimation Anims = new DoubleAnimation()
            {
                From = Start,
                To = End,
                Duration = TimeSpan.FromSeconds(Time),
                EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(Anims, ElementName);
            Storyboard.SetTargetProperty(Anims, new PropertyPath(OpacityProperty)); // well i don't actually think transparency has this effect
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(Anims);

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, () =>
            {
                storyboard.Begin();
            });
        }

        public static void Move(DependencyObject ElementName, Thickness Origin, Thickness Location, double Time)
        {
            ThicknessAnimation Anims = new ThicknessAnimation()
            {
                From = Origin,
                To = Location,
                Duration = TimeSpan.FromSeconds(Time),
                EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(Anims, ElementName);
            Storyboard.SetTargetProperty(Anims, new PropertyPath(MarginProperty));
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(Anims);

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, () =>
            {
                storyboard.Begin();
            });
        }

        private async void CloseNotf_Click(object sender, RoutedEventArgs e)
        {
            Fade(BlackMamba, 0.7, 0, 0.3);
            await Task.Delay(200);
            Move(CumNotification, new Thickness(0, 0, 0, 0), new Thickness(0, 0, 0, -600), 0.4);
            await Task.Delay(100);
            BlackMamba.Visibility = Visibility.Collapsed;

            await Task.Delay(500);
            _tcs?.SetResult(false);
        }

        private async void OKNotif_Click(object sender, RoutedEventArgs e)
        {
            Fade(BlackMamba, 0.7, 0, 0.3);
            await Task.Delay(200);
            Move(CumNotification, new Thickness(0, 0, 0, 0), new Thickness(0, 0, 0, -600), 0.4);
            await Task.Delay(100);
            BlackMamba.Visibility = Visibility.Collapsed;

            await Task.Delay(500);
            _tcs?.SetResult(true);
        }
    }
}
