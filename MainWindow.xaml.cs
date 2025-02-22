using DiscordRPC;
using DiscordRPC.Logging;
using Essence.Elements;
using Essence.Properties;
using HelixToolkit.Wpf;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using MaterialDesignThemes.Wpf;
using Microsoft.Web.WebView2.Core;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.IO.Pipes;
using System.Linq;
using System.Linq.Expressions;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using WpfAnimatedGif;
using JsonException = Newtonsoft.Json.JsonException;
using Ellipse = System.Windows.Shapes.Ellipse;
using System.Security.Cryptography;
using static System.Windows.Forms.LinkLabel;

namespace Essence
{
    internal class ExecSettings
    {
        internal static bool CollectGameData = true;

        internal static readonly string CurrentVersion = "1.0.0";
    }


    public class OwnerScript
    {
        public string ImageLink { get; set; }
        public string Name { get; set; }
        public string Script { get; set; }
        public string supported_games { get; set; }

        public OwnerScript(string imageLink, string name, string script, string moregames = "")
        {
            supported_games = moregames.ToLower();
            ImageLink = imageLink;
            Name = name;
            Script = script;
        }
    }

    public static class AnimationExtensions
    {
        public static void BeginAnimationP(this UIElement element, DependencyProperty property, AnimationTimeline animation, DispatcherPriority priority = DispatcherPriority.Render)
        {
            Application.Current.Dispatcher.BeginInvoke(priority, new Action(() =>
            {
                element.BeginAnimation(property, animation);
            }));
        }
    }
    public class Games
    {
        public string title;
        public string id;
        public string image;
    }


    public class CreationLogger
    {
        public static readonly DependencyProperty EnableLoggingProperty =
            DependencyProperty.RegisterAttached(
                "EnableLogging",
                typeof(bool),
                typeof(CreationLogger),
                new PropertyMetadata(false, OnEnableLoggingChanged));

        public static bool GetEnableLogging(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableLoggingProperty);
        }

        public static void SetEnableLogging(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableLoggingProperty, value);
        }

        private static void OnEnableLoggingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                string controlName = GetControlName(d);
                Console.WriteLine($"Construting: {controlName}");
            }
        }

        private static string GetControlName(DependencyObject obj)
        {
            if (obj is FrameworkElement element)
            {
                return string.IsNullOrWhiteSpace(element.Name) ? element.GetType().Name : element.Name;
            }
            if (obj is FrameworkContentElement contentElement)
            {
                return string.IsNullOrWhiteSpace(contentElement.Name) ? contentElement.GetType().Name : contentElement.Name;
            }
            return obj.GetType().Name;
        }
    }


    public partial class MainWindow : Window
    {
        private DispatcherTimer raintimer;
        private Random random = new Random();
        private string[] pathStrings = new string[]
        {
            //PATHS DE NATAL

            //"M23 47.689v-6.342l-3.357 1.992L18 42.305v-2.229l5-2.986v-4.168l-4 2.451v-4.416l-4 2.094v5.99l-1.653 1.23L12 39.16v-4.012L6.426 38.27L4 37.271v-2.529l5.685-3.17L6 29.75v-2.32l2.123-1.127l5.214 3.068l3.612-2.084l-.082-.065l-3.665-2.123l3.568-2.228l-3.577-2.083L7.98 23.84L6 22.871v-2.307l3.542-1.978L4 15.533v-2.529l2.321-1.114L12 15.087v-4.076l1.485-1.127l1.943 1.18l-.056 6.105l3.673 2.122l.033-4.311L23 17.079v-4.167l-5-2.988V7.71l1.643-1.05L23 8.652V2.324L24.994 1L27 2.324v6.328l3.906-2.031L33 7.84v1.992l-6 3.08v4.167l4-2.267v4.534l4-2.084v-6.524l1.455-.866l1.545.865v4.167l5.842-3.08L46 13.042v2.359l-5.495 3.17L44 20.525v2.254l-1.83.996l-5.327-3.158l-3.679 2.346l3.549 2.228l-3.659 2.122l3.772 1.992l5.389-2.986L44 27.535v2.15l-3.32 1.887l5.32 3.17v2.49l-2.522 1.037L38 35.281v3.955l-1.52 1.049L35 39.236v-6.002l-4-2.213v4.168l-4-2.268v4.168l5 2.986v2.359l-1.647.904L27 41.348v6.342L24.994 49zm-1.466-22.597L23.42 28h3.514l1.613-2.908L26.843 22h-3.514z",
            //"m21.16 16.13l-2-1.15l.89-.24a1 1 0 1 0-.52-1.93l-2.82.76L14 12l2.71-1.57l2.82.76h.26a1 1 0 0 0 .26-2L19.16 9l2-1.15a1 1 0 0 0-1-1.74L18 7.37l.3-1.11a1 1 0 1 0-1.93-.52l-.82 3L13 10.27V7.14l2.07-2.07a1 1 0 0 0 0-1.41a1 1 0 0 0-1.42 0l-.65.65V2a1 1 0 0 0-2 0v2.47l-.81-.81a1 1 0 0 0-1.42 0a1 1 0 0 0 0 1.41L11 7.3v3L8.43 8.78l-.82-3a1 1 0 1 0-1.93.52L6 7.37L3.84 6.13a1 1 0 0 0-1 1.74l2 1.13l-.84.26a1 1 0 0 0 .26 2h.26l2.82-.76L10 12l-2.71 1.57l-2.82-.76A1 1 0 1 0 4 14.74l.89.24l-2 1.15a1 1 0 0 0 1 1.74L6 16.63l-.3 1.11A1 1 0 0 0 6.39 19a1.2 1.2 0 0 0 .26 0a1 1 0 0 0 1-.74l.82-3L11 13.73v3.13l-2.07 2.07a1 1 0 0 0 0 1.41a1 1 0 0 0 .71.3a1 1 0 0 0 .71-.3l.65-.65V22a1 1 0 0 0 2 0v-2.47l.81.81a1 1 0 0 0 1.42 0a1 1 0 0 0 0-1.41L13 16.7v-3l2.57 1.49l.82 3a1 1 0 0 0 1 .74a1.2 1.2 0 0 0 .26 0a1 1 0 0 0 .71-1.23L18 16.63l2.14 1.24a1 1 0 1 0 1-1.74Z",
            //"M7.5 2.793V1h1v1.793l1.146-1.147l.707.708L8.5 4.207v2.927l2.535-1.463l.678-2.532l.966.258l-.42 1.566l1.553-.896l.5.866l-1.553.896l1.566.42l-.258.966l-2.532-.678L9 8l2.535 1.463l2.532-.678l.258.966l-1.566.42l1.553.896l-.5.866l-1.553-.896l.42 1.566l-.966.258l-.678-2.532L8.5 8.866v2.927l1.853 1.853l-.707.708L8.5 13.207V15h-1v-1.793l-1.147 1.147l-.707-.708L7.5 11.793V8.866l-2.535 1.463l-.678 2.532l-.966-.258l.42-1.566l-1.553.896l-.5-.866l1.552-.896l-1.566-.42l.26-.966l2.531.678L7 8L4.465 6.537l-2.532.678l-.259-.966l1.566-.42l-1.552-.896l.5-.866l1.552.896l-.42-1.566l.967-.258l.678 2.532L7.5 7.134V4.207L5.646 2.354l.707-.708z",
            //"m12.707 2.293l4 4a1 1 0 0 1 .175 1.178c-.247.463-.633.775-1.01.987l2.835 2.835a1 1 0 0 1 0 1.414c-.478.478-1.082.77-1.634.952l2.634 2.634a1 1 0 0 1-.392 1.656c-.692.229-1.405.389-2.119.532c-.542.108-1.175.216-1.88.306l.633 1.897A1 1 0 0 1 15 22H9a1 1 0 0 1-.949-1.316l.633-1.897a27 27 0 0 1-1.88-.306a23 23 0 0 1-1.591-.378l-.53-.154a1 1 0 0 1-.39-1.656l2.634-2.634c-.552-.181-1.156-.474-1.634-.952a1 1 0 0 1 0-1.414l2.835-2.835c-.377-.212-.763-.524-1.01-.987a1 1 0 0 1 .175-1.178l4-4a1 1 0 0 1 1.414 0m.562 16.675a26 26 0 0 1-2.123.018l-.415-.018L10.387 20h3.226zM12 4.414L9.528 6.886l.1.029l.187.043l.276.046c.383.04.717.29.85.655a1 1 0 0 1-.234 1.048L7.63 11.784c.423.13.905.216 1.372.216a1 1 0 0 1 .705 1.707l-2.76 2.761l.514.103C8.639 16.793 10.218 17 12 17a24.7 24.7 0 0 0 4.26-.378l.544-.103l.25-.051l-2.761-2.76a1 1 0 0 1 .588-1.7l.118-.008a5 5 0 0 0 1.371-.216l-3.077-3.077a1 1 0 0 1 .488-1.683l.323-.05l.172-.036q.094-.02.196-.052z",
            //"m56.195 31.68l3.033-3.215c.346-.365.363-.939 0-1.301c-.346-.346-.953-.363-1.297 0q-2.06 2.185-4.121 4.367l.012-8.439l-1.838-.002l-.012 10.389l-4.477 4.743a17.1 17.1 0 0 0-4.429-6.043a12.9 12.9 0 0 0 1.843-6.647c0-2.233-.564-4.335-1.557-6.171c1.025.049 1.724-.13 1.932-.575c.354-.758-.783-2.106-2.817-3.567L47.03 8.99a.5.5 0 0 0 .118-.161c.447-.957-2.258-3.164-6.039-4.928c-3.783-1.764-7.211-2.416-7.658-1.459a.5.5 0 0 0-.047.191l-1.839 7.501c-2.427-.618-4.19-.623-4.544.136c-.28.602.379 1.574 1.674 2.679c-5.622 1.433-9.785 6.515-9.785 12.583c0 2.433.68 4.7 1.843 6.647a17.05 17.05 0 0 0-4.371 5.911l-4.353-4.611l-.013-10.389l-1.836.002l.01 8.439l-4.119-4.367c-.344-.363-.953-.346-1.299 0c-.361.361-.344.936 0 1.301q1.518 1.606 3.035 3.215L2 33.369l.514 1.762l6.709-1.949q.591.624 1.182 1.252q2.631 2.785 5.258 5.571A17 17 0 0 0 14.911 45c0 9.389 7.611 17 17 17s17-7.611 17-17a17 17 0 0 0-.712-4.854c.005-.005.011-.005.015-.01l5.635-5.971l.93-.984l6.711 1.949l.51-1.761zM30.932 14.581c1.194.765 2.6 1.539 4.143 2.259c2.017.94 3.952 1.639 5.594 2.063a10.92 10.92 0 0 1 .804 12.044q-.249.438-.534.852h-.001a10.98 10.98 0 0 1-9.027 4.733c-3.377 0-6.4-1.533-8.42-3.937a11 11 0 0 1-.604-.791l-.005-.007a11 11 0 0 1-.534-.851a10.93 10.93 0 0 1-1.438-5.417c0-5.732 4.414-10.45 10.022-10.948M31.91 60c-8.271 0-15-6.73-15-15c0-4.432 1.936-8.416 5.002-11.163c2.384 2.867 5.977 4.694 9.998 4.694s7.614-1.827 9.999-4.695C44.975 36.584 46.91 40.568 46.91 45c0 8.27-6.726 15-15 15",
            //"M21.95 10.99c-1.79-.03-3.7-1.95-2.68-4.22c-2.98 1-5.77-1.59-5.19-4.56C6.95.71 2 6.58 2 12c0 5.52 4.48 10 10 10c5.89 0 10.54-5.08 9.95-11.01M8.5 15c-.83 0-1.5-.67-1.5-1.5S7.67 12 8.5 12s1.5.67 1.5 1.5S9.33 15 8.5 15m2-5C9.67 10 9 9.33 9 8.5S9.67 7 10.5 7s1.5.67 1.5 1.5s-.67 1.5-1.5 1.5m4.5 6c-.55 0-1-.45-1-1s.45-1 1-1s1 .45 1 1s-.45 1-1 1",
            //"M3 9a1 1 0 0 1 1-1h16a1 1 0 0 1 1 1v2a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1zm9-1v13 M19 12v7a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2v-7m2.5-4a2.5 2.5 0 0 1 0-5A4.8 8 0 0 1 12 8a4.8 8 0 0 1 4.5-5a2.5 2.5 0 0 1 0 5",
            //"M69.74 20.72c-3.15 10.72 1.76 22.4 4.88 33.09c-11.63-2.31-23.04-7.52-34.08-4.61c-11.93 3.87 24.98 22.96 27.56 24.5c-7.8 7.69-16.56 13.37-21.19 22.81c11.48 3.69 26.31.72 36.45-1.18c3.19 8.27 10.8 29.87 20.14 26.27c11.7-4.6 15.8-23.88 19.1-34.52c9.3-1.7 27.2.97 27.4-7.43c.2-10.49-13.3-18.5-19.9-24.98c3.9-7.84 19.3-24.86 11.7-29.93c-13.7-4.28-27.8 7.76-38.9 16.42c-6.44-8.91-28.9-28.2-33.16-20.44m89.56 75.13c-7.2 5.05-15.2 7.25-23.4 8.45c-2.2 6.1-4.8 10.6-7.4 15.7c28.4 92.3 44.2 178 8.1 286.1c15.4.6 29.2 4.8 43.2 10.6c13.4-9.5 31.2-21.9 46-24.8c23.1-1.9 42.9 2.9 64.2 9.1c13.2-12.1 33.3-25.7 49.1-27.2c16.3.1 30.4 4.4 44.7 8.8c6.4-3.3 10.2-9.5 15-14.2c-58.4-122.2-125.4-213.6-239.5-272.55M417.6 377.6c-11 6.3-17.8 17.1-24 27c-15.7-4.3-36.9-13.7-53-12.9c-18.9 4.1-33.6 17.2-45.6 29.8c-10.5-3.3-20.6-6.2-29-8.2c-13.1-3-29.1-5.1-37-3.6c-18.1 5.6-33.1 17.2-46.7 27.9c-14.3-6.2-28.5-12.5-43.5-13.5c-5.1-.2-8.7.7-10.2 1.7c-8.6 5.9-19.7 20.9-24.2 34.8C101 471 101 484 110 488.8c14.5 2.3 27.8-6 38.9-13.1c11.2 5.5 30.9 17.7 43.1 17.4c17.3-4.6 32.9-13.7 47.1-22.2c9.3 8.7 26.7 22.5 39.3 21.7c17.9-5.2 29.1-21.5 37.7-35.6c17 5.8 53.5 14.1 67.5 3.9c9.8-7.6 2.9-19.9.2-28.5c12.7 4.7 26.8 9.2 37.9 10.8c19.1.6 37.8 2 19.6-18.3l-12.7-13.2c13.6-1.5 33-3.4 42.6-9.5c4.9-3 2-8.5-.4-11.1c-2.7-2.9-7.8-6.1-14-8.5c-13.2-3.8-26.3-7-39.2-5"
        
        
            //PATHS MUSICAIS:
            "",
            "",
            "",
            "",
        };


        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out POINT lpPoint);


        private double fefnejnfeajnfjaenf = 1;
        private void Timer_Tick(object sender, EventArgs e)
        {
            POINT mousePosition;
            if (!GetCursorPos(out mousePosition))
                return;

            var windowBounds = new Rect(Left, Top, ActualWidth, ActualHeight);

            if (windowBounds.Contains(new Point(mousePosition.X, mousePosition.Y)) || WindowState == WindowState.Maximized)
            {
                var path = CreateRandomPath();
                RainCanvas.Children.Add(path);
                AnimatePath(path);

                raintimer.Interval = TimeSpan.FromMilliseconds((random.Next(600, 1000) / cafeina_pra_neve) * fefnejnfeajnfjaenf);
            }
            else
                raintimer.Interval = TimeSpan.FromMilliseconds(200);
        }

        private System.Windows.Shapes.Path CreateRandomPath()
        {
            // Seleciona um path aleatório da lista
            string randomPathData = pathStrings[random.Next(pathStrings.Length)];

            // Define o Path
            var path = new System.Windows.Shapes.Path
            {
                Data = Geometry.Parse(randomPathData),
                Stretch = Stretch.Uniform,
                Fill = Brushes.White,
                Opacity = random.NextDouble() - 0.2, // Opacidade aleatória entre 0 e 1
                Width = random.Next(15, 40),   // Largura aleatória
                Height = random.Next(15, 40),  // Altura aleatória
                RenderTransformOrigin = new Point(0.5, 0.5),
            };

            // Adiciona transformações
            var transformGroup = new TransformGroup();

            // Rotação inicial aleatória
            var rotateTransform = new RotateTransform { Angle = random.Next(0, 360) };
            transformGroup.Children.Add(rotateTransform);

            // Translação será adicionada na animação
            var translateTransform = new TranslateTransform();
            transformGroup.Children.Add(translateTransform);

            path.RenderTransform = transformGroup;

            // Define uma posição X aleatória e posição Y inicial entre 0 e 10
            Canvas.SetLeft(path, random.Next(-30, (int)RainCanvas.ActualWidth + 30) + 20);
            //Canvas.SetTop(path, random.Next(0, 50));
            Canvas.SetTop(path, -50);
            return path;
        }

        double speed_acelerator = 1;
        private void AnimatePath(System.Windows.Shapes.Path path)
        {
            var transformGroup = (TransformGroup)path.RenderTransform;
            var rotateTransform = (RotateTransform)transformGroup.Children[0];
            var translateTransform = (TranslateTransform)transformGroup.Children[1];

            double duration = random.Next(3000, 7000) / speed_acelerator;
            var animationY = new DoubleAnimation(0, 550, TimeSpan.FromMilliseconds(duration));

            animationY.Completed += (s, e) => RainCanvas.Children.Remove(path);


            if (false)
            {
                double rotationSpeed = random.NextDouble() * 360;
                if (random.Next(0, 5) == 0) rotationSpeed = 0;
                var rotationAnimation = new DoubleAnimation
                {
                    From = rotateTransform.Angle,
                    To = rotateTransform.Angle + rotationSpeed,
                    Duration = TimeSpan.FromMilliseconds(duration),
                    RepeatBehavior = RepeatBehavior.Forever
                };
                rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);
            }
            translateTransform.BeginAnimation(TranslateTransform.YProperty, animationY);
        }





















        private static readonly string web = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";


        private bool InjectionInProgress;

        private bool CloseCompleted;

        private WebClient WebStuff = new WebClient();

        private WebClient WebStuff2 = new WebClient();

        private bool Exib;

        private string[] set;

        private bool Baixando;

        private bool inj4;

        private DiscordRpcClient client;

        private bool RPCdisposed = true;

        private bool RPCloaded;

        //private protected readonly Flux Fluxus = new Flux();


        private RegistryKey registryKey1;
        private RegistryKey Wolfregkey;

        private bool inicializado;

        private bool stopdrag;

        //private string Test = new string((from s in Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 30)
        //select s[random.Next(s.Length)]).ToArray());

        private string key;

        private bool upd;

        double screenWidth = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
        double screenHeight = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;

        private string user_full_name = "";

        private string discordId = "";
        internal static string UserName2 = "null";
        private string UserAvatar2 = "null";

        private string appdata = Environment.GetEnvironmentVariable("localappdata");
        private string roaming = Environment.GetEnvironmentVariable("appdata");



        private string idioma = "EN";
        private double CpuSpeed = 2000;
        internal bool MaximumExecutorQuality = true;
        private bool cpu_slow = false;
        internal static string build_number = "????";
        internal static string build_date = "????";

        public MainWindow()
        {

            DebugProtect3.HideOSThreads();

            build_number = Properties.Resources.BuildInfos.Split('+')[0];
            build_date = Properties.Resources.BuildInfos.Split('+')[1];

            Environment.CurrentDirectory = "C:\\Essence";
            Directory.SetCurrentDirectory("C:\\Essence");
            //Console.Title = "Essence 2024 © Made with ♥ UI by M4A1_dev2";

            //InternalConsolePrint("Application Started", console_RichTextBox, Colors.Yellow);
            //throw new Exception("Erro fatal: o aplicativo irá crashar!");



            int cct = 0;
            if (Process.GetProcessesByName("Essence").Length > 1)
            {
                System.Windows.MessageBox.Show("You already have one Essence opened", "Ops!");
                Process.GetCurrentProcess().Kill();

                //if (!IsWindowOpen("Essence"))
                //{
                //    MessageBox.Show("HHH");
                //    foreach (Process p in Process.GetProcessesByName("Essence"))
                //    {
                //        if (p != Process.GetCurrentProcess())
                //            p.Kill();
                //    }
                //}
                //else
                //{
                //    System.Windows.MessageBox.Show("You already have one Essence opened", "Ops!");
                //    Process.GetCurrentProcess().Kill();
                //}
            }


            //if (!Internet())
            //{
            //    System.Windows.MessageBox.Show("Essence não conseguiu obter uma conexão com a internet. Verifique seu firewall ou se está próximo a um roteador.", "Sem Internet!");
            //    Process.GetCurrentProcess().Kill();
            //}

            //Directory.SetCurrentDirectory("C:\\Essence");

            try { Wolfregkey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Essence Studios", writable: true); } catch { }
            Wolfregkey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Essence Studios", writable: true);

            try { registryKey1 = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Essence Studios\\Essence", writable: true); } catch { }
            registryKey1 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Essence Studios\\Essence", writable: true);
            lm = new LanguageManager();

            try
            {
                InternalConsolePrint("Checking CPU SPEED", console_RichTextBox, Colors.Cyan);
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select MaxClockSpeed from Win32_Processor");
                foreach (ManagementObject obj in searcher.Get())
                {
                    if (Convert.ToDouble(obj["MaxClockSpeed"]) > CpuSpeed)
                        CpuSpeed = Convert.ToDouble(obj["MaxClockSpeed"]);
                }
                if (CpuSpeed <= 3000)
                {
                    cpu_slow = true;
                    InternalConsolePrint("CPU POWER LOW", console_RichTextBox, Colors.Red);
                    if (Wolfregkey.GetValueNames().Contains("MaximumQuality"))
                    {
                        if (Wolfregkey.GetValue("MaximumQuality").ToString() == "False")
                            ReduceQuality();
                    }
                    else
                        MaximumExecutorQuality = false;
                }
                else
                    InternalConsolePrint("CPU POWER OK", console_RichTextBox, Colors.Green);
            }
            catch
            {
            }

            timeX = DateTime.Now;

            InitializeComponent();
            //RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly; // Ou Default


            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            //SnapsToDevicePixels = true;
            //UseLayoutRounding = true;


            //base.Opacity = 0;

            //versão.Text = "Versão: " + CurrentVersion;
            //compilation_label.Text = "Compilation: " + build_number + " ~ " + build_date;


            compilationtxt.Text = $"Version: {ExecSettings.CurrentVersion} | Compilation: {build_number} ~ {build_date}";


            //compilation_label2.Text = "Compilation: " + build_number + " ~ " + build_date;
            compilation_label3.Text = "Essence © 2024 UI by M4A1. Compilation: " + build_number + " ~ " + build_date;
            //if (File.Exists("EssenceUpdater.exe"))
            //{
            //    try
            //    {
            //        File.Delete("EssenceUpdater.exe");
            //    }
            //    catch
            //    {
            //    }
            //}

            WebStuff.Headers.Add(HttpRequestHeader.UserAgent, web);
            WebStuff.Encoding = Encoding.UTF8;
            WebStuff2.Headers.Add(HttpRequestHeader.UserAgent, web);
            WebStuff2.Encoding = Encoding.UTF8;

            //login.Visibility = Visibility.Collapsed;

            //Welcome.Visibility = Visibility.Collapsed;

            executor.Visibility = Visibility.Hidden;
            //Spoofer.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Hidden;
            Topbar.Visibility = Visibility.Hidden;


            ExecutorGrid.Visibility = Visibility.Hidden;

            HubGrid.Visibility = Visibility.Collapsed;
            HomeGrid.Visibility = Visibility.Collapsed;
            AssistentGrid.Visibility = Visibility.Collapsed;
            ai_normal_response_border.Visibility = Visibility.Collapsed;


            SettingsGrid.Visibility = Visibility.Collapsed;

            ScriptsGrid.Visibility = Visibility.Collapsed;
            ESPpreview.Visibility = Visibility.Collapsed;
            Distancepreview.Visibility = Visibility.Collapsed;
            Trackerspreview.Visibility = Visibility.Collapsed;


            //EULA.Visibility = Visibility.Collapsed;

            RobloxOudated.Visibility = Visibility.Collapsed;

            GameselectedScroll.Visibility = Visibility.Collapsed;

            //AdminPanel.Visibility = Visibility.Collapsed;

            WindowBorder.Margin = new Thickness(170, 170, 170, 170);
            //WindowBorder.Margin = new Thickness(10, 10, 10, 10);

            NotListborder2.Margin = new Thickness(0, 40, 0, 0);
            ConsoleGrid2.Margin = new Thickness(0);
            ConsoleGrid2.Height = 0;
            ScriptListBorder2.Width = 0;


            ResizeMode = ResizeMode.NoResize;

            MinHeight = 550;
            MinWidth = 850;
        }

        internal static List<NotificationWindow> activeNotifications = new List<NotificationWindow>();
        private static string na_fila = "";
        public async static Task Notificar2(string text, double duration = 3, bool disc = false, string lnk = "")
        {
            if (na_fila.Contains(text))
                return;

            na_fila += text;

            while (activeNotifications.Count >= 4)
            {
                await Task.Delay(100);
            }

            if (disc)
                lnk = "https://discord.gg/Ku5HGekNQw";

            var notification = new NotificationWindow(text, duration, disc, lnk);
            notification.Show();

            notification.Closed += (s, e) =>
            {
                for (int i = 0; i < activeNotifications.Count; i++)
                {
                    activeNotifications[i].Top = 40 + i * 110;
                }
            };

            await Task.Delay(TimeSpan.FromSeconds(duration + 0.5));


            //remove a notificação
            na_fila = na_fila.Replace(text, "");
            MainWindow.activeNotifications.Remove(notification);
            notification.Close();
            return;
        }

        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_NOACTIVATE = 0x08000000;
        public const int WS_EX_TOOLWINDOW = 0x00000080; // Oculta da barra de tarefas

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        public static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_NOMOVE = 0x0002;
        //public const uint SWP_NOZORDER = 0x0004;
        public const uint SWP_FRAMECHANGED = 0x0020;
        bool notifacccc = false;

        public async Task Notificar(string text, double duration = 3)
        {
            if (na_fila.Contains(text))
                return;
            na_fila += text;

            while (notifacccc || WindowState == WindowState.Minimized) { await Task.Delay(100); }

            notifacccc = true;

            Activate();
            Focus();
            Topmost = false;
            Activate();
            Focus();

            try
            {
                SexyNotification.Owner = this;
            }
            catch { }

            try
            {
                SexyNotification = new SexyNotification(text, duration);
                SexyNotification.Show();
                UpdateNotification(null, null);
                LocationChanged += UpdateNotification;
                SizeChanged += UpdateNotification;

                while (!SexyNotification.kk) { await Task.Delay(100); }
                LocationChanged -= UpdateNotification;
                SizeChanged -= UpdateNotification;
                SexyNotification.Close();
                SexyNotification = null;
            }
            catch
            {

            }

            Topmost = TopMButton.IsChecked.Value;

            na_fila = na_fila.Replace(text, "");
            notifacccc = false;
        }





        //private async void CloseNotf_Click(object sender, RoutedEventArgs e)
        //{
        //    Fade(BlackMamba, 0.7, 0, 0.3);
        //    await Task.Delay(200);
        //    Move(CumNotification, new Thickness(0, 0, 0, 0), new Thickness(0, 0, 0, -600), 0.4);
        //    await Task.Delay(100);
        //    BlackMamba.Visibility = Visibility.Collapsed;
        //}


        private async void check_user_ban()
        {
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler()
            {
                CookieContainer = cookieContainer
            };

            using (var client = new HttpClient(handler))
            {

                var url = "https://usermoderation.roblox.com/v1/not-approved";
                var cookie = new Cookie(".ROBLOSECURITY", "_|WARNING:-DO-NOT-SHARE-THIS.--Sharing-this-will-allow-someone-to-log-in-as-you-and-to-steal-your-ROBUX-and-items.|_D9569523DA8F227739CDEEB0B9EFC25CED75153AA0490A58DFBE010C3075A2CF8065CAD87F30901DAF82EEA2AA79F8C96E7C9828EA594B4377B161898DCE4A3EC74F83BE43BCA515603E2375C0D47A7533F59A5568A9D509437280A1658358E2309D3E8186E8E7965F4EBB146794524CC9D8B812E38A045359A92AEF13AF6DFF8F232820B545BD276E4D7805981FBC64FE20ADA810426E86851DDDD0C7557E9A871985FA5396AB0A82986C83B857743A534DB999F55179CC393C815A1104ADC91E28F31AB443749A5599407EE1FEF742C11000E67C5756D4CA81228A461E79FDF8216EEDC2DCAE44D42DFEBCB51DFDD3A6461B23EA5FA6BDA0B0C26D93506CB58DA7AC12BC867BF540DF8CCE0AA41D70E9292C3CDD4B5D4789B284F5FD0589B18BE588D800805A166C0A6C1DDE3F6DCB3707F6BF64CBD13384961E09D16F557CEF411277949129D56DE6F3EBBA9F7B4436EEAFB51A9C050EDEF3D9267AEBE7B33571C3513F3C200BC848C32C6DEF710EFAD5A621F33AD9C68DC1C820EC17E50F2BE2B03084790D4CE3DE7C632E6BEDF0970BA578A1120C6E59998762ABAB6EB4D06C24B127A5187E409D62AEE40175B9C51D1C85FD22836E54919760402F55ED9FC7B87DBE41B57885463C7A191F398F1DADB7D00BD57FDCF55D12A23B46CCED938E9ED7F78FB50D348E9D193FBD70F7400AA827D2EB0316D16336BF9739D124D77D8AD8AD8A0DEFD25593E882BAE78EF82A5C164E5A625F6BAD4CD49AC9F8B24C233535BAB74108CBBC1A1C0DB6A8D1EB07941F842DAA25531C3FB2F96689E9C7EDE3B7DAA5B2B38211816B373A6975B9B6CA62CC059333BA1C53DAB100366EAA712695992267275C2DC13A032CD8D3DCA4AEA90399C9812FEE834CA7520BB5E24980F2EE78B61DBD6634512AAD8397354CBCEFC82C7D937839F924049E935AC6D4BDA4F0E8E30E0E5B78365F48692CCB93D142D49A1B2BAB527548FAF9EA73D6359F78EF3030C2FF9273553F164F7A35C784D62AAC8FA32CC463B92AE7F2EE807521AFC318713755C21C6E107585FDDA4A4B63FA894AE2D0BA8FFB9CD29269A9E5F09F6A485D1251B9730F2A14BE25A183FB2D");
                cookieContainer.Add(new Uri("https://usermoderation.roblox.com"), cookie);

                try
                {
                    var response = await client.GetAsync(url);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    //MessageBox.Show(responseBody);


                    JObject jsonResponse = JObject.Parse(responseBody);

                    string messageToUser = jsonResponse["messageToUser"]?.ToString();
                    string beginDate = jsonResponse["beginDate"]?.ToString();
                    string endDate = jsonResponse["endDate"]?.ToString() ?? "N/A";
                    string nextConsequenceType = jsonResponse["context"]?["NEXT_CONSEQUENCE_TYPE"]?.ToString() ?? "N/A";

                    Console.WriteLine($"Message to User: {messageToUser}");
                    Console.WriteLine($"Begin Date: {beginDate}");
                    Console.WriteLine($"End Date: {endDate}");
                    Console.WriteLine($"Next Consequence Type: {nextConsequenceType}");

                    var badUtterances = jsonResponse["badUtterances"];
                    if (badUtterances != null)
                    {
                        foreach (var utterance in badUtterances)
                        {
                            string utteranceText = utterance["utteranceText"]?.ToString();
                            Console.WriteLine($"Utterance Text: {utteranceText}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

        }

        static string GetTimeAgo(DateTime lastLogin)
        {
            TimeSpan timeDiff = DateTime.Now - lastLogin;

            if (timeDiff.TotalHours < 24)
            {
                return $"{(int)timeDiff.TotalHours} hours ago";
            }
            else if (timeDiff.TotalDays < 30)
            {
                return $"{(int)timeDiff.TotalDays} days ago";
            }
            else
            {
                return $"{(int)(timeDiff.TotalDays / 30)} months ago";
            }
        }


        string DisID = "Rola";
        private string xxx = "";
        private List<string> dwnjundj = new List<string>();
        static List<(string Date, string Version)> ExtractLatestWindowsPlayerUpdates(string input, int count)
        {
            try
            {
                var lines = input.Split('\n')
                                 .Where(line => line.Contains("New WindowsPlayer"))
                                 .Reverse()
                                 .Take(count)
                                 .ToList();

                var updates = lines
                    .Select(line =>
                    {
                        var match = Regex.Match(line, @"New WindowsPlayer version-([^ ]+) at ([^,]+)");
                        return match.Success ? (Date: match.Groups[2].Value, Version: match.Groups[1].Value) : (string.Empty, string.Empty);
                    })
                    .Where(update => !string.IsNullOrEmpty(update.Item1) && !string.IsNullOrEmpty(update.Item2)) // Use Item1 and Item2 here
                    .ToList();

                return updates;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return new List<(string, string)> { ("---", "---") };
        }

        private async void HeartBeat()
        {
            Roblox_update_soon_border2.Visibility = Visibility.Collapsed;
            Roblox_updated.Visibility = Visibility.Collapsed;

            //check_user_ban();

            var paths = new[] {
                @"C:\Users\cessa\AppData\Local\Roblox\Versions",
                @"C:\Users\cessa\AppData\Local\Bloxstrap\Versions"
            };

            KeyGay2.robloxversions = paths
            .Where(Directory.Exists)
            .SelectMany(Directory.GetDirectories)
            .Select(Path.GetFileName)
            .Where(name => name.StartsWith("version-", StringComparison.OrdinalIgnoreCase))
            .Distinct()
            .ToList();



            //string cu = "http://api.whatexpsare.online/status/";
            string cu = "http://whatexpsare.online/api/";
            while (StopAllInteractions == false)
            {

                int ggs = DebugProtect1.PerformChecks();
                //verificar se está logado com um id não permitido.
                //Se estiver, bane a pessoa (já que o ban check ocorre antes)
                try
                {
                    if (DisID.Length < 16 || DisID.Length > 20)//incorreta
                    {
                        DisID = discordId;
                        if (DisID.Length > 16 && DisID.Length < 20)//correta
                        {
                            KeyGay2.Discord_ID = DisID;
                            //ban check
                            //if (KeyGay2.Banidos.Contains(DisID))
                            //{
                            //    System.Windows.MessageBox.Show("Você está na lista-negra do Essence. Contate um ajudante se achar que é um engano.", "Código: 777 Big Win");
                            //    ShowBa(DisID);
                            //}
                        }
                    }
                }
                catch { }

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("User-Agent", "WEAO-3PService");
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                        client.Timeout = TimeSpan.FromSeconds(5);

                        try
                        {
                            Scanner.ScanAndKill();

                            HttpResponseMessage response2 = await client.GetAsync($"{cu}versions/current");

                            if (response2.IsSuccessStatusCode)
                            {
                                string content = await response2.Content.ReadAsStringAsync();

                                JObject json = JObject.Parse(content);

                                string windowsVersion = string.Empty;
                                string windowsDate = string.Empty;

                                if (json["Windows"] != null)
                                {
                                    windowsVersion = json["Windows"]?.ToString();
                                    windowsDate = json["WindowsDate"]?.ToString();
                                }
                                else if (json["windows"] != null)
                                {
                                    windowsVersion = json["windows"]?["version"]?.ToString();
                                    windowsDate = json["windows"]?["date"]?.ToString();
                                }

                                if (!string.IsNullOrEmpty(windowsVersion))
                                {
                                    dwnjundj.Add(windowsVersion.Replace("version-",""));
                                    CurrentRobloxVersion.Text = windowsVersion;
                                }
                                else
                                    CurrentRobloxVersion.Text = "---";

                                if (DateTime.TryParse(windowsDate, out DateTime updateDateUtc))
                                {
                                    CurrentRobloxVersionDate.Text = $"Last Updated: {updateDateUtc.ToLocalTime()}";
                                }
                                else
                                    CurrentRobloxVersionDate.Text = $"Last Updated: {windowsDate}";

                            }
                            else if (cu == "http://api.whatexpsare.online/status/")
                            {
                                cu = "http://whatexpsare.online/api/";
                            }
                            else
                            {
                                cu = "http://api.whatexpsare.online/status/";
                            }

                            //HttpResponseMessage response = await client.GetAsync($"{cu}versions/future");

                            //if (response.IsSuccessStatusCode)
                            //{
                            //    string content = await response.Content.ReadAsStringAsync();

                            //    JObject json = JObject.Parse(content);

                            //    string windowsVersion = string.Empty;
                            //    string windowsDate = string.Empty;

                            //    if (json["Windows"] != null)
                            //    {
                            //        windowsVersion = json["Windows"]?.ToString();
                            //        windowsDate = json["WindowsDate"]?.ToString();
                            //    }
                            //    else if (json["windows"] != null)
                            //    {
                            //        windowsVersion = json["windows"]?["version"]?.ToString();
                            //        windowsDate = json["windows"]?["date"]?.ToString();
                            //    }

                            //    if (!string.IsNullOrEmpty(windowsVersion) && !KeyGay2.robloxversions.Contains(windowsVersion))
                            //    {
                            //        //Roblox_update_soon_border.Visibility = Visibility.Visible;

                            //        if (DateTime.TryParse(windowsDate, out DateTime updateDateUtc))
                            //        {
                            //            //DateTime updateDateLocal = updateDateUtc.ToLocalTime();
                            //            //TimeSpan timeRemaining = updateDateLocal - DateTime.Now;

                            //            TimeSpan timeRemaining = updateDateUtc - DateTime.Now;

                            //            if (updateDateUtc < DateTime.Now)
                            //            {
                            //                if (CurrentRobloxVersion.Text != "---" && !KeyGay2.robloxversions.Contains(CurrentRobloxVersion.Text))
                            //                    new_roblox_update_txt.Text = "Your Roblox version is oudated";
                            //            }
                            //            else
                            //            {
                            //                if (timeRemaining.TotalMinutes < 60)
                            //                    new_roblox_update_txt.Text = $"New Roblox Update in {Math.Ceiling(timeRemaining.TotalMinutes)} minutes";                                            
                            //                else
                            //                    new_roblox_update_txt.Text = $"New Roblox Update in {Math.Ceiling(timeRemaining.TotalHours)} hours";                                          
                            //            }
                            //        }
                            //    }
                            //    else
                            //    {
                            //        //Roblox_update_soon_border.Visibility = Visibility.Collapsed;
                                    
                            //    }
                            //}
                            //else if (cu == "http://api.whatexpsare.online/status/")
                            //{
                            //    cu = "http://whatexpsare.online/api/";
                            //}
                            //else
                            //{
                            //    cu = "http://api.whatexpsare.online/status/";
                            //}

                        }
                        catch
                        {
                        }
                    }

                    xxx = await KeyGay2.Get("heartbeat", KeyGay2.current_user + "\n | \n" + KeyGay2.current_pass, force_return:true);
                    string roblox_v = xxx.Split(new string[] { "||||||||" }, StringSplitOptions.RemoveEmptyEntries)[3];

                    if (roblox_v.Contains(string.Join(".", KeyGay2.robloxversions)))
                    {
                        Roblox_updated.Visibility = Visibility.Visible;
                        Executor_patched.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        Roblox_updated.Visibility = Visibility.Collapsed;
                        Executor_patched.Visibility = Visibility.Visible;
                    }

                    int ggs2 = DebugProtect2.PerformChecks();

                    if (xxx == "Disconnect!")
                    {
                        StopAllInteractions = true;
                        MessageBox.Show("There was a problem with your Essence account and we need to close this session.");
                        RestartApp(true);
                    }

                    if(ggs == 1 || ggs2 == 1)
                    {
                        MessageBox.Show("There was a problem with your Essence account and we need to close this session.");
                        Process.GetCurrentProcess().Kill();
                    }


                    var userData = JsonConvert.DeserializeObject<Dictionary<string, object>>(xxx.Split(new string[] { "||||||||" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                    if (userData != null && userData.ContainsKey("email"))
                    {
                        try
                        {
                            await Dispatcher.InvokeAsync(() =>
                            {
                                user_r = Convert.ToInt32(userData["user_role"].ToString());
                                KeyGay2.invitecode = userData["userid"].ToString();
                                KeyGay2.invites = Convert.ToInt32(userData["invites"].ToString());
                                inviteeesss.Text = userData["invites"].ToString();
                                ProfileSettings.invitestxt.Text = userData["invites"].ToString();

                                try
                                {
                                    if (user_r < 6)
                                    {
                                        premium = true;
                                        ProfileSettings.dudu3nud3nud.Text = "Yes";
                                        ProfileSettings.subscription_typetxt.Text = "Premium";
                                        OHHHHH = true;
                                    }
                                }
                                catch { }

                                try
                                {
                                    switch (user_r)
                                    {
                                        case 0:
                                            userrolllee.Text = "Owner";
                                            ProfileSettings.dn3u2ndu2d3n.Text = "Owner";
                                            break;

                                        case 1:
                                            userrolllee.Text = "Developer";
                                            ProfileSettings.dn3u2ndu2d3n.Text = "Developer";
                                            break;

                                        case 2:
                                            userrolllee.Text = "Server Manager";
                                            ProfileSettings.dn3u2ndu2d3n.Text = "Server Manager";
                                            break;

                                        case 3:
                                            userrolllee.Text = "ADM";
                                            ProfileSettings.dn3u2ndu2d3n.Text = "ADM";
                                            break;

                                        case 4:
                                            userrolllee.Text = "Resellers";
                                            ProfileSettings.dn3u2ndu2d3n.Text = "Resellers";
                                            break;

                                        case 5:
                                            userrolllee.Text = "Close Friend";
                                            ProfileSettings.dn3u2ndu2d3n.Text = "Clsoe Friend";
                                            break;

                                        case 6:
                                            userrolllee.Text = "Beta Tester";
                                            ProfileSettings.dn3u2ndu2d3n.Text = "Beta Tester";
                                            break;

                                        case 10:
                                            Dispatcher.InvokeAsync(() => App.StartupWindow.Hide());
                                            MessageBox.Show("Opsie! Essence is in BETA tests now. And M4A1 didint allowed this login to be used. Dm him if you're subscribed in beta tests. If not, just wait for our next release!", "Oh, sorry :(");
                                            RestartApp(true);
                                            userrolllee.Text = "User";
                                            ProfileSettings.dn3u2ndu2d3n.Text = "User";
                                            break;

                                        case 69:
                                            premium = true;
                                            ProfileSettings.dudu3nud3nud.Text = "Yes";
                                            ProfileSettings.subscription_typetxt.Text = "Premium";
                                            OHHHHH = true;
                                            userrolllee.Text = "Kayne West";
                                            ProfileSettings.dn3u2ndu2d3n.Text = "Kayne West";
                                            break;
                                    }
                                }
                                catch { }

                                if (ProfileSettings.rapedevices.Children.Count > 1)
                                {
                                    for (int i = ProfileSettings.rapedevices.Children.Count - 1; i > 0; i--)
                                    {
                                        ProfileSettings.rapedevices.Children.RemoveAt(i);
                                    }
                                }

                                JArray maquinasAutorizadas = JArray.Parse(userData["maquinas_autorizadas"].ToString());
                                foreach (var maquina in maquinasAutorizadas)
                                {
                                    string hwid = maquina["hwid"].ToString();
                                    var ultimoLogin = maquina["last_login"].ToString();
                                    var lastlocation = maquina["last_location"].ToString();
                                    var devicetyp = maquina["device"].ToString();

                                    if (hwid != KeyGay2.GetHWID2())
                                    {
                                        Device lol = new Device();

                                        DateTime lastLogin = DateTime.ParseExact(ultimoLogin, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                        DateTime lastLoginUtc = lastLogin.AddHours(3);
                                        TimeSpan localOffset = TimeZoneInfo.Local.BaseUtcOffset;
                                        DateTime localTime = lastLoginUtc.Add(localOffset);
                                        lol.locationn.Text = lastlocation + " · " + GetTimeAgo(localTime);

                                        if (maquina["device"].ToString() == "Notebook")
                                            lol.computertype.Data = Geometry.Parse("M23 18h-1V5c0-1.1-.9-2-2-2H4c-1.1 0-2 .9-2 2v13H1c-.55 0-1 .45-1 1s.45 1 1 1h22c.55 0 1-.45 1-1s-.45-1-1-1m-9.5 0h-3c-.28 0-.5-.22-.5-.5s.22-.5.5-.5h3c.28 0 .5.22.5.5s-.22.5-.5.5m6.5-3H4V6c0-.55.45-1 1-1h14c.55 0 1 .45 1 1z");

                                        lol.DisconnectAccount.PreviewMouseDown += (MouseButtonEventHandler)(async (sender, e) =>
                                        {
                                            string response = await KeyGay2.Get("disconnectdevice", hwid);
                                            switch (response)
                                            {
                                                case "success":
                                                    ProfileSettings.rapedevices.Children.Remove(lol);
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
                                        });

                                        ProfileSettings.rapedevices.Children.Add(lol);
                                    }
                                }
                            });

                        }
                        catch (Exception ex)
                        {
                            await Dispatcher.InvokeAsync(() => App.StartupWindow.Hide());
                            MessageBox.Show(ex.ToString(), "Error When Loading User Account. Sorry :(");
                            RestartApp(true);
                        }
                    }

                    if (!premium)
                    {
                        ProfileSettings.sellersuportserverborder.Visibility = Visibility.Collapsed;
                        ProfileSettings.creationdateborder.Visibility = Visibility.Collapsed;
                        ProfileSettings.sellerinfoborder.Visibility = Visibility.Collapsed;
                        ProfileSettings.premiumkeyinfoborder.Visibility = Visibility.Collapsed;

                        ProfileSettings.subscription_typetxt.Text = "ADs login";
                    }
                    else
                    {
                        if (OHHHHH)
                        {
                            ProfileSettings.sellersuportserverborder.Visibility = Visibility.Collapsed;
                            ProfileSettings.creationdateborder.Visibility = Visibility.Collapsed;
                            ProfileSettings.sellerinfoborder.Visibility = Visibility.Collapsed;
                            ProfileSettings.premiumkeyinfoborder.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            ProfileSettings.sellersuportserverborder.Visibility = Visibility.Visible;
                            ProfileSettings.creationdateborder.Visibility = Visibility.Visible;
                            ProfileSettings.sellerinfoborder.Visibility = Visibility.Visible;
                            ProfileSettings.premiumkeyinfoborder.Visibility = Visibility.Visible;

                            ProfileSettings.subscription_typetxt.Text = "Premium";
                        }
                    }


                    //else
                    //{
                    //    //MessageBox.Show("Erro no login. Detalhes: " + response);
                    //    Process.GetCurrentProcess().Kill();
                    //}

                    //string[] versionshit = xxx.Split(new string[] { "||||||||" }, StringSplitOptions.RemoveEmptyEntries)[0].Split(new string[] { "||||" }, StringSplitOptions.RemoveEmptyEntries);

                    //Dictionary<string, string> knownHashes = new Dictionary<string, string>();
                    //string[] hashesss = versionshit[0].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    //foreach (string hash in hashesss)
                    //{
                    //    string name = hash.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries)[0];
                    //    string id = hash.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries)[1];
                    //    knownHashes.Add(name, id);
                    //}
                    //bool downloadRequired = CheckFilesAndHashes("C:\\Essence", knownHashes);


                    string[] funny = xxx.Split(new string[] { "||||||||" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        totaluserst.Text = funny[2];
                        Fade(totalusersgridd, 0, 1, 1);
                        totalusersgridd.Visibility = Visibility.Visible;
                    }
                    catch { }

                    await Task.Delay(80);

                    try
                    {
                        playersonline.Text = funny[1];
                        Fade(playersgridd, 0, 1, 1);
                        playersgridd.Visibility = Visibility.Visible;
                    }
                    catch { }

                    await Task.Delay(80);

                    try
                    { 
                        banswithEssence.Text = funny[0];
                        Fade(bansgridd, 0, 1, 1);
                        bansgridd.Visibility = Visibility.Visible;
                    }
                    catch { }

                    await Task.Delay(80);

                    try
                    {
                        totaluserst.Text = funny[2];
                        Fade(invitesgriddd, 0, 1, 1);
                        invitesgriddd.Visibility = Visibility.Visible;
                    }
                    catch { }                    
                }
                catch { }

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromSeconds(3);

                        HttpResponseMessage response2 = await client.GetAsync("https://setup.rbxcdn.com/DeployHistory.txt");

                        if (response2.IsSuccessStatusCode)
                        {
                            string g = await response2.Content.ReadAsStringAsync();
                            List<(string Date, string Version)> windowsPlayerUpdates = ExtractLatestWindowsPlayerUpdates(g, 5);

                            foreach (var update in windowsPlayerUpdates)
                            {
                                await Task.Delay(200);

                                if (dwnjundj.Contains(update.Version))
                                    continue;

                                Border mainBorder = new Border
                                {
                                    Background = new SolidColorBrush(Color.FromArgb(204, 10, 10, 10)),
                                    CornerRadius = new CornerRadius(10),
                                    Width = RobloxRapeHystory2.ActualWidth,
                                    Height = 40,
                                    Margin = new Thickness(0, 0, 0, 5)
                                };
                                Binding widthBinding = new Binding("ActualWidth") { Source = RobloxRapeHystory2 };
                                mainBorder.SetBinding(Border.WidthProperty, widthBinding);

                                Grid grid = new Grid();
                                System.Windows.Shapes.Path path = new System.Windows.Shapes.Path
                                {
                                    Stretch = Stretch.Uniform,
                                    Data = Geometry.Parse("m3 5.549l7.195-.967v7.029l-7.188.054zm7.195 6.842v7.105l-7.19-.985v-6.12zm.918-7.935L20.998 3v8.533l-9.885.078zM21 12.505L20.998 21l-9.885-1.353v-7.142z"),
                                    Width = 20,
                                    Height = 20,
                                    Fill = new SolidColorBrush(Color.FromArgb(255, 150, 150, 150)),
                                    HorizontalAlignment = HorizontalAlignment.Left,
                                    Margin = new Thickness(20, 0, 0, 0),
                                    VerticalAlignment = VerticalAlignment.Center
                                };

                                TextBlock dateTextBlock = new TextBlock
                                {
                                    Text = $"Updated: {update.Date}",
                                    Foreground = new SolidColorBrush(Color.FromArgb(255, 150, 150, 150)),
                                    FontFamily = new System.Windows.Media.FontFamily("/Essence;component/Fonts/#Atkinson Hyperlegible"),
                                    HorizontalAlignment = HorizontalAlignment.Left,
                                    Margin = new Thickness(50, 0, 0, 0),
                                    FontSize = 13,
                                    VerticalAlignment = VerticalAlignment.Center
                                };

                                Border innerBorder = new Border
                                {
                                    CornerRadius = new CornerRadius(5),
                                    HorizontalAlignment = HorizontalAlignment.Right,
                                    VerticalAlignment = VerticalAlignment.Center,
                                    Background = new SolidColorBrush(Color.FromArgb(204, 20, 20, 20)),
                                    Margin = new Thickness(0, 0, 20, 0)
                                };

                                TextBlock versionTextBlock = new TextBlock
                                {
                                    Text = $"version-{update.Version}",
                                    Foreground = new SolidColorBrush(Color.FromArgb(255, 150, 150, 150)),
                                    FontFamily = new System.Windows.Media.FontFamily("/Essence;component/Fonts/#Atkinson Hyperlegible"),
                                    VerticalAlignment = VerticalAlignment.Top,
                                    HorizontalAlignment = HorizontalAlignment.Left,
                                    Margin = new Thickness(5, 5, 5, 5)
                                };

                                // Adicionando o TextBlock ao Border interno
                                innerBorder.Child = versionTextBlock;

                                // Adicionando os elementos à Grid
                                grid.Children.Add(path);
                                grid.Children.Add(dateTextBlock);
                                grid.Children.Add(innerBorder);

                                // Adicionando a Grid ao Border principal
                                mainBorder.Child = grid;

                                dwnjundj.Add(update.Version);

                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    RobloxRapeHystory2.Children.Add(mainBorder);
                                });
                            }
                        }
                    }
                }
                catch
                {
                    //MessageBox.Show($"Erro: {ex.ToString()}");
                }


                await Task.Delay(5000);
            }
        }

        public TimeSpan difference5;
        private async void TimeR(string str = "", bool upd = true)
        {

            int seconds = 0;
            while (StopAllInteractions == false)
            {
                await Task.Delay(1000);
                try
                {
                    if (upd)
                    {
                        difference5 = KeyGay2.TimeLeft("C:\\Essence\\userdata\\LinkvertiseKey.txt");
                        //premium
                        if (str != "")
                        {
                            seconds++;
                            double segundos = double.Parse(str) - seconds;
                            TimeSpan time = TimeSpan.FromSeconds(segundos);

                            if (segundos > 1)
                            {
                                int totalSeconds = (int)time.TotalSeconds;
                                string formattedTime;

                                if (totalSeconds >= 2592000) // 30 dias (aproximadamente 1 mês)
                                {
                                    int months = totalSeconds / 2592000;
                                    formattedTime = $"{months} {lm.Translate("Months")}";
                                }
                                else if (totalSeconds >= 86400) // 1 dia (24 horas)
                                {
                                    int days = totalSeconds / 86400;
                                    formattedTime = $"{days} {lm.Translate("Days")}";
                                }
                                else if (totalSeconds >= 3600) // 1 hora (3600 segundos)
                                {
                                    int hours = totalSeconds / 3600;
                                    formattedTime = $"{hours} {lm.Translate("Hours")}";
                                }
                                else if (totalSeconds >= 60) // 1 minuto (60 segundos)
                                {
                                    int minutes = totalSeconds / 60;
                                    formattedTime = $"{minutes} {lm.Translate("Minutes")}";
                                }
                                else
                                {
                                    formattedTime = $"{totalSeconds} {lm.Translate("Seconds")}";
                                }


                                key_expire2.Text = formattedTime;

                                int totalDias = (int)time.TotalDays;
                                int meses = totalDias / 30;
                                int diasRestantes = totalDias % 30;

                                int horas = time.Hours;
                                int minutos = time.Minutes;
                                int segundosRestantes = time.Seconds;

                                string resultado = "";

                                if (meses > 0) resultado += $"{meses} {lm.Translate("Months")}, ";
                                if (diasRestantes > 0) resultado += $"{diasRestantes} {lm.Translate("Days")}, ";
                                if (horas > 0) resultado += $"{horas} {lm.Translate("Hours")}, ";
                                if (minutos > 0) resultado += $"{minutos} {lm.Translate("Minutes")}, ";
                                if (segundosRestantes > 0 || resultado == "") resultado += $"{segundosRestantes} {lm.Translate("Seconds")}";

                                // Remover a última vírgula, se houver
                                if (resultado.EndsWith(", "))
                                {
                                    resultado = resultado.Substring(0, resultado.Length - 2);
                                }
                                ProfileSettings.key_expirationtxt.Text = resultado;

                                //key_expire2.Text = "Premium " + lm.Translate("expira em") + ": " + FormatTime((int)time.TotalSeconds)
                                //    .Replace("D", " D")
                                //    .Replace("h", " h")
                                //    .Replace("m", " m")
                                //    .Replace("s", " s")
                                //    .Replace("m:", "m ")
                                //    .Replace("s", lm.Translate("Segundos."))
                                //    .Replace("D", lm.Translate("Dias"))
                                //    .Replace("h", lm.Translate("Horas"))
                                //    .Replace("m", lm.Translate("Minutos e"))
                                //    .Replace(":", ", ");
                            }
                            else
                            {
                                key_expire2.Text = lm.Translate("Chave Expirou!");
                                ProfileSettings.key_expirationtxt.Text = lm.Translate("Chave Expirou!");
                                break;
                            }
                        }
                        //linkvertise
                        else
                        {
                            if (difference5.TotalMilliseconds != 1)
                            {
                                int totalSeconds = (int)difference5.TotalSeconds;

                                string formattedTime;

                                if (totalSeconds >= 3600)
                                {
                                    formattedTime = $"{totalSeconds / 3600} {lm.Translate("Hours")}";
                                }
                                else if (totalSeconds >= 60)
                                {
                                    formattedTime = $"{totalSeconds / 60} {lm.Translate("Minutes")}";
                                }
                                else
                                {
                                    formattedTime = $"{totalSeconds} {lm.Translate("Seconds")}";
                                }

                                key_expire2.Text = formattedTime;
                                ProfileSettings.key_expirationtxt.Text = formattedTime;

                                //key_expire2.Text = "Key " + lm.Translate("expira em") + ": " + FormatTime((int)difference5.TotalSeconds)
                                //    .Replace("d", " d")
                                //    .Replace("h", " h")
                                //    .Replace("m", " m")
                                //    .Replace("s", " s")
                                //    .Replace("m:", "m ")
                                //    .Replace("s", lm.Translate("Segundos."))
                                //    .Replace("h", lm.Translate("Horas"))
                                //    .Replace("m", lm.Translate("Minutos e"))
                                //    .Replace(":", ", ");
                            }
                            else
                            {
                                key_expire2.Text = lm.Translate("Chave Expirou!");
                                ProfileSettings.key_expirationtxt.Text = lm.Translate("Chave Expirou!");
                                break;
                            }
                        }
                    }
                    if (Wolfregkey.GetValueNames().Contains("Nome"))
                    {
                        User_config.Text = lm.Translate("Olá") + ", " + Wolfregkey.GetValue("Nome").ToString() + "!";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        bool patched = false;
        bool patched_check = false;
        private async void CheckStatus()
        {
            InternalConsolePrint("[Thread1] Checking roblox data...", console_RichTextBox, Colors.Yellow);
            //api2.InitializeAPI(cpu_slow);
            //api2.Warn += (sender, e) => Notificar(api2.last_message, api2.timeout);


            await Task.Run(async delegate
            {
                //Process process = new Process();
                //process.StartInfo.FileName = "powershell.exe";
                //process.StartInfo.RedirectStandardOutput = true;
                //process.StartInfo.UseShellExecute = false;
                //process.StartInfo.CreateNoWindow = true;
                //process.StartInfo.Arguments = "(Get-AppxPackage -Name ROBLOXCORPORATION.ROBLOX | Select-Object -ExpandProperty Version), (Get-AppxPackage -Name ROBLOXCORPORATION.FLUSTER | Select-Object -ExpandProperty Version) -join ';'";

                string pc2 = "";
                string pc = "";
                try
                {
                    if (!set[3].Contains("OK"))
                    {
                        //process.Start();
                        //string text = process.StandardOutput.ReadToEnd();
                        //process.WaitForExit();
                        //pc = text.Trim();

                        StreamReader sr = new StreamReader(bloxfolder);
                        pc2 = sr.ReadToEnd();
                        sr.Close();

                        //"VersionGuid": "version-3243b6d003cf4642",
                        pc2 = pc2.Split(new string[] { "VersionGuid" }, StringSplitOptions.RemoveEmptyEntries)[1]
                        .Split(new string[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[1];

                        await Dispatcher.InvokeAsync(() =>
                        {
                            if ((set[3].Contains(pc) && pc.Length > 5) || (set[3].Contains(pc2) && pc2.Length > 5) || true)
                            {
                                if (RobloxOudated.Visibility == Visibility.Visible)
                                {
                                    RobloxOudated.Visibility = Visibility.Collapsed;
                                    TabControl.Visibility = Visibility.Visible;
                                }
                                patched = false;
                                patched_check = true;
                            }
                            else if (pc.Length < 5 && pc2.Length < 5)
                            {
                                Status_Label2.Text = "Roblox not Installed!";
                                patched = true;
                                patched_check = true;
                            }
                            else
                            {
                                if (pc2.Length < 5)
                                    Warnin_ServOff_14.Text = "This Roblox Version its not compatible. Try Bloxstrap to use an older version of Roblox.";

                                Status_Label2.Text = "Roblox Uncompatible";
                                patched = true;
                                patched_check = true;

                                if (pc2 != "")
                                    roblox_latest.Text = pc2;
                                else if (pc != "")
                                    roblox_latest.Text = pc;

                                Essence_suported.Text = set[3].Split(' ')[0];

                                ShowWarnings(RobloxOudated, false);
                            }
                        });
                    }
                    else
                    {
                        if (RobloxOudated.Visibility == Visibility.Visible)
                        {
                            RobloxOudated.Visibility = Visibility.Collapsed;
                            TabControl.Visibility = Visibility.Visible;
                        }
                        patched = false;
                        patched_check = true;
                    }


                }
                catch
                {
                    patched_check = true;
                }
            });
        }

        private async void AutoSave()
        {
            InternalConsolePrint("[Thread2] [LOOP] Auto save service started", console_RichTextBox, Colors.Yellow);

            string directoryPath = "C:\\Essence\\userdata\\EvoTabs";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            await Task.Delay(4000);

            while (StopAllInteractions == false)
            {
                await Task.Delay(2000);

                if (!SaveTabs)
                {
                    try
                    {
                        string[] files = Directory.GetFiles("C:\\Essence\\userdata\\EvoTabs");
                        foreach (string path in files)
                        {
                            File.Delete(path);
                        }
                    }
                    catch { }

                    return;
                }


                try
                {
                    try
                    {
                        string[] files2 = Directory.GetFiles("C:\\Essence\\userdata\\EvoTabs");
                        foreach (string path in files2)
                        {
                            File.Delete(path);
                        }
                    }
                    catch { }

                    //string[] files = Directory.GetFiles("C:\\Essence\\userdata\\EvoTabs");
                    //foreach (string path in files)
                    //{
                    //    try
                    //    {
                    //        string content = File.ReadAllText(path);
                    //        if (content.Length <= 0 || content == "print(\"Hello World\")")
                    //        {
                    //            File.Delete(path);
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine($"Erro ao processar o arquivo {path}: {ex.Message}");
                    //    }
                    //}

                    foreach (TabItem c in TabControl.Items)
                    {
                        if (c.Content is WebViewA editor)
                        {
                            string txt = await editor.GetText();
                            if (txt != "print(\"Hello World\")" && txt.Length > 2)
                            {
                                string filePath = Path.Combine(directoryPath, c.Header?.ToString() + ".Evo");

                                using (StreamWriter streamWriter = new StreamWriter(filePath, false))
                                {
                                    await streamWriter.WriteAsync(txt);
                                }
                            }
                        }
                        else if (c.Content is TextEditor editor2)
                        {
                            string txt = editor2.Text;
                            if (txt != "print(\"Hello World\")" && txt.Length > 2)
                            {
                                string filePath = Path.Combine(directoryPath, c.Header?.ToString() + ".Evo");

                                using (StreamWriter streamWriter = new StreamWriter(filePath, false))
                                {
                                    await streamWriter.WriteAsync(txt);
                                }
                            }
                        }
                    }
                }
                catch
                {

                }
            }
        }

        static bool IsWindowOpen(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            return processes.Any(p => p.MainWindowHandle != IntPtr.Zero);
        }


        long lastplaceid;
        private bool adsandsandjsa = false;
        private void StatusCheck()
        {
            Task.Run(async () =>
            {
                try
                {
                    while (!StopAllInteractions)
                    {
                        await Task.Delay(3000);

                        if (patched)
                            return;

                        if (IsWindowOpen("RobloxPlayerBeta"))
                        {
                            KeyGay2.robloxversions = new List<string>();

                            foreach(Process p in Process.GetProcessesByName("RobloxPlayerBeta"))
                            {
                                try
                                {
                                    string fullPath = p.MainModule.FileName;
                                    string folderName = Path.GetFileName(Path.GetDirectoryName(fullPath));

                                    if (folderName.Contains("version-"))
                                    {
                                        KeyGay2.robloxversions.Add(folderName);
                                    }
                                }
                                catch
                                {

                                }
                            }


                            if (!logWatcher.IsInGame)
                            {
                                await Dispatcher.InvokeAsync(() =>
                                {
                                    if (!adsandsandjsa)
                                        Notificar("Join a game to inject!");
                                    adsandsandjsa = true;

                                    fwfefeqfqefe.Fill = new SolidColorBrush(Colors.DarkCyan);
                                    Rejoinbtn.IsEnabled = false;
                                    ServerHop.IsEnabled = false;

                                    if (!string.IsNullOrEmpty(lastplayedurl))
                                    {
                                        LastplayedText0.Text = "Last Played";
                                        LastplayedText2.Text = "Start This Game";
                                    }
                                    else
                                    {
                                        LastplayedText0.Text = "Waiting for Game";
                                        LastplayedText1.Text = "Join Something!";
                                        LastplayedText2.Text = "";
                                    }
                                });
                            }

                            if (logWatcher.IsInGame)
                            {
                                adsandsandjsa = true;
                                if (ExecSettings.CollectGameData && logWatcher.CurrentPlaceId != lastplaceid && logWatcher.CurrentPlaceId != 0)
                                {
                                    lastplaceid = logWatcher.CurrentPlaceId;
                                    var result = await GetRobloxGameInfo(placeid: logWatcher.CurrentPlaceId);

                                    try
                                    {
                                        File.WriteAllText("C:\\Essence\\userdata\\LastGame.txt",
                                            $"{logWatcher.CurrentPlaceId} & {result.gameName} & {result.imageUrl}");

                                        List<string> games = new List<string>(File.ReadAllLines("C:\\Essence\\userdata\\GameHistory.txt"));
                                        games.RemoveAll(game => game.Contains(logWatcher.CurrentPlaceId.ToString()) || game.Contains(result.gameName));
                                        games.Insert(0, $"{logWatcher.CurrentPlaceId} & {result.gameName} & {result.imageUrl} & {result.creatorName}");
                                        File.WriteAllLines("C:\\Essence\\userdata\\GameHistory.txt", games);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Error updating game history: " + ex.Message);
                                    }

                                    await Dispatcher.InvokeAsync(() =>
                                    {
                                        LastplayedText0.Text = "Playing";
                                        LastplayedText2.Text = "Find Scripts";
                                        fwfefeqfqefe.Fill = new SolidColorBrush(Colors.Green);
                                        UpdateLastPlayed();
                                    });
                                }
                            }

                            if (inj4)
                            {
                                await Dispatcher.InvokeAsync(() =>
                                {
                                    Rejoinbtn.IsEnabled = true;
                                    //Status_Label.Text = "Injected";
                                    ServerHop.IsEnabled = true;
                                });
                            }
                            else if (!InjectionInProgress && !patched && logWatcher.IsInGame && !inj4)
                            {
                                InjectionInProgress = true;
                                //await Dispatcher.InvokeAsync(() => Status_Label.Text = "Loading...");

                                if (await api2.TestExecution(8))
                                {
                                    inj4 = true;
                                    EXECUTAR(Essence.Properties.Resources.loadazure, true);
                                   // await Dispatcher.InvokeAsync(() => Status_Label.Text = "Injected");
                                }
                                else
                                {                                    
                                    var result = await api2.Inject();

                                    await Dispatcher.InvokeAsync(async() =>
                                    {
                                        switch (result)
                                        {
                                            case api2.InjectionResult.Success:
                                                inj4 = true;
                                                EXECUTAR(Essence.Properties.Resources.loadazure, true);
                                                await Dispatcher.InvokeAsync(() => Notificar("Injected"));
                                                break;

                                            case api2.InjectionResult.Failed:
                                                //Status_Label.Text = "Failed To Inject";
                                                await Task.Delay(3000);
                                                break;
                                        }
                                    });
                                }

                                InjectionInProgress = false;
                            }
                        }
                        else
                        {
                            adsandsandjsa = false;
                            await Dispatcher.InvokeAsync(() =>
                            {
                                fwfefeqfqefe.Fill = new SolidColorBrush(Colors.Gray);
                                //Status_Label.Text = "Roblox not found";

                                if (!string.IsNullOrEmpty(lastplayedurl))
                                {
                                    LastplayedText0.Text = "Last Played";
                                    LastplayedText2.Text = "Start This Game";
                                }
                                else
                                {
                                    LastplayedText0.Text = "Waiting for Roblox";
                                    LastplayedText1.Text = "Start Roblox";
                                    LastplayedText2.Text = "Start Roblox Now!";
                                }
                            });

                            try
                            {
                                string logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Roblox\\logs");
                                if (Directory.Exists(logDirectory))
                                {
                                    foreach (var filePath in Directory.GetFiles(logDirectory, "*.log"))
                                    {
                                        File.Delete(filePath);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error cleaning logs: " + ex.Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    await Dispatcher.InvokeAsync(() =>
                    {
                        InternalConsolePrint(ex.ToString(), console_RichTextBox);
                        //Status_Label.Text = "Fatal Error";
                    });
                }
            });
        }

        public async void DiscordRPC()
        {
            while (!inicializado)
            {
                await Task.Delay(1000);
            }
            try
            {
                //if (RPCdisposed)
                //{
                //    client = new DiscordRpcClient("1336373573744332963")
                //    {
                //        Logger = new ConsoleLogger
                //        {
                //            Level = LogLevel.None
                //        }
                //    };

                //    client.Initialize();

                //    RPCloaded = true;
                //    RPCdisposed = false;
                //    client.SetPresence(new RichPresence
                //    {
                //        Details = "Essence Executor",
                //        Assets = new Assets
                //        {
                //            LargeImageKey = "background4",
                //        },
                //        Timestamps = new Timestamps()
                //        {
                //            Start = DateTime.UtcNow
                //        },
                //        Buttons =
                //        [
                //            new DiscordRPC.Button
                //            {
                //                Label = "Get Essence Now!",
                //                Url = "https://discord.gg/Ku5HGekNQw"
                //            }
                //        ]
                //    });
                //}
            }
            catch
            {
            }
        }

        //private async void Injetar()
        //{
        //    if (InjectionInProgress || Baixando)
        //    {
        //        return;
        //    }
        //    InjectionInProgress = true;
        //    Baixando = false;
        //    await Dispatcher.InvokeAsync(async () =>
        //    {
        //        Process[] processes = Process.GetProcessesByName("RobloxPlayerBeta");
        //        Process[] processes2 = Process.GetProcessesByName("Windows10Universal");

        //        if (processes.Any() || processes2.Any())
        //        {

        //            if (!Directory.Exists("C:\\Essence\\Injector"))
        //            {
        //                Directory.CreateDirectory("C:\\Essence\\Injector");

        //                DoubleAnimation widthAnimation = new DoubleAnimation()
        //                {
        //                    From = 0,
        //                    To = 212,
        //                    Duration = TimeSpan.FromSeconds(1),
        //                    EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
        //                };
        //                Notify.BeginAnimationP(FrameworkElement.WidthProperty, widthAnimation);
        //                InjectionStatus.Text = "Baixando Arquivos x";



        //                WebClient client = new WebClient();
        //                client.Headers.Add(HttpRequestHeader.UserAgent, web);
        //                Baixando = true;

        //                client.DownloadProgressChanged += (object sender2, DownloadProgressChangedEventArgs e2) =>
        //                {
        //                    //if(e2.ProgressPercentage )
        //                    //DoubleAnimation widthAnimation3 = new DoubleAnimation()
        //                    //{
        //                    //    From = 212,
        //                    //    To = 0,
        //                    //    Duration = TimeSpan.FromSeconds(10),

        //                    //};
        //                    //car7.BeginAnimationP(FrameworkElement.WidthProperty, widthAnimation3);
        //                    car7.Width = e2.ProgressPercentage * 212 / 100;
        //                };

        //                client.DownloadFileCompleted += async (object sender2, AsyncCompletedEventArgs e2) =>
        //                {
        //                    if (e2.Error == null && !e2.Cancelled)
        //                    {
        //                        DoubleAnimation widthAnimation2 = new DoubleAnimation()
        //                        {
        //                            From = 212,
        //                            To = 0,
        //                            Duration = TimeSpan.FromSeconds(1.5),
        //                            EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
        //                        };
        //                        Notify.BeginAnimationP(FrameworkElement.WidthProperty, widthAnimation2);

        //                        await Task.Delay(1500);

        //                        await Dispatcher.InvokeAsync(async () =>
        //                        {
        //                            ZipFile.ExtractToDirectory("C:\\Essence\\Injector\\injector.zip", "C:\\Essence\\Injector");
        //                            await Task.Delay(1000);
        //                            File.Delete("C:\\Essence\\Injector\\injector.zip");
        //                        });

        //                        await Task.Delay(1000);

        //                        Notificar("Injetando...", 3);
        //                        await Task.Delay(1000);

        //                        await Task.Run(delegate
        //                        {
        //                            Arka.Attach();
        //                        });

        //                        //ProcessStartInfo startInfo = new ProcessStartInfo();
        //                        //startInfo.FileName = "C:\\Essence\\Injector\\Injector.py";
        //                        ////startInfo.Arguments = "Injector.py";
        //                        //startInfo.WorkingDirectory = "C:\\Essence\\Injector";

        //                        //try
        //                        //{
        //                        //    Process.Start(startInfo);
        //                        //}
        //                        //catch (Exception ex)
        //                        //{
        //                        //    Notificar($"Ocorreu um erro: {ex.Message}", 3);
        //                        //}




        //                    }
        //                    else
        //                    {
        //                        Baixando = false;
        //                        InjectionInProgress = false;
        //                        Notificar("Erro Ao Baixar arquivos!", 3);
        //                        return;
        //                    }
        //                };

        //                client.DownloadFileAsync(new Uri(await KeyGay2.Get("inj")), "C:\\Essence\\Injector\\injector.zip");

        //            }
        //            else
        //            {
        //                Notificar("Injetando...", 3);

        //                await Task.Run(delegate
        //                {
        //                    Arka.Attach();
        //                });


        //                //ProcessStartInfo startInfo = new ProcessStartInfo();
        //                //startInfo.FileName = "C:\\Essence\\Injector\\python.exe";
        //                //startInfo.Arguments = "Injector.py";
        //                //startInfo.WorkingDirectory = "C:\\Essence\\Injector";

        //                //try
        //                //{
        //                //    Process.Start(startInfo);
        //                //}
        //                //catch (Exception ex)
        //                //{
        //                //    Notificar($"Ocorreu um erro: {ex.Message}", 3);
        //                //}
        //            }


        //        }
        //        else
        //        {
        //            Notificar("Abra o Roblox antes de Injetar.", 3);
        //        }
        //    });
        //}


        private SemaphoreSlim exec_control = new SemaphoreSlim(1, 2);
        private string scripts_na_fila = "";
        private async void EXECUTAR(string script, bool force = false)
        {
            Console.WriteLine(script);

            if (!inj4 && !force)
            {
                Notificar("Elemental is not Injected.");
                return;
            }

            if (scripts_na_fila.Contains(script))
                return;

            scripts_na_fila += script;
            await exec_control.WaitAsync();

            await Task.Run(async () =>
            {
                try
                {
                    await api2.SendScript(script);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("EXECUTION ERROR: " + ex.Message);
                }
                finally
                {
                    await Task.Delay(100);
                    scripts_na_fila = scripts_na_fila.Replace(script, "");
                    exec_control.Release();
                }
            });
        }


        private async void Execute(object sender, MouseButtonEventArgs e)
        {
            if (!inj4)
                Notificar("Inject Essence first!", 3);

            EXECUTAR(await GetEditorText(CurrentTabWithStuff()), true);
        }

        private void InjectIcon_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Injetar();
        }

        private async void ClearIcon_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await SetEditorTextAsync(CurrentTabWithStuff(), "");
        }

        private async void SaveIcon_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await Task.Delay(700);
            Microsoft.Win32.SaveFileDialog saveFileDialog1 = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog1.Title = "Essence - Salvar Script";
            saveFileDialog1.Filter = "Script Lua|*.lua";
            Microsoft.Win32.SaveFileDialog saveFileDialog2 = saveFileDialog1;

            bool? nullable = saveFileDialog2.ShowDialog();
            bool flag = true;

            if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
                return;

            foreach (string name in saveFileDialog2.FileNames)
            {
                StreamWriter sw = new StreamWriter(name);
                sw.Write(await GetEditorText(CurrentTabWithStuff()));
                sw.Close();
            }
        }

        private async void OpenIcon_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await Task.Delay(700);
            Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog();
            openFileDialog1.Title = "Essence - Escolha um script";
            openFileDialog1.Filter = "Script Lua|*.lua|Arquivo de texto|*.txt";
            Microsoft.Win32.OpenFileDialog openFileDialog2 = openFileDialog1;
            bool? nullable = openFileDialog2.ShowDialog();
            bool flag = true;
            if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
                return;
            await NewTabAsync(File.ReadAllText(openFileDialog2.FileName), System.IO.Path.GetFileName(openFileDialog2.FileName));
        }




        //[DllImport("user32.dll")]
        //static extern bool SetWindowDisplayAffinity(IntPtr hWnd, uint dwAffinity);
        private async void MainWin_Loaded(object sender, RoutedEventArgs e)
        {
            //if (offline_mode_enabled)
            //Notificar($"WARNING: Essence is in DEBUG MODE!", 5);

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

            LocationChanged += UpdateProfileSettings;
            SizeChanged += UpdateProfileSettings;
            ProfileSettings = new ProfileSettings();
            ProfileSettings.Owner = this;
            UpdateProfileSettings(null, null);
            ProfileSettings.CreateAccount.PreviewMouseDown += CreateAccount_PreviewMouseDown;

            ProfileSettings.UserManagementGrid.Visibility = Visibility.Collapsed;
            ProfileSettings.UserManagementBorder.Margin = new Thickness(0, 100, 0, 25);

            Inicializar1();            
        }

        bool term;


        ////private async Task ConfigurarFirewall()
        ////{
        ////    term = true;
        ////    return;

        ////    //try
        ////    //{
        ////    //    bool rego = false;
        ////    //    RegistryKey reg = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Essence Studios\\EssenceProc2", writable: true);

        ////    //    if (!reg.GetValueNames().Contains("Caminho") ||
        ////    //        reg.GetValue("Caminho").ToString() != System.Windows.Forms.Application.ExecutablePath)
        ////    //    {
        ////    //        rego = true;
        ////    //    }

        ////    //    if (rego)
        ////    //    {
        ////    //        await base.Dispatcher.Invoke(async delegate
        ////    //        {
        ////    //            StatusLoad2.Text = "Configurando Firewall. Isso pode demorar mas só precisamos fazer uma vez...";
        ////    //            InvalidateVisual();
        ////    //            await Task.Delay(1000);
        ////    //        });

        ////    //        string ruleName = "EssenceXX";
        ////    //        string programPath = System.Windows.Forms.Application.ExecutablePath;
        ////    //        string tcpCommand = $"netsh advfirewall firewall add rule name=\"{ruleName} TCP\" dir=in action=allow protocol=TCP program=\"{programPath}\" enable=yes";
        ////    //        string udpCommand = $"netsh advfirewall firewall add rule name=\"{ruleName} UDP\" dir=in action=allow protocol=UDP program=\"{programPath}\" enable=yes";

        ////    //        ProcessStartInfo psi = new ProcessStartInfo("cmd.exe")
        ////    //        {
        ////    //            RedirectStandardInput = true,
        ////    //            UseShellExecute = false,
        ////    //            CreateNoWindow = true
        ////    //        };

        ////    //        Process process = new Process();
        ////    //        process.StartInfo = psi;
        ////    //        process.Start();

        ////    //        //process.StandardInput.WriteLine("Remove-NetFirewallRule -DisplayName 'EssenceXX TCP'");
        ////    //        //process.StandardInput.WriteLine("Remove-NetFirewallRule -DisplayName 'EssenceXX UDP'");

        ////    //        process.StandardInput.WriteLine(tcpCommand);
        ////    //        process.StandardInput.WriteLine(tcpCommand.Replace("in", "out"));

        ////    //        process.StandardInput.WriteLine(udpCommand);
        ////    //        process.StandardInput.WriteLine(udpCommand.Replace("in", "out"));

        ////    //        process.StandardInput.WriteLine("exit");
        ////    //        process.WaitForExit();

        ////    //        if (process.ExitCode != 0)
        ////    //        {
        ////    //            System.Windows.MessageBox.Show($"Ocorreu um erro ao executar os comandos. Código de saída: {process.ExitCode}", "Erro ao permitir Essence no firewall");
        ////    //        }
        ////    //        else
        ////    //        {
        ////    //            RegistryKey rr = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Essence Studios\\EssenceProc2", writable: true);
        ////    //            rr.SetValue("Caminho", System.Windows.Forms.Application.ExecutablePath);
        ////    //        }

        ////    //        term = true;
        ////    //    }
        ////    //    else
        ////    //    {
        ////    //        term = true;
        ////    //    }
        ////    //}
        ////    //catch (Exception ex2)
        ////    //{
        ////    //    System.Windows.MessageBox.Show("Ocorreu um erro: " + ex2.Message, "Erro ao permitir Essence no firewall");
        ////    //    term = true;
        ////    //}
        ////}

        CriticalWarnings CriticalWindow;
        private async void ShowWarnings(Grid Obj, bool collapseMainWindow)
        {
            await Dispatcher.InvokeAsync(async () =>
            {
                if (isplaying_music)
                    FadeMusic(3, false);

                if (inicializado)
                {
                    MainWin.IsHitTestVisible = false;
                    MainWin.BeginAnimationP(UIElement.OpacityProperty, new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.4)));
                }
                else if (WelcomePageopen)
                {
                    WelcomeWindow.IsHitTestVisible = false;
                    WelcomeWindow.BeginAnimationP(UIElement.OpacityProperty, new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.4)));
                }
                else
                {
                    App.StartupWindow.MainWin.IsHitTestVisible = false;
                    App.StartupWindow.MainWin.BeginAnimationP(UIElement.OpacityProperty, new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.4)));
                }


                CriticalWindow.Opacity = 0;
                CriticalWindow.Conexão_Alterada.Visibility = Visibility.Collapsed;
                CriticalWindow.Servidor_Off.Visibility = Visibility.Collapsed;
                CriticalWindow.Net_Off.Visibility = Visibility.Collapsed;
                CriticalWindow.Serv_429.Visibility = Visibility.Collapsed;

                RobloxOudated.Visibility = Visibility.Collapsed;
                CriticalWindow.Installing_Requirements.Visibility = Visibility.Collapsed;

                CriticalWindow.reason.Text = KeyGay2.last_reason;

                Obj.Visibility = Visibility.Visible;

                CriticalWindow.Show();
                CriticalWindow.BeginAnimationP(UIElement.OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.4)));

                if (Obj == RobloxOudated && MaximumExecutorQuality)
                {
                    DoubleAnimation angleAnimation = new DoubleAnimation
                    {
                        From = -20.0,
                        To = 20.0,
                        Duration = new Duration(TimeSpan.FromMilliseconds(4000)),
                        AutoReverse = true,
                        RepeatBehavior = RepeatBehavior.Forever
                    };

                    hexagonangle.BeginAnimation(RotateTransform.AngleProperty, angleAnimation);

                    DoubleAnimation angleAnimation2 = new DoubleAnimation
                    {
                        From = 230,
                        To = 260,
                        Duration = new Duration(TimeSpan.FromMilliseconds(2000)),
                        AutoReverse = true,
                        RepeatBehavior = RepeatBehavior.Forever
                    };

                    fuckhexagon.BeginAnimationP(WidthProperty, angleAnimation2);
                }
                else if (Obj != RobloxOudated)
                {
                    await Task.Delay(400);

                    if (inicializado)
                        MainWin.Hide();
                    else
                        App.StartupWindow.MainWin.Hide();

                    if (collapseMainWindow)
                    {
                        prevent_closing = true;
                        MainWin.Close();
                    }
                }
            });
        }

        private void ReduceQuality()
        {
            MaximumExecutorQuality = false;
            fefnejnfeajnfjaenf = 2.2;
            WindowBorder.Effect = null;
        }

        private bool animado;
        private bool prevent_closing = false;
        private bool WelcomePageopen = false;
        private async void AnimateUI(bool erro = false, bool reverse = false, bool ads = false, bool welcome = false)
        {
            Left = (screenWidth - Width) / 2.0;
            Top = (screenHeight - Height) / 2.0;

            if (inicializado)
                return;

            await Dispatcher.InvokeAsync(async () =>
            {
                Show();
                if (!reverse)
                {
                    var marginAnimation = new ThicknessAnimation
                    {
                        To = new Thickness(0),
                        Duration = TimeSpan.FromSeconds(0.65),
                        EasingFunction = new CircleEase { EasingMode = EasingMode.EaseInOut },
                    };
                    WindowBorder.BeginAnimationP(MarginProperty, marginAnimation);
                    try { App.StartupWindow.WindowBorder3.BeginAnimationP(MarginProperty, marginAnimation); } catch { }

                    if (!erro && !animado)
                    {
                        await Task.Delay(700);

                        ThicknessAnimation loooo = new ThicknessAnimation()
                        {
                            From = new Thickness(35, 0, -200, 0),
                            To = new Thickness(35, 0, 0, 0),
                            Duration = TimeSpan.FromSeconds(0.7),
                            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                        };
                        App.StartupWindow.egggg.BeginAnimationP(MarginProperty, loooo);

                        //App.StartupWindow.ermm.Visibility = Visibility.Collapsed;
                        App.StartupWindow.eeee.Visibility = Visibility.Collapsed;

                        await Task.Delay(1300);

                        MainWin.BeginAnimationP(UIElement.OpacityProperty, new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.4)));

                        await Task.Delay(750);

                        //if (eula)
                        //{
                        //    ///EULA.Visibility = Visibility.Visible;
                        //    executor.Visibility = Visibility.Visible;

                        //    if (!ads)
                        //        Topbar.Visibility = Visibility.Visible;
                        //}

                        if (welcome)
                        {
                            executor.Visibility = Visibility.Collapsed;
                            ResizeMode = ResizeMode.NoResize;
                            WelcomeWindow = new WelcomePage();
                            WelcomeWindow.Show();
                            prevent_closing = true;
                            MainWin.Close();
                            WelcomePageopen = true;
                        }

                        else
                        {
                            ResizeMode = ResizeMode.CanResize;
                            executor.Visibility = Visibility.Visible;
                        }

                        MainWin.BeginAnimationP(UIElement.OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.4)));
                    }

                    if (!ads)
                    {
                        Topbar.Visibility = Visibility.Visible;
                        App.StartupWindow.MainWin.IsHitTestVisible = false;
                        App.StartupWindow.MainWin.BeginAnimationP(UIElement.OpacityProperty, new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.4)));
                        await Task.Delay(500);
                        App.StartupWindow.Visibility = Visibility.Collapsed;
                        try { App.StartupWindow.RGBTime.Stop(); } catch { }
                    }

                    animado = true;
                }

                else
                {
                    ResizeMode = ResizeMode.NoResize;
                    executor.Visibility = Visibility.Hidden;
                    Topbar.Visibility = Visibility.Collapsed;

                    App.StartupWindow.Visibility = Visibility.Visible;
                    App.StartupWindow.MainWin.IsHitTestVisible = false;
                    App.StartupWindow.MainWin.BeginAnimationP(UIElement.OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.4)));

                    var marginAnimation = new ThicknessAnimation
                    {
                        To = new Thickness(170, 170, 170, 170),
                        Duration = TimeSpan.FromSeconds(0.65),
                        EasingFunction = new CircleEase { EasingMode = EasingMode.EaseInOut },
                    };

                    WindowBorder.BeginAnimationP(MarginProperty, marginAnimation);
                    try { App.StartupWindow.WindowBorder3.BeginAnimationP(MarginProperty, marginAnimation); } catch { }

                    animado = false;
                }
            });
        }


        private async void Mudar_Nome(string nome)
        {
            if (string.IsNullOrEmpty(nome) || nome == "")
            {
                return;
            }

            await Dispatcher.InvokeAsync(() =>
            {
                UserName2 = nome;

                Wolfregkey.SetValue("Nome", nome);

                
                try { User_config.Text = lm.Translate("Olá") + ", " + nome + "!"; } catch { }
                try { ProfileSettings.f3f3nfy3uf3f.Text = nome; } catch { }

                //try { Login_name2.Text = nome; } catch { }
            });
        }

        private async void Mudar_Imagem(string imagem = "")
        {
            if (string.IsNullOrEmpty(imagem) || imagem == "" || imagem.Length < 10)
            {
                return;
            }

            await Dispatcher.InvokeAsync(() =>
            {
                UserAvatar2 = imagem;

                Wolfregkey.SetValue("Avatar", imagem);

                try
                {
                    List<string> linhas = new List<string>(File.ReadAllLines("C:\\Essence\\userdata\\logindata.txt"));
                    linhas[4] = imagem;
                    File.WriteAllLines("C:\\Essence\\userdata\\logindata.txt", linhas);
                }
                catch { }

                try
                {
                    string dest = "C:\\Essence\\userdata\\UserImgs";

                    if (!Directory.Exists(dest))
                    {
                        Directory.CreateDirectory(dest);
                    }
                }
                catch { }

                try
                {
                    BitmapImage bitmap = null;
                    bitmap = new BitmapImage(new Uri(imagem, UriKind.RelativeOrAbsolute));
                    Color pixelColor = GetPixelColor(bitmap);

                    try { ImageBehavior.SetAnimatedSource(User_Img, bitmap); } catch { }
                    try { ImageBehavior.SetAnimatedSource(User_Img2, bitmap); } catch { }
                    try { ImageBehavior.SetAnimatedSource(ProfileSettings.User_Img3, bitmap); } catch { }
                    //try { ImageBehavior.SetAnimatedSource(User_Img_miniature2, bitmap); } catch { }

                    //try { User_Img.ToolTip = "Your Image :)"; } catch { }
                }
                catch (Exception ex)
                {
                    ImageBehavior.SetAnimatedSource(User_Img, null);
                    User_Img.Source = null;
                    User_Img.Source = Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.essence.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                    ImageBehavior.SetAnimatedSource(ProfileSettings.User_Img3, null);
                    ProfileSettings.User_Img3.Source = null;
                    ProfileSettings.User_Img3.Source = Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.essence.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }

            });
        }


        private struct ConsoleLog
        {
            public string Message { get; set; }
            public ConsoleColor Color { get; set; }

            public ConsoleLog(string message, ConsoleColor color)
            {
                Message = message;
                Color = color;
            }
        }

        private List<ConsoleLog> consoleLogs = new List<ConsoleLog>();

        public void ConsolePrint2(string text, ConsoleColor color)
        {
            consoleLogs.Add(new ConsoleLog(text, color));

            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void InternalConsolePrint(string text, System.Windows.Controls.RichTextBox console_RichTextBox, Color? color = null, ConsoleColor? color2 = null)
        {
            try
            {
                if (text.Length < 5)
                    return;

                text = text.Replace("\n", "").Replace("\r", "");
                System.Windows.Documents.Paragraph paragraph = new Paragraph()
                {
                    Margin = new Thickness(2)
                };

                string[] array = text.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);


                DateTime time = DateTime.Now;

                Run run = new Run($"[{time.Day:D2}/{time.Month:D2}/{time.Year} | {time.Hour:D2}:{time.Minute:D2}:{time.Second:D2}]   >   ");
                run.Foreground = new SolidColorBrush(Color.FromRgb(100, 100, 100));
                paragraph.Inlines.Add(run);

                for (int num2 = 0; num2 < array.Length; num2++)
                {
                    string processedWord = array[num2];
                    run = new Run(processedWord + " ");
                    paragraph.Inlines.Add(run);
                }

                //Color displayColor = color ?? Color.FromRgb(100, 100, 100);
                Color displayColor = Color.FromRgb(100, 100, 100);


                paragraph.FontSize = 12;
                paragraph.Foreground = new SolidColorBrush(displayColor);
                console_RichTextBox.Document.Blocks.Add(paragraph);
                console_RichTextBox.ScrollToEnd();
            }
            catch { }
        }



        string premiumkey = "";
        bool premium = false;

        bool MODIFY = false;
        bool OFF = false;
        bool NET = false;
        bool E429 = false;

        private bool inicializado2 = false;
        private bool firstboot = false;
        internal string info = "";
        string[] ads = { "" };
        //string ads_redirect = "https://discord.gg/Ku5HGekNQw";
        bool ad_redirect = false;

        private void FatalError(string parte, string ex)
        {
            StopAllInteractions = true;
            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    Application.Current.MainWindow.Hide();
                }
                catch (Exception e) { }


                CriticalWindow criticalWindow = new CriticalWindow($"Um erro crítico ocorreu. Envie isso para o M4A1 urgentemente.\n Erro ocorreu em: {parte}\n\n\n{ex}");
                criticalWindow.Show();
            });
        }

        string bloxfolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bloxstrap\\State.json");



        private async Task CheckForDependencies()
        {
            //https://msedge.sf.dl.delivery.mp.microsoft.com/filestreamingservice/files/c1336fd6-a2eb-4669-9b03-949fc70ace0e/MicrosoftEdgeWebview2Setup.exe



            //bool net60_installed = false;
            //bool bloxstrap_installed = false;

            //string dotnetPath = @"C:\Program Files\dotnet\shared\Microsoft.NETCore.App";

            //if (Directory.Exists(dotnetPath))
            //{
            //   var versions = Directory.GetDirectories(dotnetPath)
            //                           .Select(Path.GetFileName)
            //                           .Where(v => Version.TryParse(v, out var parsedVersion) && parsedVersion >= new Version(6, 0))
            //                           .ToList();

            //    if (versions.Any())
            //    {
            //        net60_installed = true;
            //        Console.WriteLine($".NET Core 6.0 está instalado. Versões encontradas: {string.Join(", ", versions)}");
            //    }
            //}

            //if (System.IO.File.Exists(BloxstrapInterface.Path + "\\Bloxstrap.exe") && BloxstrapInterface.GetHashFromFile(BloxstrapInterface.Path + "\\Bloxstrap.exe") == BloxstrapInterface.MD5)
            //{
            //    bloxstrap_installed = true;
            //    Console.WriteLine("BloxStrap Está instalado e na versão correta.");
            //}









            //if (!net60_installed || !bloxstrap_installed)
            //{
            //    AnimateUI(true);
            //    ShowWarnings(Installing_Requirements);


            //    if (!net60_installed)
            //    {
            //        Console.WriteLine(".NET Core 6.0 OU VERSÃO MAIS ATUALIZADA NÃO ESTÁ PRESENTE");


            //        await Task.Run(async () =>
            //        {
            //            Download download = null;
            //            Dispatcher.Invoke(() =>
            //            {
            //                download = new Download();
            //                //Instalations_panel.Children.Add(download);

            //                download.Title.Text = ".NET 6.0";
            //            });

            //            string installerPath = "dotnet-installer.exe";

            //            try
            //            {
            //                using (WebClient client8 = new WebClient())
            //                {
            //                    DownloadProgressChangedEventHandler progressChangedHandler = async (s, e) =>
            //                    {
            //                        Dispatcher.Invoke(() =>
            //                        {
            //                            download.Title.Text = $".NET 6.0 [{e.ProgressPercentage}%]";
            //                            download.ProgressBar7.Width = e.ProgressPercentage * 2.50;
            //                        });
            //                    };

            //                    AsyncCompletedEventHandler fileCompletedHandler = async (s, e) =>
            //                    {
            //                        if (e.Error == null)
            //                        {
            //                            await Task.Delay(1000);

            //                            var process = new System.Diagnostics.Process();
            //                            process.StartInfo.FileName = installerPath;
            //                            process.StartInfo.Arguments = "/quiet /norestart";
            //                            process.StartInfo.UseShellExecute = false;
            //                            process.StartInfo.CreateNoWindow = true;
            //                            process.Start();

            //                            Task.Run(() =>
            //                            {
            //                                Dispatcher.Invoke(async () =>
            //                                {
            //                                    DoubleAnimation widthAnimation1 = new DoubleAnimation
            //                                    {
            //                                        From = 0,
            //                                        To = 230,
            //                                        Duration = TimeSpan.FromSeconds(5),
            //                                    };
            //                                    download.ProgressBar7.BeginAnimationP(FrameworkElement.WidthProperty, widthAnimation1);

            //                                    for (int i = 0; i <= 92; i++)
            //                                    {
            //                                        download.Title.Text = $".NET 6.0 [{i}%]";
            //                                        await Task.Delay(52);
            //                                    }
            //                                });
            //                            });


            //                            process.WaitForExit();
            //                            await Task.Delay(5000);

            //                            Dispatcher.Invoke(async () =>
            //                            {
            //                                DoubleAnimation widthAnimation2 = new DoubleAnimation
            //                                {
            //                                    From = 230,
            //                                    To = 250,
            //                                    Duration = TimeSpan.FromSeconds(2),
            //                                };
            //                                download.ProgressBar7.BeginAnimationP(FrameworkElement.WidthProperty, widthAnimation2);

            //                                for (int i = 92; i <= 100; i++)
            //                                {
            //                                    download.Title.Text = $".NET 6.0 [{i}%]";
            //                                    await Task.Delay(120);
            //                                }

            //                                await Task.Delay(2000);

            //                                net60_installed = true;
            //                                download.Downloading.Visibility = Visibility.Collapsed;
            //                                download.completed.Visibility = Visibility.Visible;

            //                                BitmapImage bitmapImage = new BitmapImage(new Uri("completed.gif", UriKind.RelativeOrAbsolute));
            //                                ImageBehavior.SetAnimatedSource(download.completed, bitmapImage);
            //                                ImageBehavior.SetRepeatBehavior(download.completed, new RepeatBehavior(1));
            //                            });
            //                        }
            //                    };

            //                    Inscreve os manipuladores de eventos
            //                    client8.DownloadProgressChanged += progressChangedHandler;
            //                    client8.DownloadFileCompleted += fileCompletedHandler;

            //                    Console.WriteLine("BAIXANDO .NET Core 6.0");
            //                    client8.DownloadFileAsync(new Uri("https://download.visualstudio.microsoft.com/download/pr/3ebc1f91-a5ba-477e-9353-198fa4e13371/35f447d6820b078fd18523764a4f0213/windowsdesktop-runtime-6.0.33-win-x64.exe"), ".\\" + installerPath);
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                MessageBox.Show(ex.ToString());
            //            }
            //        });
            //    }
            //    else
            //    {
            //        ok
            //    }

            //    while (!net60_installed) { await Task.Delay(100); }
            //    await Task.Delay(1000);





            //    if (!bloxstrap_installed)
            //    {
            //        try
            //        {
            //            Download download = null;
            //            Dispatcher.Invoke(() =>
            //            {
            //                download = new Download();
            //                Instalations_panel.Children.Add(download);

            //                download.Title.Text = "BLOXSTRAP";
            //            });

            //            await Task.Run(async () =>
            //            {
            //                bool ok = false;
            //                foreach (Process process in Process.GetProcessesByName("RobloxPlayerBeta"))
            //                {
            //                    try
            //                    {
            //                        process.Kill();
            //                        ok = true;
            //                    }
            //                    catch
            //                    {
            //                    }
            //                }

            //                if (Directory.Exists(BloxstrapInterface.Path))
            //                    Directory.Delete(BloxstrapInterface.Path, true);
            //                if (!Directory.Exists(BloxstrapInterface.Path))
            //                    Directory.CreateDirectory(BloxstrapInterface.Path);


            //                using (WebClient client8 = new WebClient())
            //                {
            //                    foreach (string file in BloxstrapInterface.Files)
            //                    {
            //                        string fileName = ((IEnumerable<string>)file.Split('/')).Last();
            //                        string filePath = BloxstrapInterface.Path + "\\" + fileName;

            //                        var tcs = new TaskCompletionSource<bool>();

            //                        DownloadProgressChangedEventHandler progressChangedHandler = async (s, e) =>
            //                        {
            //                            Dispatcher.Invoke(() =>
            //                            {
            //                                download.Title.Text = $"BLOXSTRAP [{e.ProgressPercentage}%]";
            //                                download.ProgressBar7.Width = e.ProgressPercentage * 2.50;
            //                            });
            //                        };

            //                        AsyncCompletedEventHandler fileCompletedHandler = (s, e) =>
            //                        {
            //                            if (e.Error != null)
            //                            {
            //                                Console.WriteLine($"Error downloading {fileName}: {e.Error.Message}");
            //                            }
            //                            else
            //                            {
            //                                Console.WriteLine($"{fileName} download completed.");
            //                            }
            //                            tcs.SetResult(true);
            //                        };

            //                        Inscreve os manipuladores de eventos
            //                        client8.DownloadProgressChanged += progressChangedHandler;
            //                        client8.DownloadFileCompleted += fileCompletedHandler;

            //                        client8.DownloadFileAsync(new Uri(file), filePath);
            //                        await tcs.Task;
            //                        client8.DownloadProgressChanged -= progressChangedHandler;
            //                        client8.DownloadFileCompleted -= fileCompletedHandler;

            //                        await Task.Delay(200);
            //                        Dispatcher.Invoke(() =>
            //                        {
            //                            download.Title.Text = $"BLOXSTRAP [0%]";
            //                            download.ProgressBar7.Width = 0;
            //                        });
            //                        await Task.Delay(200);
            //                    }

            //                    System.IO.File.WriteAllText(BloxstrapInterface.Path + "\\Settings.json", JsonConvert.SerializeObject((object)new
            //                    {
            //                        BootstrapperStyle = 4,
            //                        BootstrapperIcon = 8,
            //                        BootstrapperTitle = "EssenceX - Launcher",
            //                        BootstrapperIconCustomLocation = (BloxstrapInterface.Path + "\\essence.ico"),
            //                        Theme = 2,
            //                        CheckForUpdates = false,
            //                        CreateDesktopIcon = false,
            //                        MultiInstanceLaunching = true,
            //                        OhHeyYouFoundMe = false,
            //                        Channel = "Live",
            //                        ChannelChangeMode = 2,
            //                        EnableActivityTracking = true,
            //                        UseDiscordRichPresence = true,
            //                        HideRPCButtons = false,
            //                        ShowServerDetails = false,
            //                        UseOldDeathSound = true,
            //                        UseOldCharacterSounds = false,
            //                        UseDisableAppPatch = false,
            //                        UseOldAvatarBackground = false,
            //                        CursorType = 0,
            //                        EmojiType = 0,
            //                        DisableFullscreenOptimizations = false
            //                    }));

            //                    var process = new System.Diagnostics.Process();
            //                    process.StartInfo.FileName = BloxstrapInterface.Path + "\\Bloxstrap.exe";
            //                    process.StartInfo.UseShellExecute = false;

            //                    DataReceivedEventHandler handler = (DataReceivedEventHandler)((_, e) =>
            //                    {
            //                        if (e.Data == null)
            //                            return;

            //                        string resp = e.Data.Trim();
            //                        Console.WriteLine($"[BLOXSTRAP] -> {resp}");
            //                    });

            //                    process.OutputDataReceived += handler;

            //                    process.Start();

            //                    Task.Run(() =>
            //                    {
            //                        Dispatcher.Invoke(async () =>
            //                        {
            //                            DoubleAnimation widthAnimation1 = new DoubleAnimation
            //                            {
            //                                From = 125,
            //                                To = 230,
            //                                Duration = TimeSpan.FromSeconds(5),
            //                            };
            //                            download.ProgressBar7.BeginAnimationP(FrameworkElement.WidthProperty, widthAnimation1);

            //                            for (int i = 50; i <= 92; i++)
            //                            {
            //                                download.Title.Text = $"BLOXSTRAP [{i}%]";
            //                                await Task.Delay(120);
            //                            }
            //                        });
            //                    });

            //                    while (true)
            //                    {
            //                        if (File.Exists(bloxfolder))
            //                            break;
            //                        await Task.Delay(1000);
            //                    }

            //                    Dispatcher.Invoke(async () =>
            //                    {
            //                        DoubleAnimation widthAnimation2 = new DoubleAnimation
            //                        {
            //                            To = 250,
            //                            Duration = TimeSpan.FromSeconds(2),
            //                        };
            //                        download.ProgressBar7.BeginAnimationP(FrameworkElement.WidthProperty, widthAnimation2);

            //                        for (int i = 92; i <= 100; i++)
            //                        {
            //                            download.Title.Text = $"BLOXSTRAP [{i}%]";
            //                            await Task.Delay(120);
            //                        }

            //                        bloxstrap_installed = true;
            //                        download.Downloading.Visibility = Visibility.Collapsed;
            //                        download.completed.Visibility = Visibility.Visible;

            //                        BitmapImage bitmapImage = new BitmapImage(new Uri("completed.gif", UriKind.RelativeOrAbsolute));
            //                        ImageBehavior.SetAnimatedSource(download.completed, bitmapImage);
            //                        ImageBehavior.SetRepeatBehavior(download.completed, new RepeatBehavior(1));
            //                    });
            //                }
            //            });
            //        }
            //        catch
            //        {

            //        }
            //    }
            //    else
            //    {
            //        ok
            //    }

            //    while (!bloxstrap_installed) { await Task.Delay(100); }

            //    await Task.Delay(2000);

            //    Installing_Requirements.Visibility = Visibility.Collapsed;
            //    Warnings.Visibility = Visibility.Collapsed;
            //    App.StartupWindow.Visibility = Visibility.Visible;
            //    App.StartupWindow.MainWin.BeginAnimationP(UIElement.OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.4)));
            //    AnimateUI(reverse: true);
            //    executor.Visibility = Visibility.Visible;
            //}
        }

        private async void AddText(string txt)
        {
            TextBlock a = new TextBlock()
            {
                Text = txt,
                Foreground = new SolidColorBrush(Color.FromRgb(150, 150, 150)),
                FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Essence;component/Fonts/#Poppins Light"),
                FontSize = 10,
                Height = 0,
                Opacity = 0,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            DoubleAnimation kk = new DoubleAnimation()
            {
                From = 0,
                To = 15,
                Duration = TimeSpan.FromSeconds(0.35),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };

            DoubleAnimation dd = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };


            App.StartupWindow.eeee.Children.Insert(0, a);
            a.BeginAnimation(OpacityProperty, kk);
            a.BeginAnimation(HeightProperty, kk);
        }

        private void Moveinitprogress(int percentage)
        {
            DoubleAnimation sizeb2 = new DoubleAnimation()
            {
                To = percentage * 1.5,
                Duration = TimeSpan.FromSeconds(1.1),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            App.StartupWindow.SpinningBorder.BeginAnimation(WidthProperty, sizeb2);
        }

        private System.Windows.Controls.Button CreateButton(string file, string content)
        {
            System.Windows.Controls.Button button = new System.Windows.Controls.Button
            {
                Padding = new Thickness(0),
                BorderThickness = new Thickness(0),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#FFE6E6E6"),
                Background = null
            };
            button.SetValue(MaterialDesignThemes.Wpf.ButtonAssist.CornerRadiusProperty, new CornerRadius(4));
            button.Style = (Style)FindResource("MaterialDesignFlatDarkBgButton");

            button.Click += (RoutedEventHandler)(async (sender, e) =>
            {
                await NewTabAsync(content, Path.GetFileName(file));
            });

            Grid grid = new Grid
            {
                Opacity = 0.8,
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = 135
            };

            PackIcon packIcon = new PackIcon
            {
                Kind = PackIconKind.Script,
                Margin = new Thickness(0, 2, 0, 0)
            };

            TextBlock textBlock = new TextBlock
            {
                Text = Path.GetFileName(file),
                FontFamily = new System.Windows.Media.FontFamily("Calibri"),
                FontWeight = FontWeights.Normal,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 15,
                Margin = new Thickness(20, 0, 0, 0)
            };

            grid.Children.Add(packIcon);
            grid.Children.Add(textBlock);

            button.Content = grid;
            return button;
        }

        private void PopulateScriptList(string folderPath, string searchText)
        {
            if (!Directory.Exists(folderPath))
                return;

            ScriptListBox.Items.Clear();

            try
            {
                var nameMatches = new List<string>();
                var contentMatches = new List<string>();

                var files = Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories)
                     .Where(f => f.EndsWith(".lua", StringComparison.OrdinalIgnoreCase) ||
                                 f.EndsWith(".txt", StringComparison.OrdinalIgnoreCase));

                if (searchText != "" && searchText != lm.Translate("Search Scripts"))
                {
                    foreach (var file in files)
                    {
                        string fileName = Path.GetFileName(file);
                        string content = File.ReadAllText(file);

                        if (fileName.ToLower().Contains(searchText.ToLower()))
                        {
                            nameMatches.Add(file);
                        }
                        else if (content.ToLower().Contains(searchText.ToLower()))
                        {
                            contentMatches.Add(file);
                        }
                    }

                    foreach (var file in nameMatches)
                    {
                        string content = File.ReadAllText(file);
                        System.Windows.Controls.Button button = CreateButton(file, content);
                        ScriptListBox.Items.Add(button);
                    }

                    foreach (var file in contentMatches)
                    {
                        string content = File.ReadAllText(file);
                        System.Windows.Controls.Button button = CreateButton(file, content);
                        ScriptListBox.Items.Add(button);
                    }
                }
                else
                {
                    foreach (var file in files)
                    {
                        System.Windows.Controls.Button button = CreateButton(file, File.ReadAllText(file));
                        ScriptListBox.Items.Add(button);
                    }
                }
            }
            catch (Exception ex)
            {
                //InternalConsolePrint($"Ocorreu um erro durante a busca: {ex.Message}", console_RichTextBox, Colors.Red);
            }
        }


        static string ConverterParaStringCompacta(int valor)
        {
            if (valor >= 1000000)
            {
                return "+" + (valor / 1000000) + "M";
            }
            else if (valor >= 100000)
            {
                return "+" + (valor / 1000) + "k";
            }
            else if (valor >= 1000)
            {
                return "+" + (valor / 1000) + "k";
            }
            else
            {
                return valor.ToString();
            }
        }


        private async void UpdateLastPlayed()
        {

            //verificar o problema disso aqui ser chamado varias vezes e criar varios whiles

            try
            {
                if (File.Exists("C:\\Essence\\userdata\\LastGame.txt"))
                {
                    string[] datar = File.ReadAllText("C:\\Essence\\userdata\\LastGame.txt").Split(new[] { " & " }, StringSplitOptions.RemoveEmptyEntries);

                    await Task.Delay(1000);

                    if (lastplayedurl != datar[0] || LastplayedText1.Text != datar[1])
                    {
                        lastplayedurl = datar[0];
                        LastplayedText1.Text = datar[1];
                        LastGamePlayedHolder.Source = new BitmapImage(new Uri(datar[2]));

                        Fade(lastplayedgrid2, 0, 1, 1.3);
                        lastplayedgrid2.Visibility = Visibility.Visible;

                        LastplayedText1.UpdateLayout();
                        LastplayedText1.BeginAnimation(MarginProperty, null);

                        if (LastplayedText1.ActualWidth > 150)
                        {
                            double move_a = LastplayedText1.ActualWidth - 140;
                            string oldtxt = LastplayedText1.Text;

                            while (oldtxt == LastplayedText1.Text)
                            {
                                if (LastMovedGrid == HomeGrid && IsActive)
                                {
                                    Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        ThicknessAnimation aa = new ThicknessAnimation
                                        {
                                            From = new Thickness(0),
                                            To = new Thickness(-move_a, 0, 0, 0),
                                            Duration = TimeSpan.FromSeconds(6),
                                            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut },
                                            AutoReverse = true
                                        };
                                        LastplayedText1.BeginAnimation(MarginProperty, aa);
                                    });
                                    await Task.Delay(12000);
                                }
                                await Task.Delay(1000);
                            }
                        }
                    }
                }
                else
                {
                    Fade(lastplayedgrid2, 0, 1, 1.3);
                    lastplayedgrid2.Visibility = Visibility.Visible;
                    LastplayedText1.UpdateLayout();
                    LastplayedText1.BeginAnimation(MarginProperty, null);
                }

                if (last_search == "")
                {
                    WP3.Children.Clear();
                    var games = File.ReadLines("C:\\Essence\\userdata\\GameHistory.txt");

                    foreach (string line in games)
                    {
                        string[] datar = line.Split(new[] { " & " }, StringSplitOptions.None);
                        var obj = new GameThingy(datar[1], datar[2], datar[0])
                        {
                            ScriptTitle = { Text = datar[1] }
                        };

                        ((ButtonBase)obj.FindName("CoolBTN")).Click += async (sender, e) =>
                        {
                            if (areadyanimatinggrid) return;
                            areadyanimatinggrid = true;

                            WPXD.Children.Clear();
                            WPXD2.Children.Clear();
                            dwuduqwn2.Text = lm.Translate("Searching...");
                            dwuduqwn3.Visibility = Visibility.Collapsed;

                            try
                            {
                                BorderImg.Background = obj.BorderImg.Background;
                                djwndnqim.Text = datar[1] + lm.Translate(" (Searching Scripts...)");
                                selectedgameurl = datar[0];
                            }
                            catch { }

                            Move(GeneralScriptScrollViewer, GeneralScriptScrollViewer.Margin, new Thickness(MainWin.ActualWidth + 150, 75, 0, 4), 0.9);
                            Fade(GeneralScriptScrollViewer, 1, 0, 0.8);
                            Fade(skibidtoilet, 1, 0, 0.8);

                            await Task.Delay(100);

                            Move(GameselectedScroll, new Thickness(MainWin.ActualWidth + 150, 0, 0, 4), new Thickness(0, 0, 0, 4), 1.4);
                            Fade(GameselectedScroll, 0, 1, 1.7);
                            GameselectedScroll.Visibility = Visibility.Visible;

                            var j = new ThicknessAnimation
                            {
                                To = new Thickness(10, 65, 5, 4),
                                Duration = TimeSpan.FromMilliseconds(100)
                            };
                            GeneralScriptScrollViewer.BeginAnimation(MarginProperty, j);
                            selectedscriptinfoborder.Visibility = Visibility.Collapsed;

                            await Task.Delay(1400);
                            areadyanimatinggrid = false;

                            AdvancedSearch(datar[0], datar[1]);

                            var data = await GetRobloxGameInfo(placeid:long.Parse(datar[0]));
                            dwqnduwnq.Text = lm.Translate("Players online: ") + ConverterParaStringCompacta(data.playerCount);
                        };

                        WP3.Visibility = Visibility.Visible;
                        WP3.Children.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void RestartApp(bool warn)
        {
            StopAllInteractions = true;
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



        bool SaveH = true;
        bool SaveTabs = true;
        private bool finish_start_animations = false;
        private string lastplayedurl = "";
        
        WelcomePage WelcomeWindow;
        private int user_r = 10;
        private bool OHHHHH = false;


        public static bool TryAddCookie(WebRequest webRequest, Cookie cookie)
        {
            HttpWebRequest httpRequest = webRequest as HttpWebRequest;
            if (httpRequest == null)
            {
                return false;
            }

            if (httpRequest.CookieContainer == null)
            {
                httpRequest.CookieContainer = new CookieContainer();
            }

            httpRequest.CookieContainer.Add(cookie);
            return true;
        }
        DateTime timeX;
        internal static bool diwendqndnq;
        private async Task Inicializar1()
        {
            //await Task.Run(async () =>
            //{

            console_RichTextBox.Document.Blocks.Clear();
            //console_RichTextBox.set
            InternalConsolePrint("UI started", console_RichTextBox, Colors.Purple);
            //Left = (screenWidth - Width) / 2.0;
            //Top = (screenHeight - Height) / 2.0;


            
            MainWin.Hide();

            Moveinitprogress(10);
            diwendqndnq = true;
            DebugProtect3.HideOSThreads();

            try
            {
                //este código é rodado apenas uma vez
                if (!inicializado2)
                {
                    //await Task.Delay(1000);

                    try
                    {
                        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", writable: true);
                        string friendlyName = AppDomain.CurrentDomain.FriendlyName;
                        if (registryKey.GetValue(friendlyName) == null)
                        {
                            registryKey.SetValue(friendlyName, 11001, RegistryValueKind.DWord);
                        }
                    }
                    catch
                    {
                    }

                    info = "";


                    InternalConsolePrint("Connecting to server...", console_RichTextBox, Colors.Gray);

                    KeyGay2.MODIFY += async delegate
                    {
                        if (!MODIFY && !OFF && !NET)
                        {
                            MODIFY = true;
                            //AnimateUI(true);
                            CriticalWindow = new CriticalWarnings();
                            ShowWarnings(CriticalWindow.Conexão_Alterada, true);
                        }
                    };

                    KeyGay2.OFF += async delegate
                    {
                        if (!OFF)
                        {
                            NET = false;
                            OFF = true;
                            //AnimateUI(true);
                            CriticalWindow = new CriticalWarnings();
                            CriticalWindow.loading_gif1.Visibility = Visibility.Visible;
                            ShowWarnings(CriticalWindow.Servidor_Off, false);
                        }
                    };

                    KeyGay2.E429 += async delegate
                    {
                        if (!E429)
                        {
                            NET = false;
                            OFF = true;

                            CriticalWindow = new CriticalWarnings();
                            CriticalWindow.loading_gif1.Visibility = Visibility.Visible;
                            ShowWarnings(CriticalWindow.Serv_429, false);
                        }
                    };

                    KeyGay2.NET += async delegate
                    {
                        if (!NET)
                        {
                            NET = true;
                            OFF = false;
                            //AnimateUI(true);
                            CriticalWindow = new CriticalWarnings();
                            CriticalWindow.loading_gif1.Visibility = Visibility.Visible;
                            ShowWarnings(CriticalWindow.Net_Off, false);
                        }
                    };


                    bool returnk = false;
                    KeyGay2.Cancel_Error += async delegate
                    {
                        if (!NET && !OFF)
                            return;

                        if (MODIFY)
                            return;

                        NET = false;
                        OFF = false;

                        await Task.Delay(2000);

                        CriticalWindow.force = true;
                        CriticalWindow.Close();

                        if (inicializado)
                        {
                            MainWin.IsHitTestVisible = true;
                            MainWin.BeginAnimationP(UIElement.OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.4)));
                            MainWin.Show();

                            if (isplaying_music)
                                FadeMusic(3, true);
                        }
                        else if (WelcomePageopen)
                        {
                            WelcomeWindow.IsHitTestVisible = true;
                            WelcomeWindow.BeginAnimationP(UIElement.OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.4)));
                            WelcomeWindow.Show();
                        }
                        else
                        {
                            App.StartupWindow.MainWin.IsHitTestVisible = true;
                            App.StartupWindow.MainWin.BeginAnimationP(UIElement.OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.4)));
                            App.StartupWindow.MainWin.Show();
                        }

                        //if (!inicializado)
                        //{
                        //    //returnk = true;
                        //    //try { Inicializar1(); } catch { MessageBox.Show("Bruh"); }
                        //}
                        //else if (RobloxOudated.Visibility == Visibility.Collapsed)
                        //{
                        //    TabControl.Visibility = Visibility.Visible;
                        //}
                    };
                    DebugProtect3.HideOSThreads();

                    if (!Wolfregkey.GetValueNames().Contains("Idioma"))
                    {
                        string systemLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                        switch (systemLanguage)
                        {
                            case "pt":
                                lm.lang = 2;
                                try
                                {
                                    TransL();
                                    Wolfregkey.SetValue("Idioma", "PT");
                                }
                                catch { }

                                //Move(dwhududnwi, dwhududnwi.Margin, new Thickness(0, 50, 270, 0), 1);
                                break;

                            case "es":
                                lm.lang = 3;
                                try
                                {
                                    TransL();
                                    Wolfregkey.SetValue("Idioma", "ES");
                                }
                                catch { }

                                //Move(dwhududnwi, dwhududnwi.Margin, new Thickness(0, 50, 130, 0), 1);
                                break;

                            default:
                                lm.lang = 1;
                                try
                                {
                                    TransL();
                                    Wolfregkey.SetValue("Idioma", "EN");
                                }
                                catch { }

                                //Move(dwhududnwi, dwhududnwi.Margin, new Thickness(0, 50, 410, 0), 1);
                                break;
                        }
                    }
                    else
                    {
                        idioma = Wolfregkey.GetValue("Idioma").ToString();
                        switch (idioma)
                        {
                            case "EN":
                                lm.lang = 1;
                                Idioma.SelectedIndex = 0;
                                break;

                            case "PT":
                                lm.lang = 2;
                                Idioma.SelectedIndex = 1;
                                break;

                            case "ES":
                                lm.lang = 3;
                                Idioma.SelectedIndex = 2;
                                break;
                        }
                        TransL();
                    }

                    try
                    {
                        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_SystemEnclosure");

                        foreach (ManagementObject obj in searcher.Get())
                        {
                            if (obj["ChassisTypes"] != null)
                            {
                                var chassisTypes = (ushort[])obj["ChassisTypes"];
                                foreach (var type in chassisTypes)
                                {
                                    if (type == 8 || type == 9 || type == 10)
                                    {
                                        KeyGay2.pctypeshit = "Notebook";
                                        break;
                                    }
                                    else
                                    {
                                        KeyGay2.pctypeshit = "Desktop";
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro ao obter o tipo do dispositivoo: " + ex.Message);
                    }

                    using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Evo3Software"))
                    {
                        if (key.GetValueNames().Contains("FirstRegister"))
                        {
                            KeyGay2.firstlogin = false;
                        }
                    }
                    
                    int ggs = DebugProtect1.PerformChecks();                    

                    AddText(lm.Translate("Connecting"));

                    DateTime time = DateTime.Now;
                    var task1 = Task.Delay(4000);
                    var task2 = Task.Delay(8000);
                    var task3 = Task.Delay(10000);
                    var operationTask = KeyGay2.Get("info");

                    var completedTask = await Task.WhenAny(operationTask, task1);

                    try
                    {
                        if (completedTask == task1)
                        {
                            AddText(lm.Translate("This is taking longer than expected..."));
                            completedTask = await Task.WhenAny(operationTask, task2);
                            if (completedTask == task2)
                            {
                                AddText(lm.Translate("Ehh... Sorry for that"));
                                completedTask = await Task.WhenAny(operationTask, task3);
                                if (completedTask == task3)
                                {
                                    AddText(lm.Translate("Bruh. Opening the error window..."));
                                }
                            }
                        }
                    }
                    catch { }

                    info = await operationTask;
                    double secondsElapsed = (DateTime.Now - time).TotalSeconds;

                    if (ggs == 1)
                    {
                        MessageBox.Show("Critical error when loading server response", "Uhh");
                        Process.GetCurrentProcess().Kill();
                    }

                    if (StopAllInteractions)
                        return;


                    if(info.Contains("firstacc: No"))
                        KeyGay2.firstlogin = false;

                    set = info.Replace("\r", "").Replace("\n", "").Replace(Environment.NewLine, "").Split(';');

                    InternalConsolePrint("[STARTUP] Checking Maintence Status & Current version...", console_RichTextBox, Colors.Yellow);
                    AddText(lm.Translate("Checking Maintence status"));
                    Moveinitprogress(15);
                    await Task.Delay(50);


                    string porra = set[0];
                    if (porra.Contains("1"))
                    {
                        System.Windows.MessageBox.Show("Essence está em manutenção. Por favor, volte novamente mais tarde. Precisamos de um tempinho para aplicar mudanças...", "Nós também atualizamos né");
                        if (porra.Replace("1", "").Length > 2)
                        {
                            System.Windows.MessageBox.Show(porra.Replace("1", ""));
                        }
                        else
                        {
                            Process.GetCurrentProcess().Kill();
                        }
                    }


                    int ggs2 = DebugProtect2.PerformChecks();

                    try
                    {
                        AddText(lm.Translate("Checking Version"));
                        Moveinitprogress(25);
                        await Task.Delay(100);

                        bool need_upd = false;
                        var parts1 = ExecSettings.CurrentVersion.Split('.');
                        var parts2 = set[1].Split('.');
                        int maxLength = Math.Max(parts1.Length, parts2.Length);

                        for (int i = 0; i < maxLength; i++)
                        {
                            int v1 = i < parts1.Length ? int.Parse(parts1[i], CultureInfo.InvariantCulture) : 0;
                            int v2 = i < parts2.Length ? int.Parse(parts2[i], CultureInfo.InvariantCulture) : 0;

                            if (v1 < v2)
                            {
                                need_upd = true;
                            }
                        }


                        if (need_upd)
                        {
                            try
                            {
                                AddText(lm.Translate("Updating Version..."));
                                await Task.Delay(1000);
                                WebStuff2.DownloadFileCompleted += async delegate (object sender, AsyncCompletedEventArgs e)
                                {
                                    if (e.Error == null && !e.Cancelled)
                                    {
                                        try
                                        {
                                            Process.Start("EssenceUpdater.exe");
                                        }
                                        catch
                                        {
                                            System.Windows.MessageBox.Show(lm.Translate("Download failed. try to disable your antivirus software"), "Oops!");
                                        }

                                        MainWin.BeginAnimationP(UIElement.OpacityProperty, new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.4)));
                                        Process.GetCurrentProcess().Kill();
                                    }
                                    else
                                    {
                                        System.Windows.MessageBox.Show("Download Interrompido/Cancelado por motivos desconhecidos. Visite o site para baixar a versão mais recente.", "Oops");
                                        //Process.Start(set[5].Split(new string[3] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                                        MainWin.BeginAnimationP(UIElement.OpacityProperty, new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.4)));
                                        Process.GetCurrentProcess().Kill();
                                    }
                                };

                                await Task.Delay(1000);
                                //car999.Visibility = Visibility.Visible;


                                WebStuff2.DownloadFileAsync(new Uri(await KeyGay2.Get("upd")), "EssenceUpdater.exe");
                            }
                            catch (Exception ex)
                            {
                                //car999.Visibility = Visibility.Collapsed;

                                System.Windows.MessageBox.Show("Erro ao atualizar, Baixe o Essence pelo site. \r\n" + ex.Message);
                                //Process.Start(set[5].Split(new string[3] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                                MainWin.BeginAnimationP(UIElement.OpacityProperty, new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.4)));
                                Process.GetCurrentProcess().Kill();
                            }

                            return;
                        }
                    }
                    catch
                    {
                        //System.Windows.MessageBox.Show(ex.ToString());
                        System.Windows.MessageBox.Show(lm.Translate("We're updating our services. Please try again later."), "Aviso");
                        Process.GetCurrentProcess().Kill();
                    }

                    DebugProtect3.HideOSThreads();
                    if (StopAllInteractions)
                        return;


                    if (File.Exists("C:\\ProgramData\\ServiceConnect\\SInfo.lofh"))
                    {
                        try
                        {
                            string result = KeyGay2.GetB("C:\\ProgramData\\ServiceConnect\\SInfo.lofh");

                            if (!info.Contains(result) && result != "Erro" && result.Length >= 7)
                            {
                                File.Delete("C:\\ProgramData\\ServiceConnect\\SInfo.lofh");

                                CloseWriters();
                                RestartApp(false);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }

                        ShowBa(force: true);
                        return;
                    }

                    InternalConsolePrint("[STARTUP] Loading user data & configurations...", console_RichTextBox, Colors.Yellow);

                    DebugProtect3.HideOSThreads();
                    if (StopAllInteractions)
                        return;

                    AddText(lm.Translate("Loading User Data"));
                    Moveinitprogress(55);

                    if (!Directory.Exists("C:\\Essence"))
                        Directory.CreateDirectory("C:\\Essence");

                    if (!Directory.Exists("C:\\Essence\\bin"))
                        Directory.CreateDirectory("C:\\Essence\\bin");

                    if (!Directory.Exists("C:\\Essence\\userdata"))
                        Directory.CreateDirectory("C:\\Essence\\userdata");

                    if (!File.Exists("C:\\Essence\\userdata\\logindata.txt"))
                    {
                        List<string> linhas4 = new List<string>();
                        while (linhas4.Count < 6)
                        {
                            linhas4.Add("null");
                        }
                        File.WriteAllLines("C:\\Essence\\userdata\\logindata.txt", linhas4);
                        await Task.Delay(400);
                    }

                    await Task.Delay(50);
                    List<string> linhas = new List<string>(File.ReadAllLines("C:\\Essence\\userdata\\logindata.txt"));
                    while (linhas.Count < 6)
                    {
                        linhas.Add("null");
                    }
                    File.WriteAllLines("C:\\Essence\\userdata\\logindata.txt", linhas);

                    string[] names = registryKey1.GetValueNames();

                    if (!names.Contains("TopMost"))
                        registryKey1.SetValue("TopMost", "True");

                    if (!names.Contains("RPC"))
                        registryKey1.SetValue("RPC", "True");


                    if (registryKey1.GetValue("TopMost").ToString() == "False")
                    {
                        TopMButton.IsChecked = false;
                        Topmost = false;
                    }

                    if (registryKey1.GetValue("RPC").ToString() == "True")
                    {
                        RPCbutton.IsChecked = true;
                        DiscordRPC();
                    }

                    if (!File.Exists("C:\\Essence\\userdata\\GameHistory.txt"))
                    {
                        File.CreateText("C:\\Essence\\userdata\\GameHistory.txt");
                    }

                    if (!registryKey1.GetValueNames().Contains("Boot") || (registryKey1.GetValue("Boot").ToString() != "2" && registryKey1.GetValue("Boot").ToString() != "OK"))
                    {
                        registryKey1.SetValue("Boot", "0");
                        firstboot = true;
                    }

                    if (ggs2 == 1)
                    {
                        MessageBox.Show("Executor Could not load registry values", "ops");
                        Process.GetCurrentProcess().Kill();
                    }


                    bool open_changes = false;
                    if (!registryKey1.GetValueNames().Contains("Changelog"))
                    {
                        AddText(lm.Translate("Loading Changelog..."));
                        //open_changes = true;
                    }

                    if (StopAllInteractions)
                        return;

                    if (open_changes)
                    {
                        Fade(App.StartupWindow, 1, 0, 0.4);
                        Changelog chang = new Changelog();
                        chang.Show();

                        await Task.Delay(400);
                        App.StartupWindow.Hide();

                        while (!chang.ok)
                        {
                            await Task.Delay(100);
                        }
                        Fade(App.StartupWindow, 0, 1, 0.4);
                        App.StartupWindow.Show();
                        registryKey1.SetValue("Changelog", "OK");
                    }

                    if (StopAllInteractions)
                        return;

                    Topbar.Visibility = Visibility.Collapsed;

                    if (firstboot)
                    {
                        AddText(lm.Translate("Preparing Login Screen..."));
                        Moveinitprogress(100);

                        await Task.Delay(1000);
                        AnimateUI(welcome: true);
                        return;
                    }


                    ggs = DebugProtect1.PerformChecks();

                    AddText(lm.Translate("Getting Roblox Info"));
                    Moveinitprogress(75);

                    //InternalConsolePrint("Getting Roblox Information...", console_RichTextBox, Colors.Yellow);
                    string StorageFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Roblox\\LocalStorage\\appStorage.json");
                    if (File.Exists(StorageFile))
                    {
                        try
                        {
                            FileInfo fileInfo = new FileInfo(StorageFile);
                            using StreamReader sr = new StreamReader(fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                            var log = await sr.ReadLineAsync();
                            string text = log.Replace(@"\", "").Replace(",", "");


                            if (text.Contains("userId"))
                            {
                                RobloxPlayerID = PrintValue("userId", text);
                                roblox_look_finished = true;

                                linhas = new List<string>(File.ReadAllLines("C:\\Essence\\userdata\\logindata.txt"));
                                linhas[3] = RobloxPlayerID;
                                File.WriteAllLines("C:\\Essence\\userdata\\logindata.txt", linhas);

                                if (info.Contains(RobloxPlayerID))
                                {
                                    if (!KeyGay2.Roblox_IDS.Contains(RobloxPlayerID))
                                    {
                                        if (KeyGay2.Roblox_IDS.Length > 0)
                                            KeyGay2.Roblox_IDS += ", ";

                                        KeyGay2.Roblox_IDS += RobloxPlayerID;
                                    }

                                    ShowBa(RobloxPlayerID);
                                }
                            }
                        }
                        catch
                        {

                        }
                    }

                    if (StopAllInteractions)
                        return;

                    await Dispatcher.InvokeAsync(() =>
                    {
                        if (registryKey1.GetValueNames().Contains("Monaco"))
                        {
                            if (registryKey1.GetValue("Monaco").ToString() == "True")
                                selected_editor = 1;

                            if (registryKey1.GetValue("Monaco").ToString() == "False")
                            {
                                MonacoEnabled = false;
                                MonacoToggle.IsChecked = false;
                                selected_editor = 2;
                            }
                        }
                        else
                        {
                            registryKey1.SetValue("Monaco", "True");
                            selected_editor = 1;
                        }

                        if (Wolfregkey.GetValueNames().Contains("Avatar"))
                            Mudar_Imagem(Wolfregkey.GetValue("Avatar").ToString());
                        else
                        {
                            Mudar_Imagem("https://i.imgur.com/hxNqiz8.png");
                        }

                        if (Wolfregkey.GetValueNames().Contains("Nome"))
                            Mudar_Nome(Wolfregkey.GetValue("Nome").ToString());
                        else
                        {
                            Mudar_Nome("Chill Guy");
                        }
                    });





                    await Task.Delay(50);

                    if (ggs == 1)
                    {
                        Process.GetCurrentProcess().Kill();
                    }

                    //await Task.Delay(1100);
                    bool confirmada = false;
                    stopdrag = true;

                    //if (!offline_mode_enabled)
                    //{

                    AddText(lm.Translate("Checking Login"));
                    Moveinitprogress(85);

                    InternalConsolePrint("[STARTUP] Checking login & subscription...", console_RichTextBox, Colors.Yellow);

                    try
                    {
                        await Task.Run(async () =>
                        {
                            if (File.Exists("C:\\Essence\\userdata\\logindata.txt"))
                            {
                                List<string> linhasO = new List<string>(File.ReadAllLines("C:\\Essence\\userdata\\logindata.txt"));

                                if(linhasO[0].Length < 5 && linhasO[2].Length < 10)
                                {
                                    await Dispatcher.InvokeAsync(() => App.StartupWindow.Hide());
                                    try { File.Delete("C:\\Essence\\userdata\\logindata.txt"); } catch { }
                                    registryKey1.SetValue("Boot", "1");

                                    RestartApp(false);
                                    return;
                                }
                                                                
                                string response = await KeyGay2.Get("login", linhasO[0] + "\n" + linhasO[1] + "\n" + linhasO[2] + "\n" + Wolfregkey.GetValue("Nome").ToString() + "\n" + Wolfregkey.GetValue("Avatar").ToString());
                                try
                                {
                                    var userData = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
                                    if (userData != null && userData.ContainsKey("email"))
                                    {
                                        KeyGay2.current_user = linhasO[0];
                                        KeyGay2.current_pass = linhasO[1];

                                       

                                        //try
                                        //{
                                        //    if (userData["allowed"].ToString() == "false")
                                        //    {
                                        //        await Dispatcher.InvokeAsync(() => App.StartupWindow.Hide());
                                        //        MessageBox.Show("Opsie! Essence is in BETA tests now. And M4A1 didint allowed this login to be used. Dm him if you're subscribed in beta tests. If not, just wait for our next release!", "Oh, sorry :(");
                                        //        RestartApp();
                                        //    }
                                        //}
                                        //catch
                                        //{
                                        //    await Dispatcher.InvokeAsync(() => App.StartupWindow.Hide());
                                        //    MessageBox.Show("Opsie! Essence is in BETA tests now. And M4A1 didint allowed this login to be used. Dm him if you're subscribed in beta tests. If not, just wait for our next release!", "Oh, sorry :(");
                                        //    RestartApp();
                                        //}

                                        //MessageBox.Show(JsonConvert.SerializeObject(userData, Newtonsoft.Json.Formatting.Indented));

                                        try
                                        {
                                            await Dispatcher.InvokeAsync(() =>
                                            {
                                                ProfileSettings.dn32un3cr329c8n3.Text = linhasO[0];
                                                user_r = Convert.ToInt32(userData["user_role"].ToString());
                                                KeyGay2.invitecode = userData["userid"].ToString();
                                                KeyGay2.invites = Convert.ToInt32(userData["invites"].ToString());
                                                inviteeesss.Text = KeyGay2.invites.ToString();
                                                ProfileSettings.invitestxt.Text = KeyGay2.invites.ToString();

                                                ProfileSettings.invitationIDtxt.Text = KeyGay2.invitecode;

                                                if (user_r < 6)
                                                {
                                                    confirmada = true;
                                                    premium = true;
                                                    ProfileSettings.dudu3nud3nud.Text = "Yes";
                                                    ProfileSettings.subscription_typetxt.Text = "Premium";
                                                    OHHHHH = true;
                                                }

                                                switch (user_r)
                                                {
                                                    case 0:
                                                        userrolllee.Text = "Owner";
                                                        ProfileSettings.dn3u2ndu2d3n.Text = "Owner";
                                                        break;

                                                    case 1:
                                                        userrolllee.Text = "Developer";
                                                        ProfileSettings.dn3u2ndu2d3n.Text = "Developer";
                                                        break;

                                                    case 2:
                                                        userrolllee.Text = "Server Manager";
                                                        ProfileSettings.dn3u2ndu2d3n.Text = "Server Manager";
                                                        break;

                                                    case 3:
                                                        userrolllee.Text = "ADM";
                                                        ProfileSettings.dn3u2ndu2d3n.Text = "ADM";
                                                        break;

                                                    case 4:
                                                        userrolllee.Text = "Resellers";
                                                        ProfileSettings.dn3u2ndu2d3n.Text = "Resellers";
                                                        break;

                                                    case 5:
                                                        userrolllee.Text = "Close Friend";
                                                        ProfileSettings.dn3u2ndu2d3n.Text = "Clsoe Friend";
                                                        break;

                                                    case 6:
                                                        userrolllee.Text = "Beta Tester";
                                                        ProfileSettings.dn3u2ndu2d3n.Text = "Beta Tester";
                                                        break;

                                                    case 20:
                                                        userrolllee.Text = "User";
                                                        ProfileSettings.dn3u2ndu2d3n.Text = "User";
                                                        Dispatcher.InvokeAsync(() => App.StartupWindow.Hide());
                                                        MessageBox.Show("Opsie! Essence is in BETA tests now. And M4A1 didint allowed this login to be used. Dm him if you're subscribed in beta tests. If not, just wait for our next release!", "Oh, sorry :(");
                                                        RestartApp(true);
                                                        return;

                                                    case 69:
                                                        confirmada = true;
                                                        premium = true;
                                                        ProfileSettings.dudu3nud3nud.Text = "Yes";
                                                        ProfileSettings.subscription_typetxt.Text = "Premium";
                                                        OHHHHH = true;
                                                        userrolllee.Text = "Kayne West";
                                                        ProfileSettings.dn3u2ndu2d3n.Text = "Kayne West";
                                                        break;
                                                }

                                                JArray maquinasAutorizadas = JArray.Parse(userData["maquinas_autorizadas"].ToString());
                                                foreach (var maquina in maquinasAutorizadas)
                                                {
                                                    string hwid = maquina["hwid"].ToString();
                                                    var ultimoLogin = maquina["last_login"].ToString();
                                                    var lastlocation = maquina["last_location"].ToString();
                                                    var devicetyp = maquina["device"].ToString();

                                                    if (hwid == KeyGay2.GetHWID2())
                                                    {
                                                        ProfileSettings.dn3un3undu3nu.Text = lastlocation;

                                                        if(KeyGay2.pctypeshit == "Notebook")
                                                            ProfileSettings.dy3bdhb3db.Data = Geometry.Parse("M23 18h-1V5c0-1.1-.9-2-2-2H4c-1.1 0-2 .9-2 2v13H1c-.55 0-1 .45-1 1s.45 1 1 1h22c.55 0 1-.45 1-1s-.45-1-1-1m-9.5 0h-3c-.28 0-.5-.22-.5-.5s.22-.5.5-.5h3c.28 0 .5.22.5.5s-.22.5-.5.5m6.5-3H4V6c0-.55.45-1 1-1h14c.55 0 1 .45 1 1z");
                                                    }
                                                    else
                                                    {
                                                        Device lol = new Device();

                                                        DateTime lastLogin = DateTime.ParseExact(ultimoLogin, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                                        DateTime lastLoginUtc = lastLogin.AddHours(3);
                                                        TimeSpan localOffset = TimeZoneInfo.Local.BaseUtcOffset;
                                                        DateTime localTime = lastLoginUtc.Add(localOffset);
                                                        lol.locationn.Text = lastlocation + " · " + GetTimeAgo(localTime);

                                                        if(maquina["device"].ToString() == "Notebook")
                                                            lol.computertype.Data = Geometry.Parse("M23 18h-1V5c0-1.1-.9-2-2-2H4c-1.1 0-2 .9-2 2v13H1c-.55 0-1 .45-1 1s.45 1 1 1h22c.55 0 1-.45 1-1s-.45-1-1-1m-9.5 0h-3c-.28 0-.5-.22-.5-.5s.22-.5.5-.5h3c.28 0 .5.22.5.5s-.22.5-.5.5m6.5-3H4V6c0-.55.45-1 1-1h14c.55 0 1 .45 1 1z");


                                                        ProfileSettings.rapedevices.Children.Add(lol);
                                                    }
                                                }
                                            });

                                        }
                                        catch(Exception ex)
                                        {
                                            await Dispatcher.InvokeAsync(() => App.StartupWindow.Hide());
                                            MessageBox.Show(ex.ToString(), "Error When Loading User Account. Sorry :(");
                                            RestartApp(true);
                                            return;
                                        }

                                        await Task.Delay(100);

                                        await Dispatcher.InvokeAsync(() =>
                                        {
                                            AddText(lm.Translate("Checking Key..."));
                                        });

                                        await Task.Delay(100);

                                        if (!confirmada)
                                        {
                                            try
                                            {
                                                //PREMIUM KEY OK
                                                string logins = await KeyGay2.Get("check", userData["subscription"].ToString() + "\n" + linhasO[0]);

                                                if (logins.Split('\n').Length > 4)
                                                {
                                                    string[] data = logins.Split('\n');

                                                    try
                                                    {
                                                        await Dispatcher.InvokeAsync(() =>
                                                        {
                                                            ProfileSettings.key_expirationtxt.Text = data[4];
                                                            ProfileSettings.key_sellertxt.Text = data[2];
                                                            ProfileSettings.sellerservertxt.Text = data[3];
                                                            ProfileSettings.key_creation_date.Text = data[1];

                                                            premiumkey = userData["subscription"].ToString();
                                                            ProfileSettings.premiumkeytxt.Text = premiumkey.Substring(0, premiumkey.Length - 14) + "...";

                                                            stopdrag = false;
                                                            premium = true;
                                                            ProfileSettings.dudu3nud3nud.Text = "Yes";
                                                            ProfileSettings.subscription_typetxt.Text = "Premium";                                                            
                                                            confirmada = true;
                                                        });
                                                    }
                                                    catch
                                                    {

                                                    }
                                                }
                                                else if (logins == "outro")
                                                {
                                                    await Dispatcher.InvokeAsync(() => App.StartupWindow.Hide());
                                                    MessageBox.Show("This key has already been redeemed and does not belong to your account. If this is an error, please report it to your seller.", "Warning");
                                                    await Dispatcher.InvokeAsync(() => App.StartupWindow.Show());
                                                }

                                                //LINKVERTISE OK
                                                if (!confirmada && KeyGay2.TimeLeft("C:\\Essence\\userdata\\LinkvertiseKey.txt").TotalMilliseconds != 1)
                                                {
                                                    await Dispatcher.InvokeAsync(() =>
                                                    {
                                                        stopdrag = false;
                                                        confirmada = true;
                                                    });
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(ex.ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Erro no login. Detalhes: " + response);
                                        Process.GetCurrentProcess().Kill();
                                    }
                                }
                                catch (JsonException)
                                {
                                    switch (response)
                                    {
                                        //clear all login data and restart
                                        
                                        case "verification-pending":
                                            await Dispatcher.InvokeAsync(() => App.StartupWindow.Hide());
                                            MessageBox.Show("Your verification failed somehow. Please login again");
                                            break;

                                        case "new-location-detected":
                                            await Dispatcher.InvokeAsync(() => App.StartupWindow.Hide());
                                            MessageBox.Show("New location detected. Enter again in your account and we're going to send an code");
                                            break;

                                        case "multiple-machines-using-this-account":
                                            await Dispatcher.InvokeAsync(() => App.StartupWindow.Hide());
                                            MessageBox.Show("Other machine is aready using this account");
                                            break;

                                        case "incorect-password":
                                            await Dispatcher.InvokeAsync(() => App.StartupWindow.Hide());
                                            MessageBox.Show("It looks like your password has been changed. Please log back into your account.");
                                            break;

                                        case "user-not-found":
                                            await Dispatcher.InvokeAsync(() => App.StartupWindow.Hide());
                                            MessageBox.Show("Your login is no longer in our systems. Please create an new account.");
                                            break;

                                        default:
                                            await Dispatcher.InvokeAsync(() => App.StartupWindow.Hide());
                                            MessageBox.Show(response, "Internal Server Error. Try Again");
                                            break;
                                    }


                                    try { File.Delete("C:\\Essence\\userdata\\logindata.txt"); } catch { }
                                    registryKey1.SetValue("Boot", "1");


                                    RestartApp(false);
                                    return;
                                }
                                catch (Exception ex)
                                {
                                    await Dispatcher.InvokeAsync(() => App.StartupWindow.Hide());

                                    MessageBox.Show(ex.ToString());


                                    try { File.Delete("C:\\Essence\\userdata\\logindata.txt"); } catch { }
                                    registryKey1.SetValue("Boot", "1");


                                    RestartApp(false);
                                    return;
                                }
                            }
                            else
                            {
                                registryKey1.SetValue("Boot", "1");
                                RestartApp(false);
                                return;
                            }
                        });
                    }
                    catch
                    {
                        KeyGay2.current_user = "";
                        KeyGay2.current_pass = "";

                        try
                        {
                            List<string> linhas2 = new List<string>(File.ReadAllLines("C:\\Essence\\userdata\\logindata.txt"));
                            linhas2[0] = "null";
                            linhas2[1] = "null";
                            File.WriteAllLines("C:\\Essence\\userdata\\logindata.txt", linhas2);
                        }
                        catch
                        {
                            try { File.Delete("C:\\Essence\\userdata\\logindata.txt"); } catch { }
                        }
                    }
                    //}
                    //else
                    //{
                    //    stopdrag = false;
                    //    premium = true;
                    //    confirmada = true;
                    //    premiumkey = "M4A1_Debug";
                    //}
                    ggs2 = DebugProtect2.PerformChecks();


                    if (StopAllInteractions)
                        return;

                    if (!confirmada)
                    {
                        AddText(lm.Translate("Invalid Key!"));
                        await Task.Delay(500);

                        executor.Visibility = Visibility.Visible;
                        ExecutorGrid.Visibility = Visibility.Collapsed;
                        AnimateUI(true);

                        JustAnChillWindow justAnChillWindow = new JustAnChillWindow();
                        justAnChillWindow.Show();

                        Fade(justAnChillWindow, 0, 1, 0.6);

                        await Task.Delay(2000);
                        this.Hide();
                        return;
                    }
                    else
                    {
                        AddText(lm.Translate("Valid!"));
                        await Task.Delay(50);
                        AddText(lm.Translate("Loading Settings"));
                    }

                    if (ggs2 == 1)
                        Process.GetCurrentProcess().Kill();

                    inicializado2 = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                FatalError("inicializando2 - Faltal Error", ex.ToString());
                return;
            }

            await CheckForDependencies();

            try
            {
                stopdrag = false;

                //await ConfigurarFirewall();

                //while (!term)
                //{
                //    InvalidateVisual();
                //    await Task.Delay(100);
                //}

                if (!Directory.Exists("C:\\Essence"))
                {
                    try
                    {
                        Directory.CreateDirectory("C:\\Essence");
                    }
                    catch
                    {
                        System.Windows.MessageBox.Show("Não foi possível criar os diretórios automaticamente. Essence não vai carregar corretamente.");
                    }
                }

                if (!Directory.Exists("C:\\Essence\\Scripts"))
                {
                    try
                    {
                        Directory.CreateDirectory("C:\\Essence\\Scripts");
                    }
                    catch
                    {
                        System.Windows.MessageBox.Show("Não foi possível criar os diretórios automaticamente. Essence não vai carregar corretamente.");
                    }
                }

                PopulateScriptList("Scripts", "");

                if (registryKey1.GetValueNames().Contains("SaveTabs"))
                {
                    if (registryKey1.GetValue("SaveTabs").ToString() != "True")
                    {
                        SaveTabs = false;
                        SaveTabsToggle.IsChecked = false;
                    }
                }
                else
                    registryKey1.SetValue("SaveTabs", "True");

                if (registryKey1.GetValueNames().Contains("SaveH"))
                {
                    if (registryKey1.GetValue("SaveH").ToString() != "True")
                    {
                        SaveH = false;
                        SaveHistory.IsChecked = false;
                    }
                }
                else
                    registryKey1.SetValue("SaveH", "True");



                if (SaveH)
                {
                    AddText(lm.Translate("Loading AI Conversations"));
                    Moveinitprogress(90);
                    await Task.Delay(50);

                    try
                    {
                        string filePath = "C:\\Essence\\AI_history.txt";

                        if (File.Exists(filePath))
                        {
                            try
                            {
                                string[] lines = File.ReadAllLines(filePath);

                                string currentSpeaker = null;
                                List<string> currentDialog = new List<string>();
                                List<List<string>> dialogos = new List<List<string>>();

                                foreach (var line in lines)
                                {
                                    if (line.StartsWith("[USER]") || line.StartsWith("[MIA]"))
                                    {
                                        if (currentSpeaker != null && currentDialog.Count > 0)
                                        {
                                            dialogos.Add(new List<string> { currentSpeaker, string.Join(Environment.NewLine, currentDialog) });
                                        }

                                        currentSpeaker = line.Trim('[', ']');
                                        currentDialog.Clear();
                                    }
                                    else if (!string.IsNullOrWhiteSpace(line) && currentSpeaker != null)
                                    {
                                        currentDialog.Add(line.Trim());
                                    }
                                }

                                if (currentSpeaker != null && currentDialog.Count > 0)
                                {
                                    dialogos.Add(new List<string> { currentSpeaker, string.Join(Environment.NewLine, currentDialog) });
                                }

                                foreach (var dialogo in dialogos)
                                {
                                    try
                                    {
                                        if (dialogo[0] == "USER")
                                        {
                                            TextBox usertxt = USER_INPUT();
                                            usertxt.Text = dialogo[1];
                                        }
                                        else if (dialogo[0] == "MIA")
                                        {
                                            TextBox aitxtx = AI_RESPONSE();
                                            aitxtx.Text = dialogo[1];

                                            //if (dialogo[1].Contains("```lua"))
                                            //{
                                            //    aitxtx.Text = dialogo[1].Split(new string[] { "```lua" + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)[0].TrimStart().TrimEnd();

                                            //    last_ai_output =
                                            //        //"--[[\n" +
                                            //        //dialogo[1].Split(new string[] { "```lua" + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)[0].TrimStart() +
                                            //        //"]]" +
                                            //        //Environment.NewLine +
                                            //        //Environment.NewLine +
                                            //        dialogo[1].Split(new string[] { "```lua" + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)[1].Replace("```", "");
                                            //}
                                            //else
                                            //{
                                            //    aitxtx.Text = dialogo[1];
                                            //    aitxtx.Text = aitxtx.Text.Trim();
                                            //    aitxtx.Text = aitxtx.Text.Trim('\n');
                                            //}
                                        }
                                    }
                                    catch { }
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Erro ao processar o arquivo333: " + ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }

                Moveinitprogress(100);

                AddText(lm.Translate("Starting UI..."));

                if (StopAllInteractions)
                    return;


                if (premium && !OHHHHH)
                {
                    //if (premiumkey != "Essence lol2")
                    //{
                    string time = await KeyGay2.Get("KeyRes", premiumkey, force_return: true);

                    if (StopAllInteractions)
                        return;

                    if (time != "Perm" && time != "Erro")
                    {
                        TimeR(time.Split('.')[0]);
                    }
                    else if (time == "Perm")
                    {
                        key_expire2.Text = lm.Translate("Never");
                        ProfileSettings.key_expirationtxt.Text = lm.Translate("Lifetime");
                        TimeR(upd: false);
                    }
                    else
                    {
                        key_expire2.Text = lm.Translate("Error");
                        ProfileSettings.key_expirationtxt.Text = lm.Translate("Error");
                    }
                    //}
                    //else
                    //{
                    //    key_expire2.Text = lm.Translate("Never (Debug)");
                    //    AdminPanel.Visibility = Visibility.Visible;
                    //}


                    //email_key.Text = "Key:";
                    emailtxt.Text = premiumkey;
                }
                else if (OHHHHH)
                {
                    key_expire2.Text = lm.Translate("Never");
                    ProfileSettings.key_expirationtxt.Text = lm.Translate("Lifetime");
                    TimeR(upd: false);
                }
                else
                {
                    //email_key.Text = "Key:";
                    emailtxt.Text = lm.Translate("Linkvertise key");
                    ProfileSettings.key_expirationtxt.Text = lm.Translate("Linkvertise Key");

                    TimeR(upd: true);
                }

                if (StopAllInteractions)
                    return;

                ExecutionTasks();
            }
            catch (Exception ex)
            {
                FatalError("Após inicializando2", ex.ToString());
                return;
            }
            HeartBeat();

            //await Task.Delay(1500);


            //await Dispatcher.InvokeAsync(() =>
            //{
            //    DoubleAnimation go1 = new DoubleAnimation();
            //    go1.From = 50;
            //    go1.To = 100;
            //    go1.Duration = TimeSpan.FromSeconds(1);

            //    car999.BeginAnimationP(System.Windows.Controls.ProgressBar.ValueProperty, go1);
            //});

            animado = false;

            //try
            //{
            //    SoundPlayer player = new SoundPlayer(Properties.Resources.wolf2);
            //    player.Play();
            //}
            //catch { }

            //DoubleAnimation gradilolanim = new DoubleAnimation
            //{
            //    From = -1.5,
            //    To = 1,
            //    Duration = new Duration(TimeSpan.FromSeconds(0.5)),
            //    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            //};
            //B2c.BeginAnimationP(GradientStop.OffsetProperty, null);
            //B1c.BeginAnimationP(GradientStop.OffsetProperty, gradilolanim);
            //B1c.Color = Color.FromRgb(200, 20, 20);

            await Task.Delay(500);
            if (Wolfregkey.GetValueNames().Contains("Background"))
            {
                string k = Wolfregkey.GetValue("Background").ToString();
                if(k != "null")
                {
                    LoadBackg(k);
                }
            }
            AnimateUI();


            await Task.Delay(2200);

            EvxIco.Visibility = Visibility.Collapsed;
            EVXTXT.Visibility = Visibility.Collapsed;
            Status_Border.Visibility = Visibility.Collapsed;
            MinimizeBtn.Visibility = Visibility.Collapsed;
            MuteMusic.Visibility = Visibility.Collapsed;
            FullScreenBtn.Visibility = Visibility.Collapsed;
            CloseBtn.Visibility = Visibility.Collapsed;

            MusicPlayBorder2.Visibility = Visibility.Collapsed;
            Move(MusicPlayBorder2, MusicPlayBorder2.Margin, new Thickness(5, 0, 0, -90), 0.1);

            ernerinernd.Visibility = Visibility.Visible;


            //dwdwqdqnmdqw.Visibility = Visibility.Collapsed;
            //dwdwudw.Visibility = Visibility.Collapsed;
            //User_info_grid.Visibility = Visibility.Collapsed;
            //wuiadwuina.Visibility = Visibility.Collapsed;
            //dasadasdsdasa.Visibility = Visibility.Collapsed;
            //dwudhuwaduwad.Visibility = Visibility.Collapsed;


            SelectedMenuThing.Visibility = Visibility.Collapsed;
            HomeRadioButton.Visibility = Visibility.Collapsed;
            ExecutorRadioButton.Visibility = Visibility.Collapsed;
            HubRadioButton.Visibility = Visibility.Collapsed;
            ScriptsRadioButton.Visibility = Visibility.Collapsed;
            AIRadioButton.Visibility = Visibility.Collapsed;
            fggfwgwgw.Visibility = Visibility.Collapsed;

            lastplayedgrid2.Visibility = Visibility.Collapsed;
            bansgridd.Visibility = Visibility.Collapsed;
            playersgridd.Visibility = Visibility.Collapsed;
            totalusersgridd.Visibility = Visibility.Collapsed;
            invitesgriddd.Visibility = Visibility.Collapsed;

            GeneralScriptScrollViewer.Margin = new Thickness(10, 65, 5, 4);
            selectedscriptinfoborder.Visibility = Visibility.Collapsed;


            HomeGrid.Visibility = Visibility.Visible;
            //ExecutorGrid.Visibility = Visibility.Visible;

            //App.StartupWindow.RGBTime.Stop();


            Menu.Visibility = Visibility.Visible;

            ResizeMode = ResizeMode.CanResize;
            TabControl.Visibility = Visibility.Visible;

            StatusCheck();





            //ExecutorRadioButton.IsChecked = true;
            HomeRadioButton.IsChecked = true;

            LastMovedGrid = HomeGrid;


            CheckStatus();


            IntPtr consoleWindow = GetConsoleWindow();

            if (consoleWindow != IntPtr.Zero)
                AdminConsoleButton.IsChecked = true;



            //api2.AutoInject();

            //if (!cancelapiinit)
            //{
            //    Arka.EnableDebugging = false;

            //    await Task.Run(() =>
            //    {
            //        Arka.Init();
            //        apiinitialized = true;
            //    });
            //}
            //



            _mediaPlayer = new MediaPlayer();
            if (registryKey1.GetValue("Music").ToString() != "True")
            {
                _mediaPlayer.IsMuted = true;
                _isMuted = true;
                music_off.Visibility = Visibility.Visible;
                music_on.Visibility = Visibility.Collapsed;
            }
            MusicPlayer();
            //listening_users.Text = lm.Translate("Playlist Empty.");


            Thickness currentMargin = EvxIco.Margin;
            Move(EvxIco, new Thickness(currentMargin.Left, -100, currentMargin.Right, currentMargin.Bottom), new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), 1.3);
            await Task.Delay(80);
            EvxIco.Visibility = Visibility.Visible;


            currentMargin = EVXTXT.Margin;
            Move(EVXTXT, new Thickness(currentMargin.Left, -100, currentMargin.Right, currentMargin.Bottom), new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), 1.3);
            await Task.Delay(80);
            EVXTXT.Visibility = Visibility.Visible;


            currentMargin = Status_Border.Margin;
            Move(Status_Border, new Thickness(currentMargin.Left, -100, currentMargin.Right, currentMargin.Bottom), new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), 1.3);
            await Task.Delay(80);
            Status_Border.Visibility = Visibility.Visible;





            currentMargin = MuteMusic.Margin;
            Move(MuteMusic, new Thickness(currentMargin.Left, -100, currentMargin.Right, currentMargin.Bottom), new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), 1.3);
            await Task.Delay(80);
            MuteMusic.Visibility = Visibility.Visible;

            currentMargin = MinimizeBtn.Margin;
            Move(MinimizeBtn, new Thickness(currentMargin.Left, -100, currentMargin.Right, currentMargin.Bottom), new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), 1.3);
            await Task.Delay(80);
            MinimizeBtn.Visibility = Visibility.Visible;


            currentMargin = FullScreenBtn.Margin;
            Move(FullScreenBtn, new Thickness(currentMargin.Left, -100, currentMargin.Right, currentMargin.Bottom), new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), 1.3);
            await Task.Delay(80);
            FullScreenBtn.Visibility = Visibility.Visible;


            currentMargin = CloseBtn.Margin;
            Move(CloseBtn, new Thickness(currentMargin.Left, -100, currentMargin.Right, currentMargin.Bottom), new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), 1.3);
            await Task.Delay(80);
            CloseBtn.Visibility = Visibility.Visible;           



            //raintimer = new DispatcherTimer(DispatcherPriority.Background);
            //raintimer.Interval = TimeSpan.FromMilliseconds(300);
            //raintimer.Tick += Timer_Tick;
            //raintimer.Start();


            //currentMargin = dqwdndwq.Margin;
            //Move(dqwdndwq, new Thickness(-300, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), 1.3);
            //await Task.Delay(80);
            //dqwdndwq.Visibility = Visibility.Visible;

            await Task.Delay(300);

            //currentMargin = dwdwqdqnmdqw.Margin;
            //Move(dwdwqdqnmdqw, new Thickness(-300, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), 1.3);
            //await Task.Delay(80);
            //dwdwqdqnmdqw.Visibility = Visibility.Visible;


            AnimateEllipse(Ellipse1);
            AnimateEllipse(Ellipse2);
            AnimateEllipse(Ellipse3);

            UpdateLastPlayed();


            //currentMargin = dwdwudw.Margin;
            //Move(dwdwudw, new Thickness(-300, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), 1.3);
            //await Task.Delay(80);
            //dwdwudw.Visibility = Visibility.Visible;

            //await Task.Delay(300);


            //currentMargin = User_info_grid.Margin;
            //Move(User_info_grid, new Thickness(-300, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), 1.3);
            //await Task.Delay(80);
            //User_info_grid.Visibility = Visibility.Visible;


            //currentMargin = dasadasdsdasa.Margin;
            //Move(dasadasdsdasa, new Thickness(currentMargin.Left, currentMargin.Top, -300, currentMargin.Bottom), new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), 1.3);
            //await Task.Delay(80);
            //dasadasdsdasa.Visibility = Visibility.Visible;

            //currentMargin = dwudhuwaduwad.Margin;
            //Move(dwudhuwaduwad, new Thickness(currentMargin.Left, currentMargin.Top, -300, currentMargin.Bottom), new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), 1.3);
            //await Task.Delay(80);
            //dwudhuwaduwad.Visibility = Visibility.Visible;

            //currentMargin = wuiadwuina.Margin;
            //Move(wuiadwuina, new Thickness(currentMargin.Left, currentMargin.Top, -300, currentMargin.Bottom), new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), 1.3);
            //await Task.Delay(80);
            //wuiadwuina.Visibility = Visibility.Visible;

            DoubleAnimation djweduqwnu = new DoubleAnimation
            {
                From = 0,
                To = 150,
                Duration = new Duration(TimeSpan.FromSeconds(3.5)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn },
            };

            //wtfimdoingofmylife.BeginAnimation(DropShadowEffect.BlurRadiusProperty, djweduqwnu);


            //DispatcherTimer LOL = new DispatcherTimer(TimeSpan.FromMilliseconds(40), DispatcherPriority.Send, async delegate
            //{
            //    if (!ismoving && LastMovedGrid == HomeGrid && !areadyanimatinggrid)
            //    {
            //        try
            //        {
            //            wtfimdoingofmylife.Direction -= 5;
            //        }
            //        catch { }
            //    }
            //    else
            //        await Task.Delay(4000);
            //}, System.Windows.Application.Current.Dispatcher);
            //LOL.Start();



            currentMargin = HomeRadioButton.Margin;
            Move(HomeRadioButton, new Thickness(-100, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), 1.3);
            await Task.Delay(80);
            HomeRadioButton.Visibility = Visibility.Visible;




            currentMargin = ExecutorRadioButton.Margin;
            Move(ExecutorRadioButton, new Thickness(-100, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), 1.3);
            await Task.Delay(80);
            ExecutorRadioButton.Visibility = Visibility.Visible;


            currentMargin = HubRadioButton.Margin;
            Move(HubRadioButton, new Thickness(-100, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), 1.3);
            await Task.Delay(80);
            HubRadioButton.Visibility = Visibility.Visible;

            currentMargin = ScriptsRadioButton.Margin;
            Move(ScriptsRadioButton, new Thickness(-100, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), 1.3);
            await Task.Delay(80);
            ScriptsRadioButton.Visibility = Visibility.Visible;

            currentMargin = AIRadioButton.Margin;
            Move(AIRadioButton, new Thickness(-100, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), 1.3);
            await Task.Delay(80);
            AIRadioButton.Visibility = Visibility.Visible;

            currentMargin = fggfwgwgw.Margin;
            Move(fggfwgwgw, new Thickness(-100, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), 1.3);
            await Task.Delay(80);
            fggfwgwgw.Visibility = Visibility.Visible;

            currentMargin = SelectedMenuThing.Margin;
            Move(SelectedMenuThing, new Thickness(-100, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, currentMargin.Bottom), 1.3);
            await Task.Delay(80);
            SelectedMenuThing.Visibility = Visibility.Visible;

            //executor.MouseMove += Executor_MouseMove;
            //executor.DragEnter += MainWindow_DragEnter;
            //executor.DragOver += MainWindow_DragOver;
            //executor.Drop += MainWindow_Drop;





            MainWin.Width += 1;
            MainWin.Width -= 1;

            //PlayerName.Visibility = Visibility.Visible;
            PlayerImageHolder.Visibility = Visibility.Visible;


            Move_to_original();


            finish_start_animations = true;


            //string fi2lePath = "C:\\Essence\\BackgroundMusic.mp4";

            while (isSearching) { await Task.Delay(100); }
            double secondsElapsed2 = (DateTime.Now - timeX).TotalSeconds;
            inicializado = true;


            InternalConsolePrint("Executor Initialized", console_RichTextBox, Colors.Green);
            InternalConsolePrint($"STARTUP TOOK {secondsElapsed2:F2} SECCONDS", console_RichTextBox, Colors.Green);

            await Task.Delay(1500);

            Notificar($"Essence still in development. This is an BETA version. expect errors and bugs! Build {build_number}");
            //ShowBa(force: true);

            //string ppl_in_discord = await KeyGay2.Get("discordusers");

            //if(user_full_name != "" && !ppl_in_discord.Contains(user_full_name))

            if (!Wolfregkey.GetValueNames().Contains("JoinedServer"))
            {
                Wolfregkey.SetValue("JoinedServer", "Yes!");
                //Notificar("discord notification", 8, true);
            }


            //SetWindowOBSvisibility(1);


            try
            {
                var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(15));

                Random random = new Random();
                string lang_code = "en";

                if (lm.lang == 2)
                    lang_code = "br";

                var uri = new Uri($"https://M4A132-mixtral-46-7b-fastapi2.hf.space/init{lang_code}/");

                string status = premium ? "premium" : "standard";
                var item = new
                {
                    prompt = $"nome de usuário: {UserName2}\nstatus: {status}",
                    history = new List<(string, string)>(),
                    idioma = "en"
                };

                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("M4A1-KEY", KeyGay2.Gen()[0]);
                    client.DefaultRequestHeaders.Add("M4A1-N", KeyGay2.Gen()[1]);

                    string[] hh = KeyGay2.GetHWID();
                    client.DefaultRequestHeaders.Add("HWID-KEY", hh[0]);
                    client.DefaultRequestHeaders.Add("HWID-N", hh[1]);

                    using (SHA256 sha256 = SHA256.Create())
                    {
                        client.DefaultRequestHeaders.Add("Request-HASH", BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes("Nigger" + content))).Replace("-", "").ToLower());
                    }

                    var message = new HttpRequestMessage(HttpMethod.Post, uri);
                    message.Content = content;
                    var response = await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationTokenSource.Token);
                    var stream = await response.Content.ReadAsStreamAsync();

                    using (var reader = new StreamReader(stream))
                    {
                        await Task.Run(async () =>
                        {
                            string line;
                            string last_txt = "";

                            while ((line = await reader.ReadLineAsync()) != null)
                            {
                                string finalLine = line.Replace("</s>", "");
                                last_txt += finalLine;
                            }

                            await Dispatcher.InvokeAsync(async () =>
                            {
                                try
                                {
                                    last_txt = last_txt.Replace("\n", "").Replace("\r", "").Trim();
                                    int titleStart = last_txt.IndexOf("title:") + "title:".Length;
                                    titleeee = last_txt.Substring(titleStart, last_txt.IndexOf("subtitle:") - titleStart).Trim().Trim('"');

                                    subtitleeee = last_txt.Substring(last_txt.IndexOf("subtitle:") + "subtitle:".Length).Trim().Trim('"');
                                }
                                catch
                                {
                                    titleeee = "...";
                                    subtitleeee = "...";
                                }
                            });
                        });
                    }
                }

            }
            catch
            {
                titleeee = "...";
                subtitleeee = "...";
            }
        }


        Random _random = new Random();

        private async void AnimateEllipse(Ellipse ellipse)
        {
            while (true)
            {
                await Task.Delay(100);
                try
                {
                    if (!ismoving && LastMovedGrid == HomeGrid && !areadyanimatinggrid && !StopAllInteractions && IsActive)
                    {

                        double duration = _random.Next(1, 7);
                        double randomLeft = _random.Next(-70, 257);
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

        private SexyNotification SexyNotification;
        private ProfileSettings ProfileSettings = null;

        private void UpdateNotification(object sender, EventArgs e)
        {
            if (SexyNotification != null)
            {
                double mainWidth = this.Width;
                SexyNotification.Left = this.Left + (mainWidth - 400) / 2;
                SexyNotification.Top = this.Top + this.Height - 90;

                try { SexyNotification.WindowState = this.WindowState; } catch { }

            }
        }
        private void UpdateProfileSettings(object sender, EventArgs e)
        {
            if (ProfileSettings != null)
            {
                ProfileSettings.Width = this.Width;
                ProfileSettings.Height = this.Height-40;
                ProfileSettings.Left = this.Left;
                ProfileSettings.Top = this.Top + 40;
            }
        }

        private MediaPlayer _mediaPlayer;
        bool _isMuted = false;


        private bool duwhnudnqwdqwium = false;
        private bool diwendwmndikmwidmidwmiw = false;
        private async void MusicPlayBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            if (duwhnudnqwdqwium)
                return;

            duwhnudnqwdqwium = true;

            Move(musicinfowrap2, new Thickness(45, 0, 0, 0), new Thickness(45, 0, 0, 62), 0.2);
            await Task.Delay(50);
            Move(sigmagriddd2, new Thickness(45, 62, 0, 0), new Thickness(45, 0, 0, 0), 0.3);
            sigmagriddd2.Visibility = Visibility.Visible;
            await Task.Delay(300);

            while (MusicPlayBorder2.IsMouseOver || diwendwmndikmwidmidwmiw)
            {
                await Task.Delay(200);
            }

            Move(sigmagriddd2, new Thickness(45, 0, 0, 0), new Thickness(45, 62, 0, 0), 0.2);
            await Task.Delay(50);
            Move(musicinfowrap2, new Thickness(45, 0, 0, 62), new Thickness(45, 0, 0, 0), 0.3);

            await Task.Delay(300);
            duwhnudnqwdqwium = false;
        }

        private async void musiclike_Click(object sender, RoutedEventArgs e)
        {
            diwendwmndikmwidmidwmiw = true;
            musiclike.IsEnabled = false;
            musicdeslike.IsEnabled = false;


            cafeina_pra_neve = 40;
            speed_acelerator = 6;
            Fade(sigmagriddd2, 1, 0, 0.3);
            Fade(thankss, 0, 1, 0.6);
            thankss.Text = "Thankyou!";

            await KeyGay2.Get("musiclike", force_return: true);

            await Task.Delay(2000);
            Fade(thankss, 1, 0, 0.3);

            await Task.Delay(300);

            diwendwmndikmwidmidwmiw = false;
            MusicPlayBorder2.IsEnabled = false;
            cafeina_pra_neve = 4;
            speed_acelerator = 1;
            await Task.Delay(2000);
            Fade(sigmagriddd2, 0, 1, 0.3);
        }

        private async void musicdeslike_Click(object sender, RoutedEventArgs e)
        {
            diwendwmndikmwidmidwmiw = true;
            musicdeslike.IsEnabled = false;
            musiclike.IsEnabled = false;


            Fade(sigmagriddd2, 1, 0, 0.3);
            Fade(thankss, 0, 1, 0.6);
            thankss.Text = "Skip voted!";

            await KeyGay2.Get("musicdeslike", force_return: true);

            await Task.Delay(2000);
            Fade(thankss, 1, 0, 0.3);

            await Task.Delay(300);

            diwendwmndikmwidmidwmiw = false;
            MusicPlayBorder2.IsEnabled = false;
            await Task.Delay(2000);
            Fade(sigmagriddd2, 0, 1, 0.3);
        }


        string last_played_song = "";
        double cafeina_pra_neve = 1;
        bool isplaying_music = false;
        bool loading_song = false;

        private async Task FadeMusic(double duration, bool fadeIn)
        {
            if (_mediaPlayer == null)
                return;

            try
            {
                DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
                double steps = duration / timer.Interval.TotalSeconds;
                double volumeChange = 0.1 / steps;

                if (!fadeIn) volumeChange = -_mediaPlayer.Volume / steps;

                int currentStep = 0;
                _mediaPlayer.Volume = fadeIn ? 0 : _mediaPlayer.Volume;

                timer.Tick += (s, e) =>
                {
                    try
                    {
                        if (_mediaPlayer == null)
                        {
                            timer.Stop();
                            return;
                        }

                        if (currentStep >= steps)
                        {
                            _mediaPlayer.Volume = fadeIn ? 0.1 : 0;
                            timer.Stop();

                            if (!fadeIn && !loading_song)
                            {
                                isplaying_music = false;
                                cafeina_pra_neve = 1;
                                _mediaPlayer.Stop();
                                _mediaPlayer = null;
                            }
                        }
                        else
                        {
                            _mediaPlayer.Volume += volumeChange;
                            currentStep++;
                        }
                    }
                    catch
                    {
                        _mediaPlayer.Volume = fadeIn ? 0.1 : 0;
                    }
                };

                timer.Start();
            }
            catch
            {
                _mediaPlayer.Volume = fadeIn ? 0.1 : 0;
                //MessageBox.Show($"Erro ao {(fadeIn ? "aumentar" : "diminuir")} volume: {ex.Message}", fadeIn ? "IN" : "OUT");
            }
        }


        private async Task IntermissionMusic(bool fast = false)
        {
            if (_mediaPlayer == null && !loading_song)
                return;

            await Dispatcher.InvokeAsync(async () =>
            {
                MusicPlayBorder2.IsEnabled = false;
                cafeina_pra_neve = 1;
                try { _mediaPlayer.MediaEnded -= _mediaPlayer_MediaEnded; } catch { }

                try
                {
                    if (!fast)
                        await FadeMusic(3, false);
                    else
                        await FadeMusic(2, false);
                }
                catch { }

                try
                {
                    music_progress_b.BeginAnimation(WidthProperty, null);
                    music_progress_b.Width = 0;
                }
                catch { }

                try
                {
                    Move(MusicPlayBorder2, MusicPlayBorder2.Margin, new Thickness(5, 0, 0, -90), 0.7);
                    await Task.Delay(100);
                    Move(ernerinernd, ernerinernd.Margin, new Thickness(10, 0, 0, 0), 1.3);

                    await Task.Delay(1000);

                    PlayingMusicImage.Source = null;
                    gegergrggr.Visibility = Visibility.Visible;

                    PlayingMusicImage.Width = 50;
                    PlayingMusicImage.Height = 50;
                    Playingtitle.Text = lm.Translate("Intermission");
                    listening_users.Text = lm.Translate("Playlist Empty.");
                    suggested.Text = lm.Translate("Join our music event for free key!");
                }
                catch { }
            });
        }


        private async void LoadingMusicAn()
        {
            while (loading_song)
            {
                var animation2 = new DoubleAnimation
                {
                    From = 0,
                    To = 155,
                    Duration = TimeSpan.FromSeconds(1.7),
                };
                music_progress_b.BeginAnimation(WidthProperty, animation2);

                await Task.Delay(2000);
                if (!loading_song)
                    break;

                music_progress_b.HorizontalAlignment = HorizontalAlignment.Right;


                await Task.Delay(100);


                var animation3 = new DoubleAnimation
                {
                    From = 155,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(1.7),
                };
                music_progress_b.BeginAnimation(WidthProperty, animation3);

                await Task.Delay(2000);
                if (!loading_song)
                    break;


                music_progress_b.HorizontalAlignment = HorizontalAlignment.Left;
            }

            music_progress_b.HorizontalAlignment = HorizontalAlignment.Left;
        }

        private bool ffmpeg_ok = false;
        private async void MusicPlayer()
        {
            MusicPlayBorder2.IsEnabled = false;
            while (StopAllInteractions == false)
            {
                if (File.Exists("C:\\Essence\\bin\\ytdlp.exe"))
                {
                    try
                    {
                        string xxxy = xxx.Split(new string[] { "||||||||" }, StringSplitOptions.None)[2];
                        if (xxxy == "nothing")
                        {
                            await IntermissionMusic();
                        }
                        else if (xxxy.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries).Length == 7)
                        {
                            string name = xxxy.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries)[0];
                            string from = xxxy.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries)[1];
                            string song = xxxy.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries)[2];
                            string id = xxxy.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries)[3];
                            string users = xxxy.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries)[4];
                            string time = xxxy.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries)[5];
                            string duration = xxxy.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries)[6];

                            if (listening_users.Text != lm.Translate("Playlist Empty."))
                                Dispatcher.Invoke(() => listening_users.Text = lm.Translate("users listening: ") + users);


                            if (xxxy.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries)[3] != last_played_song && !loading_song)
                            {
                                loading_song = true;
                                var stopwatch = Stopwatch.StartNew();

                                try
                                {
                                    if (File.Exists($"C:\\Essence\\userdata\\Musics\\Music.mp4"))
                                        File.Delete($"C:\\Essence\\userdata\\Musics\\Music.mp4");

                                    //if (File.Exists($"C:\\Essence\\userdata\\Musics\\Music.mp3"))
                                    //    File.Delete($"C:\\Essence\\userdata\\Musics\\Music.mp3");

                                    //if (File.Exists($"C:\\Essence\\userdata\\Musics\\Music.webm"))
                                    //    File.Delete($"C:\\Essence\\userdata\\Musics\\Music.webm");                                
                                }
                                catch { }

                                MusicPlayBorder2.IsEnabled = false;
                                if (isplaying_music)
                                {
                                    try { _mediaPlayer.MediaEnded -= _mediaPlayer_MediaEnded; } catch { }

                                    music_progress_b.BeginAnimation(WidthProperty, null);
                                    music_progress_b.Width = 0;

                                    Playingtitle.Text = lm.Translate("Skipping music...");
                                    listening_users.Text = lm.Translate("Loading Next...");
                                    suggested.Text = lm.Translate("Join our music event for free key!");
                                    await FadeMusic(3, false);

                                }
                                else
                                {
                                    PlayingMusicImage.Source = null;
                                    gegergrggr.Visibility = Visibility.Visible;


                                    music_progress_b.BeginAnimation(WidthProperty, null);
                                    music_progress_b.Width = 0;

                                    PlayingMusicImage.Width = 50;
                                    PlayingMusicImage.Height = 50;
                                    Playingtitle.Text = lm.Translate("Loading Next..."); /*lm.Translate("Intermission");*/
                                    listening_users.Text = lm.Translate("Loading Next...");
                                    suggested.Text = lm.Translate("Join our music event for free key!");
                                }

                                if (LastMovedGrid != HomeGrid)
                                    Notificar("Loading Next Song: " + name);



                                Move(ernerinernd, ernerinernd.Margin, new Thickness(10, -80, 0, 0), 0.7);
                                await Task.Delay(100);
                                Move(MusicPlayBorder2, MusicPlayBorder2.Margin, new Thickness(5, 0, 0, 0), 1.3);
                                MusicPlayBorder2.Visibility = Visibility.Visible;


                                LoadingMusicAn();

                                //if (!File.Exists("C:\\Essence\\ffmpeg\\ffmpeg.exe"))
                                //{
                                //    if (Directory.Exists("C:\\Essence\\ffmpeg"))
                                //    {
                                //        Directory.Delete("C:\\Essence\\ffmpeg", true);
                                //    }

                                //    Directory.CreateDirectory("C:\\Essence\\ffmpeg");
                                //    await Task.Run(() =>
                                //    {
                                //        using (WebClient client8 = new WebClient())
                                //        {
                                //            AsyncCompletedEventHandler fileCompletedHandler = async (s, e) =>
                                //            {
                                //                Dispatcher.Invoke(() =>
                                //                {
                                //                    EVXTXT.Text = $"Essence";
                                //                });

                                //                if (e.Error == null)
                                //                {
                                //                    await Task.Delay(2000);

                                //                    ZipFile.ExtractToDirectory("C:\\Essence\\ffmpeg\\ffmpeg.zip", "C:\\Essence\\ffmpeg");
                                //                    System.IO.File.Delete("C:\\Essence\\ffmpeg\\ffmpeg.zip");
                                //                    ffmpeg_ok = true;
                                //                }
                                //                else
                                //                {
                                //                    await Task.Delay(2000);
                                //                    ffmpeg_ok = false;
                                //                    Directory.Delete("C:\\Essence\\ffmpeg");
                                //                }
                                //            };

                                //            DownloadProgressChangedEventHandler progressChangedHandler = async (s, e) =>
                                //            {
                                //                Dispatcher.Invoke(() =>
                                //                {
                                //                    EVXTXT.Text = $"Essence: Downloading ffmpeg [{e.ProgressPercentage}%]";
                                //                });
                                //            };

                                //            client8.DownloadProgressChanged += progressChangedHandler;
                                //            client8.DownloadFileCompleted += fileCompletedHandler;

                                //            client8.DownloadFileAsync(new Uri("https://github.com/ffbinaries/ffbinaries-prebuilt/releases/download/v6.1/ffmpeg-6.1-win-64.zip"), "C:\\Essence\\ffmpeg\\ffmpeg.zip");
                                //        }
                                //    });
                                //}
                                //else
                                //    ffmpeg_ok = true;


                                string extension;
                                string arguments;
                                //if (ffmpeg_ok)
                                //{
                                //    arguments = $@"""{song}"" -o ""C:\Essence\Musics\Music.mp3"" --extract-audio --audio-format mp3 --audio-quality 0 --ffmpeg-location ""C:\Essence\ffmpeg\ffmpeg.exe"" --no-check-certificate";
                                //    extension = "mp3";
                                //}
                                //else
                                //{
                                arguments = $@"""{song}"" -o ""C:\Essence\userdata\Musics\Music.mp4"" -f ""mp4"" --no-check-certificate";
                                extension = "mp4";
                                //}



                                Process process = new Process()
                                {
                                    StartInfo = new ProcessStartInfo()
                                    {
                                        FileName = @"C:\Essence\bin\ytdlp.exe",
                                        Arguments = arguments,
                                        RedirectStandardOutput = true,
                                        UseShellExecute = false,
                                        CreateNoWindow = true
                                    }
                                };

                                DataReceivedEventHandler handler = (DataReceivedEventHandler)(async (_, e) =>
                                {
                                    try
                                    {
                                        if (e.Data == null)
                                            return;

                                        string resp = e.Data.Trim();
                                        if (resp.Contains("Downloading webpage"))
                                        {
                                            Dispatcher.Invoke(() =>
                                            {
                                                listening_users.Text = lm.Translate("Loading Next...") + " [12,5%]";
                                                //RoundedPath.StrokeDashOffset = 218.75;
                                            });
                                        }
                                        else if (resp.Contains("Downloading ios player API JSON"))
                                        {
                                            Dispatcher.Invoke(() =>
                                            {
                                                listening_users.Text = lm.Translate("Loading Next...") + " [25%]";
                                                //RoundedPath.StrokeDashOffset = 187.5;                                            
                                            });
                                        }
                                        else if (resp.Contains("Downloading mweb player API JSON"))
                                        {
                                            Dispatcher.Invoke(() =>
                                            {
                                                listening_users.Text = lm.Translate("Loading Next...") + " [37,5%]";
                                                //RoundedPath.StrokeDashOffset = 156.25;
                                            });
                                        }
                                        else if (resp.Contains("[download]") && resp.Contains("of"))
                                        {
                                            try
                                            {
                                                string progress = resp.Split(new string[] { "[download]" }, StringSplitOptions.None)[1];
                                                progress = progress.Split(new string[] { "of" }, StringSplitOptions.RemoveEmptyEntries)[0];
                                                progress = progress.Replace(" ", "").Replace("[", "").Replace("]", "").Replace("%", "").Replace(".", ",");

                                                double numero = double.Parse(progress);
                                                int prrr = (int)numero;

                                                Dispatcher.Invoke(() =>
                                                {
                                                    listening_users.Text = lm.Translate("Loading Next...") + $" [{50 + (prrr / 2)}%]";
                                                    //RoundedPath.StrokeDashOffset = 125 - (prrr * 1.25);
                                                });
                                                //await Task.Delay(10);
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine($"Error parsing progress: {ex.Message}");
                                            }
                                        }
                                    }
                                    catch { }
                                });

                                await Task.Run(() =>
                                {
                                    process.Start();
                                    process.BeginOutputReadLine();
                                    process.OutputDataReceived += handler;
                                    process.WaitForExit();
                                });


                                await Task.Delay(700);

                                try
                                {
                                    gegergrggr.Visibility = Visibility.Collapsed;
                                    if (id != "")
                                    {
                                        PlayingMusicImage.Source = new BitmapImage(new Uri($"https://i.ytimg.com/vi/{id}/sddefault.jpg"));
                                        PlayingMusicImage.Width = 95;
                                        PlayingMusicImage.Height = 83;
                                    }
                                    else
                                    {
                                        PlayingMusicImage.Width = 50;
                                        PlayingMusicImage.Height = 50;
                                        PlayingMusicImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/ImageResources/essencetheme1.png"));
                                    }

                                    suggested.Text = lm.Translate("Suggested by: ") + from;
                                    listening_users.Text = lm.Translate("users listening: ") + users;
                                    MusicPlayBorder2.IsEnabled = true;
                                    musicdeslike.IsEnabled = true;
                                    musiclike.IsEnabled = true;

                                    isplaying_music = false;
                                    cafeina_pra_neve = 1;
                                    try { _mediaPlayer.Stop(); } catch { }
                                    _mediaPlayer = null;

                                    await Task.Delay(700);

                                    Playingtitle.Text = name;
                                    _mediaPlayer = new MediaPlayer();
                                    _mediaPlayer.MediaEnded += _mediaPlayer_MediaEnded;

                                    music_progress_b.BeginAnimation(WidthProperty, null);

                                    if (_isMuted)
                                        _mediaPlayer.IsMuted = true;
                                    else
                                        cafeina_pra_neve = 4;

                                    _mediaPlayer.Volume = 0;
                                    _mediaPlayer.Open(new Uri($"C:\\Essence\\userdata\\Musics\\Music.{extension}"));
                                    _mediaPlayer.Play();

                                    music_progress_b.Width = 0;
                                }
                                catch (Exception ex)
                                {
                                    //MessageBox.Show(ex.ToString(), "duwndiwnmdiwm");
                                }
                                loading_song = false;

                                await Task.Delay(700);

                                if (_mediaPlayer == null)
                                    throw new Exception("Oof");



                                try
                                {
                                    int durationInSeconds = int.Parse(duration);

                                    stopwatch.Stop();
                                    int timeElapsed = int.Parse(time) + (int)stopwatch.Elapsed.TotalSeconds;
                                    int timeRemaining = durationInSeconds - timeElapsed;
                                    double scale = (double)timeRemaining / (double)timeElapsed;
                                    //MessageBox.Show($"Proporção: {scale}");


                                    if (timeElapsed > 15)
                                    {
                                        _mediaPlayer.Position = TimeSpan.FromSeconds(timeElapsed);

                                        double erm = timeRemaining / (double)durationInSeconds;
                                        double eee = 155 - (155 * erm);

                                        if (eee < 0)
                                            eee = 0;

                                        var animation = new DoubleAnimation
                                        {
                                            From = eee,
                                            To = 155,
                                            Duration = TimeSpan.FromSeconds(timeRemaining + 10)
                                        };

                                        music_progress_b.BeginAnimation(WidthProperty, animation);
                                    }
                                    else
                                    {
                                        var animation = new DoubleAnimation
                                        {
                                            From = 0,
                                            To = 155,
                                            Duration = TimeSpan.FromSeconds(durationInSeconds)
                                        };

                                        music_progress_b.BeginAnimation(WidthProperty, animation);
                                    }


                                    await FadeMusic(4, true);
                                    last_played_song = id;
                                    isplaying_music = true;


                                    //await Task.Delay(4000);
                                    //_mediaPlayer.Volume = 1;
                                }
                                catch (Exception ex)
                                {
                                    //MessageBox.Show(ex.ToString(), "e298jeed2imdid");
                                    last_played_song = "1";
                                    loading_song = false;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        loading_song = false;
                        last_played_song = "1";
                        //MessageBox.Show(ex.ToString(), "wiedfimdnwamn");
                    }
                }
                await Task.Delay(1500);
            }
        }

        private async void _mediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            try
            {
                loading_song = false;
                //last_played_song = "1";
                isplaying_music = false;
                cafeina_pra_neve = 1;
                await Task.Delay(7000);
                IntermissionMusic(true);
            }
            catch { }
        }

        private bool StopAllInteractions = false;
        private async void ShowBa(string id = "", bool force = false)
        {
            if (!force)
            {
                while (!roblox_look_finished) { await Task.Delay(100); }
            }

            StopAllInteractions = true;
            await Dispatcher.InvokeAsync(async () =>
            {
                if (App.StartupWindow.WhatIsThis.Visibility == Visibility.Visible)
                    return;

                App.StartupWindow.Visibility = Visibility.Visible;
                App.StartupWindow.shitty2.Visibility = Visibility.Collapsed;
                App.StartupWindow.MainWin.IsHitTestVisible = true;
                App.StartupWindow.MainWin.BeginAnimationP(UIElement.OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.4)));

                if (!force)
                    try { await KeyGay2.Ban(id); } catch { }

                await Task.Delay(3000);
                App.StartupWindow.opsie();
                prevent_closing = true;
                MainWin.Close();
            });
        }

        private async void ReturnFromGame_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (areadyanimatinggrid == true)
                return;

            areadyanimatinggrid = true;

            ThicknessAnimation j = new ThicknessAnimation()
            {
                To = new Thickness(10, 65, 5, 4),
                Duration = TimeSpan.FromMilliseconds(100)
            };
            GeneralScriptScrollViewer.BeginAnimationP(MarginProperty, j);
            selectedscriptinfoborder.Visibility = Visibility.Collapsed;

            Move(GameselectedScroll, GameselectedScroll.Margin, new Thickness(MainWin.ActualWidth + 150, 0, 0, 4), 0.9);
            Fade(GameselectedScroll, 1, 0, 0.8);

            Fade(skibidtoilet, 0, 1, 0.8);



            await Task.Delay(100);

            Move(GeneralScriptScrollViewer, new Thickness(MainWin.ActualWidth + 150, 75, 0, 4), new Thickness(0, 75, 0, 4), 1.4);
            Fade(GeneralScriptScrollViewer, 0, 1, 1.7);
            GeneralScriptScrollViewer.Visibility = Visibility.Visible;

            await Task.Delay(600);
            GameselectedScroll.Visibility = Visibility.Collapsed;
            areadyanimatinggrid = false;
        }



        static async Task<string> RobloxAutoComplete(string query)
        {
            string url = $"https://apis.roblox.com/games-autocomplete/v1/get-suggestion/{query}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();

                        try
                        {
                            var json = JObject.Parse(responseData);
                            var entradas = json["entries"] ?? new JArray();

                            if (entradas.Count() == 0)
                            {
                                return "None";
                            }

                            return entradas[0]["searchQuery"]?.ToString() ?? "None";
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao processar os dados: {ex.Message}");
                            return "None";
                        }
                    }
                    else
                    {
                        return $"Erro: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao realizar a solicitação: {ex.Message}");
                    return "None";
                }
            }
        }

        private async Task<ScriptObject[]> GetScriptsFromUrl(int i, string query, int max = 999, bool game = false)
        {
            JToken jtoken;
            HttpClient client = new HttpClient();

            string parse = "";
            await Task.Run(async () =>
            {
                string link = "";
                if (query == "")
                {
                    link = "https://scriptblox.com/api/script/fetch?page=" + i.ToString();
                    await Dispatcher.InvokeAsync(() => NewScriptsTXT.Text = lm.Translate("New Scripts"));
                }
                else
                {
                    link = $"https://scriptblox.com/api/script/search?filters=free&page={i}&q={query}&max={max}";
                    if (!game)
                        await Dispatcher.InvokeAsync(() => NewScriptsTXT.Text = lm.Translate("Filtering results..."));
                }
                parse = await (await client.GetAsync(link)).Content.ReadAsStringAsync();
            });

            jtoken = JToken.Parse(parse)[(object)"result"];
            return JsonConvert.DeserializeObject<ScriptObject[]>(jtoken[(object)"scripts"].ToString());
        }


        private ScriptObject scriptttttttttttttt = null;
        Dictionary<string, int> rattinggg = new Dictionary<string, int>();
        private async void PopulateScripts(ScriptObject scriptObject1, WrapPanel WP, int position = 0)
        {
            ScriptObject scriptObject = scriptObject1;
            ScriptThingy obj = new ScriptThingy(scriptObject);

            obj.edfefdefd.PreviewMouseDown += ((sen, e) =>
            {
                try
                {
                    if (scriptttttttttttttt != null && (scriptttttttttttttt == scriptObject || scriptttttttttttttt.slug == scriptObject.slug))
                    {
                        dbudbj23dj = false;

                        ThicknessAnimation jk = new ThicknessAnimation()
                        {
                            To = new Thickness(10, 65, 5, 4),
                            Duration = TimeSpan.FromMilliseconds(100)
                        };
                        GeneralScriptScrollViewer.BeginAnimationP(MarginProperty, jk);
                        selectedscriptinfoborder.Visibility = Visibility.Collapsed;
                        scriptttttttttttttt = null;
                        return;
                    }
                }
                catch { }

                try
                {

                    scriptttttttttttttt = scriptObject;

                    selectedscriptupdated.Text = "Created " + GetTimeAgo(scriptObject.createdAt);
                    selectedscriptviews.Text = scriptObject.views + " Views";
                    selectedscripttitle.Text = scriptObject.title;
                    selectedscriptdescription.Text = scriptObject.features;

                    likeeefef.Fill = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                    dislikeshit.Fill = new SolidColorBrush(Color.FromRgb(200, 200, 200));

                    string message = "Ratting:\n";
                    foreach (var item in rattinggg)
                    {
                        message += $"{item.Key}: {item.Value}\n";
                    }

                    if (rattinggg.ContainsKey(scriptttttttttttttt.slug))
                    {
                        if (rattinggg[scriptttttttttttttt.slug] == 1)
                            dislikeshit.Fill = new SolidColorBrush(Color.FromRgb(200, 20, 20));
                        else
                            likeeefef.Fill = new SolidColorBrush(Color.FromRgb(200, 20, 20));
                    }

                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(scriptObject.game.imageUrl);
                    bitmapImage.EndInit();
                    selectedscriptimage.Background = new ImageBrush()
                    {
                        ImageSource = (ImageSource)bitmapImage,
                        Opacity = 0.65,
                        Stretch = Stretch.UniformToFill
                    };

                    dbudbj23dj = true;
                    ThicknessAnimation j = new ThicknessAnimation()
                    {
                        To = new Thickness(0, 65, 260, 4),
                        Duration = TimeSpan.FromMilliseconds(200)
                    };
                    GeneralScriptScrollViewer.BeginAnimationP(MarginProperty, j);

                    ThicknessAnimation jr = new ThicknessAnimation()
                    {
                        To = new Thickness(0, 0, 260, 4),
                        Duration = TimeSpan.FromMilliseconds(200)
                    };
                    GameselectedScroll.BeginAnimationP(MarginProperty, jr);

                    DoubleAnimation k = new DoubleAnimation()
                    {
                        From = 0,
                        To = 1,
                        Duration = TimeSpan.FromMilliseconds(600)
                    };

                    ThicknessAnimation j1 = new ThicknessAnimation()
                    {
                        From = new Thickness(10, 90, 5, 4),
                        To = GameselectedScroll.Visibility == Visibility.Visible ? new Thickness(10, 0, 5, 4) : new Thickness(10, 65, 5, 4),
                        Duration = TimeSpan.FromMilliseconds(200)
                    };


                    selectedscriptinfoborder.BeginAnimationP(MarginProperty, j1);
                    selectedscriptinfoborder.BeginAnimationP(OpacityProperty, k);
                    selectedscriptinfoborder.Visibility = Visibility.Visible;

                    try
                    {
                        selectedscriptrating.Text = "Rating: Loading...";
                        Task.Run(async () =>
                        {
                            await Dispatcher.InvokeAsync(async () =>
                            {
                                string kkk = await KeyGay2.Get("scriptrating", scriptttttttttttttt.slug, force_return: true);
                                try
                                {
                                    if (kkk != "null")
                                        selectedscriptrating.Text = $"Rating {kkk}";
                                    else
                                        selectedscriptrating.Text = "Rating: No rates yet here";
                                }
                                catch { }
                            });
                        });
                    }
                    catch { }
                }
                catch { }
            });

            if(position != 0)
                obj.dndiadiadn.Text = position.ToString();            
            else
                obj.akdaidjaidjaiodasd.Visibility = Visibility.Collapsed;

            WP.Children.Add((UIElement)obj);
        }


        private async void ExecuteSelectedScr_Click(object sender, RoutedEventArgs e)
        {
            string script = await scriptttttttttttttt.GetScript();
            EXECUTAR(script);
        }

        private async void CopySelectedScr_Click(object sender, RoutedEventArgs e)
        {
            string script = await scriptttttttttttttt.GetScript();
            Clipboard.SetText(script);
        }

        private async void OpenInEditorSrc_Click(object sender, RoutedEventArgs e)
        {
            AnimateGrid(ExecutorGrid, new Thickness(0, 60, 0, 0), ExecutorRadioButton);
            ExecutorRadioButton.IsChecked = true;
            await NewTabAsync(await scriptttttttttttttt.GetScript(), scriptttttttttttttt.title);
        }





        private async void AdvancedSearch(string placeid, string gamename, string query = "")
        {
            List<string> added = new List<string>();

            int srccount = 0;
            int srccount2 = 0;

            async Task ProcessScriptsFromUrl(int page, string searchQuery)
            {
                foreach (ScriptObject script in await GetScriptsFromUrl(page, searchQuery, 999, true))
                {
                    searchQuery = searchQuery.ToLower();
                    string scriptGameName = script.game.name.ToLower();
                    bool isUniversal = scriptGameName.Contains("universal script");
                    bool isRelevant = scriptGameName.Contains(searchQuery) || searchQuery.Contains(scriptGameName);

                    //mesmo jogo, mesma versão
                    if (((placeid == script.game.gameId && scriptGameName == searchQuery) || isUniversal) && !added.Contains(script._id))
                    {
                        PopulateScripts(script, WPXD);
                        added.Add(script._id);
                        srccount++;
                        dwuduqwn2.Text = lm.Translate("Scripts for this game version: ") + srccount;
                        dwuduqwn2.Visibility = Visibility.Visible;
                        await Task.Delay(70);
                    }
                    //mesmo jogo, versão diferente
                    else if ((placeid == script.game.gameId && isRelevant) && !added.Contains(script._id))
                    {
                        PopulateScripts(script, WPXD2);
                        added.Add(script._id);
                        srccount2++;
                        dwuduqwn3.Text = lm.Translate("Scripts for past versions of this game: ") + srccount2;
                        dwuduqwn3.Visibility = Visibility.Visible;
                        await Task.Delay(70);
                    }
                    dwuduqwn.Text = lm.Translate("Scripts for this game: ") + (srccount + srccount2).ToString();
                }
            }

            for (int i = 1; i <= 3; i++)
            {
                //ConsolePrint($"Getting related scripts for {gamename}... {i}", ConsoleColor.Yellow);
                await ProcessScriptsFromUrl(i, gamename);

                if (query != "")
                    await ProcessScriptsFromUrl(i, query);
            }

            if (srccount < 5 && gamename.Length > 8)
            {
                dwuduqwn2.Text = lm.Translate("Deep Searching...");
                for (int i = 1; i <= 3; i++)
                {
                    //ConsolePrint($"Getting related scripts for {gamename} (Deep search)... {i}", ConsoleColor.Yellow);
                    await ProcessScriptsFromUrl(i, gamename.Substring(1, gamename.Length - 6));
                    ProcessScriptsFromUrl(i, gamename.Substring(2, gamename.Length - 6));
                    ProcessScriptsFromUrl(i, gamename.Substring(3, gamename.Length - 6));
                    ProcessScriptsFromUrl(i, gamename.Substring(4, gamename.Length - 6));
                    await ProcessScriptsFromUrl(i, gamename.Substring(5, gamename.Length - 6));
                }
                await Task.Delay(200);
                if (WPXD.Children.Count < 1 && WPXD2.Children.Count < 1)
                {
                    dwuduqwn2.Text = lm.Translate("0 Results found for this game. :(");
                    dwuduqwn.Text = lm.Translate("Scripts for this game: ") + "0";
                }
            }

            djwndnqim.Text = gamename;
        }

        private int max_gozo = 0;
        private async void GeneralScriptScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                if (e.Delta < 0)
                {
                    if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight)
                    {
                        e.Handled = true;
                        max_gozo++;

                        if (max_gozo == 20)
                        {
                            pages++;
                            await SearchScripts2("", true, pages + 1);
                            max_gozo = 0;
                        }
                    }
                }
            }
        }



        private async Task<(List<ScriptObject> scripts, (int playerCount, string creatorName, string gameName, string imageUrl, string PlaceID) gameInfo)>FetchScriptsAndGameInfoAsync(int page, string query, bool skipGameInfo)
        {
            return await Task.Run(async () =>
            {
                var getScriptsTask = GetScriptsFromUrl(page, query);
                var getGameInfoTask = GetRobloxGameInfo(LOL: query, skip: skipGameInfo);

                await Task.WhenAll(getScriptsTask, getGameInfoTask);

                var scripts = await getScriptsTask;
                var gameInfo = await getGameInfoTask;

                return (scripts.ToList(), gameInfo);
            });
        }

        public class ObjectScore
        {
            public string ObjectId { get; set; }
            public int Score { get; set; }
        }

        private bool isSearching = false;
        private string selectedgameurl = "";
        private string lastsearched = "";
        private int pages = 0;
        private bool dddsdasdasd;
        private async Task SearchScripts2(string query = "", bool add_mode = false, int start = 1)
        {
            //Console.WriteLine("SearchScripts2");

            lastsearched = query;
            query = query.ToLower();
            //if (isSearching)
            //return;

            isSearching = true;
            //GeneralScriptSearch3.IsEnabled = false;

            //ConsolePrint("[SCRIPT HUB] Searching for Scripts...", ConsoleColor.Yellow);

            if (!Directory.Exists("C:\\Essence\\userdata\\Saves"))
            {
                Directory.CreateDirectory("C:\\Essence\\userdata\\Saves");
            }

            Not_Found.Visibility = Visibility.Collapsed;


            int ii = start + 3;
            if (MaximumExecutorQuality)
                ii = start + 4;


            bool savesok = false;
            bool ownersok = false;
            bool gamesok = false;
            bool trendingok = false;

            WP6.Children.Clear();
            WP6.Visibility = Visibility.Collapsed;
            jfdiafdiasjdi.Visibility = Visibility.Collapsed;

            if (!add_mode)
            {
                pages = 0;
                WP.Children.Clear();
                WP.Visibility = Visibility.Collapsed;
                NewScriptsTXT.Visibility = Visibility.Collapsed;

                WP0.Children.Clear();
                WP0.Visibility = Visibility.Collapsed;
                NewScripts2TXT.Visibility = Visibility.Collapsed;




                WP2.Children.Clear();
                WP2.Visibility = Visibility.Collapsed;
                CommunityScriptsTXT.Visibility = Visibility.Collapsed;

                WP3.Children.Clear();
                WP3.Visibility = Visibility.Collapsed;
                GamesScriptsTXT.Visibility = Visibility.Collapsed;
                games_no_data_warn.Visibility = Visibility.Collapsed;

                WP4.Children.Clear();
                WP4.Visibility = Visibility.Collapsed;
                SavedScriptsTXT.Visibility = Visibility.Collapsed;


                selectedgameurl = "";
                dwuduqwn.Text = lm.Translate("Scripts for this game: Loading...");
                dwqnduwnq.Text = lm.Translate("Players online: Loading...");
            }
            else
            {
                savesok = true;
                ownersok = true;
                gamesok = true;
            }


            //< TextBlock Text = "Loading..."  FontFamily = "Bahnschrift" Foreground = "#DD9B9B9B" FontSize = "13" />

            int exact_results = 0;

            try
            {
                for (int i = start; i < ii; i++)
                {
                    var (scripts, result) = await FetchScriptsAndGameInfoAsync(i, query, gamesok);

                    if (!gamesok)
                    {
                        gamesok = true;
                        if (query == "")
                        {
                            List<string> games = new List<string>(File.ReadAllLines("C:\\Essence\\userdata\\GameHistory.txt"));
                            foreach (string line in games)
                            {
                                string[] datar = line.Split(new string[] { " & " }, StringSplitOptions.None);

                                GameThingy obj = new GameThingy(
                                    datar[1],
                                    datar[2],
                                    datar[0]
                                );

                                //obj.ScriptTitle.Text = datar[1].Length > 16 ? datar[1].Substring(0, 16) + "..." : datar[1];
                                obj.ScriptTitle.Text = datar[1];

                                ((ButtonBase)obj.FindName("CoolBTN")).Click += (RoutedEventHandler)(async (sender, e) =>
                                {
                                    if (areadyanimatinggrid == true)
                                        return;

                                    areadyanimatinggrid = true;
                                    WPXD.Children.Clear();
                                    WPXD2.Children.Clear();
                                    dwuduqwn2.Text = lm.Translate("Searching...");
                                    dwuduqwn3.Visibility = Visibility.Collapsed;

                                    try
                                    {
                                        this.BorderImg.Background = obj.BorderImg.Background;
                                        djwndnqim.Text = datar[1] + lm.Translate(" (Searching Scripts...)");
                                        selectedgameurl = datar[0];
                                    }
                                    catch { }

                                    Move(GeneralScriptScrollViewer, GeneralScriptScrollViewer.Margin, new Thickness(MainWin.ActualWidth + 150, 75, 0, 4), 0.9);
                                    Fade(GeneralScriptScrollViewer, 1, 0, 0.8);
                                    Fade(skibidtoilet, 1, 0, 0.8);

                                    await Task.Delay(100);

                                    Move(GameselectedScroll, new Thickness(MainWin.ActualWidth + 150, 0, 0, 4), new Thickness(0, 0, 0, 4), 1.4);
                                    Fade(GameselectedScroll, 0, 1, 1.7);
                                    GameselectedScroll.Visibility = Visibility.Visible;

                                    ThicknessAnimation j = new ThicknessAnimation()
                                    {
                                        To = new Thickness(10, 65, 5, 4),
                                        Duration = TimeSpan.FromMilliseconds(100)
                                    };
                                    GeneralScriptScrollViewer.BeginAnimationP(MarginProperty, j);
                                    selectedscriptinfoborder.Visibility = Visibility.Collapsed;

                                    await Task.Delay(1400);
                                    areadyanimatinggrid = false;

                                    AdvancedSearch(datar[0], datar[1]);

                                    var data = await GetRobloxGameInfo(placeid: long.Parse(datar[0]));
                                    dwqnduwnq.Text = lm.Translate("Players online: ") + ConverterParaStringCompacta(data.playerCount);
                                });

                                WP3.Visibility = Visibility.Visible;
                                WP3.Children.Add(obj);

                                if (!dddsdasdasd)
                                {
                                    dddsdasdasd = true;

                                    DoubleAnimation e = new DoubleAnimation()
                                    {
                                        To = 250,
                                        Duration = TimeSpan.FromMilliseconds(300)
                                    };
                                    ThicknessAnimation e2 = new ThicknessAnimation()
                                    {
                                        To = new Thickness(0, 0, 5, 5),
                                        Duration = TimeSpan.FromMilliseconds(300)
                                    };

                                    obj.MainGrid.BeginAnimation(WidthProperty, e);
                                    obj.ScriptTitle.BeginAnimation(MarginProperty, e2);
                                }
                            }
                        }
                        else
                        {
                            //verifica se tem relação com o nome do jogo ou com o autor antes de exibir
                            if (query.Contains(result.gameName.ToLower()) || result.gameName.ToLower().Contains(query) || query.Contains(result.creatorName.ToLower()) || result.creatorName.ToLower().Contains(query))
                            {
                                GameThingy obj = new GameThingy(
                                    result.gameName,
                                    result.imageUrl,
                                    result.PlaceID
                                );

                                obj.MainGrid.Width = 250;
                                obj.CoolBTN.Width = 260;

                                //obj.ScriptTitle.Text = result.gameName.Length > 30 ? result.gameName.Substring(0, 30) + "..." : result.gameName;
                                obj.ScriptTitle.Text = result.gameName;

                                var storyboard = (Storyboard)obj.FindResource("MouseEnterStoryboard");
                                var widthAnimation = storyboard.Children[0] as DoubleAnimation;
                                widthAnimation.To = 260;


                                var storyboard2 = (Storyboard)obj.FindResource("MouseLeaveStoryboard");
                                var widthAnimation2 = storyboard2.Children[0] as DoubleAnimation;
                                widthAnimation2.To = 250;

                                ((ButtonBase)obj.FindName("CoolBTN")).Click += (RoutedEventHandler)(async (sender, e) =>
                                {
                                    if (areadyanimatinggrid == true)
                                        return;

                                    areadyanimatinggrid = true;
                                    WPXD.Children.Clear();
                                    WPXD2.Children.Clear();
                                    dwuduqwn2.Text = lm.Translate(" (Searching Scripts...)");
                                    dwuduqwn3.Visibility = Visibility.Collapsed;

                                    try
                                    {
                                        this.BorderImg.Background = obj.BorderImg.Background;
                                        djwndnqim.Text = result.gameName + lm.Translate("(loading...)");
                                        selectedgameurl = result.PlaceID;
                                        dwqnduwnq.Text = "Players online: " + ConverterParaStringCompacta(result.playerCount);
                                    }
                                    catch { }

                                    Move(GeneralScriptScrollViewer, GeneralScriptScrollViewer.Margin, new Thickness(MainWin.ActualWidth + 150, 75, 0, 4), 0.9);
                                    Fade(GeneralScriptScrollViewer, 1, 0, 0.8);
                                    Fade(skibidtoilet, 1, 0, 0.8);

                                    await Task.Delay(100);

                                    Move(GameselectedScroll, new Thickness(MainWin.ActualWidth + 150, 0, 0, 4), new Thickness(0, 0, 0, 4), 1.4);
                                    Fade(GameselectedScroll, 0, 1, 1.7);
                                    GameselectedScroll.Visibility = Visibility.Visible;

                                    ThicknessAnimation j = new ThicknessAnimation()
                                    {
                                        To = new Thickness(10, 65, 5, 4),
                                        Duration = TimeSpan.FromMilliseconds(100)
                                    };
                                    GeneralScriptScrollViewer.BeginAnimationP(MarginProperty, j);
                                    selectedscriptinfoborder.Visibility = Visibility.Collapsed;

                                    await Task.Delay(1400);
                                    areadyanimatinggrid = false;

                                    AdvancedSearch(result.PlaceID, result.gameName, query);
                                });

                                WP3.Visibility = Visibility.Visible;
                                WP3.Children.Add(obj);
                            }
                        }

                        if (WP3.Children.Count == 0)
                        {
                            if (query == "")
                            {
                                GamesScriptsTXT.Visibility = Visibility.Visible;
                                games_no_data_warn.Visibility = Visibility.Visible;
                                GamesScriptsTXT.Text = lm.Translate("Games you play");
                                WP3.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                GamesScriptsTXT.Visibility = Visibility.Collapsed;
                                games_no_data_warn.Visibility = Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            GamesScriptsTXT.Visibility = Visibility.Visible;

                            if (query == "")
                                GamesScriptsTXT.Text = lm.Translate("Games you play");
                            else
                                GamesScriptsTXT.Text = lm.Translate("Suggested games");
                        }
                    }

                    //if (!ownersok)
                    //{
                    //    ownersok = true;

                    //    List<OwnerScript> itemList = new List<OwnerScript>
                    //        {
                    //            new OwnerScript("https://infyiff.github.io/resources/Logo_Small.png", "Infinite Yeld", Properties.Resources.InfiniteYeld),
                    //            new OwnerScript("https://i.imgur.com/7yvZQlK.png", "Dark DEX V3", Properties.Resources.DarkDexV3),
                    //            new OwnerScript("https://raw.githubusercontent.com/Upbolt/Hydroxide/revision/github-assets/ui.png", "Hydroxide", Properties.Resources.Hydroxide),

                    //            //new OwnerScript("https://raw.githubusercontent.com/Upbolt/Hydroxide/revision/github-assets/ui.png", "Hydroxide", Properties.Resources.Hydroxide, "All"),
                    //        };

                    //    foreach (OwnerScript item in itemList)
                    //    {
                    //        try
                    //        {
                    //            if (query != "")
                    //            {
                    //                if (!item.Name.ToLower().Contains(query) && item.Name.ToLower() != query && !query.Contains(item.Name) && !item.supported_games.Contains(query))
                    //                    continue;
                    //            }

                    //            SpecialScripts obj = new SpecialScripts(
                    //                item.Name,
                    //                item.Script,
                    //                item.ImageLink
                    //            );

                    //            ((ButtonBase)obj.FindName("ExecuteBtn")).Click += (RoutedEventHandler)((sender, e) =>
                    //            {
                    //                string script = obj.script;
                    //                EXECUTAR(script);
                    //            });

                    //            //WP2.Visibility = Visibility.Visible;
                    //            WP2.Children.Add((UIElement)obj);
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            //ConsolePrint($"Error processing SAVED script: {ex}", ConsoleColor.Red);
                    //        }
                    //    }

                    //    if (WP2.Children.Count == 0)
                    //        CommunityScriptsTXT.Visibility = Visibility.Collapsed;
                    //    else
                    //        CommunityScriptsTXT.Visibility = Visibility.Visible;
                    //}

                    if (!savesok)
                    {
                        savesok = true;
                        foreach (string arq in Directory.GetFiles("C:\\Essence\\userdata\\Saves"))
                        {
                            try
                            {
                                StreamReader sr = new StreamReader(arq);
                                string[] dados = sr.ReadToEnd().Split(new string[] { "$M4endline$\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                                sr.Close();

                                if (query != "")
                                {
                                    if (!query.Contains(dados[0].ToLower()) && !dados[0].ToLower().Contains(query) && dados[0].ToLower() != query && !query.Contains(dados[3].ToLower()) && !dados[3].ToLower().Contains(query) && dados[3].ToLower() != query)
                                        continue;
                                }


                                ScriptThingy obj = new ScriptThingy(
                                    null,
                                    dados[0],
                                    dados[1],
                                    dados[2],
                                    dados[3],
                                    dados[4],
                                    dados[5]
                                );

                                obj.Salvo = true;
                                obj.ButtonImg.Visibility = Visibility.Visible;
                                //var buttonImg = (Image)obj.FindName("ButtonImg");
                                //buttonImg.Source = Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.star2.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());


                                //((ButtonBase)obj.FindName("ExecuteBtn")).Click += (RoutedEventHandler)((sender, e) =>
                                //{
                                //    string script = obj.script;
                                //    EXECUTAR(script);
                                //});

                                //((ButtonBase)obj.FindName("CopyBtn")).Click += (RoutedEventHandler)(async (sender, e) =>
                                //{
                                //    System.Windows.Clipboard.SetText(obj.script);

                                //    obj.CopiedMsg.Visibility = Visibility.Visible;
                                //    Storyboard sb = obj.TryFindResource("FadeIn") as Storyboard;
                                //    sb.Begin();

                                //    await Task.Delay(1000);

                                //    Storyboard sb2 = obj.TryFindResource("FadeOut") as Storyboard;
                                //    sb2.Completed += (sender, e) => obj.CopiedMsg.Visibility = Visibility.Hidden;
                                //    sb2.Begin();
                                //});

                                SavedScriptsTXT.Visibility = Visibility.Visible;
                                WP4.Visibility = Visibility.Visible;
                                WP4.Children.Add((UIElement)obj);
                            }
                            catch (Exception ex)
                            {
                                //MessageBox.Show(ex.ToString());
                                //ConsolePrint($"Error processing SAVED script: {ex.ToString()}", ConsoleColor.Red);
                            }
                            if (WP4.Children.Count == 0)
                            {
                                WP4.Visibility = Visibility.Collapsed;
                                //GamesScriptsTXT.Visibility = Visibility.Collapsed;
                            }
                        }
                    }


                    if (!trendingok)
                    {
                        trendingok = true;
                        try
                        {
                            Task.Run(async () =>
                            {
                                string kkk = await KeyGay2.Get("topscriptrate");
                                List<ObjectScore> topScores = JsonConvert.DeserializeObject<List<ObjectScore>>(kkk);
                                int ooo = 0;
                                foreach (var item in topScores)
                                {
                                    try
                                    {
                                        ScriptObject[] getScriptsTask = await GetScriptsFromUrl(1, item.ObjectId);
                                        if (getScriptsTask.Length < 1)
                                        {
                                            var match = Regex.Match(item.ObjectId, @"\d+$");

                                            if (match.Success)
                                            {
                                                getScriptsTask = await GetScriptsFromUrl(1, match.Value);
                                            }
                                            else
                                                continue;
                                        }

                                        ooo++;
                                        await Dispatcher.InvokeAsync(() =>
                                        {
                                            WP6.Visibility = Visibility.Visible;
                                            jfdiafdiasjdi.Visibility = Visibility.Visible;
                                            PopulateScripts(getScriptsTask[0], WP6, ooo);
                                        });                                        
                                    }
                                    catch { MessageBox.Show(item.ObjectId); }
                                }
                            });
                        }
                        catch(Exception ex) { MessageBox.Show(ex.ToString()); }
                    }


                    if (query != "")
                    {
                        List<ScriptObject> lo3l = new List<ScriptObject>();
                        List<ScriptObject> lo4l = new List<ScriptObject>();
                        foreach (ScriptObject scriptObject1 in scripts)
                        {
                            NewScriptsTXT.Visibility = Visibility.Visible;
                            WP.Visibility = Visibility.Visible;

                            //Se o nome do jogo do script não tiver nada a ver com o nome do jogo, ele é adicionado por ultimo
                            if (query == scriptObject1.game.name || query == scriptObject1.title || result.gameName == scriptObject1.game.name || result.PlaceID == scriptObject1.game.gameId || scriptObject1.game.name.ToLower().Contains(query) || scriptObject1.game.name.ToLower() == query || query.Contains(scriptObject1.game.name.ToLower()) || query.Contains(scriptObject1.title.ToLower()) || scriptObject1.title.ToLower().Contains(query))
                            {
                                PopulateScripts(scriptObject1, WP);
                                exact_results++;
                                await Task.Delay(70);
                            }
                            else if (scriptObject1.game.name.ToLower().Contains("universal script"))
                            {
                                PopulateScripts(scriptObject1, WP);
                                await Task.Delay(70);
                            }
                            else
                            {
                                lo3l.Add(scriptObject1);
                            }
                            
                        }

                        foreach (ScriptObject scriptObject1 in lo4l)
                        {
                            PopulateScripts(scriptObject1, WP);
                            await Task.Delay(70);
                        }

                        foreach (ScriptObject scriptObject1 in lo3l)
                        {
                            WP0.Visibility = Visibility.Visible;
                            NewScripts2TXT.Visibility = Visibility.Visible;
                            PopulateScripts(scriptObject1, WP0);
                            await Task.Delay(70);
                        }
                    }
                    else
                        foreach (ScriptObject scriptObject1 in scripts)
                        {
                            NewScriptsTXT.Visibility = Visibility.Visible;
                            WP.Visibility = Visibility.Visible;
                            PopulateScripts(scriptObject1, WP);
                            await Task.Delay(70);
                        }

                    await Task.Delay(200);
                }

                //mainWindow1.totalPages = Math.Max((int)jtoken[(object)"totalPages"], 1);

                //mainWindow1.SetCurrentPage(page);

            }
            catch (Exception ex)
            {
                GeneralScriptSearch3.IsEnabled = true;
                //ConsolePrint("[SCRIPT HUB] Fatal Error Loading Scripts: \n" + ex.ToString(), ConsoleColor.Red);
            }
            finally
            {
                GeneralScriptSearch3.IsEnabled = true;
                isSearching = false;

                //SpecialScriptsBorder.Visibility = Visibility.Visible;


                if (WP.Children.Count <= 0 && WP2.Children.Count <= 0 && WP0.Children.Count <= 0)
                {
                    pages = 0;
                    WP.Children.Clear();
                    WP.Visibility = Visibility.Collapsed;
                    NewScriptsTXT.Visibility = Visibility.Collapsed;

                    WP0.Children.Clear();
                    WP0.Visibility = Visibility.Collapsed;
                    NewScripts2TXT.Visibility = Visibility.Collapsed;

                    WP2.Children.Clear();
                    WP2.Visibility = Visibility.Collapsed;
                    CommunityScriptsTXT.Visibility = Visibility.Collapsed;

                    WP3.Children.Clear();
                    WP3.Visibility = Visibility.Collapsed;
                    GamesScriptsTXT.Visibility = Visibility.Collapsed;

                    Not_Found.Visibility = Visibility.Visible;
                }


                //if (WP2.Children.Count <= 0)
                //    SpecialScriptsBorder.Visibility = Visibility.Hidden;

                else
                {
                    foreach (var tabThing in WP.Children.OfType<ScriptThingy>().ToList())
                    {
                        if (tabThing.imagefailed && !tabThing.Salvo)
                        {
                            //ConsolePrint("[SCRIPT HUB] Removing Script Without Image...", ConsoleColor.Red);
                            //WP.Children.Remove(tabThing);
                            //WP0.Children.Remove(tabThing);

                            //WP0.Children.Add(tabThing);
                        }
                    }

                    if (query != "")
                    {
                        if (query.Length > 6)
                            await Dispatcher.InvokeAsync(() => NewScriptsTXT.Text = $"{exact_results} {lm.Translate("Exact Matches for")} '{query}'");
                        else
                            await Dispatcher.InvokeAsync(() => NewScriptsTXT.Text = $"{exact_results} {lm.Translate("Results for")} '{query}'");

                    }
                }
            }
        }


        internal static bool ismoving = false;
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (!stopdrag)
                {
                    if (WindowState == WindowState.Maximized)
                    {
                        WindowState = WindowState.Normal;

                        Width = 850;
                        Height = 550;

                        Point mousePosition = Mouse.GetPosition(System.Windows.Application.Current.MainWindow);
                        Point mousePositionOnScreen = System.Windows.Application.Current.MainWindow.PointToScreen(mousePosition);

                        double newLeft = mousePositionOnScreen.X - this.Left - (Width / 2);
                        double newTop = mousePositionOnScreen.Y - this.Top - 40;
                        this.Top = newTop;
                        this.Left = newLeft;
                    }

                    ismoving = true;
                    DragMove();
                    ismoving = false;
                }
            }
            catch { }
        }


        int AvalonEditBGA = 6; // A
        int AvalonEditBGR = 47; // R
        int AvalonEditBGG = 47; // G
        int AvalonEditBGB = 49; // B
        string AvalonEditFont = "Consolas";
        public TextEditor CreateNewTab()
        {
            TextEditor textEditor = new TextEditor
            {
                LineNumbersForeground = new SolidColorBrush(Color.FromRgb(100, 100, 100)),
                ShowLineNumbers = true,
                Foreground = new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue)),
                Background = new SolidColorBrush(Color.FromArgb((byte)AvalonEditBGA, (byte)AvalonEditBGR, (byte)AvalonEditBGB, (byte)AvalonEditBGG)),
                FontFamily = new FontFamily(AvalonEditFont),
                FontSize = 14,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
                WordWrap = true
            };

            textEditor.Options.EnableEmailHyperlinks = false;
            textEditor.Options.EnableHyperlinks = false;
            textEditor.Options.AllowScrollBelowDocument = false;

            using (MemoryStream xshdStream = new MemoryStream(Properties.Resources.lua))
            {
                using (XmlTextReader xshdReader = new XmlTextReader(xshdStream))
                {
                    textEditor.SyntaxHighlighting = HighlightingLoader.Load(xshdReader, HighlightingManager.Instance);
                }
            }
            return textEditor;
        }

        private int selected_editor = 1;
        public object CurrentTabWithStuff()
        {
            try
            {
                var content = TabControl.SelectedContent;

                if (selected_editor == 2)
                {
                    return content as TextEditor;
                }
                else if (selected_editor == 1)
                {
                    return content as WebViewA;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> GetEditorText(object editor)
        {
            try
            {
                if (editor == null)
                    throw new NullReferenceException();

                if (selected_editor == 1)
                {
                    return await ((WebViewA)editor).GetText();
                }
                else
                {
                    return ((TextEditor)editor).Text;
                }
            }
            catch { throw new NullReferenceException(); }
        }

        public async Task SetEditorTextAsync(object editor, string text)
        {
            try
            {
                if (editor == null)
                    throw new NullReferenceException();

                if (selected_editor == 1)
                {
                    await ((WebViewA)editor).SetText(text);
                }
                else if (selected_editor == 2)
                {
                    ((TextEditor)editor).Text = text;
                }
            }
            catch { }
        }




        private int TabCount = 0;
        private void Mini(object sender, MouseButtonEventArgs e)
        {
            base.WindowState = WindowState.Minimized;
        }

        private void FullScreenBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if (MainWin.WindowState != WindowState.Maximized)
            //    MainWin.WindowState = WindowState.Maximized;
            //else
            //    MainWin.WindowState = WindowState.Normal;
            //return;


            if (Width != SystemParameters.PrimaryScreenWidth - 100)
            {
                Width = SystemParameters.PrimaryScreenWidth - 100;
                Height = SystemParameters.PrimaryScreenHeight - 100;

                this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;
                this.Top = (SystemParameters.PrimaryScreenHeight - this.Height) / 2;

                //MainWin.WindowState = WindowState.Maximized;
            }
            else
            {
                Width = 850;
                Height = 550;

                this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;
                this.Top = (SystemParameters.PrimaryScreenHeight - this.Height) / 2;

                MainWin.WindowState = WindowState.Normal;
            }
        }

        private async void ExitMD(object sender, MouseButtonEventArgs e)
        {
            try { FadeMusic(2, false); } catch { }

            await Task.Delay(600);
            TabControl.Visibility = Visibility.Collapsed;
            FormFadeOut.Begin();
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
            TabControl.Visibility = Visibility.Collapsed;
            StopAllInteractions = true;


            if (prevent_closing)
            {
                MainGrid.Children.Clear();
            }
            else
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
            CloseCompleted = true;

            if (!prevent_closing)
                Process.GetCurrentProcess().Kill();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (!stopdrag)
                {
                    if (WindowState == WindowState.Maximized)
                    {
                        WindowState = WindowState.Normal;

                        Width = 850;
                        Height = 550;

                        Point mousePosition = Mouse.GetPosition(System.Windows.Application.Current.MainWindow);
                        Point mousePositionOnScreen = System.Windows.Application.Current.MainWindow.PointToScreen(mousePosition);

                        double newLeft = mousePositionOnScreen.X - this.Left - (Width / 2);
                        double newTop = mousePositionOnScreen.Y - this.Top - 40;
                        this.Top = newTop;
                        this.Left = newLeft;
                    }

                    ismoving = true;
                    DragMove();
                    ismoving = false;
                }
            }
            catch { }
        }

        private void TopmostFunc(object sender, RoutedEventArgs e)
        {
            Topmost = TopMButton.IsChecked.Value;
            registryKey1.SetValue("TopMost", TopMButton.IsChecked.Value);
        }

        private void SaveTabsFunc(object sender, RoutedEventArgs e)
        {
            SaveTabs = SaveTabsToggle.IsChecked.Value;
            registryKey1.SetValue("SaveTabs", SaveTabsToggle.IsChecked.Value);
        }

        private void DiscordRPCFunc(object sender, RoutedEventArgs e)
        {
            registryKey1.SetValue("RPC", RPCbutton.IsChecked.Value);
            if (RPCbutton.IsChecked.Value)
            {
                try
                {
                    client.Dispose();
                    RPCdisposed = true;
                }
                catch
                {
                    RPCbutton.IsEnabled = false;
                    RPCbutton.IsChecked = true;
                }
            }
            else
            {
                DiscordRPC();
            }
        }

        private void MultiInstanceFunc(object sender, RoutedEventArgs e)
        {
            registryKey1.SetValue("MultiInstance", MultiInstanceToggle.IsChecked.Value);
        }

        bool MonacoEnabled = true;
        private async void MonacoFunc(object sender, RoutedEventArgs e)
        {
            registryKey1.SetValue("Monaco", MonacoToggle.IsChecked.Value);
            if (MonacoToggle.IsChecked.Value)
            {
                MonacoEnabled = true;
                selected_editor = 1;
            }
            else
            {
                MonacoEnabled = false;
                selected_editor = 2;
            }

            MonacoToggle.IsEnabled = false;
            MonacoToggle.IsChecked = !MonacoToggle.IsChecked.Value;
            TabControl.Items.Clear();

            await CreateTabsFire();
            await Task.Delay(1100);

            MonacoToggle.IsEnabled = true;
            MonacoToggle.IsChecked = !MonacoToggle.IsChecked.Value;
        }


        private void CloseRoblox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int dsda = 0;
            foreach (Process p in Process.GetProcessesByName("RobloxPlayerBeta"))
            {
                p.Kill();
                dsda++;
            }

            //foreach (Process p in Process.GetProcessesByName("Windows10Universal"))
            //{
            //    p.Kill();
            //    dsda++;
            //}

            Notificar($"{dsda} Instances Closed.");
        }


        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOACTIVATE = 0x0010;

        private async void AdminConsoleButton_Click(object sender, RoutedEventArgs e)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                IntPtr consoleWindow = GetConsoleWindow();

                if (consoleWindow == IntPtr.Zero)
                {
                    AllocConsole();
                    consoleWindow = GetConsoleWindow();

                    StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput())
                    {
                        AutoFlush = true
                    };

                    var app = (App)Application.Current;
                    app.MultiTextWriterInstance = new MultiTextWriter(standardOutput, app.MultiTextWriterInstance._writers[1]);
                    Console.SetOut(app.MultiTextWriterInstance);
                    Console.SetError(app.MultiTextWriterInstance);

                    int width = 714; 
                    int height = 150;
                    int screenX = (int)SystemParameters.PrimaryScreenWidth; 
                    int screenY = (int)SystemParameters.PrimaryScreenHeight;
                    int posX = (screenX - width) / 2; 
                    int posY = screenY - height - 50; 

                    MoveWindow(consoleWindow, posX, posY, width, height, true);

                    //Console.Title = "Essence 2024 © Made with ♥ UI by M4A1_dev2";

                    foreach (var log in consoleLogs)
                    {
                        Console.ForegroundColor = log.Color;
                        Console.WriteLine(log.Message);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    FreeConsole();
                }
            });
        }





        FrameworkElement LastMovedGrid;
        bool areadyanimatinggrid = false;
        internal static bool homeggggg = true;
        private async void AnimateGrid(FrameworkElement targetControl, Thickness targetPosition, RadioButton button)
        {
            if (areadyanimatinggrid == true || LastMovedGrid == targetControl)
                return;

            areadyanimatinggrid = true;
            homeggggg = false;

            HomeRadioButton.IsEnabled = false;
            ExecutorRadioButton.IsEnabled = false;
            HubRadioButton.IsEnabled = false;
            ScriptsRadioButton.IsEnabled = false;
            AIRadioButton.IsEnabled = false;
            SettingsRadioButton.IsEnabled = false;
            button.IsEnabled = true;

            LastMovedGrid.IsHitTestVisible = false;


            //Move(LastMovedGrid, LastMovedGrid.Margin, new Thickness(60, MainWin.ActualHeight + 150, 0, 0), 0.65);
            Move(LastMovedGrid, LastMovedGrid.Margin, new Thickness(260, 200, 200, 200), 0.45);
            Fade(LastMovedGrid, 1, 0, 0.6);

            await Task.Delay(100);

            //Move(targetControl, new Thickness(60, MainWin.ActualHeight + 150, 0, 0), new Thickness(60, 40, 0, 0), 1.1);
            Move(targetControl, new Thickness(160,100, 100, 100), new Thickness(60, 40, 0, 0), 0.65);
            Fade(targetControl, 0, 1, 0.8);
            targetControl.Visibility = Visibility.Visible;

            Move(SelectedMenuThing, SelectedMenuThing.Margin, targetPosition, 0.8);

            await Task.Delay(350);
            targetControl.IsHitTestVisible = true;
            LastMovedGrid.Visibility = Visibility.Collapsed;
            LastMovedGrid = targetControl;
            areadyanimatinggrid = false;

            HomeRadioButton.IsEnabled = true;
            ExecutorRadioButton.IsEnabled = true;
            HubRadioButton.IsEnabled = true;
            ScriptsRadioButton.IsEnabled = true;
            AIRadioButton.IsEnabled = true;
            SettingsRadioButton.IsEnabled = true;

            if (LastMovedGrid == HomeGrid)
                homeggggg = true;
        }


        private void HomeRadioButtonClick(object sender, RoutedEventArgs e)
        {
            AnimateGrid(HomeGrid, new Thickness(0, 16, 0, 0), HomeRadioButton);
        }

        private void ExecutorRadioButtonClick(object sender, RoutedEventArgs e)
        {
            AnimateGrid(ExecutorGrid, new Thickness(0, 60, 0, 0), ExecutorRadioButton);
        }

        private bool first_search = true;
        private void HubRadioButtonClick(object sender, RoutedEventArgs e)
        {
            AnimateGrid(HubGrid, new Thickness(0, 104, 0, 0), HubRadioButton);

            if (first_search)
            {
                GeneralScriptScrollViewer.Visibility = Visibility.Visible;
                first_search = false;
                SearchScripts2("");
            }
        }

        private void ScriptsRadioButtonClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            //MessageBox.Show("Quick Panel disabled for now");
           // return;

            AnimateGrid(ScriptsGrid, new Thickness(0, 148, 0, 0), ScriptsRadioButton);
        }


        private string titleeee = ".";
        private string subtitleeee = ".";

        string last_ai_output = "";
        private async void AIRadioButtonClick(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Elemtnal AI disabled for now");
            //return;

            AnimateGrid(AssistentGrid, new Thickness(0, 189, 0, 0), AIRadioButton);

            if (last_ai_output == "")
                last_ai_output = lm.Translate("--AI will output scripts here");

            ieanfjeanfifnuineufenafn.Text = lm.Translate("Welcome User!");
            ieanfjeanfifnuineufenafn2.Text = lm.Translate("Our AI is ready to help you!");

            if (AICodeOutputHolder.Child == null)
            {
                AIeditor = new WebViewA(lm.Translate("--AI will output scripts here"));
            }

            if (AICodeOutputHolder.Child == null)
            {
                AIeditor.UpdateWindowPos();
                AIeditor.ToSetText = last_ai_output;
                AICodeOutputHolder.Child = AIeditor;
            }

            while (titleeee == "." && subtitleeee == ".")
            {
                await Task.Delay(100);
            }

            await Task.Delay(1100);

            if (titleeee != "...")
            {
                ieanfjeanfifnuineufenafn.Text = "";
                ieanfjeanfifnuineufenafn2.Text = "";
                for (int i = 0; i < titleeee.Length; i++)
                {
                    ieanfjeanfifnuineufenafn.Text += titleeee[i].ToString();
                    await Task.Delay(40);
                }

                for (int i = 0; i < subtitleeee.Length; i++)
                {
                    ieanfjeanfifnuineufenafn2.Text += subtitleeee[i].ToString();
                    await Task.Delay(15);
                }
            }

            IA_Escrevendo = false;
            textoIA.IsEnabled = true;
            fuhfufua.Visibility = Visibility.Visible;
            AI_writing.Visibility = Visibility.Collapsed;
        }

        private void SettingsRadioButtonClick(object sender, RoutedEventArgs e)
        {
            AnimateGrid(SettingsGrid, new Thickness(0, Menu.ActualHeight - 38, 0, 0), SettingsRadioButton);
        }

        private void HomeRadioButtonLoaded(object sender, RoutedEventArgs e)
        {
            HomeRadioButton.IsChecked = true;
            //ExecutorRadioButton.IsChecked = true;
        }



        private void Drag_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (!stopdrag)
                {
                    if (WindowState == WindowState.Maximized)
                    {
                        WindowState = WindowState.Normal;

                        Width = 850;
                        Height = 550;

                        Point mousePosition = Mouse.GetPosition(System.Windows.Application.Current.MainWindow);
                        Point mousePositionOnScreen = System.Windows.Application.Current.MainWindow.PointToScreen(mousePosition);

                        double newLeft = mousePositionOnScreen.X - this.Left - (Width / 2);
                        double newTop = mousePositionOnScreen.Y - this.Top - 40;
                        this.Top = newTop;
                        this.Left = newLeft;
                    }

                    ismoving = true;
                    DragMove();
                    ismoving = false;
                }
            }
            catch { }
        }


        static string FormatTime(int secs)
        {
            int days = secs / 86400;
            int hours = (secs % 86400) / 3600;  
            int minutes = (secs % 3600) / 60;
            int seconds = secs % 60;

            string time = "";

            if (days > 0)
            {
                time += $"{days}D";
            }

            if (hours > 0 || days > 0) 
            {
                if (time.Length > 0) time += ":";
                time += $"{hours}h";
            }

            if (minutes > 0 || hours > 0 || days > 0)  
            {
                if (time.Length > 0) time += ":";
                time += $"{minutes}m";
            }

            if (time.Length > 0) time += ":"; 
            time += $"{seconds}s";

            return time;
        }







        private static readonly HttpClient rbxclient = new HttpClient();

        private static string GetCdnUrl(string hashId, int serverNumber)
        {
            return $"https://t{serverNumber}.rbxcdn.com/{hashId}";
        }

        private static async Task<bool> DownloadFileWithRetry(string hashId, string savePath)
        {
            for (int serverNumber = 0; serverNumber <= 7; serverNumber++)
            {
                string url = GetCdnUrl(hashId, serverNumber);
                try
                {
                    HttpResponseMessage response = await rbxclient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                        File.WriteAllBytes(savePath, fileBytes);
                        //Console.WriteLine($"Arquivo baixado com sucesso de {url} e salvo em: {savePath}");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"Tentativa falhou para {url}, status: {response.StatusCode}");
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Erro ao tentar baixar {url}: {e.Message}");
                }
            }
            Console.WriteLine($"Falha ao baixar {hashId} em todos os servidores.");
            return false;
        }

        public static async Task GetAvatar3D(int userId, string saveDir = "avatar_model")
        {
            // Endpoint da API para buscar o avatar 3D
            string url = $"https://thumbnails.roblox.com/v1/users/avatar-3d?userId={userId}";

            try
            {
                HttpResponseMessage response = await rbxclient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                JObject data = JObject.Parse(responseBody);

                if (data["state"]?.ToString() != "Completed")
                {
                    Console.WriteLine("O avatar ainda está pendente ou não está disponível.");
                    return;
                }

                string imageUrl = data["imageUrl"]?.ToString();
                if (string.IsNullOrEmpty(imageUrl))
                {
                    Console.WriteLine("Imagem do avatar não disponível.");
                    return;
                }

                HttpResponseMessage modelResponse = await rbxclient.GetAsync(imageUrl);
                modelResponse.EnsureSuccessStatusCode();
                JObject modelData = JObject.Parse(await modelResponse.Content.ReadAsStringAsync());


                saveDir = $"C:\\Essence\\userdata\\{saveDir}";
                Directory.CreateDirectory(saveDir);

                string objHash = modelData["obj"]?.ToString();
                if (!string.IsNullOrEmpty(objHash))
                {
                    await DownloadFileWithRetry(objHash, Path.Combine(saveDir, $"avatar_{userId}.obj"));
                }

                string mtlHash = modelData["mtl"]?.ToString();
                if (!string.IsNullOrEmpty(mtlHash))
                {
                    await DownloadFileWithRetry(mtlHash, Path.Combine(saveDir, $"avatar_{userId}.mtl"));
                }

                // texturas, se disponíveis
                JArray textures = (JArray)modelData["textures"];
                if (textures != null)
                {
                    for (int i = 0; i < textures.Count; i++)
                    {
                        string textureHash = textures[i]?.ToString();
                        if (!string.IsNullOrEmpty(textureHash))
                        {
                            await DownloadFileWithRetry(textureHash, Path.Combine(saveDir, $"texture_{i}_{userId}.png"));
                        }
                    }
                }

                Console.WriteLine($"Todos os arquivos do avatar foram salvos em {saveDir}");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Erro na requisição: {e.Message}");
            }
        }









        PerspectiveCamera camera = new PerspectiveCamera
        {
            Position = new Point3D(0, 0, 0), 
            LookDirection = new Vector3D(0, -1, 0),
            UpDirection = new Vector3D(0, 0, 1),       
            FieldOfView = 25
        };

        private string roblox_user_name = "";

        HelixViewport3D hVp3D = new HelixViewport3D
        {
            Background = System.Windows.Media.Brushes.Transparent,
            ShowCoordinateSystem = false,
            ShowViewCube = false,
            LimitFPS = true,
            RotateAroundMouseDownPoint = true,
            RotateGesture = new MouseGesture(MouseAction.LeftClick),
            RotateCursor = Cursors.Arrow,
            ZoomCursor = Cursors.Arrow,
            PanCursor = Cursors.Arrow,
            ShowCameraTarget = false,


            //ShowFrameRate = true,
            //ShowCameraInfo = true,
            //ShowTriangleCountInfo = true,
        };

        AxisAngleRotation3D spinRotation = new AxisAngleRotation3D(new Vector3D(0, 0, 1), 0);
        private async void Create3DViewPortForUserImage(string user_name)
        {
            roblox_user_name = user_name;
            string url = $"https://zebrarblx.xyz/tools/character-downloader/?user={user_name}";
            string zipFilePath = $"{user_name}.zip";

            //await GetAvatar3D(905876656);
            //MessageBox.Show(user_name);


            string directory = $"C:\\Essence\\userdata\\{user_name}";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url);

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        if (!File.Exists($"{directory}\\{user_name}.obj"))
                        {
                            await Task.Delay(3000);
                            Create3DViewPortForUserImage(user_name);
                            throw new HttpRequestException("A resposta foi diferente de 200. Tentando novamente...");
                        }
                        else
                            throw new HttpRequestException("resposta diferente de 200. carregando arquivo obj existente...");
                    }

                    response.EnsureSuccessStatusCode();
                    using (var fs = new FileStream(zipFilePath, FileMode.Create))
                    {
                        await response.Content.CopyToAsync(fs);
                    }
                }

                if (Directory.Exists(directory))
                    Directory.Delete(directory, true);

                foreach (string filePath in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.png"))
                {
                    //ConsolePrint(filePath, ConsoleColor.Red);
                    try { File.Delete(filePath); } catch { }
                }

                ZipFile.ExtractToDirectory(zipFilePath, directory);
                File.Delete(zipFilePath);

                foreach (string filePath in Directory.GetFiles(directory, "*.png"))
                {
                    try { File.Copy(filePath, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(filePath))); }
                    catch { }
                }

                //BitmapImage bitmap = null;
                //bitmap = new BitmapImage(new Uri($"C:\\Essence\\{user_name}\\{user_name}1Tex.png", UriKind.RelativeOrAbsolute));

                //Color pixelColor = GetPixelColor(bitmap);
                //gender_color7.Color = pixelColor;

                //InternalConsolePrint("AVATAR 3D: DOWNLOADED!", console_RichTextBox, Colors.Green);
            }
            catch (Exception ex)
            {
                //InternalConsolePrint("AVATAR 3D: FAILED TO DOWNLOAD", console_RichTextBox, Colors.Red);
                //Console.WriteLine($"Ocorreu um erro: {ex.Message}");
            }

            if (File.Exists($"{directory}\\{user_name}.obj"))
            {
                //ConsolePrint($"AVATAR 3D: opening {directory}\\{user_name}.obj", ConsoleColor.Yellow);
                try
                {
                    var directionalLight = new DirectionalLight
                    {
                        Color = Colors.White,
                        Direction = new Vector3D(-1, -1, -1)
                    };
                    hVp3D.Children.Add(new ModelVisual3D { Content = directionalLight });

                    var importer = new ObjReader();

                    using (var objStream = File.OpenRead($"{directory}\\{user_name}.obj"))
                    using (var mtlStream = File.OpenRead($"{directory}\\{user_name}.mtl"))
                    {
                        var model = importer.Read(objStream, new Stream[] { mtlStream });

                        var translateTransform = new TranslateTransform3D(0, -103, 0.1);
                        var initialRotation = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90);
                        var initialRotateTransform = new RotateTransform3D(initialRotation);

                        var spinRotateTransform = new RotateTransform3D(spinRotation);

                        var transformGroup = new Transform3DGroup();
                        transformGroup.Children.Add(translateTransform);
                        transformGroup.Children.Add(initialRotateTransform);
                        transformGroup.Children.Add(spinRotateTransform);
                        model.Transform = transformGroup;

                        var rotationAnimation = new DoubleAnimation
                        {
                            From = 0,
                            To = 360,
                            Duration = TimeSpan.FromSeconds(8),
                            RepeatBehavior = RepeatBehavior.Forever
                        };

                        spinRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, rotationAnimation);

                        hVp3D.Children.Add(new ModelVisual3D { Content = model });
                    }

                    hVp3D.Camera = camera;
                    PlayerImageHolder.Child = hVp3D;
                    PlayerImageHolder.Cursor = Cursors.SizeAll;
                    //InternalConsolePrint("AVATAR 3D: LOADED", console_RichTextBox, Colors.Green);
                }
                catch (Exception ex)
                {
                    InternalConsolePrint("AVATAR 3D: FAILED TO LOAD", console_RichTextBox, Colors.Red);
                    //Console.WriteLine($"Erro ao carregar o modelo: {ex.Message}");
                }
            }

            //Environment.CurrentDirectory = $"C:\\Essence";
            //Directory.SetCurrentDirectory($"C:\\Essence");
        }

        private bool move_started = false;
        private bool return_to_original = true;
        private bool didiqmndidqiid = false;
        private async void Move_to_original()
        {
            if (move_started)
                return;

            move_started = true;
            while (true)
            {
                if (return_to_original && LastMovedGrid == ScriptsGrid && !StopAllInteractions && IsActive)
                {
                    camera.AnimateTo(new Point3D(0, 13, 0), new Vector3D(0, -14, 0), new Vector3D(0, 0, 1), 2000);
                    //camera.AnimateTo(new Point3D(-5, 11, 0.01), new Vector3D(5, -12, 0.01), new Vector3D(0, 0, 1), 2000);

                    if (didiqmndidqiid)
                    {
                        await Task.Delay(1000);

                        didiqmndidqiid = false;
                        spinRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, null);
                        spinRotation.Angle = 0;

                        var rotationAnimation = new DoubleAnimation
                        {
                            From = 0,
                            To = 360,
                            Duration = TimeSpan.FromSeconds(6),
                            RepeatBehavior = RepeatBehavior.Forever
                        };

                        spinRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, rotationAnimation);
                    }

                    await Task.Delay(2500);
                }
                else
                {
                    didiqmndidqiid = true;
                    await Task.Delay(2500);
                }
            }
        }

        double last_spinrotation_angle;
        private void PlayerImageHolder_MouseEnter(object sender, MouseEventArgs e)
        {
            return_to_original = false;
            last_spinrotation_angle = spinRotation.Angle;
            spinRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, null);
            spinRotation.Angle = last_spinrotation_angle;



            var rotationAnimation = new DoubleAnimation
            {
                From = last_spinrotation_angle,
                To = last_spinrotation_angle + 360,
                Duration = TimeSpan.FromSeconds(20),
                RepeatBehavior = RepeatBehavior.Forever
            };

            spinRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, rotationAnimation);
        }

        private void PlayerImageHolder_MouseLeave(object sender, MouseEventArgs e)
        {
            return_to_original = true;

            last_spinrotation_angle = spinRotation.Angle;
            spinRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, null);
            spinRotation.Angle = last_spinrotation_angle;

            var rotationAnimation = new DoubleAnimation
            {
                From = last_spinrotation_angle,
                To = last_spinrotation_angle + 360,
                Duration = TimeSpan.FromSeconds(6),
                RepeatBehavior = RepeatBehavior.Forever
            };

            spinRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, rotationAnimation);
        }

        private void PlayerImageHolder_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //return_to_original = true;

            //var rotationAnimation = new DoubleAnimation
            //{
            //    From = last_spinrotation_angle,
            //    To = last_spinrotation_angle + 360,
            //    Duration = TimeSpan.FromSeconds(8),
            //    RepeatBehavior = RepeatBehavior.Forever
            //};

            //spinRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, rotationAnimation);
        }


        string PrintValue(string key, string text)
        {
            try
            {
                text = text.Split(new string[] { "\"" + key + "\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                text = text.Split(new string[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                return text;
            }
            catch
            {
                return "Erro";
            }

        }



        private int playing_time = 0;
        private bool timereseted1;
        private bool timereseted2;
        private string RobloxPlayerID = "";
        private bool roblox_look_finished = false;
        logWatcher logWatcher = new logWatcher();
        private async void ExecutionTasks()
        {
            if (upd)
            {
                return;
            }
            upd = true;

            InternalConsolePrint("[ENVIRONMENT TASKS] Starting Scripts Tasks...", console_RichTextBox, Colors.Yellow);


            if (!Directory.Exists(api2.workspace))
                Directory.CreateDirectory(api2.workspace);

            Directory.CreateDirectory(api2.workspace);
            Directory.CreateDirectory(api2.workspace + "\\Essence");

            
            logWatcher.StartWatcher();
            logWatcher.userfound += (sender, e) =>
            {
                if (info.Contains(PrintValue("userId", logWatcher.useri)))
                {
                    if (!KeyGay2.Roblox_IDS.Contains(PrintValue("userId", logWatcher.useri)))
                    {
                        if (KeyGay2.Roblox_IDS.Length > 0)
                            KeyGay2.Roblox_IDS += ", ";

                        KeyGay2.Roblox_IDS += PrintValue("userId", logWatcher.useri);
                    }

                    roblox_look_finished = true;
                    ShowBa(PrintValue("userId", logWatcher.useri));
                }
            };

            bool foundUserId = false;
            bool foundDisplayName = false;
            bool foundGender = false;

            while (StopAllInteractions == false)
            {
                bool someerror = false;

                PopulateScriptList("Scripts", "");

                try
                {
                    //int errorCount = 0;
                    //using (var lua = new Lua())
                    //{
                    //    string lol = await GetEditorText(CurrentTabWithStuff());
                    //    string[] lines = lol.Split(new[] { "\n" }, StringSplitOptions.None);

                    //    foreach (var line in lines)
                    //    {
                    //        try
                    //        {
                    //            // Tente compilar a linha atual
                    //            lua.DoString("return function() " + line + " end");

                    //        }
                    //        catch (Exception)
                    //        {
                    //            errorCount++;
                    //        }
                    //    }
                    //}
                    //Syntaxerrorcounter.Text = errorCount.ToString();
                }
                catch
                {

                }


                if (File.Exists(api2.workspace + "\\Essence\\Respawn.evolu"))
                {
                    try { File.Delete(api2.workspace + "\\Essence\\Respawn.evolu"); } catch { }
                    await Task.Delay(1000);
                }

                if (File.Exists(api2.workspace + "\\Essence\\Die.evolu"))
                {
                    try { File.Delete(api2.workspace + "\\Essence\\Die.evolu"); } catch { }

                    //ConsolePrint("[Essence INTERACTIONS] PLAYER DIED", ConsoleColor.Yellow);

                    //JumpLock.Data = Geometry.Parse("M16.96 8.951H8.08v-1.56c0-.766.23-1.515.66-2.15a3.87 3.87 0 0 1 4-1.65a3.8 3.8 0 0 1 2 1.06c.159.157.3.332.42.52a.754.754 0 0 0 1.26-.83a4.937 4.937 0 0 0-.62-.75a5.26 5.26 0 0 0-2.75-1.47a5.38 5.38 0 0 0-6.43 5.27v1.59a3.12 3.12 0 0 0-2.87 3v6.94A3.16 3.16 0 0 0 7 21.981h10a3.16 3.16 0 0 0 3.25-3.06v-6.94a3.16 3.16 0 0 0-3.29-3.03m-1.53 9.84H8.49a1 1 0 0 1 0-2h6.94a1 1 0 1 1 0 2");
                    //JumpLock.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));

                    //SpeedLock.Data = Geometry.Parse("M16.96 8.951H8.08v-1.56c0-.766.23-1.515.66-2.15a3.87 3.87 0 0 1 4-1.65a3.8 3.8 0 0 1 2 1.06c.159.157.3.332.42.52a.754.754 0 0 0 1.26-.83a4.937 4.937 0 0 0-.62-.75a5.26 5.26 0 0 0-2.75-1.47a5.38 5.38 0 0 0-6.43 5.27v1.59a3.12 3.12 0 0 0-2.87 3v6.94A3.16 3.16 0 0 0 7 21.981h10a3.16 3.16 0 0 0 3.25-3.06v-6.94a3.16 3.16 0 0 0-3.29-3.03m-1.53 9.84H8.49a1 1 0 0 1 0-2h6.94a1 1 0 1 1 0 2");
                    //SpeedLock.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));

                    await Task.Delay(5000);


                    //ConsolePrint("[Essence INTERACTIONS] REMOVING FLY/NOCLIP LOOP", ConsoleColor.Gray);
                    if (Flybtn.Background.ToString() == "#64C81414")
                    {
                        Flybtn.Background = new SolidColorBrush(Color.FromArgb(100, 25, 25, 25));
                        EXECUTAR(Essence.Properties.Resources.Fly);
                    }

                    if (Clipbtn.Background.ToString() == "#64C81414")
                    {
                        EXECUTAR(Essence.Properties.Resources.noclip);
                        Clipbtn.Background = new SolidColorBrush(Color.FromArgb(100, 25, 25, 25));
                    }
                }

                if (!inj4 || File.Exists(api2.workspace + "\\Essence\\Quit.evolu") || !logWatcher.IsInGame)
                {
                    if (File.Exists(api2.workspace + "\\Essence\\Quit.evolu"))
                    {
                        InternalConsolePrint("[Essence INTERACTIONS] PLAYER LEAVED GAME", console_RichTextBox, Colors.Yellow);
                    }

                    try { File.Delete(api2.workspace + "\\Essence\\Quit.evolu"); } catch { }

                    try { File.Delete(api2.workspace + "\\Essence\\PlayerData.evolu"); } catch { }

                    if (!timereseted1)
                        playing_time = 0;

                    timereseted1 = true;
                    timereseted2 = false;

                    playing_time++;


                    //SetWalkspeed.IsEnabled = false;
                    //SetJumPower.IsEnabled = false;

                    //Rejoinbtn.IsEnabled = false;

                    //Flybtn.IsEnabled = false;
                    Flybtn.Background = new SolidColorBrush(Color.FromArgb(255, 25, 25, 25));

                    //Clipbtn.IsEnabled = false;
                    Clipbtn.Background = new SolidColorBrush(Color.FromArgb(255, 25, 25, 25));

                    //FPSbtn.IsEnabled = false;
                    FPSbtn.Background = new SolidColorBrush(Color.FromArgb(255, 25, 25, 25));

                    //Espbtn.IsEnabled = false;
                    Bypassbtn.Background = new SolidColorBrush(Color.FromArgb(255, 25, 25, 25));

                    ESPpreview.Visibility = Visibility.Collapsed;
                    Distancepreview.Visibility = Visibility.Collapsed;
                    Trackerspreview.Visibility = Visibility.Collapsed;

                    AimbotToggle.IsChecked = false;
                    TracersToggle.IsChecked = false;
                    BoxToggle.IsChecked = false;

                    speedlk = false;
                    jumplk = false;
                }

                if (inj4)
                {
                    if (!timereseted2)
                        playing_time = 0;

                    timereseted1 = false;
                    timereseted2 = true;

                    playing_time++;

                    //ServerHop.IsEnabled = true;
                    //Rejoinbtn.IsEnabled = true;
                    //Flybtn.IsEnabled = true;
                    //Clipbtn.IsEnabled = true;
                    //FPSbtn.IsEnabled = true;
                    //Bypassbtn.IsEnabled = true;

                    //SetWalkspeed.IsEnabled = true;
                    //SetJumPower.IsEnabled = true;

                    //ConsolePrint("[Essence INTERACTIONS] ADDING SPEED/JUMP LOOP", ConsoleColor.Gray);

                    //if (speedlk && speedlk_value != (int)SpeedSlider.Value)
                    //{
                    //    speedlk_value = (int)SpeedSlider.Value;
                    //    EXECUTAR("_G.EvoEnvSpeedLock = nil\ntask.wait(1.2)\n" + Properties.Resources.SpeedLock.Replace("777", speedlk_value.ToString()), true);
                    //}

                    //if (jumplk && jumplk_value != (int)JumpSlider.Value)
                    //{
                    //    jumplk_value = (int)JumpSlider.Value;
                    //    EXECUTAR("_G.EvoEnvJumpLock = nil\ntask.wait(1.2)\n" + Properties.Resources.SpeedLock.Replace("666", jumplk_value.ToString()), true);
                    //}
                    //

                    try
                    {
                        if (File.Exists(api2.workspace + "\\Essence\\Jump.evolu"))
                        {
                            StreamReader streamReader = new StreamReader(api2.workspace + "\\Essence\\Jump.evolu");
                            string jump = streamReader.ReadToEnd();
                            streamReader.Close();

                            if (double.TryParse(jump, out var v2))
                            {
                                JumpSlider.Value = v2;
                            }

                            try { File.Delete(api2.workspace + "\\Essence\\Jump.evolu"); } catch { }
                        }
                    }
                    catch { }

                    try
                    {
                        if (File.Exists(api2.workspace + "\\Essence\\Speed.evolu"))
                        {
                            StreamReader streamReader2 = new StreamReader(api2.workspace + "\\Essence\\Speed.evolu");
                            string speed = streamReader2.ReadToEnd();
                            streamReader2.Close();
                            if (double.TryParse(speed, out var v))
                            {
                                SpeedSlider.Value = v;
                            }

                            try { File.Delete(api2.workspace + "\\Essence\\Speed.evolu"); } catch { }
                        }
                    }
                    catch { }
                }

                await Task.Run(async () =>
                {
                    try
                    {
                        if (!foundUserId || !foundDisplayName || !foundGender)
                        {
                            string StorageFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Roblox\\LocalStorage\\appStorage.json");
                            if (File.Exists(StorageFile))
                            {
                                try
                                {
                                    FileInfo fileInfo = new FileInfo(StorageFile);
                                    using StreamReader sr = new StreamReader(fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                                    var log = await sr.ReadLineAsync();
                                    string text = log.Replace(@"\", "").Replace(",", "");


                                    if (!foundUserId && text.Contains("userId"))
                                    {
                                        string PlayerI = "";
                                        await Dispatcher.InvokeAsync(() =>
                                        {
                                            PlayerI = PlayerID.Text;
                                        });

                                        RobloxPlayerID = PrintValue("userId", text);
                                        roblox_look_finished = true;

                                        List<string> linhas = new List<string>(File.ReadAllLines("C:\\Essence\\userdata\\logindata.txt"));
                                        linhas[3] = RobloxPlayerID;
                                        File.WriteAllLines("C:\\Essence\\userdata\\logindata.txt", linhas);

                                        if (info.Contains(RobloxPlayerID))
                                        {
                                            if (!KeyGay2.Roblox_IDS.Contains(RobloxPlayerID))
                                            {
                                                if (KeyGay2.Roblox_IDS.Length > 0)
                                                    KeyGay2.Roblox_IDS += ", ";

                                                KeyGay2.Roblox_IDS += RobloxPlayerID;
                                            }

                                            ShowBa(RobloxPlayerID);
                                        }


                                        await Dispatcher.InvokeAsync(() =>
                                        {
                                            PlayerID.Text = lm.Translate("Player ID: ") + PrintValue("userId", text);
                                        });

                                        string url = "https://www.roblox.com/users/" + PrintValue("userId", text) + "/profile";
                                        WebClient client = new WebClient();
                                        string html = client.DownloadString(url);
                                        client.Dispose();

                                        Match matchUsername = Regex.Match(html, @"\""profileusername\"":\""(.*?)\""");
                                        Match matchAvatarUrl = Regex.Match(html, @"<meta\s*property=""og:image""\s*content=""(.*?)""\s*/>");
                                        Match DisplayName = Regex.Match(html, @"<title>(.*?) - Roblox</title>");

                                        if (DisplayName.Success)
                                        {
                                            await Dispatcher.InvokeAsync(() =>
                                            {
                                                PlayerName1.Content = lm.Translate("Current User: ") + DisplayName.Groups[1].Value;
                                            });
                                            foundDisplayName = true;
                                        }

                                        if (matchUsername.Success)
                                        {
                                            await Dispatcher.InvokeAsync(() =>
                                            {
                                                Console.WriteLine("matchUsername");
                                                Create3DViewPortForUserImage(matchUsername.Groups[1].Value);
                                            });
                                        }
                                        if (matchAvatarUrl.Success)
                                        {
                                            string avatarUrl = matchAvatarUrl.Groups[1].Value;
                                            await Dispatcher.InvokeAsync(() =>
                                            {
                                                //Console.WriteLine("matchAvatarUrl");
                                                //Create3DViewPortForUserImage("arduino_456");
                                                PlayerImage3.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(avatarUrl));
                                            });
                                        }
                                        else
                                        {
                                            await Dispatcher.InvokeAsync(() =>
                                            {
                                                Create3DViewPortForUserImage("vgwgege5");
                                                PlayerImage3.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/ImageResources/bacon.png"));
                                            });
                                            Console.WriteLine("URL da imagem do avatar não encontrado.");
                                        }


                                        foundUserId = true;
                                    }

                                    if (!foundDisplayName && text.Contains("DisplayName"))
                                    {
                                        await Dispatcher.InvokeAsync(() =>
                                        {
                                            PlayerName1.Content = "Current User: " + PrintValue("DisplayName", text);
                                        });
                                        foundDisplayName = true;
                                    }

                                    if (!foundGender && text.Contains("gender"))
                                    {
                                        string gender = PrintValue("gender", text);
                                        await Dispatcher.InvokeAsync(() =>
                                        {
                                            if (gender == "Male")
                                            {
                                                //gender_color.Color = Color.FromArgb(70, 0, 144, 255);
                                            }
                                            else if (gender == "Female")
                                            {
                                                //gender_color.Color = Color.FromArgb(70, 255, 0, 210);
                                            }
                                        });
                                        foundGender = true;
                                    }
                                }
                                catch
                                {
                                    someerror = true;
                                }
                            }
                        }
                    }
                    catch { }

                    try
                    {
                        if (logWatcher.IsInGame)
                        {
                            string GameI = "";

                            await Dispatcher.InvokeAsync(() =>
                            {
                                GameI = GameID.Text;
                            });

                            if (GameI != lm.Translate("Game ID: ") + logWatcher.CurrentPlaceId)
                            {
                                playing_time = 0;

                                await Dispatcher.InvokeAsync(() =>
                                {
                                    GameID.Text = lm.Translate("Game ID: ") + logWatcher.CurrentPlaceId;
                                    GameStatus.Text = lm.Translate("Getting Game Name...");
                                });

                                string url = "https://www.roblox.com/games/" + logWatcher.CurrentPlaceId;

                                using (var httpClient = new HttpClient())
                                {
                                    try
                                    {
                                        var response = await httpClient.GetAsync(url);
                                        var html = await response.Content.ReadAsStringAsync();

                                        string ogTitlePattern = "<meta\\s+property=\"og:title\"\\s+content=\"([^\"]+)\"";
                                        var match2 = Regex.Match(html, ogTitlePattern);

                                        if (match2.Success)
                                        {
                                            await Dispatcher.InvokeAsync(() =>
                                            {
                                                GameStatus.Text = lm.Translate("Playing ") + match2.Groups[1].Value /*+ " (" + FormatTime(playing_time) + ")"*/;
                                            });
                                        }
                                        else
                                        {
                                            Console.WriteLine("Nenhuma correspondência encontrada para og:title.");
                                            someerror = true;
                                        }
                                    }
                                    catch (HttpRequestException e)
                                    {
                                        Console.WriteLine($"Erro ao fazer a solicitação HTTP: {e.Message}");
                                        someerror = true;
                                    }
                                }
                            }
                        }

                        else
                        {
                            //Console.WriteLine("Nenhum Jogo Aberto.");

                            bool isWindowOpen = IsWindowOpen("Windows10Universal") || IsWindowOpen("RobloxPlayerBeta");

                            if (isWindowOpen)
                            {
                                await Dispatcher.InvokeAsync(() =>
                                {
                                    GameStatus.Text = lm.Translate("Home Page") + " (" + FormatTime(playing_time) + ")";
                                    GameID.Text = lm.Translate("Game ID: None");
                                });
                            }
                            else
                            {
                                await Dispatcher.InvokeAsync(() =>
                                {
                                    GameStatus.Text = lm.Translate("Waiting for user to join a game.");
                                    GameID.Text = lm.Translate("Game ID: None");
                                });
                            }
                        }
                    }
                    catch
                    {

                    }
                });

                await Task.Delay(1600);
            }
        }


        bool scriptlist_animating;
        private async void AI_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (consolegrid_animating)
                return;

            scriptlist_animating = true;
            if (ScriptListBorder2.Width == 0)
            {
                DoubleAnimation Anims = new DoubleAnimation()
                {
                    From = 0,
                    To = 175,
                    Duration = TimeSpan.FromSeconds(0.6),
                    EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
                };

                ThicknessAnimation Anims2 = new ThicknessAnimation()
                {
                    From = new Thickness(0, 40, 0, NotListborder2.Margin.Bottom),
                    To = new Thickness(175, 40, 0, NotListborder2.Margin.Bottom),
                    Duration = TimeSpan.FromSeconds(0.6),
                    EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
                };

                ThicknessAnimation Anims3 = new ThicknessAnimation()
                {
                    From = new Thickness(0),
                    To = new Thickness(175, 0, 0, 0),
                    Duration = TimeSpan.FromSeconds(0.6),
                    EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
                };

                ScriptListBorder2.BeginAnimationP(WidthProperty, Anims);

                NotListborder2.BeginAnimationP(MarginProperty, Anims2);
                ConsoleGrid2.BeginAnimationP(MarginProperty, Anims3);
            }
            else if (ScriptListBorder2.Width == 175)
            {
                DoubleAnimation Anims = new DoubleAnimation()
                {
                    From = 175,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(0.6),
                    EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
                };

                ThicknessAnimation Anims2 = new ThicknessAnimation()
                {
                    From = new Thickness(175, 40, 0, NotListborder2.Margin.Bottom),
                    To = new Thickness(0, 40, 0, NotListborder2.Margin.Bottom),
                    Duration = TimeSpan.FromSeconds(0.6),
                    EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
                };

                ThicknessAnimation Anims3 = new ThicknessAnimation()
                {
                    From = new Thickness(175, 0, 0, 0),
                    To = new Thickness(0),
                    Duration = TimeSpan.FromSeconds(0.6),
                    EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
                };

                ScriptListBorder2.BeginAnimationP(WidthProperty, Anims);

                NotListborder2.BeginAnimationP(MarginProperty, Anims2);
                ConsoleGrid2.BeginAnimationP(MarginProperty, Anims3);
            }

            await Task.Delay(500);
            scriptlist_animating = false;
        }

        bool consolegrid_animating;
        private async void Console_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (scriptlist_animating)
                return;

            consolegrid_animating = true;
            if (ConsoleGrid2.Height == 0)
            {
                DoubleAnimation Anims = new DoubleAnimation()
                {
                    From = 0,
                    To = 140,
                    Duration = TimeSpan.FromSeconds(0.6),
                    EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
                };

                ThicknessAnimation Anims2 = new ThicknessAnimation()
                {
                    From = new Thickness(NotListborder2.Margin.Left, 40, 0, 0),
                    To = new Thickness(NotListborder2.Margin.Left, 40, 0, 140),
                    Duration = TimeSpan.FromSeconds(0.6),
                    EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
                };

                ConsoleGrid2.BeginAnimationP(HeightProperty, Anims);
                NotListborder2.BeginAnimationP(MarginProperty, Anims2);
            }
            else if (ConsoleGrid2.Height == 140)
            {
                DoubleAnimation Anims = new DoubleAnimation()
                {
                    From = 140,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(0.6),
                    EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
                };

                ThicknessAnimation Anims2 = new ThicknessAnimation()
                {
                    From = new Thickness(NotListborder2.Margin.Left, 40, 0, 140),
                    To = new Thickness(NotListborder2.Margin.Left, 40, 0, 0),
                    Duration = TimeSpan.FromSeconds(0.6),
                    EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
                };

                ConsoleGrid2.BeginAnimationP(HeightProperty, Anims);
                NotListborder2.BeginAnimationP(MarginProperty, Anims2);
            }

            await Task.Delay(500);
            consolegrid_animating = false;
        }

        private void textoIA_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (textoIA.Text == lm.Translate("TextoIA"))
            {
                textoIA.Clear();
            }
        }






        private TextBox USER_INPUT()
        {
            Border ermm = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(20, 20, 20)),
                CornerRadius = new CornerRadius(10),
                Margin = new Thickness(0, 15, 5, 15),
            };

            Grid grid0 = new Grid();

            try
            {
                grid0 = new Grid
                {
                    Width = AssistentGrid.ActualWidth - 25
                };
            }
            catch
            {
                grid0 = new Grid
                {
                    Width = 660 - 25
                };
            }

            AIRapeYou.SizeChanged += delegate
            {
                try { grid0.Width = AIRapeYou.ActualWidth - 10; } catch { }
            };

            Border iconBorder = new Border
            {
                Width = 30,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 5, 5, 5),
                Clip = new RectangleGeometry
                {
                    Rect = new Rect(0, 0, 30, 30),
                    RadiusX = 30,
                    RadiusY = 30
                }
            };

            Image image = new Image
            {
                Source = new BitmapImage(new Uri(UserAvatar2)),
                Width = 30
            };

            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
            RenderOptions.SetEdgeMode(image, EdgeMode.Aliased);


            iconBorder.Child = image;
            grid0.Children.Add(iconBorder);

            TextBox textBlock = new TextBox
            {
                Text = "",
                FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Essence;component/Fonts/#Poppins Light"),
                FontSize = 13,
                Margin = new Thickness(0, 5, 40, 5),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = new SolidColorBrush(Color.FromRgb(0xDC, 0xDC, 0xDC)),
                TextWrapping = TextWrapping.Wrap,
                BorderThickness = new Thickness(0),
                BorderBrush = null,
                Background = null,
                CaretBrush = null,
                Style = null
            };

            grid0.Children.Add(textBlock);

            ermm.Child = grid0;
            AIRapeYou.Children.Add(ermm);

            return textBlock;
        }


        private TextBox AI_RESPONSE()
        {
            Border ermm = new Border
            {
                //Background = new SolidColorBrush(Color.FromRgb(20, 20, 20)),
                CornerRadius = new CornerRadius(10),
                Margin = new Thickness(5, 0, 0, 0)
            };

            Grid grid7 = new Grid();

            try
            {
                grid7 = new Grid
                {
                    Width = AssistentGrid.ActualWidth - 25
                };
            }
            catch
            {
                grid7 = new Grid
                {
                    Width = 660 - 25
                };
            }

            AIRapeYou.SizeChanged += delegate
            {
                try { grid7.Width = AIRapeYou.ActualWidth - 10; } catch { }
            };

            Border iconBorder = new Border
            {
                Width = 30,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, 5, 0, 5),
                Clip = new RectangleGeometry
                {
                    Rect = new Rect(0, 0, 30, 30),
                    RadiusX = 30,
                    RadiusY = 30
                }
            };

            Image image = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/essence.png")),
                Width = 30
            };

            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
            RenderOptions.SetEdgeMode(image, EdgeMode.Aliased);
            iconBorder.Child = image;

            grid7.Children.Add(iconBorder);

            TextBox textBlock8 = new TextBox
            {
                TextWrapping = TextWrapping.Wrap,
                FontFamily = new FontFamily(new Uri("pack://application:,,,/Essence;component/Fonts/"), "#Poppins Light"),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 13,
                TextAlignment = TextAlignment.Left,
                Foreground = Brushes.White,
                Text = "Fowarding Request",
                Margin = new Thickness(40, 5, 0, 5),
                BorderThickness = new Thickness(0),
                BorderBrush = null,
                Background = null,
                CaretBrush = null,
                Style = null
            };
            grid7.Children.Add(textBlock8);

            ermm.Child = grid7;
            AIRapeYou.Children.Add(ermm);
            return textBlock8;
        }







        private static string[] links = { KeyGay2.url + "IA" };
        private bool dnwdnq3iudn = false;

        private bool IA_Escrevendo;
        WebViewA AIeditor;
        private async void IA(string input)
        {
            if (IA_Escrevendo)
                return;
            IA_Escrevendo = true;

            fuhfufua.Visibility = Visibility.Hidden;
            AI_writing.Visibility = Visibility.Visible;
            textoIA.IsEnabled = false;
            textoIA.Text = "";


            string current_text = "";
            if (duh27dh73h.IsChecked == true)
                current_text = "[CURRENT TEXT EDIOR SCRIPT]\r\n" + await GetEditorText(CurrentTabWithStuff());

            if (current_text.Length > 500)
                current_text = "[CURRENT TEXT EDIOR SCRIPT]\r\n" + $"[SYSTEM WARNING]: User Text Editor lenght is Too Long to load here. (the max lenght is 500. But current is {current_text.Length})";


            ///////////////////////////USER INPUT
            TextBox user = USER_INPUT();
            user.Text = input;
            ////////////////////////////////AI RESPONSE
            TextBox aitext = AI_RESPONSE();


            if (!dnwdnq3iudn)
            {
                aitext.Text = "";
                dnwdnq3iudn = true;

                Fade(user_input_border_ai, 1, 0, 0.3);
                Fade(dbnduwqnduiwanduiwanu, 1, 0, 0.3);
                Fade(ieanfjeanfifnuineufenafn, 1, 0, 0.3);
                Fade(ieanfjeanfifnuineufenafn2, 1, 0, 0.3);
                Fade(qunfu23fhndu73ndn, 1, 0, 0.3);

                dd383nf82n428f.Visibility = Visibility.Visible;

                await Task.Delay(300);
                //user_input_border_ai.VerticalAlignment = VerticalAlignment.Bottom;
                //user_input_border_ai.Margin = new Thickness(8, 120, 8, 8);
                Fade(user_input_border_ai, 0, 1, 0.4);

                ai_normal_response_border.Visibility = Visibility.Visible;
                Fade(ai_normal_response_border, 0, 1, 0.7);
            }

            dwindfikwdmnkwdmwm.Width = new GridLength(0, GridUnitType.Star);
            dhjdcu3j8j8f82jf4jf4jfi8.Width = new GridLength(1, GridUnitType.Star);
            expandedddd = 1;
            cj832f82f82fjn.Text = "1";

            try
            {
                var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(10));

                Random random = new Random();
                //var uri = new Uri(links[random.Next(0, links.Length)]);
                string uri = "https://M4A132-mixtral-46-7b-fastapi2.hf.space/generate3/";
                string history = "";

                if (SaveH)
                {
                    try
                    {
                        StreamReader st = new StreamReader("C:\\Essence\\AI_history.txt");
                        history = st.ReadToEnd();
                        st.Close();
                    }
                    catch { }
                }

                List<(string, string)> his = new List<(string, string)>
                {
                    ("[SYSTEM] Conversation History:\n", history)
                };

                var item = new
                {
                    prompt = current_text + "[USER]" + Environment.NewLine + input + " [/INST]",
                    history = his,
                    idioma = "en"
                };

                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("M4A1-KEY", KeyGay2.Gen()[0]);
                    client.DefaultRequestHeaders.Add("M4A1-N", KeyGay2.Gen()[1]);

                    var message = new HttpRequestMessage(HttpMethod.Post, uri);
                    message.Content = content;

                    qunfu23fhndu73ndn.Text = lm.Translate("Making Request...");
                    var response = await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationTokenSource.Token);
                    var stream = await response.Content.ReadAsStreamAsync();

                    using (var reader = new StreamReader(stream))
                    {
                        await Task.Run(async () =>
                        {
                            string line;
                            await Dispatcher.InvokeAsync(() =>
                            {
                                qunfu23fhndu73ndn.Text = lm.Translate("AI is thinking...");
                            });

                            bool isScripting = false;
                            string kkkk = "";

                            await Dispatcher.InvokeAsync(async () =>
                            {
                                aitext.Text = "";
                                await SetEditorTextAsync(AIeditor, "");
                            });

                            bool nefdnqufnq = false;
                            while ((line = await reader.ReadLineAsync()) != null)
                            {
                                line = line.Replace("</s>", "");
                                kkkk += line;

                                if (line.Contains("`lua"))
                                    isScripting = true;
                                else if (line.Contains("```"))
                                    isScripting = false;

                                if (isScripting)
                                {
                                    await Dispatcher.InvokeAsync(async () =>
                                    {
                                        if (!nefdnqufnq && expandedddd != 2)
                                        {
                                            nefdnqufnq = true;
                                            dwindfikwdmnkwdmwm.Width = new GridLength(1.5, GridUnitType.Star);
                                            dhjdcu3j8j8f82jf4jf4jfi8.Width = new GridLength(1, GridUnitType.Star);
                                            expandedddd = 2;
                                            cj832f82f82fjn.Text = "2";
                                        }

                                        await SetEditorTextAsync(AIeditor, await GetEditorText(AIeditor) + line + Environment.NewLine);
                                    });
                                }
                                else
                                {
                                    await Dispatcher.InvokeAsync(async () =>
                                    {
                                        line = line.Replace("```", Environment.NewLine + Environment.NewLine + "[SCRIPT GENERATED]" + Environment.NewLine + Environment.NewLine);
                                        foreach (char c in line)
                                        {
                                            aitext.Text += c;
                                            await Task.Delay(8);
                                        }
                                    });

                                    await Task.Delay(1000);
                                }
                            }

                            await Dispatcher.InvokeAsync(() =>
                            {
                                aitext.Text = aitext.Text.Trim();
                                aitext.Text = aitext.Text.Trim('\n');
                            });

                            await Task.Delay(500);

                            try
                            {
                                await Dispatcher.InvokeAsync(async () =>
                                {
                                    string text = await GetEditorText(AIeditor);
                                    text = text.Replace("Mia: ", "").Replace("```lua", "").Replace("```", "").Trim();
                                    await SetEditorTextAsync(AIeditor, text);
                                });
                            }
                            catch
                            {
                                textoIA.IsEnabled = true;
                                fuhfufua.Visibility = Visibility.Visible;
                                AI_writing.Visibility = Visibility.Collapsed;
                            }


                            await Dispatcher.InvokeAsync(async () =>
                            {
                                try
                                {
                                    int scriptStartIndex = kkkk.IndexOf("```lua");
                                    int scriptEndIndex = kkkk.IndexOf("```", scriptStartIndex + 6);

                                    string text = kkkk.Substring(0, scriptStartIndex).Trim();
                                    string script = kkkk.Substring(scriptStartIndex + 6, scriptEndIndex - scriptStartIndex - 6).Trim();

                                    aitext.Text = text.Trim();
                                    await SetEditorTextAsync(AIeditor, script);
                                }
                                catch { }
                            });
                            

                            if (SaveH)
                            {
                                try
                                {
                                    using (StreamWriter sw = new StreamWriter("C:\\Essence\\AI_history.txt", true))
                                    {
                                        sw.WriteLine("[USER]");
                                        sw.WriteLine(input);

                                        sw.WriteLine("\n[MIA]");
                                        sw.WriteLine(kkkk + "\n\n\n");
                                    }
                                }
                                catch { }
                            }
                        });
                    }
                }


            }
            catch (TaskCanceledException)
            {
                qunfu23fhndu73ndn.Text = "";
                aitext.Text = "Error";
                Notificar("Há muitas pessoas usando a IA no momento. Tente novamente mais tarde", 6);
            }
            catch (HttpRequestException)
            {
                qunfu23fhndu73ndn.Text = "";
                aitext.Text = "Error";
                Notificar("Erro ao realizar a requisição HTTP. Verifique sua conexão com a internet e tente novamente.", 5);
            }
            catch (Exception ex)
            {
                qunfu23fhndu73ndn.Text = "";
                aitext.Text = "Error";
                MessageBox.Show(ex.ToString());
            }

            IA_Escrevendo = false;
            textoIA.IsEnabled = true;
            fuhfufua.Visibility = Visibility.Visible;
            AI_writing.Visibility = Visibility.Collapsed;

            textoIA.Focus();
        }

        //void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        //{
        //    EVXTXT.Text = e.VerticalChange.ToString() + "  |  " + GeneralScriptScrollViewer.VerticalOffset.ToString() + " + " + GeneralScriptScrollViewer.ViewportHeight.ToString() + " >= " + GeneralScriptScrollViewer.ExtentHeight.ToString();
        //}

        private void textoIA_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                IA(textoIA.Text);
            }
        }

        private void textoIA_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (inicializado)
            {
                if (textoIA.Text.Length < 1)
                {
                    ff2f323f2f.Visibility = Visibility.Visible;
                    rgw4btwnb.Fill = new SolidColorBrush(Color.FromRgb(30, 30, 30));
                    rgw4btwnb.IsHitTestVisible = false;
                }
                else
                {
                    ff2f323f2f.Visibility = Visibility.Collapsed;
                    rgw4btwnb.Fill = new SolidColorBrush(Color.FromRgb(240, 240, 240));
                    rgw4btwnb.IsHitTestVisible = true;
                }
            }
        }


        private async void change_img_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Wolfregkey.DeleteValue("Avatar");


                ImageBehavior.SetAnimatedSource(User_Img, null);
                User_Img.Source = null;
                User_Img.Source = Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.essence.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            catch { }

            await Task.Delay(600);

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = "Select an Image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string destinationDirectory = @"C:/Essence/userdata\\UserImgs";
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                string destinationFilePath = Path.Combine(destinationDirectory, Path.GetFileName(openFileDialog.FileName));

                try
                {
                    File.Copy(openFileDialog.FileName, destinationFilePath, true);
                    ImageBehavior.SetAnimatedSource(User_Img, new BitmapImage(new Uri(destinationFilePath, UriKind.Absolute)));

                    Wolfregkey.SetValue("Avatar", destinationFilePath);
                    Wolfregkey.SetValue("DiscordAvatarSync", "False");
                }
                catch
                {

                }
            }

        }


        LanguageManager lm;
        private void Idioma_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!inicializado)
                return;
            try
            {
                switch (Idioma.SelectedIndex + 1)
                {
                    case 1:
                        Wolfregkey.SetValue("Idioma", "EN");
                        break;

                    case 2:
                        Wolfregkey.SetValue("Idioma", "PT");
                        break;

                    case 3:
                        Wolfregkey.SetValue("Idioma", "ES");
                        break;
                }

                lm.lang = Idioma.SelectedIndex + 1;
                TransL();
            }
            catch { }
        }

        private async void TransL()
        {
            try
            {
                Execute_T.Text = lm.Translate("Execute");
                Open_T.Text = lm.Translate("Open");
                Save_T.Text = lm.Translate("Save");
                Clear_T.Text = lm.Translate("Clear");



                GeneralScriptSearch3.Text = lm.Translate("Search Scripts");
                ScriptCloud_T.Text = lm.Translate("Script Cloud");
                GamesScriptsTXT.Text = lm.Translate("Games you play");
                games_no_data_warn.Text = lm.Translate("No enough data. play some games using Essence first!");
                CommunityScriptsTXT.Text = lm.Translate("Community Choice");
                SavedScriptsTXT.Text = lm.Translate("Saved Scripts");
                NewScriptsTXT.Text = lm.Translate("New Scripts");
                NewScripts2TXT.Text = lm.Translate("Less relevant results");
                //Refresh_T.Text = lm.Translate("Refresh");



                QuickPanel_T.Text = lm.Translate("Quick Panel");
                QuickPanel_Desc.Text = lm.Translate("Change your character properties, use visuals and aimbots");
                Game_T.Text = lm.Translate("Game");
                ServerHop_T.Text = lm.Translate("Server Hop");
                Fly_T.Text = lm.Translate("Fly");
                Noclip_T.Text = lm.Translate("No Clip");


                //MiaAssistent_T.Text = lm.Translate("Mia Assistent");
                MiaAssistent_Desc.Text = lm.Translate("Hi! Need script help? Let's begin!");
                ff2f323f2f.Content = lm.Translate("Talk to assistent here");



                Settings_T.Text = lm.Translate("Settings");
                EssenceCustomization_T.Text = lm.Translate("Essence Customization");
                EssenceCustomization_Desc.Text = lm.Translate("Customize your Essence experience");
                T_Idioma.Text = lm.Translate("Language");
                Language_TT.Text = lm.Translate("Change your executor language.");
                DiscordRPC_T.Text = lm.Translate("Discord RPC");
                TT_Discord_RPC.Text = lm.Translate("Enable discord activity for Essence. Yay!");
                T_Topo_das_janelas.Text = lm.Translate("Top-Most the window");
                TT_Topo_das_janelas.Text = lm.Translate("Keep Essence on top of other windows");
                Roblox_Desc.Text = lm.Translate("Quick things you can change");

                T_Idioma22.Text = lm.Translate("Save conversations");
                SaveConversations_Desc.Text = lm.Translate("This option saves and loads all your chat history with Mia.");
                T_Idioma222.Text = lm.Translate("Clear History");
                ClearHistory_Desc.Text = lm.Translate("Delete all records of conversations with Mia");

                defef.Text = lm.Translate("Monaco is an advanced editor. But, it can cause performance issues in some pc's");
                GREdsdadfsfG.Text = lm.Translate("Save Tabs");
                dedsssfsdfef.Text = lm.Translate("Save your script tabs to re-load then when you open Essence again");








                if (Wolfregkey.GetValueNames().Contains("Nome"))
                    User_config.Text = lm.Translate("Hello") + ", " + Wolfregkey.GetValue("Nome").ToString() + "!";
                else
                    User_config.Text = lm.Translate("Hello User");
            }
            catch
            {
                await Task.Delay(1000);
                TransL();
            }
        }

        //public static readonly DependencyProperty ScriptStringVal = DependencyProperty.Register("ScriptString", typeof(ScriptDetails), typeof(Elements.ScriptThingy));

        public struct ScriptDetails // String for the script itself
        {
            public string ScriptExecute;
        }

        //public ScriptDetails SetStringValue
        //{
        //    get => (ScriptDetails)GetValue(ScriptStringVal);
        //    set => SetValue(ScriptStringVal, value);
        //}

        private void SearchScriptHub(object sender, RoutedEventArgs e)
        {
            Process.Start("https://scriptblox.com/");
        }

        private void GeneralScriptSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (GeneralScriptSearch3.Text == lm.Translate("Search Scripts"))
            {
                GeneralScriptSearch3.Text = "";
                GeneralScriptSearch3.CaretBrush = new SolidColorBrush(Color.FromArgb(100, 137, 137, 137));
            }
        }
        //private void GeneralScriptSearch_GotFocus(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    if (GeneralScriptSearch3.Text == lm.Translate("Digite o script aqui."))
        //    {
        //        GeneralScriptSearch3.Text = "";
        //        GeneralScriptSearch3.CaretBrush = new SolidColorBrush(Color.FromArgb(100, 137, 137, 137));
        //    }
        //}

        private void GeneralScriptSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (GeneralScriptSearch3.Text == "")
            //{
            //    GeneralScriptSearch3.Text = lm.Translate("Digite o script aqui.");
            //    GeneralScriptSearch3.CaretBrush = new SolidColorBrush(Color.FromArgb(0, 137, 137, 137));
            //}
        }

        //private void GeneralScriptSearch_LostFocus(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    if (GeneralScriptSearch3.Text == "")
        //    {
        //        GeneralScriptSearch3.Text = lm.Translate("Digite o script aqui.");
        //        GeneralScriptSearch3.CaretBrush = new SolidColorBrush(Color.FromArgb(0, 137, 137, 137));
        //    }
        //}

        private void GeneralScriptSearch_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != Key.Return)
                return;

            this.SearchScripts2(this.GeneralScriptSearch3.Text);
        }

        private void Image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pages = 0;
            WP.Children.Clear();
            WP.Visibility = Visibility.Collapsed;
            NewScriptsTXT.Visibility = Visibility.Collapsed;

            WP0.Children.Clear();
            WP0.Visibility = Visibility.Collapsed;
            NewScripts2TXT.Visibility = Visibility.Collapsed;

            WP2.Children.Clear();
            WP2.Visibility = Visibility.Collapsed;
            CommunityScriptsTXT.Visibility = Visibility.Collapsed;
            WP3.Children.Clear();

            this.SearchScripts2(this.GeneralScriptSearch3.Text);
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
            Storyboard.SetTargetProperty(Anims, new PropertyPath(OpacityProperty));
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


        //private void Account_Info_Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (AccountBorder.Height == 165)
        //    {
        //        DoubleAnimation Anims = new DoubleAnimation()
        //        {
        //            From = 165,
        //            To = 45,
        //            Duration = TimeSpan.FromSeconds(0.6),
        //            EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
        //        };

        //        AccountBorder.BeginAnimationP(HeightProperty, Anims);
        //    }
        //    else if (AccountBorder.Height == 45)
        //    {
        //        DoubleAnimation Anims = new DoubleAnimation()
        //        {
        //            From = 45,
        //            To = 165,
        //            Duration = TimeSpan.FromSeconds(0.6),
        //            EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
        //        };

        //        AccountBorder.BeginAnimationP(HeightProperty, Anims);
        //    }
        //}

        //private void Settings_Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (SettingBorder.Height == 200)
        //    {
        //        DoubleAnimation Anims = new DoubleAnimation()
        //        {
        //            From = 200,
        //            To = 45,
        //            Duration = TimeSpan.FromSeconds(0.8),
        //            EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
        //        };

        //        SettingBorder.BeginAnimationP(HeightProperty, Anims);
        //    }
        //    else if (SettingBorder.Height == 45)
        //    {
        //        DoubleAnimation Anims = new DoubleAnimation()
        //        {
        //            From = 45,
        //            To = 200,
        //            Duration = TimeSpan.FromSeconds(0.8),
        //            EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
        //        };

        //        SettingBorder.BeginAnimationP(HeightProperty, Anims);
        //    }
        //}



        private void AdminButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (AdminPanel.Height == 100)
            {
                DoubleAnimation Anims = new DoubleAnimation()
                {
                    From = 100,
                    To = 45,
                    Duration = TimeSpan.FromSeconds(0.8),
                    EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
                };

                AdminPanel.BeginAnimationP(HeightProperty, Anims);
            }
            else if (AdminPanel.Height == 45)
            {
                DoubleAnimation Anims = new DoubleAnimation()
                {
                    From = 45,
                    To = 100,
                    Duration = TimeSpan.FromSeconds(0.8),
                    EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
                };

                AdminPanel.BeginAnimationP(HeightProperty, Anims);
            }
        }






        public static string RandomId(int length)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string result = "";
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                result += chars[random.Next(chars.Length)];
            }

            return result;
        }


        public static string RandomMac()
        {
            string chars = "ABCDEF0123456789";
            string windows = "26AE";
            string result = "";
            Random random = new Random();

            result += chars[random.Next(chars.Length)];
            result += windows[random.Next(windows.Length)];

            for (int i = 0; i < 5; i++)
            {
                result += "-";
                result += chars[random.Next(chars.Length)];
                result += chars[random.Next(chars.Length)];

            }

            return result;
        }


        List<string> adapters0 = new List<string>();
        public void Enable_LocalAreaConection(string adapterId, bool enable = true, string interfaceName = null)
        {
            if (interfaceName == null)
            {
                interfaceName = "Ethernet";
                foreach (NetworkInterface i in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (i.Id == adapterId)
                    {
                        interfaceName = i.Name;
                        break;
                    }
                }
            }

            string control;
            if (enable)
                control = "enable";
            else
            {
                control = "disable";
                adapters0.Add(interfaceName);
            }


            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("netsh", $"interface set interface \"{interfaceName}\" {control}");
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo = psi;
            p.Start();
            p.WaitForExit();
        }


        //CancellationTokenSource cancellationTokenSource;
        //private bool Spoofer_open = false;
        private async void OpenSpooferBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("Spoofer in maintence");
            //return;

            //if (!Spoofer_open)
            //{
            //    DoubleAnimation anim = new DoubleAnimation
            //    {
            //        From = 0,
            //        To = 720,
            //        Duration = new Duration(TimeSpan.FromSeconds(1)),
            //        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut },
            //    };
            //    Spoofer.BeginAnimationP(WidthProperty, anim);
            //    Spoofer.Visibility = Visibility.Visible;
            //    await Task.Delay(500);
            //    Spoofer_open = true;
            //    try
            //    {
            //        cancellationTokenSource = new CancellationTokenSource();
            //        CancellationToken cancellationToken = cancellationTokenSource.Token;
            //        Open_Spoofer(cancellationToken);
            //    }
            //    catch
            //    {
            //        //System.Windows.MessageBox.Show("OK");
            //    }
            //}
            //else
            //{
            //    if (spoofing)
            //    {
            //        cancellationTokenSource.Cancel();

            //        await Task.Delay(1000);
            //        SpooferStatus2.Content = "Stoping...";
            //        Stop_Spoofing.Content = "Stoping...";

            //        await Task.Delay(2500);
            //        SpooferStatus2.Content = "Stoping...";

            //        if (Spoofer_open)
            //        {
            //            DoubleAnimation anim = new DoubleAnimation
            //            {
            //                From = Spoofer.Width,
            //                To = 0,
            //                Duration = new Duration(TimeSpan.FromSeconds(1)),
            //                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut },
            //            };

            //            Spoofer.BeginAnimationP(WidthProperty, anim);
            //            await Task.Delay(1000);
            //            Spoofer_open = false;
            //            Spoofer.Visibility = Visibility.Collapsed;
            //        }

            //    }
            //    else
            //    {
            //        DoubleAnimation anim = new DoubleAnimation
            //        {
            //            From = Spoofer.Width,
            //            To = 0,
            //            Duration = new Duration(TimeSpan.FromSeconds(1)),
            //            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut },
            //        };

            //        Spoofer.BeginAnimationP(WidthProperty, anim);
            //        await Task.Delay(1000);
            //        Spoofer_open = false;
            //        Spoofer.Visibility = Visibility.Collapsed;
            //    }
            //}
        }



        internal static Color GetPixelColor(BitmapImage bitmapImage)
        {
            try
            {
                System.Drawing.Bitmap bitmap;
                using (MemoryStream outStream = new MemoryStream())
                {
                    BitmapEncoder enc = new BmpBitmapEncoder();
                    enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                    enc.Save(outStream);
                    bitmap = new System.Drawing.Bitmap(outStream);
                }

                long sumR = 0;
                long sumG = 0;
                long sumB = 0;
                int totalPixels = 0;

                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        System.Drawing.Color pixelColor = bitmap.GetPixel(x, y);
                        sumR += pixelColor.R;
                        sumG += pixelColor.G;
                        sumB += pixelColor.B;
                        totalPixels++;
                    }
                }

                int avgR = (int)(sumR / totalPixels);
                int avgG = (int)(sumG / totalPixels);
                int avgB = (int)(sumB / totalPixels);

                return Color.FromRgb((byte)avgR, (byte)avgG, (byte)avgB);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Color.FromRgb(100, 100, 100);
            }
        }




        private void Image_PreviewMouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            IA(textoIA.Text);
        }

        private void MainWin_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            WWWW = Width;
            HHHH = Height;

            DoubleAnimation fuck1 = new DoubleAnimation()
            {
                From = 0,
                To = WindowBorder.ActualWidth - 100,
                Duration = TimeSpan.FromMilliseconds(100),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };

            //PlayerModifiersBorder.Visibility = Visibility.Hidden;
            //CommumScriptsBorder.Visibility = Visibility.Hidden;

            //GameInfoBorder.BeginAnimationP(WidthProperty, fuck1);

            ////PlayerModifiersBorder.Visibility = Visibility.Visible;
            //PlayerModifiersBorder.BeginAnimationP(WidthProperty, fuck1);

            ////CommumScriptsBorder.Visibility = Visibility.Visible;
            //CommumScriptsBorder.BeginAnimationP(WidthProperty, fuck1);
        }

        internal static double WWWW;
        internal static double HHHH;

        private async void Speed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Speedlabel.Text = lm.Translate("Set WalkSpeed ") + (int)SpeedSlider.Value;

            if (e.NewValue * 500.0 < 1.0)
            {
                return;
            }
            if (!inj4)
            {
                //T_Velocidade1.Text = lm.Translate("Velocidade") + ": null";
                return;
            }
            try
            {
                File.Delete(api2.workspace + "\\Essence\\Speed.evolu");
            }
            catch
            {
            }

            speedlk = true;
            //T_Velocidade1.Text = lm.Translate("Velocidade") + ": " + (int)(e * 500.0);

            //EXECUTAR("_G.EvoEnvSpeedLock = nil\ntask.wait(1.5)\n" + Properties.Resources.SpeedLock.Replace("777", ((int)e.NewValue).ToString()), true);

            //speedlk_value = (int)e.NewValue;

            //EXECUTAR("game.Players.LocalPlayer.Character.Humanoid.WalkSpeed = " + e.NewValue);

            try
            {
                File.Delete(api2.workspace + "\\Essence\\Speed.evolu");
            }
            catch
            {
            }
        }

        private async void Jump_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Jumplabel.Text = lm.Translate("Set JumpPower ") + (int)JumpSlider.Value;

            if (!inj4)
            {
                return;
            }
            try
            {
                File.Delete(api2.workspace + "\\Essence\\Jump.evolu");
            }
            catch
            {
            }

            jumplk = true;

            //T_Salto1.Text = lm.Translate("Força do pulo") + ": " + (int)(e * 500.0);

            //EXECUTAR("_G.EvoEnvJumpLock = nil\ntask.wait(1.5)\n" + Properties.Resources.SpeedLock.Replace("666", ((int)e.NewValue).ToString()), true);

            //EXECUTAR("game.Players.LocalPlayer.Character.Humanoid.JumpPower = " + e.NewValue);


            try
            {
                File.Delete(api2.workspace + "\\Essence\\Jump.evolu");
            }
            catch
            {
            }
        }
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try { await NewTabAsync(); } catch { }
        }


        //private void Executor_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if(e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        DragDrop.DoDragDrop
        //    }
        //}

        //private void MainWindow_DragEnter(object sender, DragEventArgs e)
        //{
        //    MessageBox.Show("e");
        //    if (e.Data.GetDataPresent(DataFormats.FileDrop))
        //    {
        //        e.Effects = DragDropEffects.Copy;
        //    }
        //    else
        //    {
        //        e.Effects = DragDropEffects.None;
        //    }
        //}

        //private void MainWindow_DragOver(object sender, DragEventArgs e)
        //{
        //    MessageBox.Show("sse");

        //    if (e.Data.GetDataPresent(DataFormats.FileDrop))
        //    {
        //        EVXTXT.Text = "CB3Y7RBN3URN3UYCVN3UYNCRVN";
        //        e.Handled = true;
        //    }
        //}

        //private void MainWindow_Drop(object sender, DragEventArgs e)
        //{
        //    MessageBox.Show("2222e");

        //    if (e.Data.GetDataPresent(DataFormats.FileDrop))
        //    {
        //        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

        //        if (files.Length > 0)
        //        {
        //            string filePath = files[0];

        //            try
        //            {
        //                string content = File.ReadAllText(filePath);
        //                MessageBox.Show($"Conteúdo do arquivo:\n{content}", "Conteúdo do Arquivo", MessageBoxButton.OK, MessageBoxImage.Information);
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show($"Erro ao ler o arquivo: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //        }
        //    }
        //}





        private async Task CreateTabsFire()
        {
            await Task.Run(async delegate
            {
                //try
                //{
                if (Directory.Exists("C:\\Essence\\userdata\\EvoTabs") && Directory.GetFiles("C:\\Essence\\userdata\\EvoTabs").Length != 0)
                {

                    await Dispatcher.InvokeAsync(async () =>
                    {
                        string mainTabFilePath = "C:\\Essence\\userdata\\EvoTabs\\Main Script.Evo";
                        if (File.Exists(mainTabFilePath))
                        {
                            string nome = System.IO.Path.GetFileNameWithoutExtension(mainTabFilePath);
                            StreamReader sr = new StreamReader(mainTabFilePath);
                            string script = sr.ReadToEnd();
                            sr.Close();

                            if (pri)
                            {
                                pri = false;
                                try { await NewTabAsync(script, "Main Script", true, loading_multiple_tabs: true); } catch { }
                                sr.Close();

                                await Dispatcher.InvokeAsync(() =>
                                {
                                    var addButton = TabControl.GetTemplateItem<System.Windows.Controls.Button>("AddTabButton");
                                    if (addButton != null)
                                    {
                                        addButton.Click += AddButton_Click;
                                    }
                                });
                            }
                        }

                        string[] files = Directory.GetFiles("C:\\Essence\\userdata\\EvoTabs");
                        foreach (string path in files)
                        {
                            InternalConsolePrint("[TABS] Loading Previous Tabs...", console_RichTextBox, Colors.Gray);

                            if (path.Contains(".Evo") && System.IO.Path.GetFileNameWithoutExtension(path) != "Main Script")
                            {
                                string nome = System.IO.Path.GetFileNameWithoutExtension(path);
                                StreamReader sr = new StreamReader(path);
                                string script = sr.ReadToEnd();
                                sr.Close();

                                await Task.Delay(200);
                                if (pri)
                                {
                                    pri = false;
                                    try { await NewTabAsync(script, "Main Script", true, loading_multiple_tabs: true); } catch { }
                                    sr.Close();

                                    await Dispatcher.InvokeAsync(() =>
                                    {
                                        var addButton = TabControl.GetTemplateItem<System.Windows.Controls.Button>("AddTabButton");
                                        if (addButton != null)
                                        {
                                            addButton.Click += AddButton_Click;
                                        }
                                    });
                                }
                                else
                                {
                                    try { await NewTabAsync(script, nome, true, loading_multiple_tabs: true); } catch { }
                                    sr.Close();

                                    try
                                    {
                                        File.Delete(path);
                                    }
                                    catch (Exception ex4)
                                    {
                                        Notificar(ex4.Message, 4);
                                    }
                                }
                            }

                            await Task.Delay(200);
                        }
                    });
                    await Dispatcher.InvokeAsync(() =>
                    {
                        TabControl.SelectedIndex = 0;
                    });

                }
                else
                {
                    //ConsolePrint("[TABS] Loading Main Tab...", ConsoleColor.Yellow);
                    await Task.Delay(500);
                    try { await NewTabAsync(title: "Main Script"); } catch (Exception ex) { MessageBox.Show(ex.ToString(), "kkkkkkk"); }

                    if (pri)
                    {
                        pri = false;

                        await Dispatcher.InvokeAsync(() =>
                        {
                            var addButton = TabControl.GetTemplateItem<System.Windows.Controls.Button>("AddTabButton");
                            if (addButton != null)
                            {
                                addButton.Click += AddButton_Click;
                            }
                        });
                    }
                }
            });
        }


        bool pri = true;
        private async void TextEditorLoad(object sender, RoutedEventArgs e)
        {
            while (!inicializado2 || selected_editor == 0 || !finish_start_animations) { await Task.Delay(100); }

            if (!StopAllInteractions)
            {
                await Task.Delay(1000);
                await CreateTabsFire();
                AutoSave();
            }
        }

        public async Task<TabItem> NewTabAsync(string script = "print(\"Hello World\")", string title = "Script", bool tos = false, bool loading_multiple_tabs = false)
        {
            if (TabCount > 9)
            {
                throw new NotImplementedException();
            }

            TabCount++;

            //if (title != "Script")
            //    title = title.Length > 20 ? title.Substring(0, 20) + "..." : title;
            //else
            //    title = title + " " + TabControl.Items.Count;

            if (title == "Script")
                title = title + " " + TabControl.Items.Count;

            //double xxx = 65 + (5.5 * title.Length);

            TabItem tab = null;
            bool loaded = false;

            if (selected_editor == 1)
            {
                WebViewA browser = null;
                await Dispatcher.InvokeAsync(() =>
                {
                    browser = new WebViewA(script);
                    browser.UpdateWindowPos();
                });

                if (tos)
                {
                    browser.ToSetText = script;
                }

                await Dispatcher.InvokeAsync(() =>
                {
                    tab = new TabItem
                    {
                        Content = browser,
                        Style = (TryFindResource("Tab2") as Style),
                        Header = title,
                        FontSize = 12
                    };
                });
            }
            else
            {
                TextEditor browser = null;
                await Dispatcher.InvokeAsync(() =>
                {
                    browser = CreateNewTab();
                    browser.Text = script;
                });

                await Dispatcher.InvokeAsync(() =>
                {
                    tab = new TabItem
                    {
                        Content = browser,
                        Style = (TryFindResource("Tab2") as Style),
                        Header = title
                    };
                });
            }

            await Dispatcher.InvokeAsync(() =>
            {
                tab.Width = 0;

                tab.Loaded += delegate
                {
                    if (!loaded)
                    {
                        loaded = true;
                    }
                };

                tab.Loaded += delegate
                {
                    TabCount--;
                    var closeButton = tab.GetTemplateItem<System.Windows.Controls.Button>("CloseButton");
                    if (closeButton != null)
                    {
                        if (tab.Header.ToString() == "Main Script")
                        {
                            closeButton.Width = 0.0;
                            try
                            {
                                var textbox = tab.GetTemplateItem<System.Windows.Controls.TextBox>("titletxt");
                                textbox.IsReadOnly = true;
                            }
                            catch { }
                        }
                        else
                        {
                            closeButton.Click += async delegate
                            {
                                try
                                {
                                    File.Delete("C:\\Essence\\userdata\\EvoTabs\\" + tab.Header.ToString() + ".Evo");
                                }
                                catch
                                {
                                }

                                await Dispatcher.InvokeAsync(() =>
                                {
                                    DoubleAnimation animation2 = new DoubleAnimation
                                    {
                                        To = 0,
                                        Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                                        EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseOut }
                                    };
                                    tab.BeginAnimationP(WidthProperty, animation2);
                                });

                                await Task.Delay(350);
                                TabControl.Items.Remove(tab);
                            };
                        }
                    }


                    //tab.Width = tab.GetTemplateItem<System.Windows.Controls.TextBox>("titletxt").ActualWidth + 40;
                    Dispatcher.InvokeAsync(() =>
                    {
                        DoubleAnimation animation = new DoubleAnimation
                        {
                            From = 0,
                            To = tab.Header.ToString() == "Main Script" ? 87 : tab.GetTemplateItem<System.Windows.Controls.TextBox>("titletxt").ActualWidth + 40,
                            Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                            EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseIn }
                        };
                        tab.BeginAnimationP(WidthProperty, animation);
                    });

                    loaded = true;
                };

                if (!loading_multiple_tabs)
                    TabControl.SelectedIndex = TabControl.Items.Add(tab);
                else
                    TabControl.Items.Add(tab);

            });

            return tab;
        }

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;

            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindParent<T>(parentObject);
            }
        }

        private void TitleTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var textBox = sender as TextBox;
                if (textBox != null)
                {
                    var tabItem = FindParent<TabItem>(textBox);
                    if (tabItem != null)
                    {
                        tabItem.Header = textBox.Text;
                        UpdateTabWidth(tabItem, textBox);
                        Keyboard.ClearFocus();
                    }
                }
            }
        }

        private void titletxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                var tabItem = FindParent<TabItem>(textBox);
                if (tabItem != null)
                {
                    tabItem.Header = textBox.Text;
                    UpdateTabWidth(tabItem, textBox);
                }
            }
        }

        private void TitleTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                var tabItem = FindParent<TabItem>(textBox);
                if (tabItem != null)
                {
                    tabItem.Header = textBox.Text;
                    UpdateTabWidth(tabItem, textBox);
                }
            }
        }

        private async void UpdateTabWidth(TabItem tabItem, TextBox newWidth)
        {
            //double newWidth = 65 + (5.5 * newHeader.Length);
            //tabItem.Width = newWidth + 40;
            //return;

            await Task.Delay(50);

            DoubleAnimation animation = new DoubleAnimation
            {
                To = newWidth.ActualWidth + 40,
                Duration = new Duration(TimeSpan.FromMilliseconds(50)),
                EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseIn }
            };

            tabItem.BeginAnimation(FrameworkElement.WidthProperty, animation);
        }
















        private void FlyToggle_Click(object sender, RoutedEventArgs e)
        {
            if (inj4)
            {
                EXECUTAR(Essence.Properties.Resources.Fly);

                if (Flybtn.Background.ToString() == "#64C81414")
                    Flybtn.Background = new SolidColorBrush(Color.FromArgb(100, 25, 25, 25));
                else
                    Flybtn.Background = new SolidColorBrush(Color.FromArgb(100, 200, 20, 20));
            }
        }

        private void ClipToggle_Click(object sender, RoutedEventArgs e)
        {
            if (inj4)
            {
                EXECUTAR(Essence.Properties.Resources.noclip);

                if (Clipbtn.Background.ToString() == "#64C81414")
                    Clipbtn.Background = new SolidColorBrush(Color.FromArgb(100, 25, 25, 25));
                else
                    Clipbtn.Background = new SolidColorBrush(Color.FromArgb(100, 200, 20, 20));
            }
        }

        private void EspToggle_Click(object sender, RoutedEventArgs e)
        {
            if (inj4)
            {
                EXECUTAR(Essence.Properties.Resources.ChatBypass);

                if (Bypassbtn.Background.ToString() == "#64C81414")
                    Bypassbtn.Background = new SolidColorBrush(Color.FromArgb(100, 25, 25, 25));
                else
                    Bypassbtn.Background = new SolidColorBrush(Color.FromArgb(100, 200, 20, 20));
            }
        }

        private void FPSToggle_Click(object sender, RoutedEventArgs e)
        {
            if (inj4)
            {
                EXECUTAR(Essence.Properties.Resources.FPS);

                if (FPSbtn.Background.ToString() == "#64C81414")
                    FPSbtn.Background = new SolidColorBrush(Color.FromArgb(100, 25, 25, 25));
                else
                    FPSbtn.Background = new SolidColorBrush(Color.FromArgb(100, 200, 20, 20));
            }
        }

        private bool speedlk = true;
        private int speedlk_value = 8321471;

        private bool jumplk = true;
        private int jumplk_value = 71231;

        private async void ContinueOudated_Click(object sender, RoutedEventArgs e)
        {
            ContinueOudated.IsEnabled = false;
            await Task.Delay(600);

            Notificar("Essence wasn't projected to work on this Roblox version. wait for an update or risk your account.", 20);

            MainWin.BeginAnimationP(UIElement.OpacityProperty, new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.4)));
            RobloxOudated.Visibility = Visibility.Collapsed;
            TabControl.Visibility = Visibility.Visible;
            executor.Visibility = Visibility.Visible;
            MainWin.BeginAnimationP(UIElement.OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.4)));
        }

        private void AdImage_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (ad_redirect)
            //    Process.Start(ads_redirect);
        }


        private CancellationTokenSource _cancellationTokenSource;
        string last_search = "";
        private async void GeneralScriptSearch3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!inicializado)
                return;

            last_search = GeneralScriptSearch3.Text;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            try
            {
                await Task.Delay(900, token);

                if (!token.IsCancellationRequested)
                {
                    //WP.Children.Clear();
                    SearchScripts2(last_search);
                }
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void SaveHistory_Click(object sender, RoutedEventArgs e)
        {
            SaveH = SaveHistory.IsChecked.Value;
            registryKey1.SetValue("SaveH", SaveHistory.IsChecked.Value);
        }

        private void ClearHistory_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (File.Exists("C:\\Essence\\AI_history.txt"))
                {
                    File.Delete("C:\\Essence\\AI_history.txt");
                    AIRapeYou.Children.Clear();
                    Notificar("history deleted");
                }
                else
                    Notificar("history does not exist");
            }
            catch
            {
                Notificar("Error when deleting history");
            }
        }

        //private void AISettings_Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (AISettingBorder.Height == 130)
        //    {
        //        DoubleAnimation Anims = new DoubleAnimation()
        //        {
        //            From = 130,
        //            To = 45,
        //            Duration = TimeSpan.FromSeconds(0.8),
        //            EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
        //        };

        //        AISettingBorder.BeginAnimationP(HeightProperty, Anims);
        //    }
        //    else if (AISettingBorder.Height == 45)
        //    {
        //        DoubleAnimation Anims = new DoubleAnimation()
        //        {
        //            From = 45,
        //            To = 130,
        //            Duration = TimeSpan.FromSeconds(0.8),
        //            EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
        //        };

        //        AISettingBorder.BeginAnimationP(HeightProperty, Anims);
        //    }
        //}

        private async void Path2_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = new DoubleAnimation
            {
                From = WP2.DesiredSize.Height - 35,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseOut }
            };

            WP2.BeginAnimationP(HeightProperty, collapseAnimation);
        }

        private void Launch_Aimbot_Click(object sender, RoutedEventArgs e)
        {

        }



        private void Rejoin_Click(object sender, RoutedEventArgs e)
        {
            EXECUTAR("cloneref(game:GetService(\"TeleportService\")):TeleportToPlaceInstance(game.PlaceId, game.JobId, game:GetService(\"Players\").LocalPlayer)", true);
        }

        private void Rejoin2_Click(object sender, RoutedEventArgs e)
        {
            EXECUTAR("cloneref(game:GetService(\"TeleportService\")):TeleportToPlaceInstance(game.PlaceId, game.JobId, game:GetService(\"Players\").LocalPlayer)", true);
        }


        public static async Task<(int playerCount, string creatorName, string gameName, string imageUrl, string PlaceID)> GetRobloxGameInfo(string LOL = "", long placeid = 0, bool skip = false)
        {
            if (skip || (LOL == "" && placeid == 0))
            {
                //Console.WriteLine($"GetRobloxGameInfo retorned null: SKIP -> {skip}  LOL -> {LOL} PlaceID -> {placeid}");
                return (0, "Null", "Null", "Null", "Null");
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    int playerCount = 0;
                    string universeId = "Null";
                    string gameName = "Null";
                    string creatorName = "Null";
                    string placeidd = "Null";

                    if (placeid != 0)
                    {
                        //MessageBox.Show($"PlaceID: {placeid}");
                        //Console.WriteLine($"PlaceID: {placeid}");


                        string universeUrl = $"https://apis.roblox.com/universes/v1/places/{placeid}/universe";
                        HttpResponseMessage universeResponse = await client.GetAsync(universeUrl);
                        if (!universeResponse.IsSuccessStatusCode)
                            throw new Exception($"Erro ao buscar universeId para placeId {placeid}: {universeResponse.StatusCode}");

                        string universeContent = await universeResponse.Content.ReadAsStringAsync();
                        var universeJson = JObject.Parse(universeContent);
                        universeId = universeJson["universeId"]?.ToString() ?? "Null";

                        if (universeId == "Null")
                            throw new Exception($"Nenhum universeId encontrado para placeId {placeid}");

                        //MessageBox.Show($"UniverseID: {universeId}");
                        //Console.WriteLine($"UniverseID: {universeId}");

                        HttpResponseMessage gameuniverseResponse = await client.GetAsync($"https://games.roblox.com/v1/games?universeIds={universeId}");
                        if (!gameuniverseResponse.IsSuccessStatusCode)
                            throw new Exception($"Erro ao buscar dados do jogo usando universeid {universeId}: {gameuniverseResponse.StatusCode}");

                        string gameuniversecontent = await gameuniverseResponse.Content.ReadAsStringAsync();
                        //Console.WriteLine($"Conteudo: {gameuniversecontent}");

                        JObject jsonDoc = JObject.Parse(gameuniversecontent);

                        if (jsonDoc["data"] is JArray dataArray && dataArray.Count > 0)
                        {
                            var gameData = dataArray[0];

                            placeidd = gameData["rootPlaceId"]?.ToString();
                            Console.WriteLine($"PlaceID: {placeidd}");

                            gameName = gameData["name"]?.ToString();
                            Console.WriteLine($"GameName: {gameName}");

                            playerCount = gameData["playing"]?.ToObject<int>() ?? 0;
                            Console.WriteLine($"PlayerCount: {playerCount}");

                            creatorName = gameData["creator"]?["name"]?.ToString();
                            Console.WriteLine($"CreatorName: {creatorName}");
                        }
                        else
                        {
                            Console.WriteLine("Erro: A propriedade 'data' não contém um array válido ou está vazia.");
                        }
                    }
                    else
                    {
                        LOL = LOL.ToLower();
                        //Console.WriteLine($"Search: {LOL}");
                        string query = "";

                        if (LOL.Length < 5)
                        {
                            string suggestionUrl = $"https://apis.roblox.com/games-autocomplete/v1/get-suggestion/{LOL}";
                            HttpResponseMessage suggestionResponse = await client.GetAsync(suggestionUrl);
                            if (!suggestionResponse.IsSuccessStatusCode)
                            {
                                string suggestionContent = await suggestionResponse.Content.ReadAsStringAsync();
                                //Console.WriteLine($"Suggestions: {suggestionContent}");

                                var suggestionJson = JObject.Parse(suggestionContent);
                                query = suggestionJson["entries"]?.First?["searchQuery"]?.ToString();

                                //if (string.IsNullOrEmpty(query))
                                //    throw new Exception("Nenhuma sugestão encontrada.");

                                //Console.WriteLine($"Autocomplete: {query}");
                            }
                        }
                        else
                            query = LOL;

                        if (query == "")
                            query = LOL;


                        //Console.WriteLine($"Autocomplete: skiped");

                        string searchUrl = $"https://apis.roblox.com/search-api/omni-search?searchQuery={query}&pageToken=&sessionId=90d26885-a556-4362-b4d6-d69ad00bf551&pageType=all";
                        HttpResponseMessage searchResponse = await client.GetAsync(searchUrl);
                        if (!searchResponse.IsSuccessStatusCode)
                            throw new Exception($"Erro ao buscar jogo: {searchResponse.StatusCode}");

                        string searchContent = await searchResponse.Content.ReadAsStringAsync();
                        var searchJson = JObject.Parse(searchContent);
                        var contents = searchJson["searchResults"]?.First?["contents"]?.First;

                        if (contents == null)
                            return (0, "Null", "Null", "Null", "Null");

                        universeId = contents["universeId"]?.ToString() ?? "Null";
                        gameName = contents["name"]?.ToString() ?? "Null";
                        playerCount = contents["playerCount"]?.ToObject<int>() ?? 0;
                        creatorName = contents["creatorName"]?.ToString() ?? "Null";
                        placeidd = contents["rootPlaceId"]?.ToString() ?? "Null";

                        //Console.WriteLine($"PlaceID: {placeidd}");
                        //Console.WriteLine($"UniverseID: {universeId}");
                        //Console.WriteLine($"Gamename: {gameName}");
                        //Console.WriteLine($"CreatorName: {creatorName}");
                        //Console.WriteLine($"PlayerCount: {playerCount}");
                    }

                    string thumbnailUrl = $"https://thumbnails.roblox.com/v1/games/multiget/thumbnails?universeIds={universeId}&size=768x432&format=Png&isCircular=false";
                    HttpResponseMessage thumbnailResponse = await client.GetAsync(thumbnailUrl);
                    if (!thumbnailResponse.IsSuccessStatusCode)
                        throw new Exception($"Erro ao buscar imagem: {thumbnailResponse.StatusCode}");

                    string thumbnailContent = await thumbnailResponse.Content.ReadAsStringAsync();
                    var thumbnailJson = JObject.Parse(thumbnailContent);
                    string imageUrl = thumbnailJson["data"]?.First?["thumbnails"]?.First?["imageUrl"]?.ToString() ?? "Null";

                    return (playerCount, creatorName, gameName, imageUrl, placeidd);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro GetRobloxGameInfo: {ex.ToString()}");
                    return (0, "Null", "Null", "Null", "Null");
                }
            }
        }

        private async void JoinGameSelected_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await Task.Delay(1000);
            if (selectedgameurl != "")
                Process.Start($"roblox://experiences/start?placeId={selectedgameurl}");
        }

        private async void JoinLastGamebtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await Task.Delay(1000);

            if (LastplayedText2.Text == "Find Scripts")
            {
                AnimateGrid(HubGrid, new Thickness(0, 104, 0, 0), HubRadioButton);
                HubRadioButton.IsChecked = true;

                var result = await GetRobloxGameInfo(placeid: logWatcher.CurrentPlaceId);

                areadyanimatinggrid = true;
                WPXD.Children.Clear();
                WPXD2.Children.Clear();
                dwuduqwn2.Text = lm.Translate(" (Searching Scripts...)");
                dwuduqwn3.Visibility = Visibility.Collapsed;

                try
                {
                    ImageBrush imageBrush = new ImageBrush();
                    imageBrush.ImageSource = LastGamePlayedHolder.Source;
                    this.BorderImg.Background = imageBrush;

                    djwndnqim.Text = result.gameName + lm.Translate("(loading...)");
                    selectedgameurl = logWatcher.CurrentPlaceId.ToString();
                    dwqnduwnq.Text = "Players online: " + ConverterParaStringCompacta(result.playerCount);
                }
                catch { }

                Move(GeneralScriptScrollViewer, GeneralScriptScrollViewer.Margin, new Thickness(MainWin.ActualWidth + 150, 75, 0, 4), 0.9);
                Fade(GeneralScriptScrollViewer, 1, 0, 0.8);
                Fade(skibidtoilet, 1, 0, 0.8);

                await Task.Delay(100);

                Move(GameselectedScroll, new Thickness(MainWin.ActualWidth + 150, 0, 0, 4), new Thickness(0, 0, 0, 4), 1.4);
                Fade(GameselectedScroll, 0, 1, 1.7);
                GameselectedScroll.Visibility = Visibility.Visible;

                ThicknessAnimation j = new ThicknessAnimation()
                {
                    To = new Thickness(10, 65, 5, 4),
                    Duration = TimeSpan.FromMilliseconds(100)
                };
                GeneralScriptScrollViewer.BeginAnimationP(MarginProperty, j);
                selectedscriptinfoborder.Visibility = Visibility.Collapsed;

                await Task.Delay(1400);
                areadyanimatinggrid = false;
                AdvancedSearch(result.PlaceID, result.gameName);               
            }
            else
                Process.Start($"roblox://experiences/start?placeId={lastplayedurl}");
        }

        private void RefreshHub_Click(object sender, RoutedEventArgs e)
        {
            pages = 0;
            WP.Children.Clear();
            WP.Visibility = Visibility.Collapsed;
            NewScriptsTXT.Visibility = Visibility.Collapsed;

            WP0.Children.Clear();
            WP0.Visibility = Visibility.Collapsed;
            NewScripts2TXT.Visibility = Visibility.Collapsed;

            WP2.Children.Clear();
            WP2.Visibility = Visibility.Collapsed;
            CommunityScriptsTXT.Visibility = Visibility.Collapsed;
            WP3.Children.Clear();

            SearchScripts2(lastsearched);
        }

        private async void RefreshHub2_Click(object sender, RoutedEventArgs e)
        {
            pages = 0;
            await Task.Delay(300);

            WP.Children.Clear();
            WP.Visibility = Visibility.Collapsed;
            NewScriptsTXT.Visibility = Visibility.Collapsed;

            WP0.Children.Clear();
            WP0.Visibility = Visibility.Collapsed;
            NewScripts2TXT.Visibility = Visibility.Collapsed;

            WP2.Children.Clear();
            WP2.Visibility = Visibility.Collapsed;
            CommunityScriptsTXT.Visibility = Visibility.Collapsed;
            WP3.Children.Clear();

            GeneralScriptSearch3.Text = "";
        }

        private void SavedScriptsBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Comming soon");
        }

        private void SetJumPower_Click(object sender, RoutedEventArgs e)
        {
            jumplk_value = (int)JumpSlider.Value;
            EXECUTAR("_G.EvoEnvJumpLock = nil\ntask.wait(1.2)\n" + Properties.Resources.SpeedLock.Replace("666", jumplk_value.ToString()), true);
        }

        private void SetWalkspeed_Click(object sender, RoutedEventArgs e)
        {
            speedlk_value = (int)SpeedSlider.Value;
            EXECUTAR("_G.EvoEnvSpeedLock = nil\ntask.wait(1.2)\n" + Properties.Resources.SpeedLock.Replace("777", speedlk_value.ToString()), true);
        }

        private void SearchScriptList_TextChanged(object sender, TextChangedEventArgs e)
        {
            //try
            //{
            //    if (SearchScriptList.Text == "")
            //        SearchScriptList.Text = lm.Translate("Search Scripts");

            //    PopulateScriptList("Scripts", SearchScriptList.Text);
            //}
            //catch { }
        }

        private void SearchScriptList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //try
            //{
            //    if (SearchScriptList.Text == lm.Translate("Search Scripts"))
            //        SearchScriptList.Clear();
            //}
            //catch { }
        }

        private void SearchScriptList_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (SearchScriptList.Text == "")
            //    SearchScriptList.Text = lm.Translate("Search Scripts");
        }





        internal static bool AimbotEnabled = false;
        internal static string AimbotKey = "M"; //"num.UserInputType.MouseButton2";
        internal static float FovRadius = 150f;
        internal static Color FovColor = Color.FromRgb(200, 20, 20);
        internal static float FovThickness = 0.7f;
        internal static string AimPart = "Head";
        internal static bool TeamCheck = true;


        internal static bool UseWhitelistedTeams = false;
        internal static List<string> WhitelistedTeams;
        internal static List<string> WhitelistedPlayers;

        static string AimbotConverter()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("_G.AimbotSettings = _G.AimbotSettings or {");
            sb.AppendLine($"    aimbotEnabled = {AimbotEnabled.ToString().ToLower()},");
            sb.AppendLine($"    aimbotKey = Enum.KeyCode.{AimbotKey},");
            sb.AppendLine($"    fovRadius = {FovRadius},");
            sb.AppendLine($"    fovColor = {FovColor},");
            sb.AppendLine($"    fovThickness = {FovThickness},");
            sb.AppendLine($"    aimPart = \"{AimPart}\",");
            sb.AppendLine($"    teamCheck = {TeamCheck.ToString().ToLower()},");
            sb.AppendLine($"    useWhitelistedTeams = {UseWhitelistedTeams.ToString().ToLower()},");
            sb.AppendLine($"    whitelistedTeams = {{\"{string.Join("\", \"", WhitelistedTeams)}\"}},");
            sb.AppendLine($"    whitelistedPlayers = {{\"{string.Join("\", \"", WhitelistedPlayers)}\"}}");
            sb.AppendLine("}\n");

            sb.AppendLine(Properties.Resources.Aimbot);
            return sb.ToString();
        }

        private void AimbotToggle_Click(object sender, RoutedEventArgs e)
        {
            if (inj4)
            {
                EXECUTAR(AimbotConverter());
            }
            else
                AimbotToggle.IsChecked = false;
        }

        private void TracersToggle_Click(object sender, RoutedEventArgs e)
        {
            EXECUTAR(Properties.Resources.Tracers);

            if (TracersToggle.IsChecked.Value == true)
            {
                Trackerspreview.Visibility = Visibility.Visible;
                Distancepreview.Visibility = Visibility.Visible;
            }
            else
            {
                Trackerspreview.Visibility = Visibility.Collapsed;
                Distancepreview.Visibility = Visibility.Collapsed;
            }
        }

        private void BoxToggle_Click(object sender, RoutedEventArgs e)
        {
            EXECUTAR(Properties.Resources.Tracers);

            if (BoxToggle.IsChecked.Value == true)
                ESPpreview.Visibility = Visibility.Visible;
            else
                ESPpreview.Visibility = Visibility.Collapsed;
        }

        private int expandedddd = 1;
        private void expandscriptbtn_Click(object sender, RoutedEventArgs e)
        {
            if (expandedddd == 1)
            {
                //tela dividida
                dwindfikwdmnkwdmwm.Width = new GridLength(1.5, GridUnitType.Star);
                dhjdcu3j8j8f82jf4jf4jfi8.Width = new GridLength(1, GridUnitType.Star);
                expandedddd = 2;
                cj832f82f82fjn.Text = "2";
            }
            else if (expandedddd == 2)
            {
                //espande o script
                dwindfikwdmnkwdmwm.Width = new GridLength(1, GridUnitType.Star);
                dhjdcu3j8j8f82jf4jf4jfi8.Width = new GridLength(0, GridUnitType.Star);
                expandedddd = 3;
                cj832f82f82fjn.Text = "3";
            }
            else if (expandedddd == 3)
            {
                //expande a conversa
                dwindfikwdmnkwdmwm.Width = new GridLength(0, GridUnitType.Star);
                dhjdcu3j8j8f82jf4jf4jfi8.Width = new GridLength(1, GridUnitType.Star);
                expandedddd = 1;
                cj832f82f82fjn.Text = "1";
            }
        }

        private async void copyaiscript_Click(object sender, RoutedEventArgs e)
        {
            await Task.Delay(300);

            System.Windows.Clipboard.SetText(await GetEditorText(AIeditor));
            //copyaiscript.IsEnabled = false;
            //copy_script_txt.Text = lm.Translate("Copied");

            Fade(dnu23ndu, 1, 0, 0.3);
            Fade(fnu2fn8u, 0, 1, 0.5);
            fnu2fn8u.Visibility = Visibility.Visible;

            await Task.Delay(2000);

            Fade(fnu2fn8u, 1, 0, 0.3);
            Fade(dnu23ndu, 0, 1, 0.5);

            //copy_script_txt.Text = lm.Translate("Copy Script");
            //copyaiscript.IsEnabled = true;
        }

        private async void MuteMusic_Click(object sender, RoutedEventArgs e)
        {
            await Task.Delay(300);

            _isMuted = !_isMuted;
            try { _mediaPlayer.IsMuted = _isMuted; } catch { }

            if (_isMuted)
            {
                music_off.Visibility = Visibility.Visible;
                music_on.Visibility = Visibility.Collapsed;
                cafeina_pra_neve = 1;
                try { _mediaPlayer.Volume = 0; } catch { InternalConsolePrint("ee", console_RichTextBox, Colors.Red); }
                try { LoadedMediaElement.Volume = 0; } catch { InternalConsolePrint("ee", console_RichTextBox, Colors.Red); }
                

                registryKey1.SetValue("Music", "False");
            }
            else
            {
                music_off.Visibility = Visibility.Collapsed;
                music_on.Visibility = Visibility.Visible;

                try { _mediaPlayer.Volume = 0.1; } catch { InternalConsolePrint("ee", console_RichTextBox, Colors.Red); }
                try { LoadedMediaElement.Volume = 0.1; } catch { InternalConsolePrint("ee", console_RichTextBox, Colors.Red); }

                if (isplaying_music)
                    cafeina_pra_neve = 4;

                registryKey1.SetValue("Music", "True");
            }
        }


        bool abertofffff = false;
        private async void fgfgiufifjm_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (abertofffff)
                return;
            abertofffff = true;

            Fade(h555555, 0, 1, 0.4);
            Move(h555555, new Thickness(5, 50, 5, 0), new Thickness(5, 50, 5, -100), 0.8);


            while(fgfgiufifjm.IsMouseOver || h555555.IsMouseOver)
            {
                await Task.Delay(2000);
            }

            Move(h555555, new Thickness(5, 50, 5, -100), new Thickness(5, 50, 5, 0), 0.8);
            await Task.Delay(300);
            Fade(h555555, 1, 0, 0.4);
            await Task.Delay(300);
            abertofffff = false;
        }

        private async void LogOuttt_Click(object sender, RoutedEventArgs e)
        {
            this.IsHitTestVisible = false;
            Move(WindowBorder, WindowBorder.Margin, new Thickness(WindowBorder.ActualWidth / 2, WindowBorder.ActualHeight / 2, WindowBorder.ActualWidth / 2, WindowBorder.ActualHeight / 2), 2);

            await Task.Delay(2000);
            try { File.Delete("C:\\Essence\\userdata\\logindata.txt"); } catch { }
            registryKey1.SetValue("Boot", "1");

            RestartApp(false);
        }

        private int page = 3;
        private async void news_page_kkk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(page != 1 && page != 0)
            {
                if (page == 2)
                {
                    Fade(changelog_page_shitt, 1, 0, 0.3);
                    Fade(changelogs_shitty_text, 1, 0.7, 0.3);
                    Fade(Changelog_Borderr, 1, 0, 0.3);
                }
                else
                {
                    Fade(robloxnews_page_shitt, 1, 0, 0.3);
                    Fade(robloxnews_shitty_text, 1, 0.7, 0.3);
                    Fade(robloxnews_Borderr, 1, 0, 0.3);
                }

                page = 0;


                Fade(news_page_shitt, 0, 1, 0.5);
                Fade(news_shitty_text, 0.7, 1, 0.5);
                Fade(News_Borderr, 0, 1, 0.5);

                await Task.Delay(300);
                page = 1;
            }
        }

        private async void changelog_page_kkk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (page != 2 && page != 0)
            {
                if (page == 1)
                {
                    Fade(news_page_shitt, 1, 0, 0.3);
                    Fade(news_shitty_text, 1, 0.7, 0.3);
                    Fade(News_Borderr, 1, 0, 0.3);
                }
                else
                {
                    Fade(robloxnews_page_shitt, 1, 0, 0.3);
                    Fade(robloxnews_shitty_text, 1, 0.7, 0.3);
                    Fade(robloxnews_Borderr, 1, 0, 0.3);
                }

                page = 0;
                Fade(changelog_page_shitt, 0, 1, 0.5);
                Fade(changelogs_shitty_text, 0.7, 1, 0.5);
                Fade(Changelog_Borderr, 0, 1, 0.5);

                await Task.Delay(300);
                page = 2;
            }
        }

        private async void robloxnews_page_kkk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (page != 3 && page != 0)
            {
                if (page == 1)
                {
                    Fade(news_page_shitt, 1, 0, 0.3);
                    Fade(news_shitty_text, 1, 0.7, 0.3);
                    Fade(News_Borderr, 1, 0, 0.3);
                }
                else
                {
                    Fade(changelog_page_shitt, 1, 0, 0.3);
                    Fade(changelogs_shitty_text, 1, 0.7, 0.3);
                    Fade(Changelog_Borderr, 1, 0, 0.3);
                }

                page = 0;
                Fade(robloxnews_page_shitt, 0, 1, 0.5);
                Fade(robloxnews_shitty_text, 0.7, 1, 0.5);
                Fade(robloxnews_Borderr, 0, 1, 0.5);

                await Task.Delay(300);
                page = 3;
            }
        }



        private bool duwhnudnqwdqwium2 = false;
        private async void lastplayedgrid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (duwhnudnqwdqwium2 || LastplayedText2.Text == "")
                return;

            duwhnudnqwdqwium2 = true;

            await Task.Delay(150);
            if(!lastplayedgrid2.IsMouseOver)
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

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (scrollViewer != null)
            {
                // Ajusta o deslocamento horizontal
                if (e.Delta > 0)
                {
                    if (g55553gy.ScrollableWidth < 1)
                    {
                        GeneralScriptScrollViewer.LineUp();
                        e.Handled = true;
                        return;
                    }

                    scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + 35);                    
                }
                else
                {
                    if (g55553gy.ScrollableWidth < 1)
                    {
                        GeneralScriptScrollViewer.LineDown();
                        e.Handled = true;
                        return;
                    }

                    scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - 35);
                }

                e.Handled = true;
            }
        }


        //private MediaType LoadedMediaType = MediaType.None;
        private MediaElement LoadedMediaElement = null;
        private async void UploadBg_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await Task.Delay(300);

            if (d3ndy73n23.Kind == PackIconKind.DeleteCircle) 
            {
                ImageBehavior.SetAnimatedSource(ExecutorBk, null);

                if (LoadedMediaElement != null)
                {
                    LoadedMediaElement.Stop();
                    LoadedMediaElement.Close();
                    ExecutorVBk.Child = null;
                    LoadedMediaElement = null;
                }

                ExecutorVBk.Child = null;
                LoadedMediaElement = null;

                d3ndy73n23.Kind = PackIconKind.UploadCircle;
                Wolfregkey.SetValue("Background", "null");
                return;
            }
            
            Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Essence - Choose an image/GIF/video",
                Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp;*.gif|GIFs|*.gif|Videos|*.mp4;*.avi;*.mov;*.mkv"
            };

            bool? result = openFileDialog1.ShowDialog();
            if (result == true)
                LoadBackg(openFileDialog1.FileName);            
        }

        private void LoadBackg(string ggs)
        {
            if (!File.Exists(ggs))
            {
                Wolfregkey.SetValue("Background", "null");
                return;
            }
            string extension = System.IO.Path.GetExtension(ggs).ToLower();

            try
            {
                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".bmp" || extension == ".gif")
                {
                    BitmapImage bitmap = new BitmapImage(new Uri(ggs, UriKind.RelativeOrAbsolute));
                    Color pixelColor = GetPixelColor(bitmap);
                    ImageBehavior.SetAnimatedSource(ExecutorBk, bitmap);
                    d3ndy73n23.Kind = PackIconKind.DeleteCircle;
                }
                else if (extension == ".mp4" || extension == ".avi" || extension == ".mov" || extension == ".mkv")
                {
                    LoadedMediaElement = new MediaElement
                    {
                        Source = new Uri(ggs, UriKind.RelativeOrAbsolute),
                        LoadedBehavior = MediaState.Manual,
                        UnloadedBehavior = MediaState.Close,
                        Volume = 0.1
                    };

                    if (_isMuted)
                        LoadedMediaElement.Volume = 0;

                    LoadedMediaElement.MediaEnded += (sender, e) =>
                    {
                        LoadedMediaElement.Position = TimeSpan.Zero;
                        LoadedMediaElement.Play();
                    };

                    ExecutorVBk.Child = LoadedMediaElement;
                    LoadedMediaElement.Play();
                    d3ndy73n23.Kind = PackIconKind.DeleteCircle;
                }
                else
                {
                    throw new InvalidOperationException("Tipo de arquivo não suportado.");
                }

                d3ndy73n23.Kind = PackIconKind.DeleteCircle;
                Wolfregkey.SetValue("Background", ggs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar o arquivo: {ex.ToString()}");
            }
        }



        private async void UserManagementIcon_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            executor.IsHitTestVisible = false;
            ProfileSettings.Show();
            Move(ProfileSettings.UserManagementBorder, new Thickness(0, 100, 0, 25), new Thickness(0, 25, 0, 25), 0.6);
            Fade(ProfileSettings.UserManagementGrid, 0, 1, 0.4);

            ProfileSettings.EnableBlur();

            ProfileSettings.UserManagementGrid.Visibility = Visibility.Visible;
        }

        private async void CreateAccount_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //logoutborder.IsEnabled = false;
            await Task.Delay(200);

            try 
            { 
                File.Delete("C:\\Essence\\userdata\\logindata.txt");
                DoubleAnimation opacityAnimation = new DoubleAnimation();
                opacityAnimation.From = 1.0;
                opacityAnimation.To = 0.0;
                opacityAnimation.Duration = TimeSpan.FromMilliseconds(400);

                Storyboard storyboard = new Storyboard();

                Storyboard.SetTarget(opacityAnimation, MainWin);
                Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));
                storyboard.Children.Add(opacityAnimation);

                storyboard.Begin();
                await Task.Delay(600);
                RestartApp(false);
            } 
            catch { }
            
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Quick Pannel and AI disabled for now.", "Notification");
        }



        private bool dbudbj23dj;
        private void FilterScriptHubBtn_Click(object sender, RoutedEventArgs e)
        {

        }


        private async void SaveScriptBtn_Click(object sender, RoutedEventArgs e)
        {
            scriptttttttttttttt.saved = !scriptttttttttttttt.saved;
            //var buttonImg = (Image)scriptttttttttttttt.FindName("ButtonImg");

            if (scriptttttttttttttt.saved)
                djbnjwbdj3.Fill = Brushes.Gold;
            else
                djbnjwbdj3.Fill = new SolidColorBrush(Color.FromRgb(200,200,200));

            string direc = "C:\\Essence\\userdata\\Saves";

            if (!Directory.Exists(direc))
            {
                Directory.CreateDirectory(direc);
            }

            if (File.Exists(direc + "\\" + scriptttttttttttttt._id + ".hubsave"))
            {
                File.Delete(direc + "\\" + scriptttttttttttttt._id + ".hubsave");
            }
            else
            {

                using (StreamWriter st = new StreamWriter(direc + "\\" + scriptttttttttttttt._id + ".hubsave"))
                {
                    st.WriteLine(scriptttttttttttttt.title + "$M4endline$");
                    st.WriteLine(await scriptttttttttttttt.GetScript() + "$M4endline$");
                    st.WriteLine(scriptttttttttttttt.views + "$M4endline$");
                    st.WriteLine(scriptttttttttttttt.owner != null ? scriptttttttttttttt.owner.username : scriptttttttttttttt.game.name + "$M4endline$");

                    string imgurl = scriptttttttttttttt.game.imageUrl;
                    if (imgurl.StartsWith("/images/"))
                        imgurl = "https://scriptblox.com" + imgurl;

                    st.WriteLine(imgurl + "$M4endline$");
                    st.WriteLine(scriptttttttttttttt._id + "$M4endline$");
                }
            }
        }

        private async void LikeScriptBtn_Click(object sender, RoutedEventArgs e)
        {
            likeeefef.Fill = new SolidColorBrush(Color.FromRgb(200, 20, 20));
            dislikeshit.Fill = new SolidColorBrush(Color.FromRgb(200, 200, 200));
            likeeefef.IsHitTestVisible = false;
            dislikeshit.IsHitTestVisible = false;

            ScriptObject kk = scriptttttttttttttt;
            await KeyGay2.Get("scriptrate", $"{kk.slug}\n | \nPositive", force_return: true);
            rattinggg[kk.slug] = 2;
            selectedscriptrating.Text = "Rating: Loading...";

            string kkk = await KeyGay2.Get("scriptrating", kk.slug, force_return: true);
            await Dispatcher.InvokeAsync(() =>
            {
                try
                {
                    if (kkk != "null")
                        selectedscriptrating.Text = $"Rating {kkk}";
                    else
                        selectedscriptrating.Text = "Rating: No rates yet here";
                }
                catch { }
            });          


            likeeefef.IsHitTestVisible = true;
            dislikeshit.IsHitTestVisible = true;
        }

        private async void DislikeScriptBtn_Click(object sender, RoutedEventArgs e)
        {
            dislikeshit.Fill = new SolidColorBrush(Color.FromRgb(200, 20, 20));
            likeeefef.Fill = new SolidColorBrush(Color.FromRgb(200, 200, 200));
            likeeefef.IsHitTestVisible = false;
            dislikeshit.IsHitTestVisible = false;

            ScriptObject kk = scriptttttttttttttt;
            await KeyGay2.Get("scriptrate", $"{kk.slug}\n | \nNegative", force_return: true);
            rattinggg[kk.slug] = 1;
            selectedscriptrating.Text = "Rating: Loading...";

            string kkk = await KeyGay2.Get("scriptrating", kk.slug, force_return: true);
            await Dispatcher.InvokeAsync(() =>
            {
                try
                {
                    if (kkk != "null")
                        selectedscriptrating.Text = $"Rating {kkk}";
                    else
                        selectedscriptrating.Text = "Rating: No rates yet here";
                }
                catch { }
            });


            likeeefef.IsHitTestVisible = true;
            dislikeshit.IsHitTestVisible = true;
        }

        private void MainWin_StateChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.WindowState != WindowState.Maximized)
                    SexyNotification.WindowState = this.WindowState;
            }
            catch { }
        }
    }

    public class logWatcher : IDisposable
    {
        private const string GameJoiningEntry = "[FLog::Output] ! Joining game";
        private const string GameLeavingEntry = "[FLog::SingleSurfaceApp] leaveUGCGameInternal";
        private const string GameJoinedEntryPattern = @"serverId: ([0-9\.]+)\|[0-9]+";

        private bool _isDisposed;
        internal bool StopAllInteractions;

        //public event EventHandler? OnGameJoin;
        //public event EventHandler? OnGameLeave;
        public event EventHandler userfound;

        public bool IsInGame { get; set; }
        public long CurrentPlaceId { get; set; }

        private CancellationTokenSource _cancellationTokenSource = new();

        public logWatcher()
        {

        }

        public async void StartWatcher()
        {
            string logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Roblox\\logs");

            if (!Directory.Exists(logDirectory))
                return;

            await MonitorNewLogs(logDirectory);
        }

        private async Task MonitorNewLogs(string logDirectory)
        {
            List<FileInfo> currentLogFiles = new List<FileInfo>();

            while (!StopAllInteractions)
            {
                if (Directory.Exists(logDirectory))
                {
                    foreach (string filePath in Directory.GetFiles(logDirectory, "*.log"))
                    {
                        //Console.WriteLine(filePath);
                        try { File.Delete(filePath); } catch { }
                    }
                }

                var newLogFiles = new DirectoryInfo(logDirectory).GetFiles()
                    .OrderByDescending(x => x.LastWriteTime)
                    .Take(1)
                    .ToList();


                if (!newLogFiles.SequenceEqual(currentLogFiles))
                {
                    currentLogFiles = newLogFiles;
                    _cancellationTokenSource.Cancel();
                    _cancellationTokenSource = new CancellationTokenSource();

                    foreach (var logFile in currentLogFiles)
                    {
                        if (logFile.FullName.Contains(".log"))
                        {
                            //Console.WriteLine("Abrindo log do roblox: " + logFile);
                            MonitorLogEntries(logFile);
                        }
                    }
                }

                await Task.Delay(2000);
            }
        }

        private bool first_read = true;
        private async Task MonitorLogEntries(FileInfo logFileInfo)
        {
            using FileStream logFileStream = logFileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using StreamReader sr = new(logFileStream);

            string line;

            //Console.WriteLine($"Examinando log do roblox: {logFileInfo.Name}");
            while ((line = sr.ReadLine()) != null)
            {
                ExamineLogEntry(line);
            }

            first_read = false;

            while (!_isDisposed && !_cancellationTokenSource.Token.IsCancellationRequested && !StopAllInteractions)
            {
                while ((line = sr.ReadLine()) != null)
                {
                    ExamineLogEntry(line);
                }
                await Task.Delay(1000);
            }

            if (_cancellationTokenSource.Token.IsCancellationRequested)
            {
                IsInGame = false;
                CurrentPlaceId = 0;
                Console.WriteLine("Busca por informações do usuário no arquivo de logs cancelada");
                //OnGameLeave?.Invoke(this, EventArgs.Empty);
            }
        }

        internal bool user_b = false;
        internal string useri = "";
        private void ExamineLogEntry(string entry)
        {
            if (entry.Contains(GameLeavingEntry))
            {
                if (IsInGame)
                {
                    if (!first_read)
                    {
                        //Console.WriteLine("Usuário saíu do jogo");
                        //CloseInjectors();
                    }

                    IsInGame = false;
                    CurrentPlaceId = 0;

                    //OnGameLeave?.Invoke(this, EventArgs.Empty);
                }
            }
            else if (entry.Contains(GameJoiningEntry))
            {
                var match = Regex.Match(entry, @"! Joining game '([0-9a-f\-]{36})' place ([0-9]+)");
                if (match.Success)
                {
                    CurrentPlaceId = long.Parse(match.Groups[2].Value);
                    if (!IsInGame)
                    {
                        //if (!first_read)
                        //Console.WriteLine("Usuário entrou no jogo");

                        IsInGame = true;

                        //OnGameJoin?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
            else if (entry.Contains("userId"))
            {
                useri = entry;
                //Console.WriteLine("User found event: " + useri);
                userfound?.Invoke(this, EventArgs.Empty);
            }
            else if(entry.Contains("The user is moderated with type: ") && !user_b)
            {
                user_b = true;
                MessageBox.Show(useri + " Banned");
            }
        }

        public void Dispose()
        {
            _isDisposed = true;
            GC.SuppressFinalize(this);
        }
    }

    //internal static class BloxstrapInterface
    //{
    //    internal static readonly List<string> Files = new List<string>()
    //    {
    //        "https://raw.githubusercontent.com/GatoEVX/EvoX/main/Bloxstrap/Bloxstrap.dll",
    //        "https://raw.githubusercontent.com/GatoEVX/EvoX/main/Bloxstrap/Bloxstrap.exe",
    //        "https://raw.githubusercontent.com/GatoEVX/EvoX/main/Bloxstrap/Bloxstrap.runtimeconfig.json",
    //        "https://raw.githubusercontent.com/GatoEVX/EvoX/main/Bloxstrap/essence.ico"
    //    };
    //    internal static readonly string Path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Bloxstrap";
    //    internal static readonly string MD5 = "BE39149941BB4AA927AC1F3781173B6B";

    //    internal static string GetHashFromFile(string filePath)
    //    {
    //        using FileStream inputStream = File.OpenRead(filePath);
    //        using System.Security.Cryptography.MD5 mD = System.Security.Cryptography.MD5.Create();
    //        StringBuilder stringBuilder = new StringBuilder(32);
    //        byte[] array = mD.ComputeHash(inputStream);
    //        foreach (byte b in array)
    //        {
    //            stringBuilder.Append(b.ToString("x2"));
    //        }
    //        string result = stringBuilder.ToString().ToUpper();
    //        return result;
    //    }



    //    //internal static async Task DownloadFileAsync(string fileName, string filePath, string fileUrl)
    //    //{
    //    //    using (HttpClient client = new HttpClient())
    //    //    {
    //    //        HttpResponseMessage async = await client.GetAsync(fileUrl);
    //    //        if (async.StatusCode != HttpStatusCode.OK)
    //    //            throw new Exception("Failed to download " + fileName);
    //    //        System.IO.File.WriteAllBytes(filePath, await async.Content.ReadAsByteArrayAsync());
    //    //    }
    //    //}
    //}

    public class WebViewA : Microsoft.Web.WebView2.Wpf.WebView2
    {
        public string ToSetText;
        private string LatestRecievedText;

        public bool IsDoMLoaded { get; set; }

        //public string Theme { get; set; } = "Dark";

        public event EventHandler EditorReady;

        //[DllImport("kernel32.dll", SetLastError = true)]
        //public static extern bool SetDllDirectory(string lpPathName);

        public async void WebViewInitialize(
          string BrowserExecutableFolder,
          string UserDataFolder,
          CoreWebView2EnvironmentOptions Options)
        {
            await this.EnsureCoreWebView2Async(await CoreWebView2Environment.CreateAsync(BrowserExecutableFolder, UserDataFolder, Options));
        }

        public WebViewA(string Text = "")
        {
            this.Visibility = Visibility.Hidden;

            //SetDllDirectory("C:\\Essence\\bin");

            this.WebViewInitialize((string)null, Path.GetTempPath(), null);
            this.DefaultBackgroundColor = System.Drawing.Color.Transparent;
            this.Source = new Uri("C:\\Essence\\bin\\Monaco\\index.html");
            this.CoreWebView2InitializationCompleted += new EventHandler<CoreWebView2InitializationCompletedEventArgs>(this.WebViewAPI_CoreWebView2InitializationCompleted);
            this.ToSetText = Text;

            Environment.CurrentDirectory = "C:\\Essence";
            Directory.SetCurrentDirectory("C:\\Essence");
        }

        protected virtual void OnEditorReady()
        {
            EventHandler editorReady = this.EditorReady;
            if (editorReady == null)
                return;
            editorReady((object)this, new EventArgs());
        }

        //[DllImport("user32.dll", SetLastError = true)]
        //private static extern bool SetWindowPos(
        //IntPtr hWnd, IntPtr hWndInsertAfter,
        //int X, int Y, int cx, int cy,
        //uint uFlags);

        //private const uint SWP_NOACTIVATE = 0x0010;   // Não ativa a janela
        //private const uint SWP_NOMOVE = 0x0002;        // Não move a janela
        //private const uint SWP_NOSIZE = 0x0001;        // Não altera o tamanho da janela

        private void WebViewAPI_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            this.CoreWebView2.DOMContentLoaded += new EventHandler<CoreWebView2DOMContentLoadedEventArgs>(this.CoreWebView2_DOMContentLoaded);
            this.CoreWebView2.WebMessageReceived += new EventHandler<CoreWebView2WebMessageReceivedEventArgs>(this.CoreWebView2_WebMessageReceived);
            this.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            this.CoreWebView2.Settings.AreDevToolsEnabled = false;
            this.Visibility = Visibility.Visible;

            //SetWindowPos(this.Handle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOSIZE);

        }

        private void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            this.LatestRecievedText = e.TryGetWebMessageAsString();
        }

        private async void CoreWebView2_DOMContentLoaded(object sender, CoreWebView2DOMContentLoadedEventArgs e)
        {
            await Task.Delay(500);
            this.IsDoMLoaded = true;
            await this.SetText(this.ToSetText);
            this.OnEditorReady();
            // this.SetTheme(this.Theme);
        }

        public async Task<string> GetText()
        {
            WebViewA webViewA = this;
            if (!webViewA.IsDoMLoaded)
                return string.Empty;
            string str = await webViewA.ExecuteScriptAsync("window.chrome.webview.postMessage(editor.getValue())");
            await Task.Delay(50);
            return webViewA.LatestRecievedText;
        }

        public async Task SetText(string Text)
        {
            WebViewA webViewA = this;
            if (!webViewA.IsDoMLoaded)
                return;
            string str = await webViewA.CoreWebView2.ExecuteScriptAsync("SetText(\"" + HttpUtility.JavaScriptStringEncode(Text) + "\")");
        }

        public void Refresh()
        {
            if (!this.IsDoMLoaded)
                return;
            this.ExecuteScriptAsync("Refresh();");
        }
    }
}