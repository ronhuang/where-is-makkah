using System;
using System.Windows;
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

        public string Inst1
        {
            get
            {
                return "Manually point the small arrow to north.";
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

                // Update bindings and broadcast change using GalaSoft.MvvmLight.Messenging
                RaisePropertyChanged(UnitPropertyName, oldValue, value, true);
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

                // Update bindings and broadcast change using GalaSoft.MvvmLight.Messenging
                RaisePropertyChanged(DistancePropertyName, oldValue, value, true);
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