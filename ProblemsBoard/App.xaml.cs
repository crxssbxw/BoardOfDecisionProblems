using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using ProblemsBoardLib;
using ProblemsBoardLib.DialogWindows;
using ProblemsBoardLib.Models;
using ProblemsBoardLib.ViewModel;

namespace ProblemsBoard;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        using (DatabaseContext dbContext = new())
        {
            //if (dbContext.Admins.Count() == 0)
            //{
            //    dbContext.Add(
            //        new Admin()
            //        {
            //            Login = "super",
            //            Password = Helper.EncryptString("super")
            //        });
            //    dbContext.SaveChanges();
            //}

            if (dbContext.Users.Count() == 0)
            {
                dbContext.Users.Add(
                    new User()
                    {
                        Login = "super",
                        Password = Helper.EncryptString("super"),
                        Role = "Админ"
                    });
                dbContext.SaveChanges();
            }

            if (dbContext.Themes.Count() == 0)
            {
                string project = Directory.GetCurrentDirectory();
                string jsonPath = Path.Combine(project, "Assets/JSON/BaseThemes.json");
                dbContext.AddRange(Helper.ThemesSerialization(jsonPath));
                dbContext.SaveChanges();
            }
        }
    }
}

