using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using WhereIsMakkah.Util;
using WhereIsMakkah.Model;

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
                return "Metric (kilometers)";
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

        public string VersionLabel
        {
            get
            {
                const string VerLabel = "Version=";
                string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().FullName;
                int startIndex = assemblyName.IndexOf(VerLabel) + VerLabel.Length;
                int endIndex = assemblyName.IndexOf(',', startIndex + 1);
                string version = assemblyName.Substring(startIndex, endIndex - startIndex);

                return string.Format("Version: {0}", version);
            }
        }

        public string AboutLabel
        {
            get
            {
                return "Where Is Makkah shows the direction to Makkah (Mecca.)";
            }
        }

        public string NoteLabel
        {
            get
            {
                return "Note: since Windows Phone 7 does not support Compass API yet (will be available in 7.1,) you have to manually point the top of the phone toward north first to show the direction to Makkah.";
            }
        }

        public string DonateLabel
        {
            get
            {
                return "Please click the ad if you like this application.";
            }
        }

        /// <summary>
        /// Initializes a new instance of the SettingsViewModel class.
        /// </summary>
        public SettingsViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                // Code runs "for real": Connect to service, etc...
                _metricSetting = App.AppSettings.MetricSetting;
            }
        }

        private void SendSettingsChangedMessage(string key)
        {
            var msg = new SettingsChangedMessage() { Key = key };
            Messenger.Default.Send<SettingsChangedMessage>(msg);
        }

        private bool _metricSetting = true;

        /// <summary>
        /// Property to get and set a RadioButton Setting Key.
        /// </summary>
        public bool MetricSetting
        {
            get
            {
                return _metricSetting;
            }
            set
            {
                _metricSetting = App.AppSettings.MetricSetting = value;
                SendSettingsChangedMessage("Unit");
            }
        }

        /// <summary>
        /// Property to get and set a RadioButton Setting Key.
        /// </summary>
        public bool ImperialSetting
        {
            get
            {
                return !_metricSetting;
            }
            set
            {
                _metricSetting = App.AppSettings.MetricSetting = !value;
                SendSettingsChangedMessage("Unit");
            }
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean own resources if needed

        ////    base.Cleanup();
        ////}
    }
}