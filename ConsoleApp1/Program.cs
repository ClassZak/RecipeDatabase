
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1
{
	
	class Program
	{
		static void Main()
		{
			string connectionString = "Server=localhost;Database=hellodb;Trusted_Connection=True;Encrypt=false;";


			// Добавление данных
			using (ApplicationContext applicationContext = new ApplicationContext(connectionString))
			{
				int written= applicationContext.SaveChanges();
				//if (written <= 0)
				//	throw new Exception("Не удалось сохранить данные");
			}
			// Получение данных
			using (ApplicationContext applicationContext = new ApplicationContext(connectionString))
			{
				// Получаем объекты из бд и выводим на консоль
				Console.WriteLine("Рецепты:");
				var recipes = applicationContext.Recipe.ToList();
				var ingredients = applicationContext.Ingredient.ToList();
				var recipeIngredients = applicationContext.RecipeIngredient.ToList();

				foreach (var recipe in recipes)
					Console.WriteLine(recipe);
				foreach (var ingredient in ingredients)
					Console.WriteLine(ingredient);
				foreach (var recipeIngredient in recipeIngredients)
					Console.WriteLine(recipeIngredient);
			}
		}
	}

}
