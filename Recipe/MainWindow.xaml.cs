using Microsoft.VisualStudio.PlatformUI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Recipe
{
	public class ParentItem : ObservableObject
	{
		private ObservableCollection<ChildItem> _children = new();
		public int Id { get; set; }
		public string Name { get; set; }
		public ObservableCollection<ChildItem> Children
		{
			get => _children;
			set => Set(ref _children, value);
		}
	}

	public class ChildItem : ObservableObject
	{
		public int Id { get; set; }
		public string Description { get; set; }
	}


	public class MainViewModel : ViewModelBase
	{
		public ObservableCollection<ParentItem> ParentItems { get; } = new();

		public RelayCommand<object> DeleteParentCommand { get; }
		public RelayCommand<ChildItem> DeleteChildCommand { get; }

		public MainViewModel()
		{
			DeleteParentCommand = new RelayCommand<object>(DeleteParent);
			DeleteChildCommand = new RelayCommand<ChildItem>(DeleteChild);

			// Sample data initialization
			ParentItems.Add(new ParentItem
			{
				Id = 1,
				Name = "Parent 1",
				Children = new ObservableCollection<ChildItem> {
				new ChildItem { Id = 1, Description = "Child 1" },
				new ChildItem { Id = 2, Description = "Child 2" }
			}
			});
		}

		private void DeleteParent(object param)
		{
			if (param is ParentItem item)
				ParentItems.Remove(item);
		}

		private void DeleteChild(ChildItem item)
		{
			var parent = ParentItems.FirstOrDefault(p => p.Children.Contains(item));
			parent?.Children.Remove(item);
		}
	}





	public class RelayCommand<T> : ICommand
	{
		private readonly Action<T> _execute;
		private readonly Predicate<T> _canExecute;

		public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter) =>
			_canExecute == null || _canExecute((T)parameter);

		public void Execute(object parameter) =>
			_execute((T)parameter);

		public event EventHandler CanExecuteChanged;

		public void RaiseCanExecuteChanged() =>
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
	}


	//public class RelayCommand : ICommand
	//{
	//	private Action<object> _execute;
	//	private Func<object, bool> _canExecute;

	//	public event EventHandler? CanExecuteChanged
	//	{
	//		add { CommandManager.RequerySuggested += value; }
	//		remove { CommandManager.RequerySuggested -= value; }
	//	}


	//	public RelayCommand(Action<object> execute)
	//	{
	//		_execute = execute;
	//	}
	//	public RelayCommand(Action<object> execute, Func<object, bool> canExecute) : this(execute)
	//	{
	//		_canExecute = canExecute;
	//	}
	//	public bool CanExecute(object? parameter)
	//	{
	//		return _canExecute is null || parameter is null || _canExecute(parameter);
	//	}

	//	public void Execute(object? parameter)
	//	{
	//		if (CanExecute(parameter))
	//			_execute(parameter);
	//	}
	//}


	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}
	}
}