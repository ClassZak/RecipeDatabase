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
    /// Логика взаимодействия для RecipeEditWindow.xaml
    /// </summary>
    public partial class RecipeEditWindow : Window
    {
        public bool changesSaved = false;
        public Recipe recipe = new Recipe();

        private RecipeEditWindow()
        {
            InitializeComponent();
        }
        public RecipeEditWindow(Recipe recipe) : this()
        {
            NameTextBox.Text=recipe.Name;
            IngredientsTextBox.Text=recipe.Description.ShowIngredientsToString();
            ActionsTextBox.Text = recipe.Description.Actions;
        }
        public RecipeEditWindow(ListViewContent recipe) : this()
        {
            NameTextBox.Text=recipe.Name;
            IngredientsTextBox.Text=recipe.Ingredients;
            ActionsTextBox.Text = recipe.Actions;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text is null || NameTextBox.Text == string.Empty)
            {
                MessageBox.Show
                    ("Введите название рецепта", "Пустое назва.ние!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (IngredientsTextBox.Text is null || IngredientsTextBox.Text == string.Empty)
            {
                MessageBox.Show
                    ("Введите ингредиенты", "Пустое поле!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (ActionsTextBox.Text is null || ActionsTextBox.Text == string.Empty)
            {
                MessageBox.Show
                    ("Введите описание приготовления", "Пустое поле!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            recipe.Name = NameTextBox.Text;

            string[] arr =
            IngredientsTextBox.Text.Split('\n', '\r');

            recipe.Description.Ingredients = new List<string>(arr);

            recipe.Description.Actions = ActionsTextBox.Text;
            changesSaved = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
