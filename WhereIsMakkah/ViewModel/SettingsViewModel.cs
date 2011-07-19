using System;
using System.Resources;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using WhereIsMakkah.Util;
using WhereIsMakkah.Model;
using WhereIsMakkah.Lang;

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
        public string VersionLabel
        {
            get
            {
                const string VerLabel = "Version=";
                string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().FullName;
                int startIndex = assemblyName.IndexOf(VerLabel) + VerLabel.Length;
                int endIndex = assemblyName.IndexOf(',', startIndex + 1);
                string version = assemblyName.Substring(startIndex, endIndex - startIndex);

                return string.Format(AppResources.VersionLabel, version);
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