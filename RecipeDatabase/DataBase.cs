using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents.Serialization;
using System.Xml.Serialization;

namespace RecipeDatabase
{
	internal class DataBase
	{
		List<Recipe> Recipes;
		public readonly string DATABASE_PATH;
		private bool isInputOutputWorks = false;

		public List<Recipe> GetRecipes()
		{
			return Recipes;
        }
		public DataBase()
		{
			DATABASE_PATH=Path.Combine(Directory.GetCurrentDirectory(),"Data");

            Recipes=new List<Recipe>();
            if (Directory.Exists(DATABASE_PATH))
				LoadData();
			else
				Directory.CreateDirectory(DATABASE_PATH);
		}



        #region Loading methods
        public void LoadData()
		{
			Recipes.Clear();
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Recipe));

			DirectoryInfo directory=new DirectoryInfo(DATABASE_PATH);

			foreach(FileInfo item in directory.GetFiles())
			{
				using (FileStream file = new FileStream(item.FullName, FileMode.Open, FileAccess.Read))
				{
					Recipe recipe = xmlSerializer.Deserialize(file) as Recipe;
					Recipes.Add(recipe);

					file.Close();
				}
			}
		}
		public Recipe LoadItem(Recipe recipe)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Recipe));

			if (!File.Exists(Path.Combine(DATABASE_PATH, recipe.Name + ".xml")))
				throw new FileNotFoundException($"Не найден рецепт {recipe.Name} среди сохранённых файлов");
			else
			{
				FileStream fileStream =
				new FileStream(Path.Combine(DATABASE_PATH, recipe.Name + ".xml"), FileMode.Open, FileAccess.Read);

				recipe = xmlSerializer.Deserialize(fileStream) as Recipe;
			}

			return recipe;
		}
		public void UpdateData()
		{
			StringBuilder stringBuilder = new StringBuilder();

			for(int i=0;i!=Recipes.Count;++i)
			{
				try
				{
					Recipes[i]=LoadItem(Recipes[i]);
				}
				catch(FileNotFoundException ex)
				{
					stringBuilder.AppendLine(ex.Message);
				}
			}

			if (stringBuilder.Length != 0)
				throw new Exception($"Ошибка обновления данных:\n{stringBuilder.ToString()}");
		}
        public void UpLoadData()
        {
            for (int i = 0; i != Recipes.Count; ++i)
            {
				try
				{
					Recipes[i] = LoadItem(Recipes[i]);
                }
				catch (Exception)
				{
					Recipes.RemoveAt(i--);
                }
            }
        }
        #endregion
        #region Save methods
        public void SaveData()
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Recipe));
			DirectoryInfo directory = new DirectoryInfo(DATABASE_PATH);


            foreach (Recipe item in Recipes)
			{
				using (FileStream file = new FileStream(Path.Combine(DATABASE_PATH, item.Name+".xml"), FileMode.Create, FileAccess.Write))
				{
                    file.Seek(0, SeekOrigin.Begin);
                    xmlSerializer.Serialize(file, item);

					file.Close();
				}
			}
		}
		public void DeleteExtraFiles()
		{
            DirectoryInfo directory = new DirectoryInfo(DATABASE_PATH);

            string name;
            FileInfo[] fileInfos = directory.GetFiles();

            for (int i = 0; i != fileInfos.Length; ++i)
            {
                name = fileInfos[i].Name.Substring(0, fileInfos[i].Name.LastIndexOf('.'));

                if (Recipes.Count(x => x.Name == name) == 0)
                    File.Delete(fileInfos[i].FullName);
            }
        }
		public void SaveItem(int index)
		{
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Recipe));

            using
			(
				FileStream file =
				new FileStream
				(
					Path.Combine(DATABASE_PATH, this.Recipes[index].Name + ".xml"),
					FileMode.Create,
					FileAccess.Write
				)
			)
            {
				file.Seek(0, SeekOrigin.Begin);
                xmlSerializer.Serialize(file, this.Recipes[index]);

                file.Close();
            }
        }
        #endregion
		public void AddNewItem(Recipe item)
		{
			if (!HaveElementWithName(item.Name))
				Recipes.Add(item);
			else
				throw new Exception
				($"Не удалось сохранить рецепт {item.Name}.\nРецепт с названием {item.Name} уже существует");
		}
		public bool HaveElementWithName(string name)
		{
			return Recipes.Any(x => x.Name == name);
		}
		
    }
}
