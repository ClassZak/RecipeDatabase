using RecipeDatabase1.Model;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecipeDatabase1.ViewModel
{
	public class Ingredient
	{


		private string _name = "";
		public string Name { get { return _name; } set {_name = value; } }
		private string? _measureUnit;
		public string MeasureUnit{ get { return _measureUnit is null ? "" : _measureUnit; } set {_measureUnit = value; } }
		private int? _amount;
		public string Amount
		{ 
			get
			{ 
				return _amount is null ? "" : $"{_amount}"; 
			}
			set 
			{
				{
					int parsedValue;
					bool parsingSuccess=int.TryParse(value, out parsedValue);
					if(parsingSuccess)
						_amount = parsedValue;
					else _amount = null;
				} 
			}
		}
		public Ingredient(Ingredient ingredient)
		{
			_name = ingredient.Name!;
		}
		public Ingredient()
		{
		}
		public Ingredient(string name, int? amount, string measureUnit)
		{
			_name = name;
			_amount = amount;
			_measureUnit = measureUnit;
		}


		public Ingredient(Model.Ingredient ingredient)
		{
			Name = ingredient.Name!;
		}
	}
}
