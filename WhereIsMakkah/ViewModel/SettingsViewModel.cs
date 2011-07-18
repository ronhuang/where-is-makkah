using GalaSoft.MvvmLight;

namespace WhereIsMakkah.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
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
    public class SettingsViewModel : ViewModelBase
    {
        public string ApplicationTitle
        {
            get
            {
                return "Where is Makkah";
            }
        }

        public string SettingsTitle
        {
            get
            {
                return "Settings";
            }
        }

        public string UnitLabel
        {
            get
            {
                return "Units";
            }
        }

        public string MetricLabel
        {
            get
            {
                return "Metric (meters)";
            }
        }

        public string ImperialLabel
        {
            get
            {
                return "Imperial (miles)";
            }
        }

        public string AboutTitle
        {
            get
            {
                return "About";
            }
        }

        /// <summary>
        /// Initializes a new instance of the SettingsViewModel class.
        /// </summary>
        public SettingsViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real": Connect to service, etc...
            ////}
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean own resources if needed

        ////    base.Cleanup();
        ////}
    }
}