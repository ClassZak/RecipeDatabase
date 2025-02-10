using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeDatabase1.Model
{
	internal class MeasureUnit : AModel
	{
		public string? Name { get; set; }
		public MeasureUnit()
		{
		}
		public MeasureUnit(string name)
		{
			Name=name;
		}
	}
}
