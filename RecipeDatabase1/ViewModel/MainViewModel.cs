using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeDatabase.ViewModel
{
	public class MainViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;
		public void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}


		public ObservableCollection<Model.Viewed.Recipe> Recipes { get; set; } = new();


		public MainViewModel()
		{
			List<Model.Viewed.Recipe> recipes = new()
			{
				new Model.Viewed.Recipe("dfgfg", "hhdfhfgjjj"),
				new Model.Viewed.Recipe("111dfgfg", "hhdfhfgjj345j")
			};
			recipes[0].Ingredients.Add(new("fgh", null, null, 23));

			foreach(var el in  recipes)
				Recipes.Add(el);
		}
	}
}
