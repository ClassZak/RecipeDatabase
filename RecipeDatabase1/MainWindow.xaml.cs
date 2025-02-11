using Accessibility;
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
			TabControlPages.SelectedIndex = 1;
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
						stringBuilder.AppendLine($"{ingredients.ElementAt(i).Name} {ingredientAmounts[i]} {ingredientName}");
					}
					stringBuilder.Remove(stringBuilder.Length - 2, 2);

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
			_ingredientView = (IngredientDataGrid.DataContext) as View.Ingredient;

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
						("Введите название рецепта", "Пустое название!", MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}
				if (ActionsTextBox.Text is null || ActionsTextBox.Text == string.Empty)
				{
					MessageBox.Show
						("Введите описание приготовления", "Пустое поле!", MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}
				if (_ingredientView!.GetIngredients().Count() == 0)
				{
					MessageBox.Show
						("Введите ингредиенты рецепта", "Пустые поля!", MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}


				using (ApplicationContext applicationContext = new ApplicationContext(connectionString))
				{
					Model.Recipe modelRecipe = new(RecipeNameTextBox.Text, ActionsTextBox.Text);


					List<Model.RecipeIngredient> modelRecipeIngredients=new();
					foreach(var ingredient in _ingredientView.GetIngredients().ToList())
					{
						if (string.IsNullOrWhiteSpace(ingredient.Name) || ingredient.Name == string.Empty)
							continue;

						Model.Ingredient modelIngredient;

						if (applicationContext.MeasureUnit.ToList().Count == 0 && ingredient.MeasureUnit is not null)
							applicationContext.MeasureUnit.Add(new Model.MeasureUnit(ingredient.MeasureUnit));
						if(ingredient.MeasureUnit is not null && ingredient.MeasureUnit!=string.Empty && applicationContext.MeasureUnit.ToList().Count != 0)
						{
							var measureUnits = applicationContext.MeasureUnit.ToList().Where(x => x.Name!.ToLower() == ingredient?.MeasureUnit.ToLower());
							if (!measureUnits.Any())
								applicationContext.MeasureUnit.Add(new Model.MeasureUnit(ingredient.MeasureUnit));

							Model.MeasureUnit measureUnitForIngredient= applicationContext.MeasureUnit.ToList().Where(x => x.Name == ingredient.MeasureUnit).Last();
							modelIngredient = new(ingredient.Name.ToLower(), measureUnitForIngredient.Id);
						}
						else
							modelIngredient = new(ingredient.Name.ToLower());

						if(!applicationContext.Ingredient.ToList().Any(x=>x?.Name?.ToLower()==modelIngredient?.Name?.ToLower()))
							applicationContext.Add(modelIngredient);
						else
							applicationContext.Ingredient.ToList().Where(x => x?.Name?.ToLower() == modelIngredient?.Name?.ToLower()).First().Name = ingredient.Name.ToLower();


						applicationContext.SaveChanges();
						modelRecipeIngredients.Add(new Model.RecipeIngredient(modelRecipe.Id, modelIngredient.Id, ingredient.GetAmount()));
					}
					applicationContext.Recipe.Add(modelRecipe);
					applicationContext.SaveChanges();
					foreach (var el in modelRecipeIngredients)
						el.IdRecipe= applicationContext.Recipe.ToList().Where(x=>x.Name== modelRecipe.Name && x.Actions== modelRecipe.Actions).Last().Id;
					foreach (Model.RecipeIngredient el in modelRecipeIngredients)
						applicationContext.RecipeIngredient.Add(el);
					applicationContext.SaveChanges();

					MessageBox.Show($"Рецепт \"{modelRecipe.Name}\" успешно добавлен", "Рецепт успешно добавлен", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка добавления рецепта!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void IngredientDataGridDeleteMenu_Click(object sender, RoutedEventArgs e)
		{
			List<ViewModel.Ingredient> selectedItems = new();

			foreach (object selectedItem in IngredientDataGrid.SelectedItems)
				if(selectedItem is ViewModel.Ingredient)
					selectedItems.Add((selectedItem as ViewModel.Ingredient)!);
			foreach (ViewModel.Ingredient selectedItem in selectedItems)
				_ingredientView?.Remove(selectedItem);
		}
	}
}