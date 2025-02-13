using RecipeDatabase.Model;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecipeDatabase.ViewModel
{
	public class Ingredient :AViewModel
	{


		private string _name = "";
		public string Name { get { return _name; } set {_name = value; } }
		private string? _measureUnit="шт";
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
		public Ingredient(Ingredient ingredient):base(ingredient.GetId())
		{
			_name = ingredient.Name!;
			_amount = ingredient._amount;
			_measureUnit = ingredient._measureUnit;
		}
		public Ingredient()
		{
		}
		public Ingredient(string name, int? amount, string measureUnit, int id=0):base(id)
		{
			_name = name;
			_amount = amount;
			_measureUnit = measureUnit;
		}


		public Ingredient(Model.Ingredient ingredient) : base(ingredient.Id)
		{
			Name = ingredient.Name!;
		}


		public int? GetAmount()
		{
			return _amount;
		}
	}
}
