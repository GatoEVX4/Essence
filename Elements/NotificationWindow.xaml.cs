using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Essence.Elements
{
    /// <summary>
    /// Lógica interna para NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        private string text;
        private double duration;
        private bool isdiscord = false;
        private bool clicked = false;
        public NotificationWindow(string txt, double dur, bool discord, string lnk)
        {
            text = txt;
            duration = dur;
            isdiscord = discord;
            InitializeComponent();
            Loaded += NotificationWindow_Loaded;

            if (lnk != "")
            {
                link_warning.Visibility = Visibility.Visible;
                information.Visibility = Visibility.Collapsed;
                border.PreviewMouseDown += async (s, e) =>
                {
                    clicked = true;

                    if (discord)
                    {
                        RegistryKey Wolfregkey;
                        try { Wolfregkey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Essence Studios", writable: true); } catch { }
                        Wolfregkey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Essence Studios", writable: true);

                        Wolfregkey.SetValue("JoinedServer", "Yes!");
                        titulo.Text = "Thanks For joining!\nWe hope you have the best experinces with us!!!";
                    }

                    await Task.Delay(1000);
                    Process.Start(lnk);

                    await Task.Delay(3000);

                    Storyboard fadeOutStoryboard = (Storyboard)FindResource("FadeOut");
                    fadeOutStoryboard.Begin(this);
                };
            }
        }

        private async void NotificationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Defina a cor e as propriedades da notificação dependendo do contexto
            if (isdiscord)
            {
                Image.Visibility = Visibility.Visible;
                textox.Visibility = Visibility.Collapsed;

                titulo.Text = "Hey, if you still didn't joined your discord server, Please click here :)";
                titulo.Foreground = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                titulo.Margin = new Thickness(70, 0, 0, 0);

                Gradient.Color = Color.FromRgb(86, 98, 246);
            }

            // Posiciona a notificação no canto superior direito
            Dispatcher.Invoke(() =>
            {
                var screen = System.Windows.Forms.Screen.PrimaryScreen;
                Left = screen.WorkingArea.Right - Width - 10;
                Top = 40;
                textox.Text = text;

                foreach (var notification in MainWindow.activeNotifications)
                {
                    if (notification != this)
                    {
                        notification.Top += Height + 10; // Move as notificações existentes
                    }
                }
                MainWindow.activeNotifications.Add(this);
            }, DispatcherPriority.Send);

            // Animação de entrada
            Dispatcher.Invoke(() =>
            {
                Storyboard fadeInStoryboard = (Storyboard)FindResource("FadeIn");
                fadeInStoryboard.Begin(this);
            }, DispatcherPriority.Send);

            // Atraso dependendo do contexto
            if (isdiscord)
            {
                await Task.Delay(TimeSpan.FromSeconds(duration - 2));

                if (clicked)
                    return;
            }
            else
            {
                await Task.Delay(TimeSpan.FromSeconds(duration));
            }

            // Animação de saída
            Dispatcher.Invoke(() =>
            {
                Storyboard fadeOutStoryboard = (Storyboard)FindResource("FadeOut");
                fadeOutStoryboard.Begin(this);
            }, DispatcherPriority.Send);
        }

    }
}
