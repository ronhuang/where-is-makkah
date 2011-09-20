using System;
using System.Device.Location;
using System.Windows.Media.Media3D;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
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
        /// The <see cref="OffsetFromNorthMatrix" /> property's name.
        /// </summary>
        public const string OffsetFromNorthMatrixPropertyName = "OffsetFromNorthMatrix";

        private Matrix3D _offsetFromNorthMatrix = Matrix3D.Identity;

        /// <summary>
        /// Gets the OffsetFromNorthMatrix property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public Matrix3D OffsetFromNorthMatrix
        {
            get
            {
                return _offsetFromNorthMatrix;
            }

            set
            {
                if (_offsetFromNorthMatrix == value)
                {
                    return;
                }

                var oldValue = _offsetFromNorthMatrix;
                _offsetFromNorthMatrix = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(OffsetFromNorthMatrixPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="LocationDetermined" /> property's name.
        /// </summary>
        public const string LocationDeterminedPropertyName = "LocationDetermined";

        private bool _locationDetermined = false;

        /// <summary>
        /// Gets the LocationDetermined property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public bool LocationDetermined
        {
            get
            {
                return _locationDetermined;
            }

            set
            {
                if (_locationDetermined == value)
                {
                    return;
                }

                var oldValue = _locationDetermined;
                _locationDetermined = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(LocationDeterminedPropertyName);
                RaisePropertyChanged(DirectionDeterminedPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="MotionDetermined" /> property's name.
        /// </summary>
        public const string MotionDeterminedPropertyName = "MotionDetermined";

        private bool _motionDetermined = false;

        /// <summary>
        /// Gets the MotionDetermined property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public bool MotionDetermined
        {
            get
            {
                return _motionDetermined;
            }

            set
            {
                if (_motionDetermined == value)
                {
                    return;
                }

                var oldValue = _motionDetermined;
                _motionDetermined = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(MotionDeterminedPropertyName);
                RaisePropertyChanged(DirectionDeterminedPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="DirectionDetermined" /> property's name.
        /// </summary>
        public const string DirectionDeterminedPropertyName = "DirectionDetermined";

        /// <summary>
        /// Gets the DirectionDetermined property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool DirectionDetermined
        {
            get
            {
                return _locationDetermined && _motionDetermined;
            }
        }

        /// <summary>
        /// The <see cref="CurrentMatrix" /> property's name.
        /// </summary>
        public const string CurrentMatrixPropertyName = "CurrentMatrix";

        private Matrix3D _currentMatrix = Matrix3D.Identity;

        /// <summary>
        /// Gets the CurrentMatrix property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public Matrix3D CurrentMatrix
        {
            get
            {
                return _currentMatrix;
            }

            set
            {
                if (_currentMatrix == value)
                {
                    return;
                }

                var oldValue = _currentMatrix;
                _currentMatrix = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(CurrentMatrixPropertyName);
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

            Feedback = AppResources.FeedbackStartingLabel;

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
                    OffsetFromNorthMatrix = RotateZTransform(destZ);
                    LocationDetermined = true;

                    StopLocationSensor();
                    break;
            }
        }

        private void StopLocationSensor()
        {
            if (_watcher == null)
            {
                return;
            }

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
            const double arrowWidth = 400.0;
            const double arrowHeight = 500.0;
            const double layoutWidth = 400.0;
            const double layoutHeight = 500.0;

            // Translate the image along the negative Z-axis such that it occupies 50% of the
            // vertical field of view.
            double fovY = Math.PI / 2.0;
            double translationZ = -arrowHeight / Math.Tan(fovY / 2.0);

            // You can create a 3D effect by creating a number of simple 
            // tranformation Matrix3D matrixes and then multiply them together.
            Matrix3D centerImageAtOrigin = TranslationTransform(
                     -arrowWidth / 2.0,
                     -arrowHeight / 2.0, 0);
            Matrix3D invertYAxis = CreateScaleTransform(1.0, -1.0, 1.0);
            Matrix3D translateAwayFromCamera = TranslationTransform(0, 0, translationZ);
            Matrix3D perspective = PerspectiveTransformFovRH(fovY,
                    layoutWidth / layoutHeight,                         // aspect ratio
                    1.0,                                                // near plane
                    1000.0);                                            // far plane
            Matrix3D viewport = ViewportTransform(layoutWidth, layoutHeight);

            Matrix3D m = centerImageAtOrigin * invertYAxis;
            m = m * OffsetFromNorthMatrix;
            m = m * XnaMatrixToMatrix3D(e.Attitude.RotationMatrix);
            m = m * translateAwayFromCamera;
            m = m * perspective;
            m = m * viewport;

            CurrentMatrix = m;
            MotionDetermined = true;
        }

        private Matrix3D XnaMatrixToMatrix3D(Matrix matrix)
        {
            Matrix3D m = new Matrix3D();

            m.M11 = matrix.M11; m.M12 = matrix.M12; m.M13 = matrix.M13; m.M14 = matrix.M14;
            m.M21 = matrix.M21; m.M22 = matrix.M22; m.M23 = matrix.M23; m.M24 = matrix.M24;
            m.M31 = matrix.M31; m.M32 = matrix.M32; m.M33 = matrix.M33; m.M34 = matrix.M34;
            m.OffsetX = matrix.M41; m.OffsetY = matrix.M42; m.OffsetZ = matrix.M43; m.M44 = matrix.M44;

            return m;
        }

        private Matrix3D CreateScaleTransform(double sx, double sy, double sz)
        {
            Matrix3D m = new Matrix3D();

            m.M11 = sx; m.M12 = 0.0; m.M13 = 0.0; m.M14 = 0.0;
            m.M21 = 0.0; m.M22 = sy; m.M23 = 0.0; m.M24 = 0.0;
            m.M31 = 0.0; m.M32 = 0.0; m.M33 = sz; m.M34 = 0.0;
            m.OffsetX = 0.0; m.OffsetY = 0.0; m.OffsetZ = 0.0; m.M44 = 1.0;

            return m;
        }

        private Matrix3D TranslationTransform(double tx, double ty, double tz)
        {
            Matrix3D m = new Matrix3D();
            m.M11 = 1.0; m.M12 = 0.0; m.M13 = 0.0; m.M14 = 0.0;
            m.M21 = 0.0; m.M22 = 1.0; m.M23 = 0.0; m.M24 = 0.0;
            m.M31 = 0.0; m.M32 = 0.0; m.M33 = 1.0; m.M34 = 0.0;
            m.OffsetX = tx; m.OffsetY = ty; m.OffsetZ = tz; m.M44 = 1.0;
            return m;
        }

        private Matrix3D RotateZTransform(double degree)
        {
            double theta = degree * Math.PI / 180.0;
            double cos = Math.Cos(theta);
            double sin = Math.Sin(theta);

            Matrix3D m = new Matrix3D();
            m.M11 = cos; m.M12 = sin; m.M13 = 0.0; m.M14 = 0.0;
            m.M21 = -sin; m.M22 = cos; m.M23 = 0.0; m.M24 = 0.0;
            m.M31 = 0.0; m.M32 = 0.0; m.M33 = 1.0; m.M34 = 0.0;
            m.OffsetX = 0.0; m.OffsetY = 0.0; m.OffsetZ = 0.0; m.M44 = 1.0;
            return m;
        }

        private Matrix3D PerspectiveTransformFovRH(double fieldOfViewY, double aspectRatio, double zNearPlane, double zFarPlane)
        {
            double height = 1.0 / Math.Tan(fieldOfViewY / 2.0);
            double width = height / aspectRatio;
            double d = zNearPlane - zFarPlane;

            Matrix3D m = new Matrix3D();
            m.M11 = width; m.M12 = 0; m.M13 = 0; m.M14 = 0;
            m.M21 = 0; m.M22 = height; m.M23 = 0; m.M24 = 0;
            m.M31 = 0; m.M32 = 0; m.M33 = zFarPlane / d; m.M34 = -1;
            m.OffsetX = 0; m.OffsetY = 0; m.OffsetZ = zNearPlane * zFarPlane / d; m.M44 = 0;

            return m;
        }

        private Matrix3D ViewportTransform(double width, double height)
        {
            Matrix3D m = new Matrix3D();

            m.M11 = width / 2.0; m.M12 = 0.0; m.M13 = 0.0; m.M14 = 0.0;
            m.M21 = 0.0; m.M22 = -height / 2.0; m.M23 = 0.0; m.M24 = 0.0;
            m.M31 = 0.0; m.M32 = 0.0; m.M33 = 1.0; m.M34 = 0.0;
            m.OffsetX = width / 2.0; m.OffsetY = height / 2.0; m.OffsetZ = 0.0; m.M44 = 1.0;

            return m;
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