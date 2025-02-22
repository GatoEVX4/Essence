using DiscordRPC;
using Essence.Elements;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfAnimatedGif;
using Path = System.IO.Path;
using Ellipse = System.Windows.Shapes.Ellipse;

namespace Essence
{
    /// <summary>
    /// Lógica interna para WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Window
    {

        private RegistryKey registryKey1;
        private RegistryKey Wolfregkey;
        LanguageManager lm;


        public WelcomePage()
        {

            try { Wolfregkey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Essence Studios", writable: true); } catch { }
            Wolfregkey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Essence Studios", writable: true);

            try { registryKey1 = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Essence Studios\\Essence", writable: true); } catch { }
            registryKey1 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Essence Studios\\Essence", writable: true);
            lm = new LanguageManager();



            InitializeComponent();
            Opacity = 0;

            EULA.Visibility = Visibility.Collapsed;

            W2_page0.Visibility = Visibility.Collapsed;
            W2_page2.Visibility = Visibility.Visible;
            //W2_premium2.Visibility = Visibility.Collapsed;

            W2_page32.Visibility = Visibility.Collapsed;
            W2_page42.Visibility = Visibility.Collapsed;


            EmailConfirmationPage2.Visibility = Visibility.Collapsed;
            DiscordConfirmationPage2.Visibility = Visibility.Collapsed;
            ConfirmEmailAcessible.Visibility = Visibility.Collapsed;

            Gif5.Visibility = Visibility.Collapsed;
            Gif6.Visibility = Visibility.Collapsed;
            Gif7.Visibility = Visibility.Collapsed;

            Fader.Visibility = Visibility.Collapsed;
        }


        private async Task<bool> Notificar2(string Title, string Text)
        {
            var notification = new CumNotification2(Title, Text);

            griid.Children.Add(notification);
            bool result = await notification.ShowAsync();
            griid.Children.Remove(notification);

            return result;
        }





        //private DispatcherTimer raintimer;
        //private Random random = new Random();
        //private string[] pathStrings = new string[]
        //{
        //    "M23 47.689v-6.342l-3.357 1.992L18 42.305v-2.229l5-2.986v-4.168l-4 2.451v-4.416l-4 2.094v5.99l-1.653 1.23L12 39.16v-4.012L6.426 38.27L4 37.271v-2.529l5.685-3.17L6 29.75v-2.32l2.123-1.127l5.214 3.068l3.612-2.084l-.082-.065l-3.665-2.123l3.568-2.228l-3.577-2.083L7.98 23.84L6 22.871v-2.307l3.542-1.978L4 15.533v-2.529l2.321-1.114L12 15.087v-4.076l1.485-1.127l1.943 1.18l-.056 6.105l3.673 2.122l.033-4.311L23 17.079v-4.167l-5-2.988V7.71l1.643-1.05L23 8.652V2.324L24.994 1L27 2.324v6.328l3.906-2.031L33 7.84v1.992l-6 3.08v4.167l4-2.267v4.534l4-2.084v-6.524l1.455-.866l1.545.865v4.167l5.842-3.08L46 13.042v2.359l-5.495 3.17L44 20.525v2.254l-1.83.996l-5.327-3.158l-3.679 2.346l3.549 2.228l-3.659 2.122l3.772 1.992l5.389-2.986L44 27.535v2.15l-3.32 1.887l5.32 3.17v2.49l-2.522 1.037L38 35.281v3.955l-1.52 1.049L35 39.236v-6.002l-4-2.213v4.168l-4-2.268v4.168l5 2.986v2.359l-1.647.904L27 41.348v6.342L24.994 49zm-1.466-22.597L23.42 28h3.514l1.613-2.908L26.843 22h-3.514z",
        //    "m21.16 16.13l-2-1.15l.89-.24a1 1 0 1 0-.52-1.93l-2.82.76L14 12l2.71-1.57l2.82.76h.26a1 1 0 0 0 .26-2L19.16 9l2-1.15a1 1 0 0 0-1-1.74L18 7.37l.3-1.11a1 1 0 1 0-1.93-.52l-.82 3L13 10.27V7.14l2.07-2.07a1 1 0 0 0 0-1.41a1 1 0 0 0-1.42 0l-.65.65V2a1 1 0 0 0-2 0v2.47l-.81-.81a1 1 0 0 0-1.42 0a1 1 0 0 0 0 1.41L11 7.3v3L8.43 8.78l-.82-3a1 1 0 1 0-1.93.52L6 7.37L3.84 6.13a1 1 0 0 0-1 1.74l2 1.13l-.84.26a1 1 0 0 0 .26 2h.26l2.82-.76L10 12l-2.71 1.57l-2.82-.76A1 1 0 1 0 4 14.74l.89.24l-2 1.15a1 1 0 0 0 1 1.74L6 16.63l-.3 1.11A1 1 0 0 0 6.39 19a1.2 1.2 0 0 0 .26 0a1 1 0 0 0 1-.74l.82-3L11 13.73v3.13l-2.07 2.07a1 1 0 0 0 0 1.41a1 1 0 0 0 .71.3a1 1 0 0 0 .71-.3l.65-.65V22a1 1 0 0 0 2 0v-2.47l.81.81a1 1 0 0 0 1.42 0a1 1 0 0 0 0-1.41L13 16.7v-3l2.57 1.49l.82 3a1 1 0 0 0 1 .74a1.2 1.2 0 0 0 .26 0a1 1 0 0 0 .71-1.23L18 16.63l2.14 1.24a1 1 0 1 0 1-1.74Z",
        //    "M7.5 2.793V1h1v1.793l1.146-1.147l.707.708L8.5 4.207v2.927l2.535-1.463l.678-2.532l.966.258l-.42 1.566l1.553-.896l.5.866l-1.553.896l1.566.42l-.258.966l-2.532-.678L9 8l2.535 1.463l2.532-.678l.258.966l-1.566.42l1.553.896l-.5.866l-1.553-.896l.42 1.566l-.966.258l-.678-2.532L8.5 8.866v2.927l1.853 1.853l-.707.708L8.5 13.207V15h-1v-1.793l-1.147 1.147l-.707-.708L7.5 11.793V8.866l-2.535 1.463l-.678 2.532l-.966-.258l.42-1.566l-1.553.896l-.5-.866l1.552-.896l-1.566-.42l.26-.966l2.531.678L7 8L4.465 6.537l-2.532.678l-.259-.966l1.566-.42l-1.552-.896l.5-.866l1.552.896l-.42-1.566l.967-.258l.678 2.532L7.5 7.134V4.207L5.646 2.354l.707-.708z",
        //    "m12.707 2.293l4 4a1 1 0 0 1 .175 1.178c-.247.463-.633.775-1.01.987l2.835 2.835a1 1 0 0 1 0 1.414c-.478.478-1.082.77-1.634.952l2.634 2.634a1 1 0 0 1-.392 1.656c-.692.229-1.405.389-2.119.532c-.542.108-1.175.216-1.88.306l.633 1.897A1 1 0 0 1 15 22H9a1 1 0 0 1-.949-1.316l.633-1.897a27 27 0 0 1-1.88-.306a23 23 0 0 1-1.591-.378l-.53-.154a1 1 0 0 1-.39-1.656l2.634-2.634c-.552-.181-1.156-.474-1.634-.952a1 1 0 0 1 0-1.414l2.835-2.835c-.377-.212-.763-.524-1.01-.987a1 1 0 0 1 .175-1.178l4-4a1 1 0 0 1 1.414 0m.562 16.675a26 26 0 0 1-2.123.018l-.415-.018L10.387 20h3.226zM12 4.414L9.528 6.886l.1.029l.187.043l.276.046c.383.04.717.29.85.655a1 1 0 0 1-.234 1.048L7.63 11.784c.423.13.905.216 1.372.216a1 1 0 0 1 .705 1.707l-2.76 2.761l.514.103C8.639 16.793 10.218 17 12 17a24.7 24.7 0 0 0 4.26-.378l.544-.103l.25-.051l-2.761-2.76a1 1 0 0 1 .588-1.7l.118-.008a5 5 0 0 0 1.371-.216l-3.077-3.077a1 1 0 0 1 .488-1.683l.323-.05l.172-.036q.094-.02.196-.052z",
        //    "m56.195 31.68l3.033-3.215c.346-.365.363-.939 0-1.301c-.346-.346-.953-.363-1.297 0q-2.06 2.185-4.121 4.367l.012-8.439l-1.838-.002l-.012 10.389l-4.477 4.743a17.1 17.1 0 0 0-4.429-6.043a12.9 12.9 0 0 0 1.843-6.647c0-2.233-.564-4.335-1.557-6.171c1.025.049 1.724-.13 1.932-.575c.354-.758-.783-2.106-2.817-3.567L47.03 8.99a.5.5 0 0 0 .118-.161c.447-.957-2.258-3.164-6.039-4.928c-3.783-1.764-7.211-2.416-7.658-1.459a.5.5 0 0 0-.047.191l-1.839 7.501c-2.427-.618-4.19-.623-4.544.136c-.28.602.379 1.574 1.674 2.679c-5.622 1.433-9.785 6.515-9.785 12.583c0 2.433.68 4.7 1.843 6.647a17.05 17.05 0 0 0-4.371 5.911l-4.353-4.611l-.013-10.389l-1.836.002l.01 8.439l-4.119-4.367c-.344-.363-.953-.346-1.299 0c-.361.361-.344.936 0 1.301q1.518 1.606 3.035 3.215L2 33.369l.514 1.762l6.709-1.949q.591.624 1.182 1.252q2.631 2.785 5.258 5.571A17 17 0 0 0 14.911 45c0 9.389 7.611 17 17 17s17-7.611 17-17a17 17 0 0 0-.712-4.854c.005-.005.011-.005.015-.01l5.635-5.971l.93-.984l6.711 1.949l.51-1.761zM30.932 14.581c1.194.765 2.6 1.539 4.143 2.259c2.017.94 3.952 1.639 5.594 2.063a10.92 10.92 0 0 1 .804 12.044q-.249.438-.534.852h-.001a10.98 10.98 0 0 1-9.027 4.733c-3.377 0-6.4-1.533-8.42-3.937a11 11 0 0 1-.604-.791l-.005-.007a11 11 0 0 1-.534-.851a10.93 10.93 0 0 1-1.438-5.417c0-5.732 4.414-10.45 10.022-10.948M31.91 60c-8.271 0-15-6.73-15-15c0-4.432 1.936-8.416 5.002-11.163c2.384 2.867 5.977 4.694 9.998 4.694s7.614-1.827 9.999-4.695C44.975 36.584 46.91 40.568 46.91 45c0 8.27-6.726 15-15 15",
        //    "M21.95 10.99c-1.79-.03-3.7-1.95-2.68-4.22c-2.98 1-5.77-1.59-5.19-4.56C6.95.71 2 6.58 2 12c0 5.52 4.48 10 10 10c5.89 0 10.54-5.08 9.95-11.01M8.5 15c-.83 0-1.5-.67-1.5-1.5S7.67 12 8.5 12s1.5.67 1.5 1.5S9.33 15 8.5 15m2-5C9.67 10 9 9.33 9 8.5S9.67 7 10.5 7s1.5.67 1.5 1.5s-.67 1.5-1.5 1.5m4.5 6c-.55 0-1-.45-1-1s.45-1 1-1s1 .45 1 1s-.45 1-1 1",
        //    "M3 9a1 1 0 0 1 1-1h16a1 1 0 0 1 1 1v2a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1zm9-1v13 M19 12v7a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2v-7m2.5-4a2.5 2.5 0 0 1 0-5A4.8 8 0 0 1 12 8a4.8 8 0 0 1 4.5-5a2.5 2.5 0 0 1 0 5",
        //    "M69.74 20.72c-3.15 10.72 1.76 22.4 4.88 33.09c-11.63-2.31-23.04-7.52-34.08-4.61c-11.93 3.87 24.98 22.96 27.56 24.5c-7.8 7.69-16.56 13.37-21.19 22.81c11.48 3.69 26.31.72 36.45-1.18c3.19 8.27 10.8 29.87 20.14 26.27c11.7-4.6 15.8-23.88 19.1-34.52c9.3-1.7 27.2.97 27.4-7.43c.2-10.49-13.3-18.5-19.9-24.98c3.9-7.84 19.3-24.86 11.7-29.93c-13.7-4.28-27.8 7.76-38.9 16.42c-6.44-8.91-28.9-28.2-33.16-20.44m89.56 75.13c-7.2 5.05-15.2 7.25-23.4 8.45c-2.2 6.1-4.8 10.6-7.4 15.7c28.4 92.3 44.2 178 8.1 286.1c15.4.6 29.2 4.8 43.2 10.6c13.4-9.5 31.2-21.9 46-24.8c23.1-1.9 42.9 2.9 64.2 9.1c13.2-12.1 33.3-25.7 49.1-27.2c16.3.1 30.4 4.4 44.7 8.8c6.4-3.3 10.2-9.5 15-14.2c-58.4-122.2-125.4-213.6-239.5-272.55M417.6 377.6c-11 6.3-17.8 17.1-24 27c-15.7-4.3-36.9-13.7-53-12.9c-18.9 4.1-33.6 17.2-45.6 29.8c-10.5-3.3-20.6-6.2-29-8.2c-13.1-3-29.1-5.1-37-3.6c-18.1 5.6-33.1 17.2-46.7 27.9c-14.3-6.2-28.5-12.5-43.5-13.5c-5.1-.2-8.7.7-10.2 1.7c-8.6 5.9-19.7 20.9-24.2 34.8C101 471 101 484 110 488.8c14.5 2.3 27.8-6 38.9-13.1c11.2 5.5 30.9 17.7 43.1 17.4c17.3-4.6 32.9-13.7 47.1-22.2c9.3 8.7 26.7 22.5 39.3 21.7c17.9-5.2 29.1-21.5 37.7-35.6c17 5.8 53.5 14.1 67.5 3.9c9.8-7.6 2.9-19.9.2-28.5c12.7 4.7 26.8 9.2 37.9 10.8c19.1.6 37.8 2 19.6-18.3l-12.7-13.2c13.6-1.5 33-3.4 42.6-9.5c4.9-3 2-8.5-.4-11.1c-2.7-2.9-7.8-6.1-14-8.5c-13.2-3.8-26.3-7-39.2-5"
        //};

        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //     Cria um novo Path com valores aleatórios
        //    var path = CreateRandomPath();
        //    RainCanvas.Children.Add(path);
        //    AnimatePath(path);

        //    raintimer.Interval = TimeSpan.FromMilliseconds(random.Next(300, 1000));
        //}

        //private System.Windows.Shapes.Path CreateRandomPath()
        //{
        //     Seleciona um path aleatório da lista
        //    string randomPathData = pathStrings[random.Next(pathStrings.Length)];

        //     Define o Path
        //    var path = new System.Windows.Shapes.Path
        //    {
        //        Data = Geometry.Parse(randomPathData),
        //        Stretch = Stretch.Uniform,
        //        Fill = Brushes.White,
        //        Opacity = random.NextDouble() - 0.2, // Opacidade aleatória entre 0 e 1
        //        Width = random.Next(15, 40),   // Largura aleatória
        //        Height = random.Next(15, 40),  // Altura aleatória
        //        RenderTransformOrigin = new Point(0.5, 0.5),
        //    };

        //     Adiciona transformações
        //    var transformGroup = new TransformGroup();

        //     Rotação inicial aleatória
        //    var rotateTransform = new RotateTransform { Angle = random.Next(0, 360) };
        //    transformGroup.Children.Add(rotateTransform);

        //     Translação será adicionada na animação
        //    var translateTransform = new TranslateTransform();
        //    transformGroup.Children.Add(translateTransform);

        //    path.RenderTransform = transformGroup;

        //     Define uma posição X aleatória e posição Y inicial entre 0 e 10
        //    Canvas.SetLeft(path, random.Next(-30, (int)RainCanvas.ActualWidth + 30) + 20);
        //    Canvas.SetTop(path, random.Next(0, 50));
        //    Canvas.SetTop(path, -50);
        //    return path;
        //}

        //double speed_acelerator = 1;
        //private void AnimatePath(System.Windows.Shapes.Path path)
        //{
        //    var transformGroup = (TransformGroup)path.RenderTransform;
        //    var rotateTransform = (RotateTransform)transformGroup.Children[0];
        //    var translateTransform = (TranslateTransform)transformGroup.Children[1];

        //    double duration = random.Next(3000, 7000) / speed_acelerator;
        //    var animationY = new DoubleAnimation(0, 457, TimeSpan.FromMilliseconds(duration));

        //    animationY.Completed += (s, e) => RainCanvas.Children.Remove(path);


        //    if (false)
        //    {
        //        double rotationSpeed = random.NextDouble() * 360;
        //        if (random.Next(0, 5) == 0) rotationSpeed = 0;
        //        var rotationAnimation = new DoubleAnimation
        //        {
        //            From = rotateTransform.Angle,
        //            To = rotateTransform.Angle + rotationSpeed,
        //            Duration = TimeSpan.FromMilliseconds(duration),
        //            RepeatBehavior = RepeatBehavior.Forever
        //        };
        //        rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);
        //    }
        //    translateTransform.BeginAnimation(TranslateTransform.YProperty, animationY);
        //}





        private async void MainWin_Loaded(object sender, RoutedEventArgs e)
        {
            Inicializar1();
        }

        MediaPlayer _mediaPlayerrrr;
        private async Task Inicializar1()
        {
            Show();
            MainWin.BeginAnimationP(UIElement.OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.4)));

            if (!Directory.Exists("C:\\Essence"))
                Directory.CreateDirectory("C:\\Essence");

            if (!Directory.Exists("C:\\Essence\\bin"))
                Directory.CreateDirectory("C:\\Essence\\bin");

            if (!File.Exists("C:\\Essence\\userdata\\logindata.txt"))
            {
                File.CreateText("C:\\Essence\\userdata\\logindata.txt");
            }

            List<string> linhas = new List<string>(File.ReadAllLines("C:\\Essence\\userdata\\logindata.txt"));
            while (linhas.Count < 6)
            {
                linhas.Add("null");
            }
            File.WriteAllLines("C:\\Essence\\userdata\\logindata.txt", linhas);

            string systemLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            switch (systemLanguage)
            {
                case "en":
                    lm.lang = 1;
                    Wolfregkey.SetValue("Idioma", "EN");
                    break;

                case "pt":
                    lm.lang = 2;
                    Wolfregkey.SetValue("Idioma", "PT");
                    break;

                case "es":
                    lm.lang = 3;
                    Wolfregkey.SetValue("Idioma", "ES");
                    break;

                default:
                    lm.lang = 1;
                    Wolfregkey.SetValue("Idioma", "EN");
                    W2_page2.Visibility = Visibility.Collapsed;
                    W2_page0.Visibility = Visibility.Visible;
                    break;
            }
            TransL();


            if (registryKey1.GetValue("Boot").ToString() == "1")
            {
                //wuawdwud.Text = "Sign Up";
                LoginBtnTXT.Text = "Log in";

                LoginTXT.Text = "Enter your email";
                PassTXT.Text = "Enter your password";


                forgotpass.Visibility = Visibility.Visible;
            }
            else
            {
                iscreatingacc = true;
                //Welcome_Page3_Title1.Text = "Create an account";
                //wuawdwud.Text = "Log in";
                LoginBtnTXT.Text = "Sign Up";

                LoginTXT.Text = "Enter an email";
                PassTXT.Text = "Create a password";


                forgotpass.Visibility = Visibility.Collapsed;
            }

            if (!KeyGay2.firstlogin)
            {
                byb77b7tvb.Visibility = Visibility.Collapsed;
            }

            //raintimer = new DispatcherTimer(DispatcherPriority.SystemIdle);
            //raintimer.Interval = TimeSpan.FromMilliseconds(300); // Chuva a cada 500ms
            //raintimer.Tick += Timer_Tick;
            //raintimer.Start();

            //AnimateEllipse(Ellipse1);
            //AnimateEllipse(Ellipse2);
            //AnimateEllipse(Ellipse3);

            await Task.Delay(1500);            

            var matches = Regex.Matches(await KeyGay2.Get("registreds"), @"'([^']*)'");
            userssss = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                userssss[i] = matches[i].Groups[1].Value;
            }

            //await Task.Delay(1500);
            //_mediaPlayerrrr = new MediaPlayer();

            //await Dispatcher.InvokeAsync(async () =>
            //{                
            //    _mediaPlayerrrr.Volume = 0.1;

            //    string musicFilePath = "C:\\Essence\\userdata\\Musics\\BackgroundMusic.mp3";
            //    Uri musicUri = new Uri(musicFilePath);

            //    if (!File.Exists(musicFilePath))
            //    {
            //        //MessageBox.Show("O arquivo de música não foi encontrado.");
            //        return;
            //    }
            //    _mediaPlayerrrr.Open(musicUri);
            //    _mediaPlayerrrr.Play();
            //    _mediaPlayerrrr.MediaEnded += MediaPlayer_MediaEnded;

            //    double fadeDuration = 4000; // Total duration in milliseconds (3 seconds)
            //    double fadeSteps = 500;      // Number of steps (adjust for smoother transitions)
            //    double fadeInterval = fadeDuration / fadeSteps; // Interval per step in milliseconds
            //    double fadeStep = 0.2 / fadeSteps; // Volume increment per step (target volume is 0.1)

            //    while (_mediaPlayerrrr.Volume < 0.2)
            //    {
            //        _mediaPlayerrrr.Volume += fadeStep; // Increase volume
            //        if (_mediaPlayerrrr.Volume >= 0.2)
            //        {
            //            _mediaPlayerrrr.Volume = 0.2; // Ensure it doesn't go above 0.1
            //            break;
            //        }
            //        await Task.Delay((int)fadeInterval); // Wait before increasing volume again
            //    }
            //});
        }

