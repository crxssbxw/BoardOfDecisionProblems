using BoardOfDecisionProblems.Commands;
using BoardOfDecisionProblems.Forms;
using BoardOfDecisionProblems.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BoardOfDecisionProblems.ViewModel
{
    public class ThemesViewModel : BaseViewModel
    {
        private ObservableCollection<Theme> themes = new();
        public ObservableCollection<Theme> Themes
        {
            get => themes;
            set
            {
                themes = value;
                OnPropertyChanged(nameof(Themes));
            }
        }

        private Theme selectedTheme = new();
        public Theme SelectedTheme
        {
            get => selectedTheme;
            set
            {
                selectedTheme = value;
                OnPropertyChanged(nameof(SelectedTheme));
            }
        }

        public ThemesViewModel()
        {
            dbContext.Problems.Load();
            foreach (var theme in dbContext.Themes)
            {
                Themes.Add(theme);
            }
            ViewSource.Source = Themes;
        }

        #region Commands

        private RelayCommand add;
        public RelayCommand Add
        {
            get
            {
                return add ?? (add = new(obj =>
                {
                    Theme theme = new();
                    NewTheme newTheme = new NewTheme();
                    newTheme.DataContext = theme;

                    if(newTheme.ShowDialog() == true)
                    {
                        dbContext.Themes.Add(theme);
                        dbContext.SaveChanges();

                        Themes.Add(theme);
                    }
                }, obj => true));
            }
        }

        private RelayCommand delete;
        public RelayCommand Delete
        {
            get
            {
                return delete ?? (delete = new(obj =>
                {
                    if(SelectedTheme.Problems != null)
                    {
                        MessageBox.Show("Есть проблемы, ссылающиеся на эту тему!", "Осторожно");
                    }
                    var result = MessageBox.Show("Вы уверены, что хотите удалить данную тему?", "Предупреждение", MessageBoxButton.YesNo);

                    if(result == MessageBoxResult.Yes)
                    {
                        dbContext.Themes.Remove(SelectedTheme);
                        dbContext.SaveChanges();
                        Themes.Remove(SelectedTheme);
                    }
                }, obj => SelectedTheme != null));
            }
        }

        #endregion
    }
}
