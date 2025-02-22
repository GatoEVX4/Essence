using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
using System.Windows.Threading;

namespace EssenceWPF
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public string XenoVersion = "1.0.5";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct ClientInfo
        {
            public string version;
            public string name;
            public int id;
        }

        private readonly DispatcherTimer @ӓ;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [CompilerGenerated]
        private List<ClientInfo> @Ӕ = new List<ClientInfo>();

        [CompilerGenerated]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string @ӕ = "";

        internal Button @Ӗ;

        internal Button @ӗ;

        internal StackPanel @Ә;

        private bool @ә;

        public List<ClientInfo> ActiveClients
        {
            [CompilerGenerated]
            get
            {
                return @Ӕ;
            }
            [CompilerGenerated]
            private set
            {
                @Ӕ = value;
            }
        }

        public string SupportedVersion
        {
            [CompilerGenerated]
            get
            {
                return @ӕ;
            }
            [CompilerGenerated]
            private set
            {
                @ӕ = value;
            }
        }

        [DllImport("Xeno.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Initialize")]
        private static extern void @Ӛ();

        [DllImport("Xeno.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetClients")]
        private static extern nint @ӛ();

        [DllImport("Xeno.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "Execute")]
        private static extern void @Ӝ(byte[] @ӓ, string[] @Ӕ, int @ӕ);

        [DllImport("Xeno.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "Compilable")]
        private static extern nint @ӝ(byte[] @ӓ);

        private void @Ӡ(object? @ӓ, EventArgs @Ӕ)
        {
            Close();
        }

        private async void @ӡ()
        {
            try
            {
                using HttpClient client = new HttpClient();
                if (!(await client.GetStringAsync("https://rizve.us.to/Xeno/LatestVersion") != XenoVersion))
                {
                }
            }
            catch (HttpRequestException)
            {
            }
        }

        private void @Ӣ(object @ӓ, EventArgs @Ӕ)
        {
            Dictionary<int, ClientInfo> newClients = @ӣ().ToDictionary((ClientInfo c) => c.id);
            (from cb in @Ә.Children.OfType<CheckBox>()
             where !newClients.ContainsKey(@Ӥ(cb.Content.ToString()))
             select cb).ToList().ForEach(delegate (CheckBox cb)
             {
                 @Ә.Children.Remove(cb);
             });
            foreach (ClientInfo value in newClients.Values)
            {
                if (!@Ӧ(value) && !string.IsNullOrWhiteSpace(value.name))
                {
                    @ӧ(value);
                }
            }
            ActiveClients = (from cb in @Ә.Children.OfType<CheckBox>()
                             where cb.IsChecked.GetValueOrDefault()
                             select cb).Select(delegate (CheckBox cb)
                             {
                                 ClientInfo result = default(ClientInfo);
                                 result.name = @ӥ(cb.Content.ToString());
                                 result.id = @Ӥ(cb.Content.ToString());
                                 return result;
                             }).ToList();
        }

        public void ExecuteScript(string script)
        {
            string[] array = ActiveClients.Select((ClientInfo c) => c.name).ToArray();
            @Ӝ(Encoding.UTF8.GetBytes(script), array, array.Length);
        }

        public string GetCompilableStatus(string script)
        {
            nint ptr = @ӝ(Encoding.ASCII.GetBytes(script));
            string result = Marshal.PtrToStringAnsi(ptr);
            Marshal.FreeCoTaskMem(ptr);
            return result;
        }

        private List<ClientInfo> @ӣ()
        {
            List<ClientInfo> list = new List<ClientInfo>();
            nint num = @ӛ();
            while (true)
            {
                ClientInfo item = Marshal.PtrToStructure<ClientInfo>(num);
                if (item.name == null)
                {
                    break;
                }
                list.Add(item);
                num += Marshal.SizeOf<ClientInfo>();
            }
            return list;
        }

        private static int @Ӥ(string @ӓ)
        {
            return int.Parse(@ӓ.Split(new string[] { ", PID: " }, StringSplitOptions.None)[1]);
        }

        private static string @ӥ(string @ӓ)
        {
            return @ӓ.Split(new string[] { ", PID: " }, StringSplitOptions.None)[0].Trim();
        }

        private bool @Ӧ(ClientInfo @ӓ)
        {
            return @Ә.Children.OfType<CheckBox>().Any((CheckBox cb) => cb.Content.ToString() == $"{@ӓ.name}, PID: {@ӓ.id}");
        }

        private async Task @ӧ(ClientInfo @ӓ)
        {
            if (!(@ӓ.name == "N/A"))
            {
                @Ә.Children.Add(new CheckBox
                {
                    Content = $"{@ӓ.name}, PID: {@ӓ.id}",
                    Foreground = Brushes.White,
                    FontFamily = new FontFamily("Franklin Gothic Medium"),
                    IsChecked = true,
                    Background = Brushes.Black
                });
                while (string.IsNullOrWhiteSpace(SupportedVersion))
                {
                    await Task.Delay(5);
                }
                if (SupportedVersion != @ӓ.version)
                {
                    MessageBox.Show($"Xeno might not be compatible on the client {@ӓ.name} with {@ӓ.version}\n\nSupported version: {SupportedVersion}\n\nClick OK to continue using Xeno.", "Version Mismatch", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            MouseLeftButtonDown += (_, _) => DragMove();

            @Ӛ();
            @ӡ();


            MessageBox.Show("Initialized");

            base.MouseLeftButtonDown += delegate
            {
                DragMove();
            };
            @ӓ = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100.0)
            };
            @ӓ.Tick += @Ӣ;
            @ӓ.Start();
            Application.Current.MainWindow.Closed += @Ӡ;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e) => @Ӛ();

    }
}
