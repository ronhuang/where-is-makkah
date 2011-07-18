using System;
using Microsoft.Phone.Controls;
using System.Windows.Media;
using Microsoft.Phone.Shell;
using System.Windows.Data;
using WhereIsMakkah.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using WhereIsMakkah.Util;
using System.Windows.Media.Animation;

namespace WhereIsMakkah
{
    public partial class MainPage : PhoneApplicationPage
    {
        private double currentRotationZ = 0.0;

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

                    var binding = new Binding("Busy") { Source = this.LayoutRoot.DataContext };
                    BindingOperations.SetBinding(progressIndicator, ProgressIndicator.IsVisibleProperty, binding);
                };

            this.Unloaded += (sender, e) =>
                {
                    var progressIndicator = SystemTray.ProgressIndicator;
                    if (progressIndicator == null)
                    {
                        return;
                    }

                    SystemTray.SetProgressIndicator(this, null);
                };

            Messenger.Default.Register<AnimateArrowMessage>(this, (msg) => ReceiveMessage(msg));
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

                    var anim = IndeterminateArrow.Children[0] as DoubleAnimationUsingKeyFrames;
                    anim.KeyFrames[0].Value = currentRotationZ + 2.0;
                    anim.KeyFrames[1].Value = currentRotationZ - 2.0;
                    anim.KeyFrames[2].Value = currentRotationZ;

                    IndeterminateArrow.Begin();
                }
                else
                {
                    if (DeterminateArrow.GetCurrentState() != ClockState.Stopped)
                    {
                        return null;
                    }

                    var anim = DeterminateArrow.Children[0] as DoubleAnimation;
                    anim.From = currentRotationZ;
                    anim.To = msg.DestinationZ;
                    currentRotationZ = msg.DestinationZ;

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
    }
}
