using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RecipeDatabase
{
    /// <summary>
    /// Логика взаимодействия для DetailedDescriptionWindow.xaml
    /// </summary>
    public partial class DetailedDescriptionWindow : Window
    {
        public DetailedDescriptionWindow()
        {
            InitializeComponent();
        }

        public DetailedDescriptionWindow(string name,string ingredients, string actions) : this()
        {
            this.Title=name;
            this.RecipeName.Text = name;
            this.Ingredients.Text=ingredients;
            this.Actions.Text = actions;
        }
    }
}
