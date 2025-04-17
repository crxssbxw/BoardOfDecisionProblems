using Microsoft.EntityFrameworkCore;
using ProblemsBoardLib.Commands;
using ProblemsBoardLib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            get => themes;
            set
            {
                themes = value;
                OnPropertyChanged(nameof(Themes));
            }
        }

        public Department Department { get; set; }

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

        #region Commands

        private RelayCommand addTheme;

        public ThemesViewModel(Department department)
        {
            Department = department;
            dbContext.Departments.Load();
            foreach (var theme in dbContext.Themes.Where(a => a.Department.DepartmentId == Department.DepartmentId || a.Department == null)) 
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
                        Themes = this.Themes,
                        Department = Department,
                        Title = "Новая тема"
                    };
                },
                obj => true));
            }
        }

        #endregion
    }
}
