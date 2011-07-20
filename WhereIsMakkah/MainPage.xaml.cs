using System;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Media.Animation;
using System.Text;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GalaSoft.MvvmLight.Messaging;
using WhereIsMakkah.ViewModel;
using WhereIsMakkah.Util;
using WhereIsMakkah.Lang;

namespace WhereIsMakkah
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            this.Loaded += (sender, e) =>
                {
                    // Localize the text on application bar.
                    if (ApplicationBar != null)
                    {
                        var refresh = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                        refresh.Text = AppResources.ApplicationBarRefreshLabel;
                        var settings = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
                        settings.Text = AppResources.ApplicationBarSettingsLabel;
                    }
                };

            Messenger.Default.Register<AnimateArrowMessage>(this, (msg) => ReceiveMessage(msg));
            Messenger.Default.Register<GoToPageMessage>(this, (msg) => ReceiveMessage(msg));
        }

        private void RefreshClicked(object sender, EventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
            {
                vm.SensorStartCommand.Execute(null);
            }
        }

        private void SettingsClicked(object sender, EventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
            {
                vm.SettingsCommand.Execute(null);
            }
        }

        private object ReceiveMessage(AnimateArrowMessage msg)
        {
            if (msg.Run)
            {
                if (msg.Indeterminate)
                {
                    if (IndeterminateArrow.GetCurrentState() != ClockState.Stopped)
                    {
                        return null;
                    }

                    var vm = DataContext as MainViewModel;
                    var anim = IndeterminateArrow.Children[0] as DoubleAnimationUsingKeyFrames;
                    anim.KeyFrames[0].Value = vm.CurrentRotationZ + 2.0;
                    anim.KeyFrames[1].Value = vm.CurrentRotationZ - 2.0;
                    anim.KeyFrames[2].Value = vm.CurrentRotationZ;

                    IndeterminateArrow.Begin();
                }
                else
                {
                    if (DeterminateArrow.GetCurrentState() != ClockState.Stopped)
                    {
                        return null;
                    }

                    var vm = DataContext as MainViewModel;
                    var anim = DeterminateArrow.Children[0] as DoubleAnimation;
                    anim.From = vm.CurrentRotationZ;
                    anim.To = msg.DestinationZ;
                    vm.CurrentRotationZ = msg.DestinationZ;

                    DeterminateArrow.Begin();
                }
            }
            else
            {
                if (IndeterminateArrow.GetCurrentState() != ClockState.Stopped)
                {
                    IndeterminateArrow.Stop();
                }
                if (DeterminateArrow.GetCurrentState() != ClockState.Stopped)
                {
                    DeterminateArrow.Stop();
                }
            }
            return null;
        }

        private object ReceiveMessage(GoToPageMessage msg)
        {
            StringBuilder sb = new StringBuilder("/View/");
            sb.Append(msg.PageName);
            sb.Append(".xaml");

            NavigationService.Navigate(new System.Uri(sb.ToString(), System.UriKind.Relative));
            return null;
        }
    }
}
