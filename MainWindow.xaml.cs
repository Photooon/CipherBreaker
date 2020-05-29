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

namespace CipherBreaker
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			DebugWindow debugWindow = new DebugWindow();
			debugWindow.Show();
			this.TaskListBox.ItemsSource = CommonData.Tasks;
			Task task1 = new Task();
			task1.Name = "first";
			CommonData.Tasks.Add(task1);
			Task task2 = new Task();
			task2.Name = "second";
			CommonData.Tasks.Add(task2);
		}
		private void NewTaskButton_Click(object sender, RoutedEventArgs e)
		{
			NewTaskWindow newTaskWindow = new NewTaskWindow();
			newTaskWindow.Show();
		}

		private void GuideButton_Click(object sender, RoutedEventArgs e)
		{
			GuideWindow guideWindow = new GuideWindow();
			guideWindow.Show();
		}

		private void AlgorithmButton_Click(object sender, RoutedEventArgs e)
		{
			AlgorithmWindow algorithmWindow = new AlgorithmWindow();
			algorithmWindow.Show();
		}

		private void OptionButton_Click(object sender, RoutedEventArgs e)
		{
			OptionWindow optionWindow = new OptionWindow();
			optionWindow.Show();
		}

		private void TaskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DetailButton.Visibility = Visibility.Visible;
			DetailButton.IsEnabled = true;
		}

		private void DetailButton_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
