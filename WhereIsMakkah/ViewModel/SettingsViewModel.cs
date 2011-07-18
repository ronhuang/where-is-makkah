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

        private void SendSettingsChangedMessage(string key)
        {
            var msg = new SettingsChangedMessage() { Key = key };
            Messenger.Default.Send<SettingsChangedMessage>(msg);
        }

        /// <summary>
        /// Property to get and set a RadioButton Setting Key.
        /// </summary>
        public bool MetricSetting
        {
            get
            {
                return App.AppSettings.MetricSetting;
            }
            set
            {
                App.AppSettings.MetricSetting = value;
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
                return !App.AppSettings.MetricSetting;
            }
            set
            {
                App.AppSettings.MetricSetting = !value;
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