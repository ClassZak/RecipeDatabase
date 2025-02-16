using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Data;
using System.Xml.Linq;
using RecipeDatabase.Services.Commands;
using System.Windows.Controls;

namespace RecipeDatabase.ViewModel
{
	public class Ingredient : INotifyPropertyChanged
	{
		private ObservableCollection<Model.Viewed.Ingredient> IngredientsViewModels { get; set; }
		public event PropertyChangedEventHandler? PropertyChanged;

		private ICollectionView _ingredients;
		public ICollectionView Ingredients
		{
			get { return _ingredients; }
			set { _ingredients = value; }
		}
		public void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		private RelayCommand? _deleteCommand;
		public RelayCommand Delete
		{
			get
			{
				return _deleteCommand ?? (_deleteCommand = new(obj =>
				{
					DataGrid? dataGrid = obj as DataGrid;
					if(dataGrid is not null)
					{
						List<Model.Viewed.Ingredient> selectedIngredients = new();
						foreach (var el in dataGrid.SelectedItems)
							if (el is Model.Viewed.Ingredient)
								selectedIngredients.Add((el as Model.Viewed.Ingredient)!);
						foreach (var el in selectedIngredients)
							IngredientsViewModels.Remove(el);
					}
				}));
			}
		}

		public Ingredient()
		{
			IngredientsViewModels = new();
			Ingredients = CollectionViewSource.GetDefaultView(IngredientsViewModels);
		}
	}
}
