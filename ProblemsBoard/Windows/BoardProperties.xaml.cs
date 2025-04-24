using ProblemsBoardLib.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProblemsBoard.Windows
{
	/// <summary>
	/// Логика взаимодействия для BoardProperties.xaml
	/// </summary>
	public partial class BoardProperties : Window
	{
		private BoardPropertiesViewModel BoardPropertiesViewModel { get; set; }
		public BoardProperties()
		{
			InitializeComponent();
		}

		public BoardProperties(BoardPropertiesViewModel viewModel)
		{
			InitializeComponent();
			BoardPropertiesViewModel = viewModel;
			DataContext = BoardPropertiesViewModel;
		}

		private void OkBT_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
