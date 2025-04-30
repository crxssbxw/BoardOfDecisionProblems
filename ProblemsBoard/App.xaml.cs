using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using ProblemsBoardLib;
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
            if (dbContext.Admins.Count() == 0)
            {
                dbContext.Add(
                    new Admin()
                    {
                        Login = "super",
                        Password = Helper.EncryptString("super")
                    });
                dbContext.SaveChanges();
            }

            if (dbContext.Themes.Count() == 0)
            {
                string project = Directory.GetCurrentDirectory();
                dbContext.AddRange(Helper.ThemesSerialization(@$"{project}\..\..\..\Assets\JSON\BaseThemes.json"));
                dbContext.SaveChanges();
            }
        }
    }
}

