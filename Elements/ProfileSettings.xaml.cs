using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Essence.Elements
{
    internal enum AccentState
    {
        ACCENT_DISABLED = 1,
        ACCENT_ENABLE_GRADIENT = 0,
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
        ACCENT_ENABLE_BLURBEHIND = 3,
        ACCENT_INVALID_STATE = 4
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AccentPolicy
    {
        public AccentState AccentState;
        public int AccentFlags;
        public int GradientColor;
        public int AnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }

    internal enum WindowCompositionAttribute
    {
        // ...
        WCA_ACCENT_POLICY = 19
        // ...
    }


    /// <summary>
    /// Lógica interna para xaml
    /// </summary>
    public partial class ProfileSettings : Window
    {
        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);
        internal void EnableBlur()
        {
            var windowHelper = new WindowInteropHelper(this);

            var accent = new AccentPolicy();
            accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

            var accentStructSize = Marshal.SizeOf(accent);

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData();
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;

            SetWindowCompositionAttribute(windowHelper.Handle, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }

        internal void DisableBlur()
        {
            //this.Opacity = 0;
        }





        private void UserManagementBorder_Loaded(object sender, RoutedEventArgs e)
        {
            EnableBlur();
        }

        public ProfileSettings()
        {
            InitializeComponent();
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

        private async void accmanagementborder_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            accmanagementborder.Background = new SolidColorBrush(Color.FromRgb(40, 40, 40));
            keymanagementborder.Background = new SolidColorBrush(Color.FromRgb(20, 20, 20));
            devicesmanagementborder.Background = new SolidColorBrush(Color.FromRgb(20, 20, 20));

            Fade(keymanagementviwer, 1, 0, 0.3);
            Fade(devicesmanagementviwer, 1, 0, 0.3);
            Fade(accmanagementviwer, 0, 1, 0.5);
            accmanagementviwer.Visibility = Visibility.Visible;

            sby2bsyh2bysbn.Text = "Account Management";

            await Task.Delay(300);
            keymanagementviwer.Visibility = Visibility.Collapsed;
            devicesmanagementviwer.Visibility = Visibility.Collapsed;
        }

        private async void keymanagementborder_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            keymanagementborder.Background = new SolidColorBrush(Color.FromRgb(40, 40, 40));
            accmanagementborder.Background = new SolidColorBrush(Color.FromRgb(20, 20, 20));
            devicesmanagementborder.Background = new SolidColorBrush(Color.FromRgb(20, 20, 20));

            Fade(accmanagementviwer, 1, 0, 0.3);
            Fade(devicesmanagementviwer, 1, 0, 0.3);
            Fade(keymanagementviwer, 0, 1, 0.5);
            keymanagementviwer.Visibility = Visibility.Visible;

            sby2bsyh2bysbn.Text = "Subscription Management";

            await Task.Delay(300);
            accmanagementviwer.Visibility = Visibility.Collapsed;
            devicesmanagementviwer.Visibility = Visibility.Collapsed;
        }

        private async void devicesmanagementborder_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            devicesmanagementborder.Background = new SolidColorBrush(Color.FromRgb(40, 40, 40));
            keymanagementborder.Background = new SolidColorBrush(Color.FromRgb(20, 20, 20));
            accmanagementborder.Background = new SolidColorBrush(Color.FromRgb(20, 20, 20));

            Fade(accmanagementviwer, 1, 0, 0.3);
            Fade(keymanagementviwer, 1, 0, 0.3);
            Fade(devicesmanagementviwer, 0, 1, 0.5);

            devicesmanagementviwer.Visibility = Visibility.Visible;

            sby2bsyh2bysbn.Text = "Devices Management";

            await Task.Delay(300);
            accmanagementviwer.Visibility = Visibility.Collapsed;
            keymanagementviwer.Visibility = Visibility.Collapsed;
        }

        private async void UserManagementIcon_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Move(UserManagementBorder, new Thickness(0, 25, 0, 25), new Thickness(0, 100, 0, 25), 0.4);
            Fade(UserManagementGrid, 1, 0, 0.4);

            DisableBlur();

            await Task.Delay(500);
            UserManagementGrid.Visibility = Visibility.Collapsed;
            Hide();
            App.window.executor.IsHitTestVisible = true;
        }
        

        private void copykeysupportserver_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(sellerservertxt.Text);
        }
    }
}
