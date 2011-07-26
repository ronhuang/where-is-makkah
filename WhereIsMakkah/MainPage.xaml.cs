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
                    var progressIndicator = SystemTray.ProgressIndicator;
                    if (progressIndicator != null)
                    {
                        return;
                    }

                    progressIndicator = new ProgressIndicator();
                    progressIndicator.IsIndeterminate = true;

                    SystemTray.SetProgressIndicator(this, progressIndicator);

                    // Bind progress indicator to Busy property
                    var binding = new Binding("Busy") { Source = this.LayoutRoot.DataContext };
                    BindingOperations.SetBinding(progressIndicator, ProgressIndicator.IsVisibleProperty, binding);

                    // Localize the text on application bar.
                    if (ApplicationBar != null)
                    {
                        var refresh = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                        refresh.Text = AppResources.ApplicationBarRefreshLabel;
                        var settings = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
                        settings.Text = AppResources.ApplicationBarSettingsLabel;
                    }
                };

            this.Unloaded += (sender, e) =>
                {
                    var progressIndicator = SystemTray.ProgressIndicator;
                    if (progressIndicator == null)
                    {
                        return;
                    }

                    SystemTray.SetProgressIndicator(this, null);

                    // Unbind progress indicator to Busy property
                    BindingOperations.SetBinding(progressIndicator, ProgressIndicator.IsVisibleProperty, null);
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
