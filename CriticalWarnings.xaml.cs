using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Essence
{
    public partial class CriticalWarnings : Window
    {
        public CriticalWarnings()
        {
            InitializeComponent();            
        }

        private bool CloseCompleted;
        internal bool force = false;
        private async void ExitMD(object sender, MouseButtonEventArgs e)
        {
            await Task.Delay(600);
            FormFadeOut.Begin();
        }

        private void MainWin_Closing(object sender, CancelEventArgs e)
        {
            if (!force)
            {
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
            if (!force)
            {
                Process.GetCurrentProcess().Kill();
            }
        }


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


        LanguageManager lm = new LanguageManager();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UnautorizedAction.Text = lm.Translate("Unauthorized action detected. Stop immediately to avoid further measures.");
            FollowRules.Text = lm.Translate("Follow the rules to continue using our services.");


            UpdatingServers.Text = lm.Translate("We’re working to resolve the problem as quickly as possible. Thank you for your patience!");
            ServersUpdating.Text = lm.Translate("Our servers are updating!");


            CheckInternet.Text = lm.Translate("Please check your network settings and firewall configuration to restore access.");
            NoInternet.Text = lm.Translate("No Internet Connection Detected");

        }
    }
}
