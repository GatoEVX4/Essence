using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Essence
{
    /// <summary>
    /// Lógica interna para CriticalWindow.xaml
    /// </summary>
    public partial class CriticalWindow : Window
    {
        private string error = "Error?";
        public CriticalWindow(string error)
        {
            this.error = error;
            InitializeComponent();
        }

        private void Mini(object sender, MouseButtonEventArgs e)
        {
            base.WindowState = WindowState.Minimized;
        }

        private async void ExitMD(object sender, MouseButtonEventArgs e)
        {
            await Task.Delay(600);
            FormFadeOut.Begin();
        }

        private bool CloseCompleted;
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
                DragMove();
            }
            catch { }
        }

        private async void MainWin_Loaded(object sender, RoutedEventArgs e)
        {
            Restart.IsEnabled = false;

            await Task.Delay(2300);
            error_box.Text = error;
            await Task.Delay(3000);
            Restart.IsEnabled = true;
        }

        private async void Restart_Click(object sender, RoutedEventArgs e)
        {
            Restart.IsEnabled = false;
            string fileName = Process.GetCurrentProcess().MainModule.FileName;
            Process.Start(fileName);

            await Task.Delay(600);
            Process.GetCurrentProcess().Kill();
        }

        private async void CopyBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText(error);

            copied.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.4)));
            await Task.Delay(2000);
            copied.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.4)));
        }
    }
}
