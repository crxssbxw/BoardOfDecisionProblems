<<<<<<< HEAD
﻿using ProblemsBoardLib.Commands;
<<<<<<< HEAD
using ProblemsBoardLib.Forms;
=======
﻿using Microsoft.EntityFrameworkCore;
using ProblemsBoardLib.Commands;
>>>>>>> 4bee76b (Fixed Themes, now themes can add to department)
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
=======
using System.Windows;
>>>>>>> 33ba9c2 (Added New Logo, refactor themes vm)

namespace ProblemsBoardLib.ViewModel
{
    public class ThemesViewModel : BaseViewModel
    {
        public ThemesViewModel(Department department)
        {
            Department = dbContext.Departments.Find(department.DepartmentId);
            dbContext.Themes.Load();
            if (Department != null && Department.Themes != null)
            {
                foreach (var theme in Department.Themes)
                {
                    Themes.Add(theme);
                }
            }

            foreach (var theme in dbContext.Themes.Where(a => a.Department == null))
            {
                Themes.Add(theme);
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
<<<<<<< HEAD
<<<<<<< HEAD
        private Theme selectedTheme = new();
        /// <summary>
        /// Объект выбранной Темы в таблице
        /// </summary>
=======
=======
        public Department Department { get; set; }
=======
        private string title;
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private Department department;
        public Department Department 
        { 
            get => department; 
            set 
            { 
                department = value;
                OnPropertyChanged(nameof(Department));
            }
        }

        private Theme theme;
        public Theme Theme
        {
            get => theme;
            set
            {
                theme = value;
                OnPropertyChanged(nameof(Theme));
            }
        }
>>>>>>> 33ba9c2 (Added New Logo, refactor themes vm)

>>>>>>> 4bee76b (Fixed Themes, now themes can add to department)
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

        public ThemesViewModel(Department department)
=======
        private bool isThisDepartment;
        public bool IsThisDepartment
>>>>>>> 33ba9c2 (Added New Logo, refactor themes vm)
        {
            get => isThisDepartment;
            set
            {
                isThisDepartment = value;
                OnPropertyChanged(nameof(IsThisDepartment));
            }
        }

        private bool isNoEdit;
        public bool IsNoEdit
        {
            get => isNoEdit;
            set
            {
                isNoEdit = value;
                OnPropertyChanged(nameof(IsNoEdit));
            }
        }

        #region Commands

        private RelayCommand addTheme;
        public RelayCommand AddTheme
        {
            get
            {
                return addTheme ?? (addTheme = new(obj =>
                {
                    Title = "Добавить";
                    IsNoEdit = true;
                    Theme = new();
                },
                obj => true));
            }
        }

        private RelayCommand editTheme;
        public RelayCommand EditTheme
        {
            get
            {
                return editTheme ?? (editTheme = new(obj =>
                {
                    Title = "Изменить";
                    IsNoEdit = false;
                    Theme = SelectedTheme;
                    if (SelectedTheme.Department == Department)
                        IsThisDepartment = true;
                    else IsThisDepartment = false;
                },
                obj => SelectedTheme != null));
            }
        }

        private RelayCommand acceptTheme;
        public RelayCommand AcceptTheme
        {
            get
            {
                return acceptTheme ?? (acceptTheme = new(obj =>
                {
                    
                    if (Theme.ThemeId == 0)
                    {
                        if (IsThisDepartment)
                        {
                            Theme.Department = Department;
                        }
                        dbContext.Add(Theme);
                        try
                        {
                            dbContext.SaveChanges();
                            Themes.Add(Theme);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        try
                        {
                            dbContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                },
                obj => true));
>>>>>>> ce1a19b (Added themes view in menu, adding themes to db, model changes)
            }
        }

        #endregion
    }
}
