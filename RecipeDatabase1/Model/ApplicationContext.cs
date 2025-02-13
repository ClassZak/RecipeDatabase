using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeDatabase1.Model
{
	class RecipeDataBaseContext : DbContext
	{
		private string _connectionString;

		public RecipeDataBaseContext(string connectionString)
		{
			_connectionString = connectionString;
			Database.EnsureCreated();
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			foreach (var el in modelBuilder.Model.GetEntityTypes())
			{
				var name = el.GetTableName();
				if (name is null)
					continue;

				if (name[name.Length - 1] == 's')
					el.SetTableName(name.Substring(0, name.Length - 1));
			}
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(_connectionString);
		}

		public DbSet<Ingredient> Ingredient { get; set; }
		public DbSet<Recipe> Recipe { get; set; }
		public DbSet<RecipeIngredient> RecipeIngredient { get; set; }
		public DbSet<MeasureUnit> MeasureUnit { get; set; }
	}
}
