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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;

namespace RecipeDatabase
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        PageChanges pageChanges = new PageChanges();

		public MainWindow()
		{
			InitializeComponent();
		}

		private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			try
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


				Recipe newRecipe = new Recipe();
				newRecipe.Name = NameTextBox.Text;

				

				string[] arr =
				System.Text.RegularExpressions.Regex.Replace(IngredientsTextBox.Text, "\n", "").Split('\n', '\r');
				newRecipe.Description.Ingredients = new List<string>(arr);
				newRecipe.Description.Actions = ActionsTextBox.Text;

				_dataBase.AddNewItem(newRecipe);
				_dataBase.SaveItem(_dataBase.GetRecipes().IndexOf(newRecipe));


				MessageBox.Show
				(
					$"Рецепт \"{newRecipe.Name}\" добавлен",
					"Добавление рецепта",
					MessageBoxButton.OK,
					MessageBoxImage.Information
				);
				NameTextBox.Text = IngredientsTextBox.Text = ActionsTextBox.Text = "";
				UpdateListView();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка добавления рецепта!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		private void Window_Initialized(object sender, EventArgs e)
		{
			UpdateListView();
		}




		private void UpdateData()
		{
			_dataBase.UpdateData();
		}
		private void UpdateListView()
		{
			for (int i = 0; i != _dataBase.GetRecipes().Count; ++i)
			{
				ListViewItem item = new ListViewItem();
				var data = new ListViewContent
				(
					_dataBase.GetRecipes()[i].Name,
					_dataBase.GetRecipes()[i].Description.ShowIngredientsToString(),
					_dataBase.GetRecipes()[i].Description.Actions
				);
				item.Content = data;
				item.MouseDoubleClick += new MouseButtonEventHandler(RecipesView_MouseDoubleClick);


				if (!HaveListViewItem(ref this.Recipes, ref item))
					Recipes.Items.Add(item);
			}
		}


		public bool HaveListViewItem(ref ListView list, ref ListViewItem item)
		{
			for (int i = 0; i != list.Items.Count; ++i)
			{
				if (((ListViewItem)(list.Items.GetItemAt(i))).Content.Equals(item.Content))
					return true;
			}

			return false;
		}
		private bool HaveListBoxItem(ref ListBox list, string name)
		{
			for (int i=0;i!=list.Items.Count;++i)
			{
				if ((string)list.Items.GetItemAt(i)==name)
					return true;
			}

			return false;
		}



		private void UpdateButton_Click(object sender, RoutedEventArgs e)
		{
			this._dataBase.UpLoadData();
			UpdateListView();
		}



		private void UpdateDeleteSlectionListBox()
		{
			List<Recipe> dataBaseList = _dataBase.GetRecipes();

			for (int i=0;i!= DeleteSlectionListBox.Items.Count;++i)
			{
				string str = DeleteSlectionListBox.Items[i] as string;
				if(!(str is null))
				{
					if (dataBaseList.Count(x => x.Name == str) == 0)
					{
						DeleteSlectionListBox.Items.RemoveAt(i--);
						continue;
					}
				}
			}
			for (int i = 0; i != dataBaseList.Count; ++i)
				if (!HaveListBoxItem(ref this.DeleteSlectionListBox, dataBaseList[i].Name))
					DeleteSlectionListBox.Items.Add(dataBaseList[i].Name);
		}
        private void UpdateEditSlectionListBox()
        {
            List<Recipe> dataBaseList = _dataBase.GetRecipes();

            for (int i = 0; i != EditSlectionListBox.Items.Count; ++i)
            {
                string str = EditSlectionListBox.Items[i] as string;
                if (!(str is null))
                {
                    if (dataBaseList.Count(x => x.Name == str) == 0)
                    {
                        EditSlectionListBox.Items.RemoveAt(i--);
                        continue;
                    }
                }
            }
            for (int i = 0; i != dataBaseList.Count; ++i)
                if (!HaveListBoxItem(ref this.EditSlectionListBox, dataBaseList[i].Name))
                    EditSlectionListBox.Items.Add(dataBaseList[i].Name);
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			TabControl reference = (TabControl)(sender);



			if(pageChanges.IsChanged(reference.SelectedIndex))
			{
				switch(reference.SelectedIndex)
				{
					case 3:
						{
							UpdateDeleteSlectionListBox();
                            if (DeleteSlectionListBox.Items.Count != 0)
                                DeleteSlectionListBox.SelectedIndex = 0;
                        }
					break;
					case 2:
						{
							UpdateEditSlectionListBox();
							if (EditSlectionListBox.Items.Count != 0)
								EditSlectionListBox.SelectedIndex = 0;
                        }
					break;
				}
			}
		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			if (DeleteSlectionListBox.SelectedIndex != -1)
			{
				this._dataBase.GetRecipes().RemoveAll
					(x => x.Name == (string)DeleteSlectionListBox.Items[DeleteSlectionListBox.SelectedIndex]);

				_dataBase.DeleteExtraFiles();

				Recipes.Items.Clear();
				UpdateListView();
			}

			UpdateDeleteSlectionListBox();
			UpdateEditSlectionListBox();
		}
		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			if (EditSlectionListBox.SelectedIndex != -1)
			{
				Recipe recipe;

                try
				{
					recipe =
					_dataBase.GetRecipes().Find
					(
						x => x.Name ==
						(string)EditSlectionListBox.Items.GetItemAt(EditSlectionListBox.SelectedIndex)
					);
                }
				catch(Exception)
				{
					return;
				}


				RecipeEditWindow recipeWindow = new RecipeEditWindow(recipe);
				recipeWindow.ShowDialog();


				if(recipeWindow.changesSaved)
				{

					_dataBase.GetRecipes()
						.Find
						(x => x.Name == recipe.Name).
						Description = recipeWindow.recipe.Description;
					_dataBase.SaveItem(_dataBase.GetRecipes().IndexOf(_dataBase.GetRecipes()
						.Find
						(x => x.Name == recipe.Name)));

					DeleteSlectionListBox.Items.Clear();
					EditSlectionListBox.Items.Clear();

					{
						int index = -1;
						for(int i=0;i!=Recipes.Items.Count;++i)
						{
							ListViewContent listViewContent = (ListViewContent)(((ListViewItem)(Recipes.Items.GetItemAt(i))).Content);

                            if (!(listViewContent is null))
							{
								if (listViewContent.Name == recipe.Name)
								{
									index = i;
                                    break;
								}
							}
                        }

						if(index!=-1)
                            Recipes.Items.RemoveAt(index);
                    }

					UpdateDeleteSlectionListBox();
					UpdateEditSlectionListBox();
					UpdateListView();

					MessageBox.Show
					(
						$"Рецепт \"{recipe.Name}\" изменён",
						"Редактирование",
						MessageBoxButton.OK,
						MessageBoxImage.Information
					);
                }
			}

        }

		private void RecipesView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
            ListViewContent content =(ListViewContent)(((ListViewItem)(Recipes.Items.GetItemAt(Recipes.SelectedIndex))).Content);


			DetailedDescriptionWindow detailedDescriptionWindow =
			new DetailedDescriptionWindow(content.Name, content.Ingredients, content.Actions);
			detailedDescriptionWindow.ShowDialog();
        }




        private void AddMenuItem_Click(object sender, RoutedEventArgs e)
        {
			TabControlPages.SelectedIndex = 3;
        }
		private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
			TabControlPages.SelectedIndex = 4;
        }
		private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
			TabControlPages.SelectedIndex = 2;
        }
    }
}
