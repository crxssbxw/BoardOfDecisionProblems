using ProblemsBoardLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProblemsBoardLib.ViewModel
{
    /// <summary>
    /// Общее модельное представление, реализует интерфейс INotifyPropertyChanged для обновления связанных значений
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Контекст БД
        /// </summary>
        public DatabaseContext dbContext = App.dbContext;
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CollectionViewSource ViewSource { get; set; } = new();
        public ICollectionView CollectionView { get => ViewSource.View; }


    }
}
