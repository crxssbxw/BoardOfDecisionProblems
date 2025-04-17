using ProblemsBoardLib.Commands;
using ProblemsBoardLib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProblemsBoardLib.ViewModel
{
    public class AddEditThemeViewModel : BaseViewModel, IDataErrorInfo
    {
        private Theme theme;
        public Theme Theme
        {
            get => theme;
            set
            {
                theme = value;
                OnPropertyChanged(nameof(Theme));
                OnPropertyChanged("");
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

        public bool IsNoEdit { get; set; } = true;

        public Department Department { get; set; }

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

        public ObservableCollection<Theme> Themes { get; set; }

        public string Name
        {
            get => Theme.Name;
            set => Theme.Name = value;
        }
        public string? Description
        {
            get => Theme.Description;
            set => Theme.Description = value;
        }
        public int DaysToDecide
        {
            get => Theme.DaysToDecide;
            set => Theme.DaysToDecide = value;
        }

        public string this[string columnName]
        {
            get
            {
                string error = null;

                switch (columnName)
                {
                    case "Name":
                        if (string.IsNullOrEmpty(Name))
                            error = "Имя не может быть пустым";
                        break;
                    case "DaysToDecide":
                        if (string.IsNullOrEmpty(DaysToDecide.ToString()))
                            error = "Укажите дни решения";
                        if (DaysToDecide <= 0)
                            error = "Дни решения должны быть строго больше 0";
                        break;
                }
                return error;
            }
        }

        public string Error
        {
            get
            {
                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(DaysToDecide.ToString()))
                    return "Поля не могут быть пустыми";
                return null;
            }
        }

        private RelayCommand acceptTheme;
        public RelayCommand AcceptTheme
        {
            get
            {
                return acceptTheme ?? (acceptTheme = new(obj =>
                {
                    
                    if (IsThisDepartment && Theme.ThemeId == 0)
                    {
                        var dep = dbContext.Departments.Find(Department.DepartmentId);
                        if (dep.Themes == null)
                            dep.Themes = [];

                        dep.Themes.Add(Theme);
                    }
                    else if (Theme.ThemeId == 0)
                        dbContext.Themes.Add(Theme);

                    try
                    {
                        dbContext.SaveChanges();
                        Themes.Add(Theme);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }, 
                obj => string.IsNullOrEmpty(Error)));
            }
        }
    }
}
