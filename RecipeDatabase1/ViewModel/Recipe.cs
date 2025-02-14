using RecipeDatabase.Model;
using RecipeDatabase.Model.DataBase;
using RecipeDatabase.Services.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;

namespace RecipeDatabase.ViewModel
{
	internal class Recipe : INotifyPropertyChanged
	{
		string connectionString = "Server=localhost;Database=RecipeDataBase;Trusted_Connection=True;Encrypt=false;";
		private ObservableCollection<Model.Viewed.Recipe> RecipeViewModels { get; set; }
		public event PropertyChangedEventHandler? PropertyChanged;

		private ICollectionView? _recipes;
		public ICollectionView? RecipesModelView
		{
			get { return _recipes; }
			set { _recipes = value; }
		}
		public void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public RelayCommand _updateCommand;
		public RelayCommand UpdateCommand
		{
			get
			{
				return _updateCommand ??
				(_updateCommand = new RelayCommand(obj => 
				{
					using (RecipeDataBaseContext recipeDataBaseContext = new(connectionString))
					{
						var recipes = recipeDataBaseContext.Recipe.ToList();
						for (int recipeIndex = 0; recipeIndex < recipes.Count; ++recipeIndex)
						{
							Thread.Sleep(1);

							List<int> ingredientIds = new List<int>();
							object recipeIngredients = recipeDataBaseContext.RecipeIngredient.ToList();
							recipeIngredients = (recipeIngredients as IEnumerable<RecipeIngredient>)!.Where(x => x.IdRecipe == recipes[recipeIndex].Id);
							StringBuilder stringBuilder = new StringBuilder();

							var ingredients = recipeDataBaseContext.Ingredient.ToList().Where(x => (recipeIngredients as IEnumerable<RecipeIngredient>)!.Any(y => y.IdIngredient == x.Id));
							ViewModel.Ingredient ingredientsViewModel=new ViewModel.Ingredient();
							foreach (var ingredient in ingredients)
								ingredientsViewModel.Add(new Model.Viewed.Ingredient(ingredient));

							Model.Viewed.Recipe recipe = new Model.Viewed.Recipe(recipes[recipeIndex]);
							recipe.Ingredients= ingredientsViewModel;

							RecipeViewModels.Add(recipe);
						}
					}
				}));
			}
		}
		public Recipe()
		{
			RecipeViewModels = new();
			RecipesModelView = CollectionViewSource.GetDefaultView(RecipeViewModels);
			UpdateCommand.Execute(this);
		}
		public void Remove(int recipeIndex)
		{
			RecipeViewModels.RemoveAt(recipeIndex);
			OnPropertyChanged(nameof(RecipeViewModels));
		}
	}
}
