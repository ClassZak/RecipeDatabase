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

namespace RecipeDatabase1.View
{
	internal class Recipe : ICollectionView, INotifyPropertyChanged
	{
		private ObservableCollection<ViewModel.Recipe> RecipeViewModels { get; set; }
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
		public Recipe()
		{
			RecipeViewModels = new ();
			RecipesModelView = CollectionViewSource.GetDefaultView(RecipeViewModels);
		}
		public void Update(IEnumerable<ViewModel.Recipe> elements)
		{
			RecipeViewModels.Clear();
			foreach (var element in elements)
				RecipeViewModels.Add(element);
			OnPropertyChanged(nameof(RecipeViewModels));
		}
		public void Remove(ViewModel.Recipe ingredient)
		{
			RecipeViewModels.Remove(ingredient);
			OnPropertyChanged(nameof(RecipeViewModels));
		}
		public void Remove(int recipeIndex)
		{
			RecipeViewModels.RemoveAt(recipeIndex);
			OnPropertyChanged(nameof(RecipeViewModels));
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
