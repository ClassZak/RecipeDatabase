using Microsoft.EntityFrameworkCore;
using RecipeDatabase.Model;
using RecipeDatabase.Model.DataBase;
using RecipeDatabase.Model.Viewed;
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace RecipeDatabase.ViewModel
{
	internal class Recipe : INotifyPropertyChanged
	{
		string connectionString = "Server=localhost;Database=RecipeDataBase;Trusted_Connection=True;Encrypt=false;";
		public ObservableCollection<Model.Viewed.Recipe> Recipes { get; set; } = new();
		public event PropertyChangedEventHandler? PropertyChanged;
		public void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public Recipe()
		{
			Update.Execute(this);
		}
		public void Remove(int recipeIndex)
		{
			Recipes.RemoveAt(recipeIndex);
			OnPropertyChanged(nameof(Recipes));
		}
		private RelayCommand? _update;
		public RelayCommand Update
		{
			get
			{
				return _update ?? (_update = new(obj =>
				{
					Task.Run(() =>
					{
						using (RecipeDataBaseContext recipeDataBaseContext = new(connectionString))
						{
							List<Model.DataBase.Recipe> recipes = recipeDataBaseContext.Recipe.ToList();
							List<Model.Viewed.Recipe> viewedRecipes = new();
							foreach (var recipe in recipes)
							{
								List<Model.DataBase.RecipeIngredient> recipeIngredients = recipeDataBaseContext.RecipeIngredient.ToList().Where(x => x.IdRecipe == recipe.Id).ToList();
								List<Model.DataBase.Ingredient> ingredients = recipeDataBaseContext.Ingredient.ToList().Where(x => recipeIngredients.Any(y => x.Id == y.IdIngredient)).ToList();

								List<Model.Viewed.Ingredient> viewedIngredients = new();
								for (int i = 0; i < ingredients.Count; ++i)
								{
									MeasureUnit? measureUnit = null;
									measureUnit = recipeDataBaseContext.MeasureUnit.ToList().Where(x => x.Id == ingredients[i].Id).FirstOrDefault();

									Model.Viewed.Ingredient ingredient = new(ingredients[i]);
									ingredient.Amount = recipeIngredients[i].Amount;
									ingredient.MeasureUnit = recipeDataBaseContext.MeasureUnit.ToList().Where(x => x.Id == ingredients[i].Id).FirstOrDefault()?.Name;

									viewedIngredients.Add(ingredient);
								}

								Model.Viewed.Recipe viewedRecipe = new(recipe);

								foreach (var el in viewedIngredients)
									el.IdRecipe = recipe.Id;
								for (int i = 0; i < viewedIngredients.Count; ++i)
									viewedRecipe.Ingredients.Add(viewedIngredients[i]);

								viewedRecipes.Add(viewedRecipe);
							}
							Application.Current.Dispatcher.Invoke(new Action(() =>
							{
								Recipes.Clear();
								foreach (var el in viewedRecipes)
									Recipes.Add(el);

								OnPropertyChanged(nameof(Recipes));
							}));
						}
					});
				}));
			}
		}
		private RelayCommand? _delete;
		public RelayCommand Delete
		{
			get
			{
				return _delete ?? (_delete = new(obj =>
				{
					DataGrid? dataGrid = obj as DataGrid;
					List<Model.Viewed.Recipe> selectedRecipes = new();
					if (dataGrid is not null)
						if (dataGrid.SelectedItems is not null)
						{
							foreach (var el in dataGrid?.SelectedItems!)
								if (el is Model.Viewed.Recipe)
									selectedRecipes.Add((el as Model.Viewed.Recipe)!);
							Task.Run(() =>
							{
								using (RecipeDataBaseContext recipeDataBaseContext = new(connectionString))
								{
									foreach (var el in selectedRecipes)
									{
										recipeDataBaseContext.Database.ExecuteSqlRaw("DELETE FROM RecipeIngredient WHERE IdRecipe={0}", el.Id);
										recipeDataBaseContext.Database.ExecuteSqlRaw("DELETE FROM Recipe WHERE Id={0}", el.Id);
									}
									recipeDataBaseContext.SaveChangesAsync();
								}
							});
							foreach (var el in selectedRecipes)
								Recipes.Remove(el);
						}
				}));
			}
		}
		private RelayCommand? _deleteIngredient;
		public RelayCommand DeleteIngredient
		{
			get
			{
				return _deleteIngredient ??= new RelayCommand(obj =>
				{
					if (obj is IList selectedItems)
					{
						var ingredientsToDelete = selectedItems.Cast<Model.Viewed.Ingredient>().ToList();

						// Удаление из базы данных
						Task.Run(() =>
						{
							using (var context = new RecipeDataBaseContext(connectionString))
							{
								foreach (var ingredient in ingredientsToDelete)
								{
									var relation = context.RecipeIngredient
										.FirstOrDefault(ri => ri.IdRecipe == ingredient.IdRecipe
														   && ri.IdIngredient == ingredient.Id);
									if (relation != null)
									{
										context.RecipeIngredient.Remove(relation);
									}
								}
								context.SaveChanges();
							}
						});

						// Удаление из ViewModel
						foreach (var ingredient in ingredientsToDelete)
						{
							var recipe = Recipes.FirstOrDefault(r => r.Id == ingredient.IdRecipe);
							recipe?.Ingredients.Remove(ingredient);
						}
						OnPropertyChanged(nameof(Recipes));
					}
				});
			}
		}
	}
}
