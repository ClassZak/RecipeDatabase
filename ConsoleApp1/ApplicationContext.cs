
using Microsoft.EntityFrameworkCore;
using ConsoleApp1.Model;

namespace ConsoleApp1
{
	public class ApplicationContext : DbContext
	{
		protected readonly string _connectionString;
		public ApplicationContext(string connectionString)
		{
			_connectionString = connectionString;
			Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			foreach (var entityType in modelBuilder.Model.GetEntityTypes())
			{
				// Убираем "s" в конце имени таблицы
				var tableName = entityType.GetTableName();
				if (tableName != null && tableName.EndsWith("s"))
				{
					entityType.SetTableName(tableName.Substring(0, tableName.Length - 1));
				}
			}

			base.OnModelCreating(modelBuilder);
		}

		public DbSet<Recipe> Recipe { get; set; }
		public DbSet<Ingredient> Ingredient { get; set; }
		public DbSet<RecipeIngredient> RecipeIngredient { get; set; }


		public void AddNewIngredient(Model.Ingredient ingredient, bool addUnique = false)
		{
			bool allowAdding = false;
			{
				if (!addUnique)
					allowAdding = true;
				else
				{
					var ingredients = addUnique ? Ingredient.ToList() : [];
					if (ingredients.Find(x => x.Name == ingredient.Name) is not null)
						allowAdding = true;
				}
			}
			if (allowAdding)
				Ingredient.Add(ingredient);
		}
		public void AddNewRecipe(Model.Recipe recipe, List<int> ingredients)
		{
			if (ingredients.Count < 0)
				throw new Exception("Wrong recipe. No ingredients");

			Recipe.Add(recipe);

			foreach (var ingredient in ingredients)
				RecipeIngredient.Add(new Model.RecipeIngredient(Recipe.ToList().Last().Id, ingredient));
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(_connectionString);
		}
	}
}
