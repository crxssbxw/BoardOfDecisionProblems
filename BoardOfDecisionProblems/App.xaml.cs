using BoardOfDecisionProblems.Forms;
using BoardOfDecisionProblems.Models;
using BoardOfDecisionProblems.RoleModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace BoardOfDecisionProblems
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Контекст базы данных
        /// </summary>
        public static DatabaseContext dbContext = new();

        public static ResponsibleUser CurrentResponsible = new();

        private static SuperUser? admin = new();
        public static SuperUser? Admin { get => admin; set => admin = value; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            // Set the default culture to en-IN
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ru-RU");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("ru-RU");
            CultureInfo.DefaultThreadCurrentCulture.DateTimeFormat.ShortDatePattern = new("d");

            Directory.CreateDirectory("Settings");

            if (File.Exists("Settings/Admin.json"))
            {
                using (FileStream fs = new("Settings/Admin.json", FileMode.Open))
                {
                    Admin = await JsonSerializer.DeserializeAsync<SuperUser>(fs);
                }
            }
            else
            {
                using (FileStream fs = new("Settings/Admin.json", FileMode.CreateNew))
                {
                    Admin = new() { Login = "admin", Password = "admin" };
                    await JsonSerializer.SerializeAsync(fs, Admin);
                }
            }
            base.OnStartup(e);
        }
    }
}
