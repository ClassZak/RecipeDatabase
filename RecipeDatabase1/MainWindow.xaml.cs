using Accessibility;
using Microsoft.EntityFrameworkCore;
using RecipeDatabase1.Model;
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
			Dispatcher.Invoke(() =>
			{
				UpdateProgressBar.IsIndeterminate = true;
				Grid.SetRowSpan(RecipesDataGrid, 1);
				UpdateProgressBarGrid.Visibility=Visibility.Visible;
			});
			try
			{

				using (RecipeDataBaseContext RecipeDataBaseContext = new(connectionString))
				{
					var recipes = RecipeDataBaseContext.Recipe.ToList();
					List<ViewModel.Recipe> viewModelRecipes = new List<ViewModel.Recipe>();
					Dispatcher.Invoke(() => 
					{
						UpdateProgressBar.IsIndeterminate = false;
						UpdateProgressBar.Maximum=recipes.Count;
					});
					for (int recipeIndex=0; recipeIndex<recipes.Count;++recipeIndex)
					{
						Thread.Sleep(1);
						Dispatcher.Invoke(() =>
						{
							UpdateProgressBar.Value=recipeIndex;
						});

						List<int> ingredientIds = new List<int>();
						object recipeIngredients = RecipeDataBaseContext.RecipeIngredient.ToList();
						recipeIngredients=(recipeIngredients as IEnumerable<Model.RecipeIngredient>)!.Where(x => x.IdRecipe == recipes[recipeIndex].Id);
						StringBuilder stringBuilder = new StringBuilder();

						var ingredients = RecipeDataBaseContext.Ingredient.ToList().Where(x => (recipeIngredients as IEnumerable<Model.RecipeIngredient>)!.Any(y => y.IdIngredient == x.Id));
						List<int?> ingredientAmounts = new();
						foreach(var ingredient in ingredients)
						{
							var ingredientAmount = (recipeIngredients as IEnumerable<Model.RecipeIngredient>)!.Where(x => x.IdIngredient==ingredient.Id);
							foreach (var recipeIngredient in ingredientAmount)
								ingredientAmounts.Add(recipeIngredient.Amount);
						}


						for(int i=0;i<ingredients.Count();++i)
						{
							var ingredientMeasure = RecipeDataBaseContext.MeasureUnit.ToList().Where(x => x.Id == ingredients.ElementAt(i).IdMeasureUnit);
							string ingredientName = "";
							if(ingredientMeasure is not null)
								ingredientName= !ingredientMeasure!.Any() ? "" : ingredientMeasure.First().Name!;
							stringBuilder.AppendLine($"{ingredients.ElementAt(i).Name} {ingredientAmounts[i]} {ingredientName}");
						}
						if (stringBuilder.Length > 2)
							stringBuilder.Remove(stringBuilder.Length - 2, 2);

						viewModelRecipes.Add(new ViewModel.Recipe(recipes[recipeIndex].Name!, recipes[recipeIndex].Actions!, stringBuilder.ToString(), recipes[recipeIndex].Id));
					}

					Dispatcher.Invoke
					(()=>
					{
						_recipeView?.Update(viewModelRecipes);
					});
				}
			}
			catch (Exception)
			{

			}
			finally
			{
				Dispatcher.Invoke
				(() =>
				{
					Grid.SetRowSpan(RecipesDataGrid, 2);
					UpdateProgressBarGrid.Visibility = Visibility.Collapsed;
				});
			}
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


				using (RecipeDataBaseContext RecipeDataBaseContext = new RecipeDataBaseContext(connectionString))
				{
					Model.Recipe modelRecipe = new(RecipeNameTextBox.Text, ActionsTextBox.Text);


					List<Model.RecipeIngredient> modelRecipeIngredients=new();
					foreach(var ingredient in _ingredientView.GetIngredients().ToList())
					{
						if (string.IsNullOrWhiteSpace(ingredient.Name) || ingredient.Name == string.Empty)
							continue;

						Model.Ingredient modelIngredient;

						if (RecipeDataBaseContext.MeasureUnit.ToList().Count == 0 && ingredient.MeasureUnit is not null)
							RecipeDataBaseContext.MeasureUnit.Add(new Model.MeasureUnit(ingredient.MeasureUnit));
						if(ingredient.MeasureUnit is not null && ingredient.MeasureUnit!=string.Empty && RecipeDataBaseContext.MeasureUnit.ToList().Count != 0)
						{
							var measureUnits = RecipeDataBaseContext.MeasureUnit.ToList().Where(x => x.Name!.ToLower() == ingredient?.MeasureUnit.ToLower());
							if (!measureUnits.Any())
								RecipeDataBaseContext.MeasureUnit.Add(new Model.MeasureUnit(ingredient.MeasureUnit));

							Model.MeasureUnit measureUnitForIngredient= RecipeDataBaseContext.MeasureUnit.ToList().Where(x => x.Name == ingredient.MeasureUnit).Last();
							modelIngredient = new(ingredient.Name.ToLower(), measureUnitForIngredient.Id);
						}
						else
							modelIngredient = new(ingredient.Name.ToLower());

						if(!RecipeDataBaseContext.Ingredient.ToList().Any(x=>x?.Name?.ToLower()==modelIngredient?.Name?.ToLower()))
							RecipeDataBaseContext.Add(modelIngredient);
						else
							RecipeDataBaseContext.Ingredient.ToList().Where(x => x?.Name?.ToLower() == modelIngredient?.Name?.ToLower()).First().Name = ingredient.Name.ToLower();


						RecipeDataBaseContext.SaveChanges();
						modelRecipeIngredients.Add(new Model.RecipeIngredient(modelRecipe.Id, modelIngredient.Id, ingredient.GetAmount()));
					}
					RecipeDataBaseContext.Recipe.Add(modelRecipe);
					RecipeDataBaseContext.SaveChanges();
					foreach (var el in modelRecipeIngredients)
						el.IdRecipe= RecipeDataBaseContext.Recipe.ToList().Where(x=>x.Name== modelRecipe.Name && x.Actions== modelRecipe.Actions).Last().Id;
					foreach (Model.RecipeIngredient el in modelRecipeIngredients)
						RecipeDataBaseContext.RecipeIngredient.Add(el);
					RecipeDataBaseContext.SaveChanges();

					MessageBox.Show($"Рецепт \"{modelRecipe.Name}\" успешно добавлен", "Рецепт успешно добавлен", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка добавления рецепта!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void DeleteRecipes(List<ViewModel.Recipe> recipes)
		{
			List<int> recipeIds= new List<int>();
			foreach (var recipe in recipes)
				recipeIds.Add(recipe.GetId());

			DeleteRecipes(recipeIds);
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



		private void DeleteRecipes(List<int> recipeIds)
		{
			using (RecipeDataBaseContext RecipeDataBaseContext=new(connectionString))
			{
				for (int i = 0; i < recipeIds.Count; i++)
					RecipeDataBaseContext.Database.ExecuteSqlRaw("DELETE FROM RecipeIngredient WHERE IdRecipe = {0}",recipeIds[i]);

				for (int i = 0; i < recipeIds.Count; i++)
					RecipeDataBaseContext.Database.ExecuteSqlRaw("DELETE FROM Recipe WHERE Id = {0}", recipeIds[i]);

				RecipeDataBaseContext.SaveChanges();
			}
		}
		private void DeleteSelectedRecipes(List<ViewModel.Recipe> recipes)
		{
			Task.Run(() =>
			{
				DeleteRecipes(recipes);
				Dispatcher.Invoke(() =>
				{
					foreach(var el in recipes)
						_recipeView?.Remove(el);
				});
			});
		}
		private void RecipeDataGridDeleteMenu_Click(object sender, RoutedEventArgs e)
		{
			List<ViewModel.Recipe> selectedItems = new();
			foreach (var el in RecipesDataGrid.SelectedItems)
				if (el is ViewModel.Recipe)
					selectedItems.Add((el as ViewModel.Recipe)!);

			DeleteSelectedRecipes(selectedItems);
		}
	}
}