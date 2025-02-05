using System;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1
{
	public class RecipeDBModel : DBModel
	{
		private RecipeModel _recipe=new();
		public RecipeDBModel(RecipeModel recipe) 
		{
			_recipe = recipe;
		}
		public string? Name { get { return _recipe.Name; } set { _recipe.Name = value; } } 
		public string? Actions { get { return _recipe.Actions; } set { _recipe.Actions = value; } }
		public int IdDescription { get { return _recipe.IdDescription; } set { _recipe.IdDescription = value; } }
	}
	public class IngredientDBModel : DBModel
	{
		private IngredientModel _ingredient=new();
		public IngredientDBModel(IngredientModel ingredient)
		{
			_ingredient = ingredient;
		}
		public string? Name{ get { return _ingredient.Name; } set{ _ingredient.Name = value; } }
	}
	public class DescriptionDBModel : DBModel
	{
		private DescriptionModel _description=new();
		public DescriptionDBModel(DescriptionModel description)
		{
			_description = description;
		}
		public int IdRecipe { get { return _description.IdRecipe; } set { _description.IdRecipe = value; } }
		public int IdIngredient { get { return _description.IdIngredient; } set { _description.IdIngredient = value; } }
	}


	public class ApplicationContext : DbContext
	{
		protected readonly string _connectionString;
		public ApplicationContext(string connectionString)
		{
			_connectionString = connectionString;
			Database.EnsureCreated();
		}
		
		public DbSet<RecipeDBModel> RecipeModel { get; set; }
		public DbSet<IngredientDBModel> IngredientModel { get; set; }
		public DbSet<DescriptionDBModel> DescriptionModel { get; set; }


		public void AddNewIngredient(IngredientModel ingredient, bool addUnique=false)
		{
			bool allowAdding=false;
			{
				if (!addUnique)
					allowAdding = true;
				else
				{
					var ingredients = addUnique ? IngredientModel.ToList() : [];
					if (ingredients.Find(x=> x.Name==ingredient.Name) is not null)
						allowAdding=true;
				}
			}
			if (allowAdding)
				IngredientModel.Add(new (ingredient));
		}
		public void AddNewRecipe(RecipeModel recipe, List<int> ingredients)
		{
			if (ingredients.Count < 0)
				throw new Exception("Wrong recipe. No ingredients");

			RecipeModel.Add(new(recipe));

			foreach(var ingredient in ingredients)
				DescriptionModel.Add(new(new DescriptionModel(RecipeModel.ToList().Last().Id, ingredient)));
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(_connectionString);
		}
	}
	class Program
	{
		static void Main()
		{
			string connectionString = "Server=localhost;Database=hellodb;Trusted_Connection=True;Encrypt=false;";


			// Добавление данных
			using (ApplicationContext applicationContext = new ApplicationContext(connectionString))
			{
				int written= applicationContext.SaveChanges();
				if (written <= 0)
					throw new Exception("Не удалось сохранить данные");
			}
			// Получение данных
			using (ApplicationContext applicationContext = new ApplicationContext(connectionString))
			{
				// Получаем объекты из бд и выводим на консоль
				Console.WriteLine("Список пользователей:");
			}
		}
	}

}
