using Accessibility;
using Microsoft.EntityFrameworkCore;
using RecipeDatabase.Model;
using RecipeDatabase.Model.DataBase;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RecipeDatabase
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		string connectionString = "Server=localhost;Database=RecipeDataBase;Trusted_Connection=True;Encrypt=false;";
		ViewModel.Recipe? _recipeView;
		ViewModel.Ingredient? _ingredientView;
		public MainWindow()
		{
			InitializeComponent();
		}

		private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
		private void AddMenuItem_Click(object sender, RoutedEventArgs e)
		{
			TabControlPages.SelectedIndex = 1;
		}

		private event EventHandler RecipeViewUpdateStartedHandler;
		private void RecipeViewUpdateStartedHandling()
		{
			Dispatcher.Invoke(() =>
			{
				UpdateProgressBar.IsIndeterminate = true;
				Grid.SetRowSpan(RecipesDataGrid, 1);
				UpdateProgressBarGrid.Visibility = Visibility.Visible;
			});
		}

		private void UpdateRecipeView()
		{
		}
		private void UpdateButton_Click(object sender, RoutedEventArgs e)
		{
			Task.Run(() =>
			{
				UpdateRecipeView();
			});
			if (TabControlPages.SelectedIndex != 0)
				TabControlPages.SelectedIndex = 0;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			_recipeView = (RecipesDataGrid.DataContext) as ViewModel.Recipe;
			_ingredientView = (IngredientDataGrid.DataContext) as ViewModel.Ingredient;

			Task.Run(() =>
			{
				UpdateRecipeView();
			});
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
		}

		private void IngredientDataGridDeleteMenu_Click(object sender, RoutedEventArgs e)
		{
		}



		private void RecipeDataGridDeleteMenu_Click(object sender, RoutedEventArgs e)
		{
		}
	}
}