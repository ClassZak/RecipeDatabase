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

		public Ingredient()
		{
			IngredientsViewModels = new();
			Ingredients = CollectionViewSource.GetDefaultView(IngredientsViewModels);
		}

		public void Clear()
		{
			IngredientsViewModels.Clear();
			OnPropertyChanged(nameof(IngredientsViewModels));
		}
		public void Add(Model.Viewed.Ingredient element)
		{
			IngredientsViewModels.Add(element);
			OnPropertyChanged(nameof(IngredientsViewModels));
		}
		public void Remove(Model.Viewed.Ingredient ingredient)
		{
			IngredientsViewModels.Remove(ingredient);
			OnPropertyChanged(nameof(IngredientsViewModels));
		}
		public IEnumerable<Model.Viewed.Ingredient> GetIngredients()
		{
			return IngredientsViewModels;
		}
	}
}
