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

namespace RecipeDatabase1
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		string connectionString = "Server=localhost;Database=RecipeDataBase;Trusted_Connection=True;Encrypt=false;";
		View.Recipe? _recipeView;
		View.Ingredient? _ingredientView;
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
			TabControlPages.SelectedIndex = 3;
		}
		private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
		{
			TabControlPages.SelectedIndex = 4;
		}
		private void EditMenuItem_Click(object sender, RoutedEventArgs e)
		{
			TabControlPages.SelectedIndex = 2;
		}


		private void UpdateRecipeView()
		{
			using (ApplicationContext applicationContext = new(connectionString))
			{
				var recipes = applicationContext.Recipe.ToList();
				List<ViewModel.Recipe> viewModelRecipes = new List<ViewModel.Recipe>();
				foreach (var recipe in recipes)
				{
					List<int> ingredientIds = new List<int>();
					object recipeIngredients = applicationContext.RecipeIngredient.ToList();
					recipeIngredients=(recipeIngredients as IEnumerable<Model.RecipeIngredient>)!.Where(x => x.IdRecipe == recipe.Id);
					StringBuilder stringBuilder = new StringBuilder();

					var ingredients = applicationContext.Ingredient.ToList().Where(x => (recipeIngredients as IEnumerable<Model.RecipeIngredient>)!.Any(y => y.IdIngredient == x.Id));
					List<int?> ingredientAmounts = new();
					foreach(var ingredient in ingredients)
					{
						var ingredientAmount = (recipeIngredients as IEnumerable<Model.RecipeIngredient>)!.Where(x => x.IdIngredient==ingredient.Id);
						foreach (var recipeIngredient in ingredientAmount)
							ingredientAmounts.Add(recipeIngredient.Amount);
					}


					for(int i=0;i<ingredients.Count();++i)
					{
						var ingredientMeasure = applicationContext.MeasureUnit.ToList().Where(x => x.Id == ingredients.ElementAt(i).IdMeasureUnit);
						string ingredientName = "";
						if(ingredientMeasure is not null)
							ingredientName= !ingredientMeasure!.Any() ? "" : ingredientMeasure.First().Name!;
						stringBuilder.Append($"{ingredients.ElementAt(i).Name}\t{ingredientAmounts[i]}\t{ingredientName}\n");
					}
					stringBuilder.Remove(stringBuilder.Length - 1, 1);

					viewModelRecipes.Add(new ViewModel.Recipe(recipe.Name!, recipe.Actions!, stringBuilder.ToString()));
				}

				Dispatcher.Invoke
				(()=>
					_recipeView?.Update(viewModelRecipes)
				);
			}
		}
		private void UpdateButton_Click(object sender, RoutedEventArgs e)
		{
			Task.Run(() =>
			{
				UpdateRecipeView();
			});
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			_recipeView = (RecipesDataGrid.DataContext) as View.Recipe;
			_ingredientView = (dataGrid.DataContext) as View.Ingredient;
			{
				ViewModel.Ingredient ingredient = new("gh", null, "");
				ingredient.IsEditable = true;
				_ingredientView?.Add(ingredient);
			}

			Task.Run(() =>
			{
				UpdateRecipeView();
			});
		}

		private void UpdateMenuItem_Click(object sender, RoutedEventArgs e)
		{
			Task.Run(() =>
			{
				UpdateRecipeView();
			});
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (RecipeNameTextBox.Text is null || RecipeNameTextBox.Text == string.Empty)
				{
					MessageBox.Show
						("Введите название рецепта", "Пустое назва.ние!", MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}
				/*if (IngredientsTextBox.Text is null || IngredientsTextBox.Text == string.Empty)
				{
					MessageBox.Show
						("Введите ингредиенты", "Пустое поле!", MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}*/
				if (ActionsTextBox.Text is null || ActionsTextBox.Text == string.Empty)
				{
					MessageBox.Show
						("Введите описание приготовления", "Пустое поле!", MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}

				//var ingredients=IngredientsTextBox.Text.Split("\r\n");

				/*MessageBox.Show
				(
					$"Рецепт \"{newRecipe.Name}\" добавлен",
					"Добавление рецепта",
					MessageBoxButton.OK,
					MessageBoxImage.Information
				);*/
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка добавления рецепта!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}