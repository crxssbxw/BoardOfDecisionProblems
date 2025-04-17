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
            }
        }

        #endregion
    }
}
