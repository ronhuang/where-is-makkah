using Microsoft.Phone.Controls;
using System.Windows.Media;
using Microsoft.Phone.Shell;
using System.Windows.Data;

namespace WhereIsMakkah
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            this.Loaded += (sender, e) =>
                {
                    var progressIndicator = SystemTray.ProgressIndicator;
                    if (progressIndicator != null)
                    {
                        return;
                    }

                    progressIndicator = new ProgressIndicator();
                    progressIndicator.IsIndeterminate = true;

                    SystemTray.SetProgressIndicator(this, progressIndicator);

                    var binding = new Binding("Busy") { Source = this.LayoutRoot.DataContext };
                    BindingOperations.SetBinding(progressIndicator, ProgressIndicator.IsVisibleProperty, binding);
                };

            this.Unloaded += (sender, e) =>
                {
                    var progressIndicator = SystemTray.ProgressIndicator;
                    if (progressIndicator == null)
                    {
                        return;
                    }

                    SystemTray.SetProgressIndicator(this, null);
                };
        }
    }
}
