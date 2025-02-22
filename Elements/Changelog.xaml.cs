using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Essence.Elements
{
    /// <summary>
    /// Lógica interna para Changelog.xaml
    /// </summary>
    public partial class Changelog : Window
    {
        public Changelog()
        {
            InitializeComponent();
            Opacity = 0;
        }

        bool CloseCompleted;
        bool preventc;
        internal bool ok { get; set; }
        private async void ExitMD(object sender, MouseButtonEventArgs e)
        {
            await Task.Delay(600);
            Close();
        }

        private void CloseWriters()
        {
            Console.SetOut(null);
            Console.SetError(null);
            //Console.SetError(App._originalConsoleOut);

            App._logFile?.Close();
            App._logFile?.Dispose();
        }

        private void MainWin_Closing(object sender, CancelEventArgs e)
        {
            if (!preventc)
            {
                CloseWriters();

                if (!CloseCompleted)
                {
                    FormFadeOut.Begin();
                    e.Cancel = true;
                }
                Process.GetCurrentProcess().Kill();
            }
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            if (!preventc)
            {
                CloseCompleted = true;
                Process.GetCurrentProcess().Kill();
            }
        }


        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try 
            {
                DragMove();
            }
            catch { }
        }
        private void Mini(object sender, MouseButtonEventArgs e)
        {
            base.WindowState = WindowState.Minimized;
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
        private async void finishbtn_Click(object sender, RoutedEventArgs e)
        {
            finishbtn.IsEnabled = false;

            await Task.Delay(300);
            Move(udunduwen, new Thickness(0,0,0,0), new Thickness(200, 225, 200, 225), 2.3);
            await Task.Delay(2300);

            ok = true;
            preventc = true;
            Close();
        }

        LanguageManager lm = new LanguageManager();
        private void MainWin_Loaded(object sender, RoutedEventArgs e)
        {
            Move(udunduwen, new Thickness(200, 225, 200, 225), new Thickness(0,0,0,0), 2);
            Opacity = 1;

            EVXTXT.Text = lm.Translate("What's New");
            ChangelogTXT.Text = lm.Translate("Essence Features Changelog");
            finishbtn.Content = lm.Translate("Nice!");
        }
    }
}
