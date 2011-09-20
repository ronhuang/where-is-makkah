using System;
using System.Windows;
using System.Device.Location;
using System.Windows.Threading;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using Microsoft.Phone.Shell;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using WhereIsMakkah.Util;
using WhereIsMakkah.Lang;

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
                if (LocationServiceSetting)
                {
                    return _feedback;
                }
                else
                {
                    return AppResources.FeedbackNoAccessLabel;
                }
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

        private double _distance = -1.0;

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
                if (LocationServiceSetting && Distance >= 0.0)
                {
                    var unit = MetricSetting ? AppResources.MetricUnitLabel : AppResources.ImperialUnitLabel;

                    return string.Format(AppResources.DistanceLabel, Distance.ToString("0"), unit);
                }
                else
                {
                    return "";
                }
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

        /// <summary>
        /// The <see cref="LocationServiceSetting" /> property's name.
        /// </summary>
        public const string LocationServiceSettingPropertyName = "LocationServiceSetting";

        private bool _locationServiceSetting = true;

        /// <summary>
        /// Gets the LocationServiceSetting property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public bool LocationServiceSetting
        {
            get
            {
                return _locationServiceSetting;
            }

            set
            {
                if (_locationServiceSetting == value)
                {
                    return;
                }

                var oldValue = _locationServiceSetting;
                _locationServiceSetting = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(DistanceLabelPropertyName);
                RaisePropertyChanged(FeedbackPropertyName);
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
                _locationServiceSetting = App.AppSettings.LocationServiceSetting;
            }

            SensorStartCommand = new RelayCommand(() => StartSensor());
            SensorStopCommand = new RelayCommand(() => StopSensor());
            SettingsCommand = new RelayCommand(() => GoToSettingsPage());

            Messenger.Default.Register<SettingsChangedMessage>(this, (msg) => ReceiveMessage(msg));
        }

        private static readonly GeoCoordinate Makkah = new GeoCoordinate(21.4166666666666667, 39.8166666666666667);
        private GeoCoordinateWatcher _watcher;
        private Motion _motion;

        private void StartSensor()
        {
            StartLocationSensor();
            StartMotionSensor();
        }

        private void StopSensor()
        {
            StopLocationSensor();
            StopMotionSensor();
        }

        private void StartLocationSensor()
        {
            if (!LocationServiceSetting)
            {
                return;
            }

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

            Feedback = AppResources.FeedbackStartingLabel;

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
                        Feedback = AppResources.FeedbackNoAccessLabel;
                    }
                    else
                    {
                        Feedback = AppResources.FeedbackNoFunctionLabel;
                    }

                    StopLocationSensor();
                    break;

                case GeoPositionStatus.Initializing:
                    // moving on.
                    break;

                case GeoPositionStatus.NoData:
                    Feedback = AppResources.FeedbackNotAvailableLabel;

                    StopLocationSensor();
                    break;

                case GeoPositionStatus.Ready:
                    GeoCoordinate loc = _watcher.Position.Location;

                    Feedback = AppResources.FeedbackReadyLabel;
                    Distance = GeoDistanceCalculator.DistanceInKilometers(loc.Latitude, loc.Longitude, Makkah.Latitude, Makkah.Longitude);

                    var destZ = 360.0 - GeoDistanceCalculator.InitialBearing(loc.Latitude, loc.Longitude, Makkah.Latitude, Makkah.Longitude); // counter-clockwise

                    StopLocationSensor();
                    var msg = new AnimateArrowMessage() { Run = true, Indeterminate = false, DestinationZ = destZ };
                    Messenger.Default.Send<AnimateArrowMessage>(msg);
                    break;
            }
        }

        private void StopLocationSensor()
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

        private void StartMotionSensor()
        {
            // Check to see whether the Motion API is supported on the device.
            if (!Motion.IsSupported)
            {
                Feedback = AppResources.FeedbackMotionNotSupportedLabel;
                return;
            }

            // If the Motion object is null, initialize it and add a CurrentValueChanged
            // event handler.
            if (_motion == null)
            {
                _motion = new Motion();
                _motion.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
                _motion.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<MotionReading>>(_motion_CurrentValueChanged);
            }

            // Try to start the Motion API.
            try
            {
                _motion.Start();
            }
            catch (Exception ex)
            {
                Feedback = AppResources.FeedbackMotionUnableToStartLabel;
            }
        }

        private void _motion_CurrentValueChanged(object sender, SensorReadingEventArgs<MotionReading> e)
        {
            // This event arrives on a background thread. Use BeginInvoke to call
            // CurrentValueChanged on the UI thread.
            DispatcherHelper.CheckBeginInvokeOnUI(() => CurrentValueChanged(e.SensorReading));
        }

        private void CurrentValueChanged(MotionReading e)
        {
            AttitudeReading ar = e.Attitude;
            Feedback = String.Format("Pitch: {0}, Yaw: {1}, Roll: {2}", ar.Pitch, ar.Yaw, ar.Roll);
        }

        private void StopMotionSensor()
        {
            if (_motion == null)
            {
                return;
            }

            _motion.Stop();
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
            if (key.Equals("LocationService"))
            {
                LocationServiceSetting = App.AppSettings.LocationServiceSetting;
            }
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}