using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Model
{
	public class Recipe : AModel
	{
		public string? Name { get; set; }
		public string? Actions { get; set; }
		public int IdDescription { get; set; }
		public Recipe()
		{
		}
		public Recipe(string name, string actions, int idDescription)
		{
			Name = name;
			Actions = actions;
			IdDescription = idDescription;
		}
	}
}
