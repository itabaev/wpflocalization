using System.Windows;
using WpfLocalization.Localization;

namespace WpfLocalization
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            LocalizationManager.Instance.LocalizationProvider = new ResxLocalizationProvider();
        }
    }
}
