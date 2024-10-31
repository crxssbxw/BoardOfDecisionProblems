using BoardOfDecisionProblems.Models;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Windows;

namespace BoardOfDecisionProblems
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static DatabaseContext dbContext = new();

        protected override void OnStartup(StartupEventArgs e)
        {
            // Set the default culture to en-IN
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ru-RU");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("ru-RU");
            CultureInfo.DefaultThreadCurrentCulture.DateTimeFormat.ShortDatePattern = new("d");

            base.OnStartup(e);
        }
    }
}
