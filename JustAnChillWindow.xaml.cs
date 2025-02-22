using Essence.Elements;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Ellipse = System.Windows.Shapes.Ellipse;
using System.Threading;

namespace Essence
{
    /// <summary>
    /// Lógica interna para JustAnChillWindow.xaml
    /// </summary>
    public partial class JustAnChillWindow : Window
    {
        public JustAnChillWindow()
        {
            InitializeComponent();
            this.Opacity = 0;
            CustomAD.Opacity = 0;
            Vertise.Visibility = Visibility.Collapsed;
            Premium.Visibility = Visibility.Collapsed;
            Comp.Visibility = Visibility.Collapsed;
            InviteFriends.Visibility = Visibility.Collapsed;
        }

        public static void Fade(DependencyObject ElementName, double Start, double End, double Time)
        {
            if (Start == End)
                return;

            DoubleAnimation anim = new DoubleAnimation()
            {
                From = Start,
                To = End,
                Duration = TimeSpan.FromSeconds(Time),
                EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
            };

            Storyboard storyboard = new Storyboard();
            Storyboard.SetTarget(anim, ElementName);
            Storyboard.SetTargetProperty(anim, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(anim);
            storyboard.Begin();
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
            storyboard.Begin();            
        }

        private void Mini(object sender, MouseButtonEventArgs e)
        {
            base.WindowState = WindowState.Minimized;
        }

        bool CloseCompleted;
        private async void ExitMD(object sender, MouseButtonEventArgs e)
        {
            await Task.Delay(600);
            FormFadeOut.Begin();
        }

        private void MainWin_Closing(object sender, CancelEventArgs e)
        {
            if (!CloseCompleted)
            {
                FormFadeOut.Begin();
                e.Cancel = true;
            }
            Process.GetCurrentProcess().Kill();
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            CloseCompleted = true;
            Process.GetCurrentProcess().Kill();
        }


        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                isDragging = true;
                DragMove();
                isDragging = false;
            }
            catch { }
        }


        private void RestartApp(bool warn)
        {
            if (warn)
            {
                //if (MessageBox.Show("A fatal error has occurred. do you want to restart Essence?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                //{
                //    string fileName = Process.GetCurrentProcess().MainModule.FileName;
                //    Process.Start(fileName);
                //}
            }
            else
            {
                string fileName = Process.GetCurrentProcess().MainModule.FileName;
                Process.Start(fileName);
            }

            Process.GetCurrentProcess().Kill();
        }

        private int user_r = 20;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private async Task Online()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                string xxx = await KeyGay2.Get("heartbeat", $"{KeyGay2.current_user}\n | \n{KeyGay2.current_pass}");
                if (xxx == "Disconnect!")
                {
                    MessageBox.Show("There was a problem with your Essence account and we need to close this session.");
                    RestartApp(true);
                    return;
                }

