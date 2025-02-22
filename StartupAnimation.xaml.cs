using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using System.Windows.Shell;
using System.Windows.Threading;

namespace Essence
{
    /// <summary>
    /// Lógica interna para StartupAnimation.xaml
    /// </summary>
    public partial class StartupAnimation : Window
    {
        public StartupAnimation()
        {
            InitializeComponent();

            kkkk.Visibility = Visibility.Collapsed;
            w13.Visibility = Visibility.Collapsed;
            SpinningBorder.Visibility = Visibility.Collapsed;
            SpinningBorder2.Visibility = Visibility.Collapsed;
            WindowBorder3.Margin = new Thickness(320, 178.5, 320, 178.5);
            egggg.Margin = new Thickness(35, 0, -200, 0);
        }


        [DllImport("user32.dll")]
        static extern bool SetWindowDisplayAffinity(IntPtr hWnd, uint dwAffinity);
        private void MainWin_Loaded(object sender, RoutedEventArgs e)
        {
            //var chrome = new WindowChrome
            //{
            //    CaptionHeight = 0,  // Remove a barra de título
            //    ResizeBorderThickness = new Thickness(0),  // Mantém a redimensionabilidade
            //    GlassFrameThickness = new Thickness(0),
            //    UseAeroCaptionButtons = false,
            //};
            //WindowChrome.SetWindowChrome(this, chrome);


            //IntPtr hwnd = new WindowInteropHelper(this).Handle;
            //SetWindowDisplayAffinity(hwnd, 0x11);

            DoubleAnimation fadein = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            eee.BeginAnimation(OpacityProperty, fadein);
            Inicializar();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {

                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;

                    Width = 740;
                    Height = 457;

                    Point mousePosition = Mouse.GetPosition(System.Windows.Application.Current.MainWindow);
                    Point mousePositionOnScreen = System.Windows.Application.Current.MainWindow.PointToScreen(mousePosition);

                    double newLeft = mousePositionOnScreen.X - this.Left - (Width / 2);
                    double newTop = mousePositionOnScreen.Y - this.Top - 40;
                    this.Top = newTop;
                    this.Left = newLeft;
                }

                DragMove();

            }
            catch { }
        }


        private async void ee_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                //ermm.Text = (SpinningBorder.Width / 2).ToString();
                ggg.Radius = SpinningBorder.Width / 3;
            }, DispatcherPriority.Send);
        }

        internal DispatcherTimer RGBTime;
        private RegistryKey registryKey1;
        private async Task Inicializar()
        {
            try
            {
                await Dispatcher.InvokeAsync(async () =>
                {
                    try { registryKey1 = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Essence Studios\\Essence", writable: true); } catch { }
                    registryKey1 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Essence Studios\\Essence", writable: true);

                    if (!registryKey1.GetValueNames().Contains("Music"))
                        registryKey1.SetValue("Music", "True");

                    ThicknessAnimation lol = new ThicknessAnimation()
                    {
                        To = new Thickness(0),
                        Duration = TimeSpan.FromSeconds(0.5),
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
                    };
                    WindowBorder3.BeginAnimation(MarginProperty, lol);

                    DoubleAnimation fadein = new DoubleAnimation()
                    {
                        From = 0,
                        To = 1,
                        Duration = TimeSpan.FromSeconds(0.5)
                    };
                    kkkk.BeginAnimation(OpacityProperty, fadein);
                    kkkk.Visibility = Visibility.Visible;

                    DoubleAnimation sizeb1 = new DoubleAnimation()
                    {
                        From = 0,
                        To = 150,
                        Duration = TimeSpan.FromSeconds(0.7),
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                    };
                    w13.BeginAnimation(WidthProperty, sizeb1);
                    w13.Visibility = Visibility.Visible;
                    await Task.Delay(1000);



                    DispatcherTimer RGBTime = new DispatcherTimer(TimeSpan.FromMilliseconds(40), DispatcherPriority.Send, delegate
                    {
                        try
                        {
                            rgbRotation.Angle += 20;
                            rgbRotation2.Angle += 20;
                        }
                        catch { }
                    }, System.Windows.Application.Current.Dispatcher);
                    RGBTime.Start();

                    //DoubleAnimation sizeb2 = new DoubleAnimation()
                    //{
                    //    From = 0,
                    //    To = 10,
                    //    Duration = TimeSpan.FromSeconds(1),
                    //    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                    //};
                    //SpinningBorder.BeginAnimation(WidthProperty, sizeb2);
                    SpinningBorder.Visibility = Visibility.Visible;
                    SpinningBorder2.Visibility = Visibility.Visible;

                }, DispatcherPriority.Send);
            }
            catch { }
        }

        bool CloseCompleted;
        private void MainWin_Closing(object sender, CancelEventArgs e)
        {
            if (!CloseCompleted)
            {
                FormFadeOut.Begin();
                e.Cancel = true;
            }

            Process.GetCurrentProcess().Kill();

            //using (Dispatcher.DisableProcessing())
            //{

            //}
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            CloseCompleted = true;
            Process.GetCurrentProcess().Kill();
        }

        private async void ExitMD(object sender, MouseButtonEventArgs e)
        {
            await Task.Delay(600);
            FormFadeOut.Begin();
        }

        internal async void opsie()
        {
            WhatIsThis.Visibility = Visibility.Visible;

            lolexitbtn.Opacity = 0;
            banimage.Visibility = Visibility.Collapsed;
            banimage.Visibility = Visibility.Visible;

            var marginAnimation = new ThicknessAnimation
            {
                From = new Thickness(170, 170, 170, 170),
                To = new Thickness(10, 10, 10, 10),
                Duration = TimeSpan.FromSeconds(0.65),
                EasingFunction = new CircleEase { EasingMode = EasingMode.EaseInOut },
            };
            WindowBorder3.BeginAnimationP(MarginProperty, marginAnimation);

            try
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.death_sound2);
                player.Play();
            }
            catch { }
            killbeffect2.Opacity = 0.7;

            DoubleAnimation animlo = new DoubleAnimation()
            {
                From = 2000,
                To = 680,
                Duration = new Duration(TimeSpan.FromMilliseconds(400)),
                AutoReverse = false,
                EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseOut }
            };
            banimage.BeginAnimationP(Image.WidthProperty, animlo);

            await Task.Delay(2500);
            DoubleAnimation animlolo = new DoubleAnimation()
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromMilliseconds(500),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            killbeffect.BeginAnimationP(OpacityProperty, animlolo);

            DoubleAnimation animlolok = new DoubleAnimation()
            {
                From = 1,
                To = 0.0,
                Duration = TimeSpan.FromMilliseconds(7000),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            killbeffect2.BeginAnimationP(OpacityProperty, animlolok);

            DoubleAnimation animlolokk = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(3000),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };
            lolexitbtn.BeginAnimationP(OpacityProperty, animlolokk);
        }
    }
}
