using Microsoft.EntityFrameworkCore;
using ProblemsBoardLib.Commands;
using ProblemsBoardLib.Models;
using ProblemsBoardLib.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            get => themes;
            set
            {
                themes = value;
                OnPropertyChanged(nameof(Themes));
            }
        }

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

        private Theme selectedTheme;
        public Theme SelectedTheme
        {
            get => selectedTheme;
            set
            {
                selectedTheme = value;
                OnPropertyChanged(nameof(SelectedTheme));
            }
        }

        private bool isThisDepartment;
        public bool IsThisDepartment
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
                            LoggingTool.NewThemeSet(Theme);
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
                            LoggingTool.ThemeChanged(Theme);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                },
                obj => true));
            }
        }

        #endregion
    }
}