                var userData = JsonConvert.DeserializeObject<Dictionary<string, object>>(xxx.Split(new[] { "||||||||" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                if (userData != null && userData.ContainsKey("email"))
                {
                    Dispatcher.Invoke(async () =>
                    {
                        user_r = Convert.ToInt32(userData["user_role"].ToString());
                        KeyGay2.invitecode = userData["userid"].ToString();
                        KeyGay2.invites = Convert.ToInt32(userData["invites"].ToString());

                        d3uybd3ubdu3db.Text = KeyGay2.invites > 0
                            ? $"Awesome! You can redeem your {KeyGay2.invites * 3} days premium now!"
                            : "Invite friends and expand our community!";

                        LastplayedText0.Text = KeyGay2.invites > 0 ? "Click here to" : "Click here to copy it";
                        LastplayedText1.Text = KeyGay2.invites > 0 ? "Redeem Premium" : KeyGay2.invitecode;

                        invitecount.Text = $"Current invites: {KeyGay2.invites}";

                        if (KeyGay2.invites > 0)
                        {
                            adfdafa.Source = new BitmapImage(new Uri("pack://application:,,,/ImageResources/Ads+Premium/ic--round-redeem.png"));
                            ThicknessAnimation j = new ThicknessAnimation()
                            {
                                To = new Thickness(8, 30, 0, 0)
                            };
                            fdsgsgsgs.BeginAnimation(MarginProperty, j);
                            await Task.Delay(300);
                            dsfdsfsdffs.Fill = Brushes.Red;
                            f432f34f32.Color = Colors.Red;
                            await Task.Delay(500);
                            fsfeafaefae.Fill = Brushes.Red;
                            f432f34f.Color = Colors.Red;
                        }
                        else
                        {

                        }
                    });
                }

                await Task.Delay(5000);
            }
        }

        bool isDragging = false;

        LanguageManager lm = new LanguageManager();
        private RegistryKey Wolfregkey;
        private RegistryKey registryKey1;

        private async void MainWin_Loaded(object sender, RoutedEventArgs e)
        {
            try { Wolfregkey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Essence Studios", writable: true); } catch { }
            Wolfregkey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Essence Studios", writable: true);

            try { registryKey1 = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Essence Studios\\Essence", writable: true); } catch { }
            registryKey1 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Essence Studios\\Essence", writable: true);

            //Notificar($"Essence still in development. This is an BETA version. expect errors and bugs! Build {build_number}", 12);

            //Inicializar1();

            //MessageBox.Show("hii");

            //Fade(this, 0, 1, 0.6);
            //AnimateChose();

            Dispatcher.Invoke(() =>
            {
                AnimateEllipse(Ellipse1);
                AnimateEllipse(Ellipse2);
                AnimateEllipse(Ellipse3);
                //AnimateEllipse2();

                dnd3ndu3n.Text = Wolfregkey?.GetValueNames().Contains("Nome") == true
                    ? $"Hi {Wolfregkey.GetValue("Nome")}!"
                    : "Hi Essence User!";

                LastplayedText1.Text = KeyGay2.invitecode;
                invitecount.Text = $"Current invites: {KeyGay2.invites}";
            });

            if (KeyGay2.invites > 0)
            {
                d3uybd3ubdu3db.Text = $"Aweasome! You can reedem your {KeyGay2.invites * 3} days premium now!";
                LastplayedText0.Text = "Click here to";
                LastplayedText1.Text = "Reedem Premium";
            }
            else
            {
                d3uybd3ubdu3db.Text = "Invite friends and expand our community!";
                LastplayedText0.Text = "Click here to copy it";
                LastplayedText1.Text = KeyGay2.invitecode;
            }

            Online();

            DispatcherTimer LOL = new DispatcherTimer(DispatcherPriority.Render);
            LOL.Interval = TimeSpan.FromMilliseconds(25);
            LOL.Tick += (sender, e) =>
            {
                if (page != 0 || isDragging)
                    LOL.Interval = TimeSpan.FromMilliseconds(1000);
                else
                    LOL.Interval = TimeSpan.FromMilliseconds(25);

                try
                {
                    fff3.Angle -= 4;
                }
                catch { }
            };
            LOL.Start();



            compilationtxt.Text = $"Version: {ExecSettings.CurrentVersion} | Compilation: {MainWindow.build_number} ~ {MainWindow.build_date}";

            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>()
            {
                new Dictionary<string, object>()
                {
                    { "Imagem", "pack://application:,,,/ImageResources/Ads+Premium/Resellers/serenity.png" },
                    { "Nome", "Serenity" },
                    { "Url", "https://store.serenitytube.net/" },
                    { "Rate", 5 }
                },
                new Dictionary<string, object>()
                {
                    { "Imagem", "pack://application:,,,/ImageResources/Ads+Premium/Resellers/robloxcheatz.png" },
                    { "Nome", "Iyla" },
                    { "Url", "https://robloxcheatz.com/" },
                    { "Rate", 5 }
                },
                new Dictionary<string, object>()
                {
                    { "Imagem", "pack://application:,,,/ImageResources/Ads+Premium/Resellers/expresskeys.png" },
                    { "Nome", "expresskeys" },
                    { "Url", "https://expresskeys.store/" },
                    { "Rate", 5 }
                },
                new Dictionary<string, object>()
                {
                    { "Imagem", "pack://application:,,,/ImageResources/Ads+Premium/Resellers/2cdfbb01cc428b58cedf97e7614d0f60.png" },
                    { "Nome", "buywave.xyz" },
                    { "Url", "https://buywave.xyz/" },
                    { "Rate", 5 }
                }

            };


            foreach (var item in items)
            {
                try
                {
                    Elements.Resellers resellers = new Elements.Resellers();

                    BitmapImage newImage = new BitmapImage();
                    newImage.BeginInit();
                    newImage.UriSource = new Uri(item["Imagem"].ToString());
                    newImage.EndInit();

                    resellers.normal = newImage;

                    FormatConvertedBitmap convertedBitmap = new FormatConvertedBitmap();
                    convertedBitmap.BeginInit();
                    convertedBitmap.Source = newImage;
                    convertedBitmap.DestinationFormat = PixelFormats.Gray32Float;
                    convertedBitmap.EndInit();
                    resellers.Imgggg.Source = convertedBitmap;
                    resellers.gray = convertedBitmap;



                    resellers.dffdafdffda.Text = item["Nome"].ToString();
                    resellers.dffdafda.Text = item["Url"].ToString();
                    resellers.urll = item["Url"].ToString();

                    int rate = Convert.ToInt32(item["Rate"]);

                    resellers.star1.Visibility = Visibility.Collapsed;
                    resellers.star2.Visibility = Visibility.Collapsed;
                    resellers.star3.Visibility = Visibility.Collapsed;
                    resellers.star4.Visibility = Visibility.Collapsed;
                    resellers.star5.Visibility = Visibility.Collapsed;

                    if (rate >= 1)
                        resellers.star1.Visibility = Visibility.Visible;
                    if (rate >= 2)
                        resellers.star2.Visibility = Visibility.Visible;
                    if (rate >= 3)
                        resellers.star3.Visibility = Visibility.Visible;
                    if (rate >= 4)
                        resellers.star4.Visibility = Visibility.Visible;
                    if (rate == 5)
                        resellers.star5.Visibility = Visibility.Visible;

                    feaifnfnm.Children.Add(resellers);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                await Task.Delay(2400);
            }
        }

        ColorAnimation o = new ColorAnimation()
        {
            From = Color.FromRgb(200, 200, 200),
            To = Color.FromRgb(255, 0, 0),
            Duration = TimeSpan.FromSeconds(1.3)
        };

        ColorAnimation om = new ColorAnimation()
        {
            From = Color.FromRgb(255, 0, 0),
            To = Color.FromRgb(200, 200, 200),
            Duration = TimeSpan.FromSeconds(0.7)
        };



        DoubleAnimation k = new DoubleAnimation()
        {
            From = 20,
            To = 50,
            Duration = TimeSpan.FromSeconds(1.3)
        };

        DoubleAnimation k2 = new DoubleAnimation()
        {
            From = 50,
            To = 20,
            Duration = TimeSpan.FromSeconds(0.9)
        };

        private int page = 0;

        //private async Task AnimateChose()
        //{
        //    while (true)
        //    {
        //        if (anim_chos)
        //        {
        //            invitef.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, o);
        //            ds1.BeginAnimation(DropShadowEffect.ColorProperty, o);
        //            ds1.BeginAnimation(DropShadowEffect.BlurRadiusProperty, k);
        //            await Task.Delay(2000);
        //            invitef.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, om);
        //            ds1.BeginAnimation(DropShadowEffect.ColorProperty, om);
        //            ds1.BeginAnimation(DropShadowEffect.BlurRadiusProperty, k2);

        //            await Task.Delay(500);

        //            Becomepremium.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, o);
        //            ds2.BeginAnimation(DropShadowEffect.ColorProperty, o);
        //            ds2.BeginAnimation(DropShadowEffect.BlurRadiusProperty, k);
        //            await Task.Delay(2000);
        //            Becomepremium.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, om);
        //            ds2.BeginAnimation(DropShadowEffect.ColorProperty, om);
        //            ds2.BeginAnimation(DropShadowEffect.BlurRadiusProperty, k2);

        //            await Task.Delay(500);

        //            Uselinkvertise.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, o);
        //            ds3.BeginAnimation(DropShadowEffect.ColorProperty, o);
        //            ds3.BeginAnimation(DropShadowEffect.BlurRadiusProperty, k);
        //            await Task.Delay(2000);
        //            Uselinkvertise.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, om);
        //            ds3.BeginAnimation(DropShadowEffect.ColorProperty, om);
        //            ds3.BeginAnimation(DropShadowEffect.BlurRadiusProperty, k2);

        //            await Task.Delay(500);
        //        }
        //        else
        //        {
        //            fdfd.BeginAnimation(DropShadowEffect.BlurRadiusProperty, k);
        //            await Task.Delay(3000);
        //            fdfd.BeginAnimation(DropShadowEffect.BlurRadiusProperty, k2);
        //        }

        //        await Task.Delay(1000);
        //    }

        //} 


        Random _random = new Random();

        private async void AnimateEllipse(Ellipse ellipse)
        {
            while (true)
            {
                await Task.Delay(100);
                try
                {
                    if (page == 2)
                    {

                        double duration = _random.Next(1, 7);
                        double randomLeft = _random.Next(-70, 200);
                        double randomTop = _random.Next(-70, 70);

                        var leftAnimation = new DoubleAnimation
                        {
                            To = randomLeft,
                            Duration = TimeSpan.FromSeconds(duration),
                            EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
                        };

                        var topAnimation = new DoubleAnimation
                        {
                            To = randomTop,
                            Duration = TimeSpan.FromSeconds(duration),
                            EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
                        };

                        Canvas.SetLeft(ellipse, 0);
                        Canvas.SetTop(ellipse, 0);

                        ellipse.BeginAnimation(Canvas.LeftProperty, leftAnimation);
                        ellipse.BeginAnimation(Canvas.TopProperty, topAnimation);

                        await Task.Delay(TimeSpan.FromSeconds(duration));
                    }
                }
                catch
                {
                    await Task.Delay(500);
                }
            }
        }


        private Storyboard currentStoryboard;  // Variável para armazenar o storyboard atual.
        private DoubleAnimation leftAnimation, topAnimation; // Animations para armazenar os estados.

        private async void AnimateEllipse2()
        {
            while (true)
            {
                await Task.Delay(100);

                if (isDragging)
                {
                    // Pausar as animações imediatamente quando isDragging for true
                    PauseAnimations();
                    return;
                }

                try
                {
                    if (page == 1)
                    {
                        // Verificar se estamos começando ou retomando a animação
                        if (currentStoryboard == null || currentStoryboard.GetCurrentState() == ClockState.Stopped)
                        {
                            // Criar animações quando não houver animação em curso
                            leftAnimation = new DoubleAnimation
                            {
                                From = 50,
                                To = -200,
                                Duration = TimeSpan.FromSeconds(5),
                                EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseOut }
                            };

                            topAnimation = new DoubleAnimation
                            {
                                From = 50,
                                To = -200,
                                Duration = TimeSpan.FromSeconds(5),
                                EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseOut }
                            };

                            currentStoryboard = new Storyboard();
                            Storyboard.SetTarget(leftAnimation, Ellipse12);
                            Storyboard.SetTarget(topAnimation, Ellipse12);

                            Storyboard.SetTargetProperty(leftAnimation, new PropertyPath(Canvas.LeftProperty));
                            Storyboard.SetTargetProperty(topAnimation, new PropertyPath(Canvas.TopProperty));

                            currentStoryboard.Children.Add(leftAnimation);
                            currentStoryboard.Children.Add(topAnimation);

                            currentStoryboard.Begin();  // Iniciar animações do Storyboard
                        }

                        if (isDragging)
                        {
                            PauseAnimations();
                            while (isDragging) await Task.Delay(100);
                            ResumeAnimations();
                        }
                        // Espera o tempo necessário ou verifica se está em isDragging
                        await Task.Delay(TimeSpan.FromSeconds(3.5));
                        if (isDragging)
                        {
                            PauseAnimations();
                            while (isDragging) await Task.Delay(100);
                            ResumeAnimations();
                        }

                        // Fade out e resetar animações
                        Fade(Ellipse12, 1, 0.3, 0.1);
                        currentStoryboard.Stop();  // Parar animações atuais
                        currentStoryboard = null;

                        // Animações para voltar à posição original
                        leftAnimation = new DoubleAnimation
                        {
                            From = -200,
                            To = 50,
                            Duration = TimeSpan.FromSeconds(2.5),
                            EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseOut }
                        };

                        topAnimation = new DoubleAnimation
                        {
                            From = -200,
                            To = 50,
                            Duration = TimeSpan.FromSeconds(2.5),
                            EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseOut }
                        };

                        currentStoryboard = new Storyboard();
                        Storyboard.SetTarget(leftAnimation, Ellipse12);
                        Storyboard.SetTarget(topAnimation, Ellipse12);

                        Storyboard.SetTargetProperty(leftAnimation, new PropertyPath(Canvas.LeftProperty));
                        Storyboard.SetTargetProperty(topAnimation, new PropertyPath(Canvas.TopProperty));

                        currentStoryboard.Children.Add(leftAnimation);
                        currentStoryboard.Children.Add(topAnimation);

                        currentStoryboard.Begin();  // Iniciar animações do Storyboard

                        await Task.Delay(400);
                        Fade(Ellipse12, 0.3, 1, 0.6);

                        // Verifica se isDragging mudou para true enquanto anima
                        if (isDragging)
                        {
                            PauseAnimations();
                            while (isDragging) await Task.Delay(100);
                            ResumeAnimations();
                        }

                        await Task.Delay(800);
                        currentStoryboard.Stop();  // Parar animações antes de reiniciar
                        currentStoryboard = null;
                    }
                }
                catch
                {
                    await Task.Delay(500);
                }
            }
        }

