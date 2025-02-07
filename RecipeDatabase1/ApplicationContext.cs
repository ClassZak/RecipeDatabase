using Microsoft.EntityFrameworkCore;
using RecipeDatabase1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeDatabase1
{
    class ApplicationContext : DbContext
    {
        private string _connectionString;

        public ApplicationContext(string connectionString)
        {
            _connectionString = connectionString;
            Database.EnsureCreated();
        }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

            foreach(var el in modelBuilder.Model.GetEntityTypes())
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

		public DbSet<Ingredient> Ingredient{ get; set; }
        public DbSet<Recipe> Recipe{ get; set; }
        public DbSet<RecipeIngredient> RecipeIngredient{ get; set; }
	}
}
