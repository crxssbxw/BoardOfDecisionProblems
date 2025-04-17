using ProblemsBoardLib.Commands;
<<<<<<< HEAD
using ProblemsBoardLib.Forms;
using ProblemsBoardLib.Models;
using Microsoft.EntityFrameworkCore;
using ProblemsBoardLib.ViewModel;
=======
using ProblemsBoardLib.Models;
>>>>>>> ce1a19b (Added themes view in menu, adding themes to db, model changes)
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ProblemsBoardLib.ViewModel
{
    /// <summary>
    /// Представление модели Тем
    /// </summary>
    public class ThemesViewModel : BaseViewModel
    {
        private ObservableCollection<Theme> themes = new();
        /// <summary>
        /// Коллекция Тем
        /// </summary>
        public ObservableCollection<Theme> Themes
        {
=======

namespace ProblemsBoardLib.ViewModel
{
    public class ThemesViewModel : BaseViewModel
    {
        private AddEditThemeViewModel addEditThemeViewModel;
        public AddEditThemeViewModel AddEditThemeViewModel 
        { 
            get => addEditThemeViewModel; 
            set
            {
                addEditThemeViewModel = value;
                OnPropertyChanged(nameof(AddEditThemeViewModel));
            }
        }

        private ObservableCollection<Theme> themes = new();
        public ObservableCollection<Theme> Themes 
        { 
>>>>>>> ce1a19b (Added themes view in menu, adding themes to db, model changes)
            get => themes;
            set
            {
                themes = value;
                OnPropertyChanged(nameof(Themes));
            }
        }

<<<<<<< HEAD
        private Theme selectedTheme = new();
        /// <summary>
        /// Объект выбранной Темы в таблице
        /// </summary>
=======
        private Theme selectedTheme;
>>>>>>> ce1a19b (Added themes view in menu, adding themes to db, model changes)
        public Theme SelectedTheme
        {
            get => selectedTheme;
            set
            {
                selectedTheme = value;
                OnPropertyChanged(nameof(SelectedTheme));
            }
        }

<<<<<<< HEAD
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
        /// <summary>
        /// Команда добавления темы
        /// </summary>
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

                        LogEvent logEvent = new LogEvent()
                        {
                            Date = DateOnly.FromDateTime(DateTime.Now),
                            Time = TimeOnly.FromDateTime(DateTime.Now),
                            Title = $"Добавление темы №{theme.ThemeId}",
                            User = "Admin"
                        };
                        logEvent.Object = $"Тема №{theme.ThemeId}";
                        dbContext.Add(logEvent);
                        ProblemViewModel.LogsViewModel.LogEvents.Add(logEvent);

                        dbContext.SaveChanges();

                        Themes.Add(theme);
                    }
                }, obj => true));
            }
        }

        private RelayCommand delete;
        /// <summary>
        /// Команда удаления темы
        /// </summary>
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

                        LogEvent logEvent = new LogEvent()
                        {
                            Date = DateOnly.FromDateTime(DateTime.Now),
                            Time = TimeOnly.FromDateTime(DateTime.Now),
                            Title = $"Добавление темы №{SelectedTheme.ThemeId}",
                            User = "Admin"
                        };
                        logEvent.Object = $"Тема №{SelectedTheme.ThemeId}";
                        dbContext.Add(logEvent);
                        ProblemViewModel.LogsViewModel.LogEvents.Add(logEvent);

                        dbContext.SaveChanges();
                        Themes.Remove(SelectedTheme);
                    }
                }, obj => SelectedTheme != null));
=======
        #region Commands

        private RelayCommand addTheme;

        public ThemesViewModel()
        {
            foreach (var theme in dbContext.Themes) 
            {
                Themes.Add(theme);
            }
        }

        public RelayCommand AddTheme
        {
            get
            {
                return addTheme ?? (addTheme = new(obj =>
                {
                    AddEditThemeViewModel = new()
                    {
                        Theme = new(),
                        Themes = this.Themes
                    };
                },
                obj => true));
>>>>>>> ce1a19b (Added themes view in menu, adding themes to db, model changes)
            }
        }

        #endregion
    }
}
