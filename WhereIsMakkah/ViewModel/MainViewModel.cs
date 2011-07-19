using System;
using System.Windows;
using System.Device.Location;
using Microsoft.Phone.Shell;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Command;
using WhereIsMakkah.Util;
using GalaSoft.MvvmLight.Messaging;

namespace WhereIsMakkah.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public string ApplicationTitle
        {
            get
            {
                return "Where Is Makkah";
            }
        }

        public string Instruction
        {
            get
            {
                return "Manually point the small arrow to north.";
            }
        }

        /// <summary>
        /// The <see cref="CurrentRotationZ" /> property's name.
        /// </summary>
        public const string CurrentRotationZPropertyName = "CurrentRotationZ";

        private double _currentRotationZ = 0.0;

        /// <summary>
        /// Gets the CurrentRotationZ property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public double CurrentRotationZ
        {
            get
            {
                return _currentRotationZ;
            }

            set
            {
                if (_currentRotationZ == value)
                {
                    return;
                }

                var oldValue = _currentRotationZ;
                _currentRotationZ = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(CurrentRotationZPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="Feedback" /> property's name.
        /// </summary>
        public const string FeedbackPropertyName = "Feedback";

        private string _feedback = "";

        /// <summary>
        /// Gets the Feedback property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public string Feedback
        {
            get
            {
                return _feedback;
            }

            set
            {
                if (_feedback == value)
                {
                    return;
                }

                var oldValue = _feedback;
                _feedback = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(FeedbackPropertyName);
            }
        }

        private bool _metricSetting = true;

        /// <summary>
        /// Gets the MetricSetting property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public bool MetricSetting
        {
            get
            {
                return _metricSetting;
            }

            set
            {
                if (_metricSetting == value)
                {
                    return;
                }

                var oldValue = _metricSetting;
                _metricSetting = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(DistanceLabelPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="Distance" /> property's name.
        /// </summary>
        public const string DistancePropertyName = "Distance";

        private double _distance = 0.0;

        /// <summary>
        /// Gets the Distance property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public double Distance
        {
            get
            {
                if (MetricSetting)
                {
                    return _distance;
                }
                else
                {
                    return _distance * 0.621371192;
                }
            }

            set
            {
                if (_distance == value)
                {
                    return;
                }

                var oldValue = _distance;
                _distance = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(DistanceLabelPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="Unit" /> property's name.
        /// </summary>
        public const string DistanceLabelPropertyName = "DistanceLabel";

        public string DistanceLabel
        {
            get
            {
                return string.Format("Makkah is approximately {0} {1} away.",
                    Distance.ToString("0"),
                    MetricSetting ? "kilometer(s)" : "mile(s)");
            }
        }

        /// <summary>
        /// The <see cref="Busy" /> property's name.
        /// </summary>
        public const string BusyPropertyName = "Busy";

        private bool _busy = false;

        /// <summary>
        /// Gets the Busy property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public bool Busy
        {
            get
            {
                return _busy;
            }

            set
            {
                if (_busy == value)
                {
                    return;
                }

                var oldValue = _busy;
                _busy = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(BusyPropertyName);
            }
        }

        public RelayCommand SensorStartCommand
        {
            get;
            private set;
        }

        public RelayCommand SensorStopCommand
        {
            get;
            private set;
        }

        public RelayCommand SettingsCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                // Code runs "for real"
                _metricSetting = App.AppSettings.MetricSetting;
            }

            SensorStartCommand = new RelayCommand(() => StartSensor());
            SensorStopCommand = new RelayCommand(() => StopSensor());
            SettingsCommand = new RelayCommand(() => GoToSettingsPage());

            Messenger.Default.Register<SettingsChangedMessage>(this, (msg) => ReceiveMessage(msg));
        }

        private static readonly GeoCoordinate Makkah = new GeoCoordinate(21.4166666666666667, 39.8166666666666667);
        private GeoCoordinateWatcher _watcher;

        private void StartSensor()
        {
            if (_watcher == null)
            {
                _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
                _watcher.MovementThreshold = 20;
                _watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(_watcher_StatusChanged);
            }

            if (Busy)
            {
                return;
            }

            Feedback = "Starting location service. This could take up a while.";

            Busy = true;
            var msg = new AnimateArrowMessage() { Run = true, Indeterminate = true };
            Messenger.Default.Send<AnimateArrowMessage>(msg);
            _watcher.Start();
        }

        private void _watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    if (_watcher.Permission == GeoPositionPermission.Denied)
                    {
                        Feedback = "Application does not have permission to access location.";
                    }
                    else
                    {
                        Feedback = "Location is not functioning on this device";
                    }

                    StopSensor();
                    break;

                case GeoPositionStatus.Initializing:
                    // moving on.
                    break;

                case GeoPositionStatus.NoData:
                    Feedback = "Location data is not available.";

                    StopSensor();
                    break;

                case GeoPositionStatus.Ready:
                    GeoCoordinate loc = _watcher.Position.Location;

                    Feedback = "Large arrow is pointing toward Makkah.";
                    Distance = GeoDistanceCalculator.DistanceInKilometers(loc.Latitude, loc.Longitude, Makkah.Latitude, Makkah.Longitude);

                    var destZ = 360.0 - GeoDistanceCalculator.InitialBearing(loc.Latitude, loc.Longitude, Makkah.Latitude, Makkah.Longitude); // counter-clockwise

                    StopSensor();
                    var msg = new AnimateArrowMessage() { Run = true, Indeterminate = false, DestinationZ = destZ };
                    Messenger.Default.Send<AnimateArrowMessage>(msg);
                    break;
            }
        }

        private void StopSensor()
        {
            if (_watcher == null)
            {
                return;
            }

            Busy = false;
            var msg = new AnimateArrowMessage() { Run = false };
            Messenger.Default.Send<AnimateArrowMessage>(msg);
            _watcher.Stop();
        }

        private void GoToSettingsPage()
        {
            var msg = new GoToPageMessage() { PageName = "SettingsPage" };
            Messenger.Default.Send<GoToPageMessage>(msg);
        }

        private void ReceiveMessage(SettingsChangedMessage msg)
        {
            // FIXME: for the time being, only raise unit propery
            var key = msg.Key;
            if (key.Equals("Unit"))
            {
                MetricSetting = App.AppSettings.MetricSetting;
            }
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}