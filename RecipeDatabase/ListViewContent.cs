using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeDatabase
{
    public class ListViewContent
    {
        public string Name { get; set; }
        public string Ingredients { get; set; }
        public string Actions { get; set; }

        public ListViewContent(string name, string ingredients, string actions)
        {
            Name = name;
            Ingredients = ingredients;
            Actions = actions;
        }
    }
}
