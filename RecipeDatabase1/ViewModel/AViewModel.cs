namespace RecipeDatabase1.ViewModel
{
	public abstract class AViewModel
	{
		protected readonly int _id = 0;
		protected AViewModel(int id=0) 
		{
			_id = id;
		}
		public int GetId()
		{
			return _id;
		}
	}
}
