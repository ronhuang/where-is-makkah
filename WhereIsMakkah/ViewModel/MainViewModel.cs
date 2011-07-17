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

        public string Feedback
        {
            get
            {
                return "The big arrow should point to Makkah.";
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

        private bool InitSensor()
        {
            return true;
        }

        private void StartSensor()
        {
        }

        private void StopSensor()
        {
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}