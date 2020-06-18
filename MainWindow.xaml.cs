using CipherBreaker.Store;
using Microsoft.VisualBasic;
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
using System.Diagnostics;
using System.IO;

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
			this.TaskListBox.ItemsSource = CommonData.Tasks;

			SqliteClient dbClient = new SqliteClient(CommonData.DbSource);
			dbClient.Open();
			var taskList = dbClient.QueryAllTask();
			foreach(var task in taskList)
			{
				CommonData.Tasks.Add(task);
			}
			dbClient.Close();

			//DebugWindow debugWindow = new DebugWindow();

			/*右键快捷加密解密部分*/
			var args = Environment.GetCommandLineArgs();
			if (args.Length > 2)
			{
				string originPath = args[2];	//获取加密文件路径

				FileScheme fileScheme = new FileScheme();

				if (args[1] == "encrypt")		//加密文件
                {
					fileScheme.File2Bytes(originPath, originPath += ".cb");
				}
				else if (args[1] == "decrypt" && originPath.Contains(".cb"))	 //解密文件，且检查是否为加密过的文件
                {
					int index = originPath.IndexOf(".cb");
					fileScheme.Bytes2File(originPath, originPath.Remove(index, 3));
				}
			}
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			SqliteClient dbClient = new SqliteClient(CommonData.DbSource);
			dbClient.Open();
			dbClient.ClearTask();
			foreach(var task in CommonData.Tasks)
			{
				dbClient.InsertTask(task);
			}
			dbClient.Close();
		}

		private void NewTaskButton_Click(object sender, RoutedEventArgs e)
		{
			NewTaskWindow newTaskWindow = new NewTaskWindow(this);
			newTaskWindow.Show();
		}

		private void OptionButton_Click(object sender, RoutedEventArgs e)
		{
			
		}

		private void TaskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (TaskListBox.SelectedItem == null)
			{
				return;
			}
			Task task = TaskListBox.SelectedItem as Task;

			Page page = null;
			if (task.OptType == OperationType.Encode)
			{
				page = new EncodePage(task);
				//ContentControl.Content = new Frame() { Content = encodePage };
			}
			else if (task.OptType == OperationType.Decode)
			{
				page = new DecodePage(task);
				//ContentControl.Content = new Frame() { Content = decodePage };
			}
			else if (task.OptType == OperationType.Break)
			{
				page = new BreakPage(task);
				//ContentControl.Content = new Frame() { Content = breakPage };
			}
			ContentControl.Content = new Frame() { Content = page };
		}

		private void RemoveItem(object sender, RoutedEventArgs e)
		{
			if (TaskListBox.SelectedIndex != -1)
			{
				CommonData.Tasks.RemoveAt(TaskListBox.SelectedIndex);
			}
		}

		private void Settings_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			OptionWindow optionWindow = new OptionWindow();
			optionWindow.Show();
		}

		private void NewTask_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			NewTaskWindow newTaskWindow = new NewTaskWindow(this);
			newTaskWindow.Show();
		}
	}
}
