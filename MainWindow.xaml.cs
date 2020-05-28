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

			Task task1 = new Task();
			task1.Name = "first";
			Task task2 = new Task();
			task2.Name = "second";
			Task task3 = new Task();
			task3.Name = "third";
			this.TaskListBox.Items.Add(task1);
			this.TaskListBox.Items.Add(task2);
			this.TaskListBox.Items.Add(task3);
		}
	}
}
