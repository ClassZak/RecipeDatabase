using RecipeDatabase1.Model;

namespace RecipeDatabase1.ViewModel
{
	public class Ingredient
	{
		public bool IsEditable=false;


		private string _name = "";
		public string Name { get { return _name; } set { if(IsEditable) _name = value; } }
		private string? _measureUnit;
		public string MeasureUnit{ get { return _measureUnit is null ? "" : _measureUnit; } set { if (IsEditable) _measureUnit = value; } }
		private int? _amount;
		public string Amount{ get { return _amount is null ? "" : $"{_amount}"; } set { if (IsEditable) _amount = int.Parse(value); } }
		public Ingredient(Ingredient ingredient)
		{
			Name = ingredient.Name!;
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