        Random _random = new Random();
        private bool shutup = false;
        private async void AnimateEllipse(Ellipse ellipse)
        {
            while (true)
            {
                await Task.Delay(100);

                if (!shutup)
                {
                    try
                    {
                        double duration = _random.Next(4, 8);
                        double randomLeft = _random.Next(-500, 600);
                        double randomTop = _random.Next(-200, 200);

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
                    catch
                    {
                        await Task.Delay(500);
                    }
                }
            }
        }
        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            _mediaPlayerrrr.Position = new TimeSpan(0);
        }

        bool animations = true;










        private void Mini(object sender, MouseButtonEventArgs e)
        {
            base.WindowState = WindowState.Minimized;
        }

        private bool CloseCompleted;
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

            //using (Dispatcher.DisableProcessing())
            //{

            //}
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            CloseCompleted = true;
            Process.GetCurrentProcess().Kill();
        }


        private async void TransL()
        {
            try
            {
                Welcome_Page2_Title.Text = lm.Translate("Sorry, your language is not supported");
                Welcome_Page2_Desc.Text = lm.Translate("Choose one of the available languages below.");
                ConfirmLangButton.Content = lm.Translate("Confirm");


                //EnterUsingDiscordTXT.Text = lm.Translate("Verify using Discord");

                //EMAIL_TXTT.Text = lm.Translate("Email");
                //PASSWORD_TXTT.Text = lm.Translate("Password");

                Welcome_Page3_Title.Text = lm.Translate("Profile Settings");
                Welcome_Page3_Desc.Text = lm.Translate("Customize your account");
                Welcome_Page3_Name.Text = lm.Translate("Username");
                Welcome_Page3_Name_Copiar.Text = lm.Translate("Profile Picture");
                name_sync.Text = lm.Translate("Synced with your discord");
                avatar_sync.Text = lm.Translate("Synced with your discord");
                Welcome_Page3_Continue.Content = lm.Translate("Continue");
                user_avatar.Text = lm.Translate("Insert an image link here!");

                Welcome_Page4_Title.Text = lm.Translate("All Done!");
                Welcome_Page4_Desc.Text = lm.Translate("Now Your Essence are Fully Configured");
            }
            catch
            {
                await Task.Delay(1000);
                TransL();
            }
        }

        public void Fade(DependencyObject ElementName, double Start, double End, double Time)
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

        public void Move(DependencyObject ElementName, Thickness Origin, Thickness Location, double Time)
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


        private void English_Btn_Click(object sender, RoutedEventArgs e)
        {
            Fade(dwhududnwi0, dwhududnwi0.Opacity, 1, 0.3);
            Fade(dwhududnwi1, dwhududnwi1.Opacity, 0, 0.3);
            Fade(dwhududnw2, dwhududnw2.Opacity, 0, 0.3);

            lm.lang = 1;

            try
            {
                TransL();
                Wolfregkey.SetValue("Idioma", "EN");
            }
            catch { }
        }

        private void Portuguese_Btn_Click(object sender, RoutedEventArgs e)
        {
            Fade(dwhududnwi0, dwhududnwi0.Opacity, 0, 0.3);
            Fade(dwhududnwi1, dwhududnwi1.Opacity, 1, 0.3);
            Fade(dwhududnw2, dwhududnw2.Opacity, 0, 0.3);

            lm.lang = 2;

            try
            {
                TransL();
                Wolfregkey.SetValue("Idioma", "PT");
            }
            catch { }
        }

        private void Spanish_Btn_Click(object sender, RoutedEventArgs e)
        {
            Fade(dwhududnwi0, dwhududnwi0.Opacity, 0, 0.3);
            Fade(dwhududnwi1, dwhududnwi1.Opacity, 0, 0.3);
            Fade(dwhududnw2, dwhududnw2.Opacity, 1, 0.3);

            lm.lang = 3;

            try
            {
                TransL();
                Wolfregkey.SetValue("Idioma", "ES");
            }
            catch { }
        }



        private bool animatiooo = false;

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
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

        private async void ConfirmLangButton_Click(object sender, RoutedEventArgs e)
        {
            if (animatiooo)
                return;

            animatiooo = true;

            Move(W2_page0, new Thickness(60, 0, 0, 0), new Thickness(-1395, 0, 0, 0), 1.2);
            Move(W2_page2, new Thickness(900, 0, 0, 0), new Thickness(0, 0, 0, 0), 1.2);
            W2_page2.Visibility = Visibility.Visible;

            await Task.Delay(1000);
            animatiooo = false;
        }


        private async void change_img1_Click(object sender, RoutedEventArgs e)
        {
            await Task.Delay(500);

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = "Select an Image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string destinationDirectory = "C:\\Essence\\userdata\\UserImgs";
                    if (!Directory.Exists(destinationDirectory))
                    {
                        Directory.CreateDirectory(destinationDirectory);
                    }

                    string destinationFilePath = Path.Combine(destinationDirectory, Path.GetFileName(openFileDialog.FileName));
                    File.Copy(openFileDialog.FileName, destinationFilePath, true);

                    user_avatar.Text = destinationFilePath;
                    Wolfregkey.SetValue("Avatar", destinationFilePath);
                    avatar_sync.Visibility = Visibility.Collapsed;
                    Wolfregkey.SetValue("DiscordAvatarSync", "False");
                    //Color pixelColor = MainWindow.GetPixelColor(bitmap);
                    //User_Img2_Color.Color = pixelColor;

                    await Task.Delay(200);

                    BitmapImage defaultBitmap = new BitmapImage(new Uri(destinationFilePath, UriKind.RelativeOrAbsolute));
                    ImageBehavior.SetAnimatedSource(User_Img2, defaultBitmap);
                }
                catch
                {

                }
            }
        }



        private async void finishbtn_Click(object sender, RoutedEventArgs e)
        {
            finishbtn.IsEnabled = false;
            registryKey1.SetValue("Boot", "2");
            await Task.Delay(600);

            Wolfregkey.SetValue("Nome", user_name7.Text);
            Wolfregkey.SetValue("Avatar", user_avatar.Text);

            if (user_name7.Text == Username)
                Wolfregkey.SetValue("DiscordNameSync", "True");
            else
                Wolfregkey.SetValue("DiscordNameSync", "False");


            if (user_avatar.Text == AvatarImage)
                Wolfregkey.SetValue("DiscordNameSync", "True");
            else
                Wolfregkey.SetValue("DiscordNameSync", "False");


            registryKey1.SetValue("Termos", Essence.Properties.Resources.Contrato.Length.ToString());

            Fade(this, 1, 0, 2);
            await Task.Delay(2000);

            Process.Start(Process.GetCurrentProcess().MainModule.FileName);
            Process.GetCurrentProcess().Kill();
        }


        private void reset_img_Click(object sender, RoutedEventArgs e)
        {
            user_avatar.Text = AvatarImage;
        }










        bool iscreatingacc = true;


        string[] userssss = { "" };
        private async void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            await Task.Delay(400);

            LoginTXT.IsEnabled = false;
            PassTXT.IsEnabled = false;

            try
            {
                if (!Regex.IsMatch(LoginTXT.Text, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    LoginTXT.IsEnabled = true;
                    PassTXT.IsEnabled = true;

                    //animar o logintxt
                    ShakeTextBox(LoginTXT);
                    return;
                }

                if (PassTXT.Text.Length <= 5)
                {
                    LoginTXT.IsEnabled = true;
                    PassTXT.IsEnabled = true;

                    //animar o passtxt
                    ShakeTextBox(PassTXT);

                    return;
                }
            }
            catch
            {
                LoginTXT.IsEnabled = true;
                PassTXT.IsEnabled = true;
                return;
            }

            //if (byb77b7tvb.Visibility == Visibility.Visible && Inviteshitty.Text.Length != 6 && MessageBox.Show("Proceed without adding an invite code?", "Question", MessageBoxButton.YesNo) == MessageBoxResult.No)
            //{
            //    LoginTXT.IsEnabled = true;
            //    PassTXT.IsEnabled = true;
            //    return;
            //}

            CreateAccount.IsEnabled = false;
            EnterUsingDiscord.IsEnabled = false;
            Gif6.Visibility = Visibility.Visible;

            Move(Gif6, new Thickness(0, 0, 0, -80), new Thickness(0, 0, 0, 0), 0.6);
            Move(LoginBtnTXT2, new Thickness(0, 0, 0, 0), new Thickness(0, -80, 0, 0), 0.4);

            Fade(Fader, 0, 0.7, 0.3);
            Fader.Visibility = Visibility.Visible;

            await Task.Delay(1200);
            var matches = Regex.Matches(await KeyGay2.Get("registreds"), @"'([^']*)'");
            userssss = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                userssss[i] = matches[i].Groups[1].Value;
            }
            //if (!userssss.Contains(LoginTXT.Text) && crea)
            //{
            //    bool result = await Notificar2("Info", "this email dont exist in our servers. do you want to create an new account?");

            //    if (!result)
            //    {
            //        CloseEmailConfirmation();
            //        return;
            //    }
            //}

            string response = "";
            if (iscreatingacc)
                response = await KeyGay2.Get("emailcreate", LoginTXT.Text + "\n" + PassTXT.Text + "\n" + Wolfregkey.GetValue("Idioma") + "\n" + Inviteshitty.Text);
            else
                response = await KeyGay2.Get("login", LoginTXT.Text + "\n" + PassTXT.Text + "\n" + "null" + "\n" + "null" + "\n" + "null");
            MessageBox.Show(response);

            try
            {
                var userData = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

                if (userData != null && userData.ContainsKey("email"))
                {
                    await Task.Delay(300);

                    KeyGay2.current_user = LoginTXT.Text;
                    KeyGay2.current_pass = PassTXT.Text;

                    List<string> linhas = new List<string>(File.ReadAllLines("C:\\Essence\\userdata\\logindata.txt"));
                    linhas[0] = LoginTXT.Text;
                    linhas[1] = PassTXT.Text;
                    File.WriteAllLines("C:\\Essence\\userdata\\logindata.txt", linhas);

                    try
                    {
                        user_name7.Text = userData["nome"].ToString();

                        user_avatar.Text = userData["avatar"].ToString();

                        await Task.Delay(200);

                        BitmapImage bitmap = new BitmapImage(new Uri(userData["avatar"].ToString(), UriKind.RelativeOrAbsolute));
                        ImageBehavior.SetAnimatedSource(User_Img2, bitmap);
                    }
                    catch { }


                    avatar_sync.Visibility = Visibility.Collapsed;
                    name_sync.Visibility = Visibility.Collapsed;

                    if (animatiooo)
                        return;

                    animatiooo = true;
                    Move(W2_page2, new Thickness(0, 0, 0, 0), new Thickness(-1495, 0, 0, 0), 1.2);
                    shutup = false;


                    //fefefef.Visibility = Visibility.Collapsed;

                    Move(W2_page32, new Thickness(900, 0, 0, 0), new Thickness(0, 0, 0, 0), 1.2);
                    W2_page32.Visibility = Visibility.Visible;

                    await Task.Delay(1200);
                    animatiooo = false;
                }
                else
                {
                    MessageBox.Show("Erro no login. Detalhes: " + response);
                    CloseEmailConfirmation();
                }
            }
            catch (JsonException)
            {
                LoginTXT.IsEnabled = true;
                PassTXT.IsEnabled = true;

                //MessageBox.Show(response, "DNudnaujnnfjmfgjfm");

                switch (response)
                {
                    case "user-created":
                        Welcome_Page3_Title1_Copiar2.Text = "We've sent a code to '" + LoginTXT.Text + "'";
                        EmailConfirmationPage2.Visibility = Visibility.Visible;
                        Move(EmailConfirmationPage2, new Thickness(0, 40, 0, -300), new Thickness(0, 40, 0, 0), 0.6);

                        await Task.Delay(300);

                        ClearEmailCodes();
                        dfuwaduwbdu = true;
                        break;

                    case "user-aready-exist":
                        MessageBox.Show("this email is aready registred. and the password dont match.");
                        CloseEmailConfirmation();
                        break;

                    case "new-location-detected" or "verification-pending":
                        Welcome_Page3_Title1_Copiar2.Text = "We've sent a code to '" + LoginTXT.Text + "'";
                        EmailConfirmationPage2.Visibility = Visibility.Visible;
                        Move(EmailConfirmationPage2, new Thickness(0, 40, 0, -300), new Thickness(0, 40, 0, 0), 0.6);

                        await Task.Delay(300);

                        ClearEmailCodes();
                        dfuwaduwbdu = true;
                        break;

                    case "multiple-machines-using-this-account":
                        MessageBox.Show("Other machine is aready using this account and you dont have an signature that suports that.");
                        CloseEmailConfirmation();
                        break;

                    case "incorect-password":
                        ShakeTextBox(PassTXT);
                        CloseEmailConfirmation();
                        MessageBox.Show("Incorrect Passsword or email");
                        break;

                    case "user-not-found":
                        MessageBox.Show("This email is not registred");
                        CloseEmailConfirmation();
                        break;

                    default:
                        MessageBox.Show(response, "Internal Server Error. Try Again.");
                        CloseEmailConfirmation();
                        break;
                }
            }
            catch (Exception ex)
            {
                CloseEmailConfirmation();
                MessageBox.Show(ex.ToString());
            }
        }

        private void forgotpass_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Notificar2("Confirmation", $"we are going to send an code to {LoginTXT.Text}. If its all okay, then press OK");
        }

        private void ClearEmailCodes()
        {
            digit1.Clear();
            digit1.IsEnabled = true;
            digit1.Focus();

            digit2.Clear();
            digit2.IsEnabled = true;

            digit3.Clear();
            digit3.IsEnabled = true;

            digit4.Clear();
            digit4.IsEnabled = true;

            digit5.Clear();
            digit5.IsEnabled = true;

            digit6.Clear();
            digit6.IsEnabled = true;

            Gif7.Visibility = Visibility.Collapsed;
            //ResendEmailCode.Visibility = Visibility.Visible;
        }


        private async void LoginTXT_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                await Task.Delay(500);

                //email_mal_formatado.Visibility = Visibility.Collapsed;
                email_in_use.Visibility = Visibility.Collapsed;
                if (LoginTXT.Text != "Enter an email" && LoginTXT.Text != "Enter your email")
                {
                    //if (!Regex.IsMatch(LoginTXT.Text, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                    //    email_mal_formatado.Visibility = Visibility.Visible;

                    if (userssss.Contains(LoginTXT.Text) && iscreatingacc && Regex.IsMatch(LoginTXT.Text, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                        email_in_use.Visibility = Visibility.Visible;             
                }
            }
            catch { }
        }

        private void PassTXT_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (LoginTXT.Text.Length > 2 && PassTXT.Text.Length > 5)
                {
                    //loginbtn_border.Background = new SolidColorBrush(Color.FromRgb(200, 20, 20));
                }
                else
                {
                    //loginbtn_border.Background = new SolidColorBrush(Color.FromRgb(50, 50, 50));
                }
            }
            catch { }
        }



        private async void CloseEmailConfirmation()
        {
            dfuwaduwbdu = false;
            Fade(Fader, 0.7, 0, 0.3);
            await Task.Delay(200);
            Move(EmailConfirmationPage2, new Thickness(0, 40, 0, 0), new Thickness(0, 40, 0, -600), 0.4);
            await Task.Delay(100);
            Fader.Visibility = Visibility.Collapsed;


            await Task.Delay(300);
            Move(Gif6, new Thickness(0, 0, 0, 0), new Thickness(0, 0, 0, -80), 0.4);
            Move(LoginBtnTXT2, new Thickness(0, -80, 0, 0), new Thickness(0, 0, 0, 0), 0.6);

            EmailConfirmationPage2.Visibility = Visibility.Collapsed;

            LoginTXT.IsEnabled = true;
            PassTXT.IsEnabled = true;

            CreateAccount.IsEnabled = true;
            EnterUsingDiscord.IsEnabled = true;

            Gif7.Visibility = Visibility.Collapsed;
            //ResendEmailCode.Visibility = Visibility.Visible;
        }



        private void EmailConfirmationPage_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseEmailConfirmation();
        }

        private void ResendEmailCode_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Notificar2("Confirmation", "faaineifnaeifimaeif?");
        }

        private async void checkcodee()
        {
            digit1.IsEnabled = false;
            digit2.IsEnabled = false;
            digit3.IsEnabled = false;
            digit4.IsEnabled = false;
            digit5.IsEnabled = false;
            digit6.IsEnabled = false;


            //ResendEmailCode.Visibility = Visibility.Collapsed;
            Gif7.Visibility = Visibility.Visible;

            await Task.Delay(2000);

            try
            {
                Topmost = false;

                string response = await KeyGay2.Get("emailverify", LoginTXT.Text + "\n" + digit1.Text + digit2.Text + digit3.Text + digit4.Text + digit5.Text + digit6.Text + "\n" + Inviteshitty.Text);


                try
                {
                    var userData = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

                    if (userData != null && userData.ContainsKey("email"))
                    {
                        await Task.Delay(300);

                        KeyGay2.current_user = LoginTXT.Text;
                        KeyGay2.current_pass = PassTXT.Text;

                        List<string> linhas = new List<string>(File.ReadAllLines("C:\\Essence\\userdata\\logindata.txt"));
                        linhas[0] = LoginTXT.Text;
                        linhas[1] = PassTXT.Text;
                        File.WriteAllLines("C:\\Essence\\userdata\\logindata.txt", linhas);

                        try
                        {
                            user_name7.Text = userData["nome"].ToString();

                            user_avatar.Text = userData["avatar"].ToString();

                            await Task.Delay(200);

                            BitmapImage bitmap = new BitmapImage(new Uri(userData["avatar"].ToString(), UriKind.RelativeOrAbsolute));
                            ImageBehavior.SetAnimatedSource(User_Img2, bitmap);
                        }
                        catch { }


                        avatar_sync.Visibility = Visibility.Collapsed;
                        name_sync.Visibility = Visibility.Collapsed;

                        if (animatiooo)
                            return;

                        animatiooo = true;
                        Move(W2_page2, new Thickness(0, 0, 0, 0), new Thickness(-1495, 0, 0, 0), 1.2);
                        shutup = false;

                        //fefefef.Visibility = Visibility.Collapsed;

                        Move(W2_page32, new Thickness(900, 0, 0, 0), new Thickness(0, 0, 0, 0), 1.2);
                        W2_page32.Visibility = Visibility.Visible;

                        await Task.Delay(1200);
                        animatiooo = false;
                    }
                    else
                    {
                        MessageBox.Show("Erro no login. Detalhes: " + response);
                        CloseEmailConfirmation();
                    }
                }
                catch (JsonException)
                {
                    switch (response)
                    {
                        case "incorrect-code":
                            ClearEmailCodes();
                            MessageBox.Show("incorrect-code");
                            break;

                        case "code-expired":
                            ClearEmailCodes();
                            MessageBox.Show("Your verify code expired. Try again to generate a new one");
                            break;

                        case "user-not-found":
                            CloseEmailConfirmation();
                            MessageBox.Show("user-not-found");
                            break;

                        default:
                            CloseEmailConfirmation();
                            MessageBox.Show(response, "Internal Server Error. Try Again.");
                            break;
                    }
                }
                Topmost = true;

            }
            catch
            {
                CloseEmailConfirmation();
            }
        }



        private bool dfuwaduwbdu = false;
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!dfuwaduwbdu) return;

            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (Clipboard.ContainsText())
                {
                    string pastedText = Clipboard.GetText();

                    var matches = Regex.Matches(pastedText, @"\d");
                    string[] numericStrings = matches.Cast<Match>().Select(m => m.Value).Take(6).ToArray();

                    if (numericStrings.Length == 6)
                    {
                        digit1.Text = numericStrings[0];
                        digit2.Text = numericStrings[1];
                        digit3.Text = numericStrings[2];
                        digit4.Text = numericStrings[3];
                        digit5.Text = numericStrings[4];
                        digit6.Text = numericStrings[5];

                        checkcodee();
                    }

                }
            }
        }


        private void digit1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
                Dispatcher.BeginInvoke(new Action(() => digit2.Focus()), System.Windows.Threading.DispatcherPriority.Background);
            else if (e.Key != Key.Back && e.Key != Key.Delete)
                e.Handled = true;
        }

        private void digit2_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = !(e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9);

            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                if (digit2.Text != "")
                    digit2.Text = "";
                else
                {
                    digit1.Text = "";
                    digit1.Focus();
                }
            }
            else if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
                Dispatcher.BeginInvoke(new Action(() => digit3.Focus()), System.Windows.Threading.DispatcherPriority.Background);
            else
                e.Handled = true;
        }

        private void digit3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                if (digit3.Text != "")
                    digit3.Text = "";
                else
                {
                    digit2.Text = "";
                    digit2.Focus();
                }
            }
            else if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
                Dispatcher.BeginInvoke(new Action(() => digit4.Focus()), System.Windows.Threading.DispatcherPriority.Background);
            else
                e.Handled = true;
        }

        private void digit4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                if (digit4.Text != "")
                    digit4.Text = "";
                else
                {
                    digit3.Text = "";
                    digit3.Focus();
                }
            }
            else if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
                Dispatcher.BeginInvoke(new Action(() => digit5.Focus()), System.Windows.Threading.DispatcherPriority.Background);
            else
                e.Handled = true;
        }

        private void digit5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                if (digit5.Text != "")
                    digit5.Text = "";
                else
                {
                    digit4.Text = "";
                    digit4.Focus();
                }
            }
            else if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
                Dispatcher.BeginInvoke(new Action(() => digit6.Focus()), System.Windows.Threading.DispatcherPriority.Background);
            else
                e.Handled = true;
        }

        private async void digit6_KeyDown(object sender, KeyEventArgs e)
        {
            if (digit1.IsEnabled == false)
                return;

            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                if (digit6.Text != "")
                    digit6.Text = "";
                else
                {
                    digit5.Text = "";
                    digit5.Focus();
                }
            }
            else if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {                
                Dispatcher.BeginInvoke(new Action(() => checkcodee()), System.Windows.Threading.DispatcherPriority.Background);
            }
            else
                e.Handled = true;
        }



        private static int GetAvailablePort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            try
            {
                listener.Start();
                return ((IPEndPoint)listener.LocalEndpoint).Port;
            }
            catch
            {
                return 49160;
            }
            finally
            {
                listener.Stop();
            }
        }

        private string Username = "";
        private string AvatarImage = "";
        private bool logged_with_discord = false;
        private string last_auth_link = "";
        private async void EnterUsingDiscord_Click(object sender, RoutedEventArgs e)
        {
            //if (byb77b7tvb.Visibility == Visibility.Visible && Inviteshitty.Text.Length != 6 && MessageBox.Show("Proceed without adding an invite code?", "Question", MessageBoxButton.YesNo) == MessageBoxResult.No)
            //    return;


            Gif5.Visibility = Visibility.Visible;
            CreateAccount.IsEnabled = false;
            EnterUsingDiscord.IsEnabled = false;

            Move(Gif5, new Thickness(0, 0, 0, -80), new Thickness(0, 0, 0, 0), 0.6);
            Move(EnterUsingDiscordTXT, new Thickness(0, 0, 0, 0), new Thickness(0, -80, 0, 0), 0.4);

            Fade(Fader, 0, 0.7, 0.3);
            Fader.Visibility = Visibility.Visible;

            await Task.Delay(1200);

            DiscordConfirmationPage2.Visibility = Visibility.Visible;
            Move(DiscordConfirmationPage2, new Thickness(0, 40, 0, -300), new Thickness(0, 40, 0, 0), 0.6);

            int port = GetAvailablePort();

            string[] values = KeyGay2.Gen();
            string[] hwidddd = KeyGay2.GetHWID();
            last_auth_link = $"https://discord.com/api/oauth2/authorize?client_id=1336373573744332963&redirect_uri=https://essenceapi.discloud.app/oauth_redirect&response_type=code&scope=email+identify+guilds.join&state=1210371208252760074...{port}...{values[0]}...{values[1]}...{hwidddd[0]}...{hwidddd[1]}...{Wolfregkey.GetValue("Idioma")}...{Inviteshitty.Text}";
            Process.Start(last_auth_link);

            await Task.Delay(1000);

            string url = $"http://localhost:{port}/";
            using var server = new HttpListener();
            server.Prefixes.Add(url);
            server.Start();

            var context = await server.GetContextAsync();
            if (context.Request.IsWebSocketRequest)
            {
                var webSocketContext = await context.AcceptWebSocketAsync(null);
                var webSocket = webSocketContext.WebSocket;

                var buffer = new byte[1024];
                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        Console.WriteLine("Received");

                        string resposta = Encoding.UTF8.GetString(buffer, 0, result.Count);

                        if (resposta.Length < 100)
                            resposta = "Error";

                        if (!resposta.Contains("Key:"))
                            resposta = "Error";

                        else
                        {
                            string[] resp_key = resposta.Split(new string[] { "Key:" }, StringSplitOptions.RemoveEmptyEntries);

                            string chave = $"M4A1-KEY-{values[1]}-{resp_key[1]}";
                            string re = KeyGay2.Desencriptar(resp_key[0], chave);
                            resposta = re;
                        }

                        //MessageBox.Show(resposta);



                        if (resposta != "Error" && resposta != "777")
                        {
                            try
                            {
                                string[] dados = resposta.Split(new string[] { " -M4separator- " }, StringSplitOptions.RemoveEmptyEntries);
                                logged_with_discord = true;


                                string userid = dados[0];
                                string username = dados[1];
                                Username = username;

                                user_name7.Text = username;

                                string imagem = dados[2];
                                AvatarImage = imagem;
                                user_avatar.Text = imagem;

                                string token = dados[3];
                                //string email = dados[4];

                                KeyGay2.current_user = userid;

                                List<string> linhas = new List<string>(File.ReadAllLines("C:\\Essence\\userdata\\logindata.txt"));

                                linhas[0] = userid;
                                //linhas[0] = email;
                                linhas[2] = userid;
                                linhas[4] = imagem;

                                //segurança para o token
                                string[] result5 = KeyGay2.Encriptar(token);
                                linhas[5] = result5[0] + " " + result5[1];

                                File.WriteAllLines("C:\\Essence\\userdata\\logindata.txt", linhas);


                                var matches = Regex.Matches(await KeyGay2.Get("registreds"), @"'([^']*)'");
                                userssss = new string[matches.Count];
                                for (int i = 0; i < matches.Count; i++)
                                {
                                    userssss[i] = matches[i].Groups[1].Value;
                                }

                                ermmm();
                                return;

                                //if (email.Length > 5 && Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$") && !userssss.Contains(email))
                                //{
                                //    Move(DiscordConfirmationPage, new Thickness(0, 0, 30, 93), new Thickness(0, 0, 30, -600), 0.4);
                                //    await Task.Delay(100);
                                //    Move(ConfirmEmailAcessible, new Thickness(0, 0, 30, -300), new Thickness(0, 0, 30, 93), 0.6);
                                //    ConfirmEmailAcessible.Visibility = Visibility.Visible;


                                //    Welcome_Page3_Title1_Copiar4.Text = $"Can we associate the email \"{email.Split('@')[0].Substring(0, email.Split('@')[0].Length - 3) + "..."}\" with your account and use it for identity authentication purposes?";
                                //}
                                //else
                                //{
                                //    //MessageBox.Show(email);
                                //    ermmm();
                                //}
                            }
                            catch (Exception ex)
                            {
                                CloseDc();
                                await Task.Delay(400);
                                Notificar2("Could not connect to discord.", ex.Message);
                                return;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error in validating response");
                            CloseDc();
                            await Task.Delay(400);
                            Notificar2("Could not connect to discord.", $"The site response was malformed. Try again");
                        }
                    }
                }
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
        }

        private async void CloseDc()
        {
            Fade(Fader, 0.7, 0, 0.3);
            await Task.Delay(200);
            Move(DiscordConfirmationPage2, new Thickness(0, 40, 0, 0), new Thickness(0, 40, 0, -600), 0.4);
            await Task.Delay(100);
            Fader.Visibility = Visibility.Collapsed;


            await Task.Delay(300);
            DiscordConfirmationPage2.Visibility = Visibility.Collapsed;

            Move(Gif5, new Thickness(0, 0, 0, 0), new Thickness(0, 0, 0, -80), 0.4);
            Move(EnterUsingDiscordTXT, new Thickness(0, -80, 0, 0), new Thickness(0, 0, 0, 0), 0.6);

            LoginTXT.IsEnabled = true;
            PassTXT.IsEnabled = true;

            CreateAccount.IsEnabled = true;
            EnterUsingDiscord.IsEnabled = true;
        }

        private async void CloseDiscordConfirmationPage_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseDc();
        }

        private void ReopenAuthLink_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start(last_auth_link);
        }


        private void LoginTXT_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (LoginTXT.Text == "Enter your email" || LoginTXT.Text == "Enter an email")
                LoginTXT.Clear();
        }

        private void PassTXT_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (PassTXT.Text == "Enter your password" || PassTXT.Text == "Create a password")
                PassTXT.Clear();
        }



        private async void ermmm()
        {
            Fade(Fader, 0.7, 0, 0.3);
            await Task.Delay(200);
            Move(ConfirmEmailAcessible, new Thickness(0, 0, 30, 93), new Thickness(0, 0, 30, -600), 0.4);
            Move(DiscordConfirmationPage2, new Thickness(0, 40, 0, 0), new Thickness(0, 40, 0, -600), 0.4);
            await Task.Delay(100);
            Fader.Visibility = Visibility.Collapsed;
            await Task.Delay(300);
            ConfirmEmailAcessible.Visibility = Visibility.Collapsed;


            animatiooo = true;
            Move(W2_page2, new Thickness(0, 0, 0, 0), new Thickness(-1495, 0, 0, 0), 1.2);
            shutup = false;

            Move(W2_page32, new Thickness(900, 0, 0, 0), new Thickness(0, 0, 0, 0), 1.2);
            W2_page32.Visibility = Visibility.Visible;

            await Task.Delay(1200);
            animatiooo = false;
        }

        private void OKNotif_Click(object sender, RoutedEventArgs e)
        {
            ermmm();
        }


        private void CancelNotf_Click(object sender, RoutedEventArgs e)
        {
            List<string> linhas = new List<string>(File.ReadAllLines("C:\\Essence\\userdata\\logindata.txt"));
            linhas[0] = "---";
            File.WriteAllLines("C:\\Essence\\userdata\\logindata.txt", linhas);

            ermmm();
        }









        private async void Continue2_Click(object sender, RoutedEventArgs e)
        {
            if (animatiooo)
                return;

            animatiooo = true;

            Move(W2_page32, new Thickness(0, 0, 0, 0), new Thickness(-1395, 0, 0, 0), 1.2);
            Move(W2_page42, new Thickness(900, 0, 0, 0), new Thickness(0, 0, 0, 0), 1.2);



            W2_page42.Visibility = Visibility.Visible;

            await Task.Delay(1200);
            W2_page32.Visibility = Visibility.Collapsed;
            animatiooo = false;
        }

        private void user_name7_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!logged_with_discord)
                return;

            try
            {
                if (user_name7.Text == Username)
                    name_sync.Visibility = Visibility.Visible;
                else
                    name_sync.Visibility = Visibility.Collapsed;
            }
            catch { }
        }

        private void ShakeTextBox(TextBox textBox)
        {
            var animation = new DoubleAnimationUsingKeyFrames
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                RepeatBehavior = new RepeatBehavior(1)
            };

            var keyFrames = new DoubleKeyFrameCollection
            {
                new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))),
                new LinearDoubleKeyFrame(-15, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.05))),
                new LinearDoubleKeyFrame(15, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.1))),
                new LinearDoubleKeyFrame(-15, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.15))),
                new LinearDoubleKeyFrame(15, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.2))),
                new LinearDoubleKeyFrame(-15, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.25))),
                new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.3)))
            };

            animation.KeyFrames = keyFrames;

            var transform = new TranslateTransform();
            textBox.RenderTransform = transform;

            transform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        

        private async void user_avatar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (user_avatar.Text == "")
                user_avatar.Text = lm.Translate("Insert an image link here!");

            if (user_avatar.Text == lm.Translate("Insert an image link here!"))
            {
                try
                {
                    BitmapImage defaultBitmap = new BitmapImage(new Uri("https://i.imgur.com/hxNqiz8.png", UriKind.RelativeOrAbsolute));
                    ImageBehavior.SetAnimatedSource(User_Img2, defaultBitmap);
                    return;
                }
                catch { }
            }
            else if (user_avatar.Text.Contains("https"))
            {
                try
                {
                    BitmapImage eee = new BitmapImage(new Uri("pack://application:,,,/ImageResources/Settings/Rolling-1s-100px.gif"));
                    ImageBehavior.SetAnimatedSource(User_Img2, eee);

                    using (HttpClient client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromSeconds(5);
                        HttpResponseMessage response = await client.GetAsync(user_avatar.Text);
                        if (response.IsSuccessStatusCode)
                        {
                            eee = new BitmapImage(new Uri(user_avatar.Text, UriKind.RelativeOrAbsolute));
                            ImageBehavior.SetAnimatedSource(User_Img2, eee);
                        }
                        else
                        {
                            eee = new BitmapImage(new Uri("https://i.imgur.com/hxNqiz8.png", UriKind.RelativeOrAbsolute));
                            ImageBehavior.SetAnimatedSource(User_Img2, eee);
                        }
                    }
                }
                catch
                {
                    BitmapImage eee = new BitmapImage(new Uri("https://i.imgur.com/hxNqiz8.png", UriKind.RelativeOrAbsolute));
                    ImageBehavior.SetAnimatedSource(User_Img2, eee);
                }
            }
            

            if (!logged_with_discord)
                return;

            try
            {
                if (user_avatar.Text == AvatarImage)
                    avatar_sync.Visibility = Visibility.Visible;
                else
                    avatar_sync.Visibility = Visibility.Collapsed;
            }
            catch { }
        }

        private void Run_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Concordo.IsEnabled = true;
            Discordo.IsEnabled = true;
            //EVXTXT2.Text = "| EULA and Privacity Police";

            //versão1.Text = "Versão: " + ExecSettings.CurrentVersion;

            int first_line = 1;
            EULARichTextBox.IsReadOnly = true;
            string[] palavras = new string[0] { /*"Essence", "Essence.", "Ian", "m4a1_lindo2.", "\"Essence\"", "\"Essence\".", "\"Essence\":", "(\"Essence\")", "\"Essence\","*/ };
            EULARichTextBox.Document.Blocks.Clear();
            string[] array = Essence.Properties.Resources.Contrato.Split(new string[2] { "\r\n", "\n" }, StringSplitOptions.None);
            foreach (string line in array)
            {
                Paragraph paragraph = new Paragraph();
                string[] array2 = line.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                bool lineContainsSpecialWord = false;
                string[] array3 = array2;
                for (int num2 = 0; num2 < array3.Length; num2++)
                {
                    string processedWord = array3[num2].TrimEnd('#');
                    Run run = new Run(processedWord.Replace("#", "") + " ");
                    if (processedWord.StartsWith("#"))
                    {
                        lineContainsSpecialWord = true;
                    }
                    else if (palavras.Contains(processedWord, StringComparer.OrdinalIgnoreCase))
                    {
                        run.FontWeight = FontWeights.Bold;
                        run.Foreground = Brushes.DodgerBlue;
                    }
                    paragraph.Inlines.Add(run);
                }
                if (lineContainsSpecialWord)
                {
                    paragraph.FontWeight = FontWeights.Bold;
                    paragraph.FontSize = 13.5;
                    paragraph.Foreground = Brushes.White;
                }
                else if (first_line < 3)
                {
                    first_line += 1;
                    paragraph.FontWeight = FontWeights.Bold;
                    paragraph.FontSize = 15.0;
                    paragraph.Foreground = Brushes.White;
                }
                else
                {
                    paragraph.FontWeight = FontWeights.Normal;
                    paragraph.FontSize = 11.0;
                    paragraph.Foreground = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                }
                EULARichTextBox.Document.Blocks.Add(paragraph);
            }


            Fade(EULA, 0, 1, 0.5);
            EULA.Visibility = Visibility.Visible;
        }



        private void Discordo_Click(object sender, RoutedEventArgs e)
        {
            Discordo.IsEnabled = false;

            MessageBox.Show("You can't use Essence withou accepting this :(", "Erm");
            MainWin.BeginAnimationP(UIElement.OpacityProperty, new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.4)));
            Process.GetCurrentProcess().Kill();
        }

        private async void Concordo_Click(object sender, RoutedEventArgs e)
        {
            //EVXTXT2.Text = "";

            Concordo.IsEnabled = false;
            Discordo.IsEnabled = false;
            registryKey1.SetValue("Termos", Essence.Properties.Resources.Contrato.Length.ToString());
            await Task.Delay(700);

            Fade(EULA, 1, 0, 0.5);
            await Task.Delay(500);
            EULA.Visibility = Visibility.Collapsed;
        }


        private async void Inviteshitty_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Inviteshitty.Text.Length == 6 && KeyGay2.firstlogin && Inviteshitty.Text != "Insert it here!")
                {
                    Inviteshitty.IsEnabled = false;
                    Inviteshitty.Text = Inviteshitty.Text.ToUpper();
                }
            }
            catch { }
        }

        private void Inviteshitty_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Inviteshitty.Text == "Insert it here!")
                Inviteshitty.Text = "";
        }

        private void SignUpBtn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            iscreatingacc = true;

            Welcome_Page3_Title1.Text = "Sign up account";

            Fade(login_txt, 1, 0.7, 0.3);
            Fade(loginborder, 1, 0, 0.3);

            Fade(signup_txt, 0.7, 1, 0.5);
            Fade(signupborder, 0, 1, 0.5);

            LoginBtnTXT.Text = "Sign Up";

            LoginTXT.Text = "Enter an email";
            PassTXT.Text = "Create a password";

            name_border.Visibility = Visibility.Visible;
            forgotpass.Visibility = Visibility.Collapsed;

            //if (KeyGay2.firstlogin)
            //    byb77b7tvb.Visibility = Visibility.Visible;
        }

        private void LogInBtn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            iscreatingacc = false;

            Welcome_Page3_Title1.Text = "Welcome back!";

            Fade(signup_txt, 1, 0.7, 0.3);
            Fade(signupborder, 1, 0, 0.3);

            Fade(login_txt, 0.7, 1, 0.5);
            Fade(loginborder, 0, 1, 0.5);

            LoginBtnTXT.Text = "Log In";

            LoginTXT.Text = "Enter your email";
            PassTXT.Text = "Enter your password";

            name_border.Visibility = Visibility.Collapsed;
            forgotpass.Visibility = Visibility.Visible;
            //byb77b7tvb.Visibility = Visibility.Collapsed;
            email_in_use.Visibility = Visibility.Collapsed;
            Inviteshitty.Text = "Insert it here!";
        }
    }
}