        // Função para pausar animações
        private void PauseAnimations()
        {
            if (currentStoryboard != null)
            {
                currentStoryboard.Pause();  // Pausar as animações em curso
            }
        }

        // Função para retomar animações
        private void ResumeAnimations()
        {
            if (currentStoryboard != null)
            {
                currentStoryboard.Resume();  // Retomar a animação de onde parou
            }
        }



        WebView2 webView;
        bool wclosed = true;
        private async void WebView_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (lvgrid != null && wclosed != true)
            {
                if (e.IsSuccess)
                {
                    webView.CoreWebView2.WebMessageReceived += WebView_WebMessageReceived;
                    webView.CoreWebView2.NavigationCompleted += WebView_CoreWebView2_NavigationCompleted;
                    //webView.CoreWebView2.Settings.AreDevToolsEnabled = false;

                    await Task.Delay(8000);

                    if (lvgrid != null && !wclosed)
                    {
                        lvgrid.Visibility = Visibility.Visible;
                        webView.Visibility = Visibility.Visible;

                        Width = SystemParameters.PrimaryScreenWidth - 100;
                        Height = SystemParameters.PrimaryScreenHeight - 100;

                        this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;
                        this.Top = (SystemParameters.PrimaryScreenHeight - this.Height) / 2;


                        closelv.Visibility = Visibility.Visible;
                        //Status_Border.Visibility = Visibility.Hidden;
                    }
                }
                else
                {
                    if (!e.InitializationException.Message.Contains("anulada") && !e.InitializationException.Message.Contains("ABORT"))
                    {
                        closelv.Visibility = Visibility.Collapsed;
                        //Status_Border.Visibility = Visibility.Visible;
                        System.Windows.MessageBox.Show("Um erro impediu o carregamento do WebView. Se o problema persistir, contate um ajudante no servidor do Essence\r\n" + e.InitializationException.Message, "Oops!");
                        Lv_Closed();
                    }
                }
            }
        }

        private void WebView_CoreWebView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            //webView.Visibility = Visibility.Visible;


            if (e.IsSuccess)
            {
                AddClickHandlerToButton();
            }
        }

        bool add;
        internal async void AddClickHandlerToButton()
        {
            if (add)
                return;

            add = true;
            int i = 0;
            while (true)
            {
                if (feito2)
                    break;

                string script = "";
                if (!feito1)
                {
                    script = @"

                            var divs = document.querySelectorAll('.adStepFoot');

                            divs.forEach(function(div) {
                                if (div.textContent.includes('completed this')) {
                                    div.addEventListener('click', function() {
                                        window.chrome.webview.postMessage('M4_Here1');
                                    });
                                }
                            });

                            (function() {
                                var texto = 'Volte e aperte em continuar';
                                if(document.body.innerHTML.includes(texto)) {
                                    window.chrome.webview.postMessage('M4_Here1');
                                }
                            })()";
                }
                else
                {
                    script = @"
                            var divs = document.querySelectorAll('.adStepFoot');

                            divs.forEach(function(div) {
                                if (div.textContent.includes('completed this')) {
                                    div.addEventListener('click', function() {
                                        window.chrome.webview.postMessage('M4_Here2');
                                    });
                                }
                            });

                            (function() {
                                var texto = 'Volte e aperte em continuar';
                                if(document.body.innerHTML.includes(texto)) {
                                    window.chrome.webview.postMessage('M4_Here2');
                                }
                            })()";
                }

                try { await webView.CoreWebView2.ExecuteScriptAsync(script); } catch { }
                await Task.Delay(2000);
                i++;
            }
        }


        bool feito1;
        bool feito2;

        private async void WebView_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string message = e.WebMessageAsJson;
            if (message.Contains("M4_Here1") && !feito1)
            {
                feito1 = true;
                Lv_Completed();
                webView.CoreWebView2.Navigate("https://link-target.net/1036674/evxrbx1");
                webView.Visibility = Visibility.Hidden;
                await Task.Delay(8000);
                webView.Visibility = Visibility.Visible;
            }

            else if (message.Contains("M4_Here2") && !feito2)
            {
                feito2 = true;
                webView.Visibility = Visibility.Hidden;
                Lv_Completed();
            }
        }




        private async Task LoadWebView()
        {
            try
            {              
                webView = new WebView2();
                webView.Visibility = Visibility.Hidden;

                webView.DefaultBackgroundColor = System.Drawing.Color.Transparent;
                webView.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                webView.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                webView.CreationProperties = new CoreWebView2CreationProperties();
                
                webView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;

                lvgrid.Child = webView;
                
                await webView.EnsureCoreWebView2Async(await CoreWebView2Environment.CreateAsync((string)null, Path.GetTempPath(), (CoreWebView2EnvironmentOptions)null));

                webView.Source = new Uri("https://link-center.net/1036674/evxrbx2", UriKind.RelativeOrAbsolute);

            }
            catch
            {
                closelv.Visibility = Visibility.Collapsed;
                //Status_Border.Visibility = Visibility.Visible;
                Lv_Closed();
            }
        }



        internal void CloseWeb()
        {
            Fade(lvstartgrid, 0, 1, 0.3);

            wclosed = true;
            try { webView.CoreWebView2.WebMessageReceived -= WebView_WebMessageReceived; } catch { }
            try { webView.CoreWebView2.NavigationCompleted -= WebView_CoreWebView2_NavigationCompleted; } catch { }
            try { webView.CoreWebView2InitializationCompleted -= WebView_CoreWebView2InitializationCompleted; } catch { }


            if (webView != null)
            {
                try { webView.Dispose(); } catch { }
            }
            lvgrid.Child = null;

            try
            {
                foreach (Process p in Process.GetProcessesByName("msedgewebview2"))
                {
                    p.Kill();
                }
            }
            catch { }

            closelv.Visibility = Visibility.Collapsed;
            closelv.IsEnabled = true;            
            lvstartgrid.Visibility = Visibility.Visible;
        }

        private async void Lv_Refresh()
        {
            try
            {
                foreach (Process p in Process.GetProcessesByName("msedgewebview2"))
                {
                    p.Kill();
                }
            }
            catch { }

            await Task.Delay(1000);
            await STARTLV2();
        }

        int comp = 0;
        private async void Lv_Completed()
        {
            
            comp++;
            if (comp == 1)
            {
                LVstatus.Text = "1/2 Completed. Redirecting...";
                Move(progresslol, new Thickness(20, 0, 680, 10), new Thickness(20, 0, 20, 10), 8);
                await Task.Delay(8000);
            }

            else if (comp == 2)
            {
                string[] result = KeyGay2.Encriptar(DateTime.Now.AddHours(12).ToString());

                StreamWriter sw = new StreamWriter("C:\\Essence\\userdata\\LinkvertiseKey.txt");
                sw.WriteLine(result[0]);
                sw.WriteLine(result[1]);
                sw.Close();


                Fade(MainWin, 1, 0, 0.5);
                await Task.Delay(500);

                this.Width = 740;
                this.Height = 457;

                this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;
                this.Top = (SystemParameters.PrimaryScreenHeight - this.Height) / 2;

                closelv.Visibility = Visibility.Collapsed;
                LVstatus.Text = "Linkvertise Completed.";
                Vertise.Visibility = Visibility.Collapsed;
                Comp.Visibility = Visibility.Visible;
                Welcome_Page3_Title1.Text = "";
                Welcome_Page3_Desc1.Text = "";

                CloseWeb();

                Fade(MainWin, 0, 1, 0.8);

                await Task.Delay(800);

                var scaleXAnimation = new DoubleAnimation
                {
                    From = 1,
                    To = -1,
                    Duration = TimeSpan.FromSeconds(1)
                };

                var scaleTransform = (ScaleTransform)((TransformGroup)AnimatedPath.RenderTransform).Children[0];
                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleXAnimation);




                string title = lm.Translate("Thankyou for using our system!");
                for (int i = 0; i < title.Length; i++)
                {
                    Welcome_Page3_Title1.Text += title[i].ToString();
                    await Task.Delay(40);
                }

                string title2 = lm.Translate("we'll see you back here in 12h <3");
                for (int i = 0; i < title2.Length; i++)
                {
                    Welcome_Page3_Desc1.Text += title2[i].ToString();
                    await Task.Delay(15);
                }

                await Task.Delay(2000);

                Fade(MainWin, 1, 0, 1);
                await Task.Delay(1000);

                string fileName = Process.GetCurrentProcess().MainModule.FileName;
                Process.Start(fileName);
                await Task.Delay(600);
                FormFadeOut.Begin();
            }
        }

        private async void Lv_Closed()
        {
            wclosed = true;

            if (comp == 2)
                return;

            await Task.Delay(100);
            Fade(MainWin, 1, 0, 0.3);
            await Task.Delay(300);

            CloseWeb();

            await Task.Delay(700);

            this.Width = 740;
            this.Height = 457;

            this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;
            this.Top = (SystemParameters.PrimaryScreenHeight - this.Height) / 2;

            closelv.Visibility = Visibility.Collapsed;
            closelv.IsEnabled = true;
            Vertise.Visibility = Visibility.Collapsed;
            Fade(MainWin, 0, 1, 0.5);
        }

        private async Task STARTLV2()
        {
            wclosed = false;

            LVstatus.Text = "Loading linkvertise...";
            await Task.Delay(1000);


            var timeout = Task.Delay(10000);
            var cumm = await Task.WhenAny(LoadWebView(), timeout);
            await cumm;

            if (cumm == timeout)
            {
                LVstatus.Text = "This is taking longer than expected. Restarting...";

                await Task.Delay(1000);

                Lv_Refresh();
                try { CloseWeb(); } catch { }
            }

        }

        private void closelv_Click(object sender, RoutedEventArgs e)
        {
            wclosed = true;
            closelv.IsEnabled = false;
            Lv_Closed();
        }


        string AD_redirect = "";
        private async void Uselinkvertise_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Move(progresslol, new Thickness(20, 0, 700, 10), new Thickness(20, 0, 20, 10), 15);

            await Task.Delay(100);

            ad_free_border.Visibility = Visibility.Collapsed;
            unc_border.Visibility = Visibility.Collapsed;
            anti_ban_border.Visibility = Visibility.Collapsed;

            Fade(lvstartgrid, 1, 0, 0.5);

            if (ads_result.Length > 10 && ads_result.Split('\n').Length >= 3)
            {
                try
                {
                    CustomAD_Title.Text = ads_result.Split('\n')[0];
                    dhdahdui.Source = new BitmapImage(new Uri(ads_result.Split('\n')[1]));
                    AD_redirect = ads_result.Split('\n')[2];
                }
                catch { }



                CustomAD_Desc.Text = "Click in the banner to know more!";
                Fade(CustomAD, 0, 1, 0.6);
                CustomAD.Visibility = Visibility.Visible;

                ds2.BeginAnimation(DropShadowEffect.BlurRadiusProperty, k);
                await Task.Delay(3000);
                ds2.BeginAnimation(DropShadowEffect.BlurRadiusProperty, k2);
            }
            else
            {
                await Task.Delay(700);

                Move(ad_free_border, new Thickness(65, 0, 0, -400), new Thickness(0, 0, 0, 0), 1);
                ad_free_border.Visibility = Visibility.Visible;

                await Task.Delay(1000);

                Move(unc_border, new Thickness(65, 0, 0, -400), new Thickness(65, 0, 0, 0), 1);
                unc_border.Visibility = Visibility.Visible;

                await Task.Delay(1000);

                Move(anti_ban_border, new Thickness(-135, 0, 0, -400), new Thickness(65, 0, 0, 0), 1);
                anti_ban_border.Visibility = Visibility.Visible;
            }



            await STARTLV2();
        }




        private async void Becomepremium_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private async void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            await Task.Delay(300);

            if (AD_redirect != "")
                Process.Start(AD_redirect);
        }


        private async void check_key_btn_Click(object sender, RoutedEventArgs e)
        {
            keyinput.IsEnabled = false;
            check_key_btn.IsEnabled = false;

            if(keyinput.Text.Length == 34 && keyinput.Text != "XXXXXX-XXXXXX-XXXXXX-XXXXXX-XXXXXX" && keyinput.Text.Contains("-"))
            {
                try
                {
                    Premiumkeystatustxt.Text = "Checking Key...";
                    await Task.Delay(2000);
                    string logins = await KeyGay2.Get("check", keyinput.Text + "\n" + KeyGay2.current_user);

                    if (logins.Split('\n').Length > 4)
                    {
                        string[] data = logins.Split('\n');

                        Premiumkeystatustxt.Text = "Valid. ThankYou :)";
                        await Task.Delay(1500);

                        Fade(Premium, 1.0, 0.0, 1);
                        string fileName = Process.GetCurrentProcess().MainModule.FileName;
                        Process.Start(fileName);
                        Close();
                    }
                    else if (logins == "outro")
                    {
                        Premiumkeystatustxt.Text = "Key Already redeemed.";
                        MessageBox.Show("Oops! This key has already been redeemed and does not belong to your account. If this is an error, please report it to your seller.", "Warning");
                    }
                    else
                        Premiumkeystatustxt.Text = "Key Invalid.";
                }
                catch 
                {
                }
            }
            else
                Premiumkeystatustxt.Text = "Invalid Key Format";

            await Task.Delay(1500);
            Premiumkeystatustxt.Text = "Already have a key?";

            keyinput.IsEnabled = true;
            check_key_btn.IsEnabled = true;
        }

        private void keyinput_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (keyinput.Text == "XXXXXX-XXXXXX-XXXXXX-XXXXXX-XXXXXX")
                keyinput.Text = "";
        }




        private bool duwhnudnqwdqwium2 = false;
        private async void lastplayedgrid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (duwhnudnqwdqwium2 || LastplayedText2.Text == "")
                return;

            duwhnudnqwdqwium2 = true;

            await Task.Delay(150);
            if (!lastplayedgrid2.IsMouseOver)
            {
                duwhnudnqwdqwium2 = false;
                return;
            }

            Move(ermmmwdw, new Thickness(75, 20, 0, 0), new Thickness(75, 20, 0, -73), 0.2);
            await Task.Delay(50);
            Move(LastplayedText2, new Thickness(75, 20, 0, -73), new Thickness(75, 20, 0, 0), 0.3);
            LastplayedText2.Visibility = Visibility.Visible;
            await Task.Delay(300);

            while (lastplayedgrid2.IsMouseOver)
            {
                await Task.Delay(200);
            }

            Move(LastplayedText2, new Thickness(75, 20, 0, 0), new Thickness(75, 20, 0, -73), 0.2);
            await Task.Delay(50);
            Move(ermmmwdw, new Thickness(75, 20, 0, -73), new Thickness(75, 20, 0, 0), 0.3);

            await Task.Delay(300);
            duwhnudnqwdqwium2 = false;
        }


        private async void goback_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {           
            if (animatingsht)
                return;

            animatingsht = true;

            Fade(news_page_shitt, news_page_shitt.Opacity, 0, 0.3);
            Fade(news_shitty_text, news_shitty_text.Opacity, 0.7, 0.3);
            Fade(Premium, Premium.Opacity, 0, 0.3);

            Fade(changelog_page_shitt, changelog_page_shitt.Opacity, 0, 0.3);
            Fade(changelogs_shitty_text, changelogs_shitty_text.Opacity, 0.7, 0.3);
            Fade(InviteFriends, InviteFriends.Opacity, 0, 0.3);

            Fade(robloxnews_page_shitt, robloxnews_page_shitt.Opacity, 0, 0.3);
            Fade(robloxnews_shitty_text, robloxnews_shitty_text.Opacity, 0.7, 0.3);
            Fade(Vertise, Vertise.Opacity, 0, 0.3);


            Fade(home_page_shitt, home_page_shitt.Opacity, 1, 0.5);
            Fade(home_shitty_text, home_shitty_text.Opacity, 1, 0.5);
            Fade(ChooseGrid, ChooseGrid.Opacity, 1, 0.7);
            ChooseGrid.Visibility = Visibility.Visible;


            await Task.Delay(600);
            didiajdia.Visibility = Visibility.Collapsed;

            InviteFriends.Visibility = Visibility.Collapsed;
            Vertise.Visibility = Visibility.Collapsed;
            Premium.Visibility = Visibility.Collapsed;
            animatingsht = false;
            page = 0;
            try { CloseWeb(); } catch { }
        }


        private bool animatingsht = false; //home, premium, invite firends, linkvertise
        private async void news_page_kkk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (animatingsht)
                return;

            animatingsht = true;
            page = 1;

            Fade(home_page_shitt, home_page_shitt.Opacity, 0, 0.3);
            Fade(home_shitty_text, home_shitty_text.Opacity, 0.7, 0.3);
            Fade(ChooseGrid, ChooseGrid.Opacity, 0, 0.3);



            Fade(changelog_page_shitt, changelog_page_shitt.Opacity, 0, 0.3);
            Fade(changelogs_shitty_text, changelogs_shitty_text.Opacity, 0.7, 0.3);
            Fade(InviteFriends, InviteFriends.Opacity, 0, 0.3);

            Fade(robloxnews_page_shitt, robloxnews_page_shitt.Opacity, 0, 0.3);
            Fade(robloxnews_shitty_text, robloxnews_shitty_text.Opacity, 0.7, 0.3);
            Fade(Vertise, Vertise.Opacity, 0, 0.3);



            Fade(news_page_shitt, news_page_shitt.Opacity, 1, 0.5);
            Fade(news_shitty_text, news_shitty_text.Opacity, 1, 0.5);
            Fade(Premium, Premium.Opacity, 1.0, 0.7);
            Premium.Visibility = Visibility.Visible;
            didiajdia.Visibility = Visibility.Visible;

            await Task.Delay(600);
            InviteFriends.Visibility = Visibility.Collapsed;
            Vertise.Visibility = Visibility.Collapsed;            
            ChooseGrid.Visibility = Visibility.Collapsed;
            animatingsht = false;

            try { CloseWeb(); } catch { }
        }

        private async void changelog_page_kkk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (animatingsht)
                return;
            animatingsht = true;
            page = 2;

            Fade(home_page_shitt, home_page_shitt.Opacity, 0, 0.3);
            Fade(home_shitty_text, home_shitty_text.Opacity, 0.7, 0.3);
            Fade(ChooseGrid, ChooseGrid.Opacity, 0, 0.3);


            Fade(news_page_shitt, news_page_shitt.Opacity, 0, 0.3);
            Fade(news_shitty_text, news_shitty_text.Opacity, 0.7, 0.3);
            Fade(Premium, Premium.Opacity, 0, 0.3);

            Fade(robloxnews_page_shitt, robloxnews_page_shitt.Opacity, 0, 0.3);
            Fade(robloxnews_shitty_text, robloxnews_shitty_text.Opacity, 0.7, 0.3);
            Fade(Vertise, Vertise.Opacity, 0, 0.3);


            Fade(changelog_page_shitt, changelog_page_shitt.Opacity, 1, 0.5);
            Fade(changelogs_shitty_text, changelogs_shitty_text.Opacity, 1, 0.5);
            Fade(InviteFriends, InviteFriends.Opacity, 1.0, 0.7);
            InviteFriends.Visibility = Visibility.Visible;

            await Task.Delay(600);
            didiajdia.Visibility = Visibility.Collapsed;

            Vertise.Visibility = Visibility.Collapsed;
            Premium.Visibility = Visibility.Collapsed;
            ChooseGrid.Visibility = Visibility.Collapsed;
            animatingsht = false;

            try { CloseWeb(); } catch { }
        }

        string ads_result = "";
        private async void robloxnews_page_kkk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (animatingsht)
                return;
            animatingsht = true;
            page = 3;

            Fade(home_page_shitt, home_page_shitt.Opacity, 0, 0.3);
            Fade(home_shitty_text, home_shitty_text.Opacity, 0.7, 0.3);
            Fade(ChooseGrid, ChooseGrid.Opacity, 0, 0.3);


            Fade(news_page_shitt, news_page_shitt.Opacity, 0, 0.3);
            Fade(news_shitty_text, news_shitty_text.Opacity, 0.7, 0.3);
            Fade(Premium, Premium.Opacity, 0, 0.3);

            Fade(changelog_page_shitt, changelog_page_shitt.Opacity, 0, 0.3);
            Fade(changelogs_shitty_text, changelogs_shitty_text.Opacity, 0.7, 0.3);
            Fade(InviteFriends, InviteFriends.Opacity, 0, 0.3);

            Fade(robloxnews_page_shitt, robloxnews_page_shitt.Opacity, 1, 0.5);
            Fade(robloxnews_shitty_text, robloxnews_shitty_text.Opacity, 1, 0.5);
            Fade(Vertise, Vertise.Opacity, 1.0, 0.7);
            Vertise.Visibility = Visibility.Visible;

            await Task.Delay(600);
            didiajdia.Visibility = Visibility.Collapsed;

            InviteFriends.Visibility = Visibility.Collapsed;
            Premium.Visibility = Visibility.Collapsed;
            ChooseGrid.Visibility = Visibility.Collapsed;
            animatingsht = false;

            ads_result = await KeyGay2.Get("ads", force_return: true);
        }


        private void Run_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://discord.gg/r7EwQ3zG");
        }

        private async void logoutbtn_Click(object sender, RoutedEventArgs e)
        {
            this.IsHitTestVisible = false;
            await Task.Delay(1000);
            try { File.Delete("C:\\Essence\\userdata\\logindata.txt"); } catch { }
            registryKey1.SetValue("Boot", "1");

            RestartApp(false);
        }

        private async void GenerateInvBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(KeyGay2.invitecode);

            if (fdsgsgsgs.Margin == new Thickness(8, 30, 0, 160))
            {
                ThicknessAnimation j = new ThicknessAnimation()
                {
                    To = new Thickness(8, 30, 0, 100)
                };
                fdsgsgsgs.BeginAnimation(MarginProperty, j);
                await Task.Delay(500);
                dsfdsfsdffs.Fill = Brushes.Red;
                f432f34f32.Color = Colors.Red;
            }

            if (KeyGay2.invites > 0)
            {
                GenerateInvBtn.IsEnabled = false;
                await Task.Delay(1000);
                string response = await KeyGay2.Get("reedempremium", KeyGay2.current_user + "\n");
                switch (response)
                {
                    case "success":
                        RestartApp(false);
                        return;

                    case "user-not-found":
                        MessageBox.Show("Error when reedem premium: User not found", "Ops!");
                        break;

                    case "user-is-premium-already":
                        MessageBox.Show("Error when reedem premium: You aready have an premium subscription.");
                        break;

                    case "user-have-no-invites":
                        MessageBox.Show("Error when reedem premium: You dont have any invites yet", "Ops!");
                        break;

                    case "failed-to-reedem":
                        MessageBox.Show("Error when reedem premium: Unknow error. try again or contact suport", "Ops!");
                        break;

                    default:
                        MessageBox.Show(response, "Error - try again or contact suport");
                        break;
                }
            }
            
        }

    }
}
