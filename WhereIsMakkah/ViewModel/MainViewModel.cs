using System;
using System.Windows;
using System.Device.Location;
using Microsoft.Phone.Shell;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Command;

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

        public string DistanceLabel
        {
            get
            {
                return string.Format("Distance to Makkah is {0} {1}.", _distance, _unit);
            }
        }

        /// <summary>
        /// The <see cref="Unit" /> property's name.
        /// </summary>
        public const string UnitPropertyName = "Unit";

        private string _unit = "meters";

        /// <summary>
        /// Gets the Unit property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public string Unit
        {
            get
            {
                return _unit;
            }

            set
            {
                if (_unit == value)
                {
                    return;
                }

                var oldValue = _unit;
                _unit = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(UnitPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="Distance" /> property's name.
        /// </summary>
        public const string DistancePropertyName = "Distance";

        private float _distance = 0;

        /// <summary>
        /// Gets the Distance property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public float Distance
        {
            get
            {
                return _distance;
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
                RaisePropertyChanged(DistancePropertyName);
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
            }

            SensorStartCommand = new RelayCommand(() => StartSensor());
            SensorStopCommand = new RelayCommand(() => StopSensor());
        }

        private GeoCoordinateWatcher _watcher;

        private void StartSensor()
        {
            if (_watcher == null)
            {
                _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
                _watcher.MovementThreshold = 20;
                _watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(_watcher_StatusChanged);
                _watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(_watcher_PositionChanged);
            }

            Feedback = "Starting location service. This could take up to one minute.";

            bool started = _watcher.TryStart(true, TimeSpan.FromMilliseconds(60000));
            if (started)
            {
                if (_watcher.Status == GeoPositionStatus.Ready)
                {
                    Feedback = string.Format("Available: {0}, {1}, {2}",
                        _watcher.Position.Location.Latitude.ToString("0.000"),
                        _watcher.Position.Location.Longitude.ToString("0.000"),
                        _watcher.Position.Location.Course.ToString("0.000"));
                }
                else
                {
                    Feedback = "Location data is not currently available. Please try again later.";
                }
            }
            else
            {
                Feedback = "The location service could not be started.";
            }
        }

        void _watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    if (_watcher.Permission == GeoPositionPermission.Denied)
                    {
                        Feedback = "This app does not have permission to access location.";
                    }
                    else
                    {
                        Feedback = "Location is not functioning on this device";
                    }
                    break;

                case GeoPositionStatus.Initializing:
                    break;

                case GeoPositionStatus.NoData:
                    Feedback = "Location data is not available.";
                    break;

                case GeoPositionStatus.Ready:
                    Feedback = string.Format("Available: {0}, {1}, {2}",
                        _watcher.Position.Location.Latitude.ToString("0.000"),
                        _watcher.Position.Location.Longitude.ToString("0.000"),
                        _watcher.Position.Location.Course.ToString("0.000"));
                    break;
            }
        }

        private void _watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            Feedback = string.Format("{0}, {1}, {2}",
                e.Position.Location.Latitude.ToString("0.000"),
                e.Position.Location.Longitude.ToString("0.000"),
                e.Position.Location.Course.ToString("0.000"));
        }

        private void StopSensor()
        {
            if (_watcher == null)
            {
                return;
            }

            _watcher.Stop();
            _watcher = null;
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}