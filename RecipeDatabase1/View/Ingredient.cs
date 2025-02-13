using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeDatabase;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Data;
using System.Xml.Linq;

namespace RecipeDatabase.View
{
	internal class Ingredient : ICollectionView, INotifyPropertyChanged
	{
		private ObservableCollection<ViewModel.Ingredient> IngredientsViewModels { get; set; }
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
		public void Add(Model.Ingredient element)
		{
			IngredientsViewModels.Add(new ViewModel.Ingredient(element));
			OnPropertyChanged(nameof(IngredientsViewModels));
		}
		public void Add(ViewModel.Ingredient element)
		{
			IngredientsViewModels.Add(element);
			OnPropertyChanged(nameof(IngredientsViewModels));
		}
		public void Update(IEnumerable<Model.Ingredient> elements)
		{
			IngredientsViewModels.Clear();
			foreach (var element in elements)
				IngredientsViewModels.Add(new ViewModel.Ingredient(element));
			OnPropertyChanged(nameof(IngredientsViewModels));
		}
		public void Remove(ViewModel.Ingredient ingredient)
		{
			IngredientsViewModels.Remove(ingredient);
			OnPropertyChanged(nameof(IngredientsViewModels));
		}
		public IEnumerable<ViewModel.Ingredient> GetIngredients()
		{
			return IngredientsViewModels;
		}



		public event EventHandler CurrentChanged;
		public event CurrentChangingEventHandler CurrentChanging;
		public event NotifyCollectionChangedEventHandler? CollectionChanged;
		public bool CanFilter => throw new NotImplementedException();

		public bool CanGroup => throw new NotImplementedException();

		public bool CanSort => throw new NotImplementedException();

		public CultureInfo Culture { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public object CurrentItem => throw new NotImplementedException();

		public int CurrentPosition => throw new NotImplementedException();

		public Predicate<object> Filter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public ObservableCollection<GroupDescription> GroupDescriptions => throw new NotImplementedException();

		public ReadOnlyObservableCollection<object> Groups => throw new NotImplementedException();

		public bool IsCurrentAfterLast => throw new NotImplementedException();

		public bool IsCurrentBeforeFirst => throw new NotImplementedException();

		public bool IsEmpty => throw new NotImplementedException();

		public SortDescriptionCollection SortDescriptions => throw new NotImplementedException();

		public IEnumerable SourceCollection => throw new NotImplementedException();


		public bool Contains(object item)
		{
			throw new NotImplementedException();
		}

		public IDisposable DeferRefresh()
		{
			throw new NotImplementedException();
		}

		public bool MoveCurrentTo(object item)
		{
			throw new NotImplementedException();
		}

		public bool MoveCurrentToFirst()
		{
			throw new NotImplementedException();
		}

		public bool MoveCurrentToLast()
		{
			throw new NotImplementedException();
		}

		public bool MoveCurrentToNext()
		{
			throw new NotImplementedException();
		}

		public bool MoveCurrentToPosition(int position)
		{
			throw new NotImplementedException();
		}

		public bool MoveCurrentToPrevious()
		{
			throw new NotImplementedException();
		}

		public void Refresh()
		{
			throw new NotImplementedException();
		}

		public IEnumerator GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
