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

			DebugWindow debugWindow = new DebugWindow();
			//debugWindow.Show();

			Task testTask = new Task();
			testTask.Name = "test";                                       //测试用
			testTask.type = SchemeType.Caesar;
			testTask.OptType = OperationType.Encode;                        //测试用
			testTask.OriginText = "jmpwfzpv";      //测试用
			testTask.Key = "2";
			testTask.Date = DateTime.Now;                                  //测试用
			CommonData.Tasks.Add(testTask);
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
			OptionWindow optionWindow = new OptionWindow();
			optionWindow.Show();
		}

		private void TaskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (TaskListBox.SelectedItem == null)
			{
				return;
			}
			Task task = TaskListBox.SelectedItem as Task;
			if (task.OptType == OperationType.Encode)
			{
				EncodePage encodePage = new EncodePage(task);
				ContentControl.Content = new Frame() { Content = encodePage };
				encodePage.TaskTitle.Content += task.ToString();
				encodePage.SchemeType.Content += task.type.ToString();
				encodePage.Key.Content += task.Key;
				encodePage.Text.Text += task.OriginText;
				encodePage.Date.Text += task.Date.ToString();
			}
			else if (task.OptType == OperationType.Decode)
			{
				DecodePage decodePage = new DecodePage(task);
				ContentControl.Content = new Frame() { Content = decodePage };
				decodePage.TaskTitle.Content += task.ToString();
				decodePage.SchemeType.Content += task.type.ToString();
				decodePage.Key.Content += task.Key;
				decodePage.Text.Text += task.ResultText;
				decodePage.Date.Text += task.Date.ToString();
			}
			else if (task.OptType == OperationType.Break)
			{
				BreakPage breakPage = new BreakPage(task);
				ContentControl.Content = new Frame() { Content = breakPage };
				breakPage.TaskTitle.Content += task.ToString();
				breakPage.Text.Text += task.OriginText;
				breakPage.Date.Text += task.Date.ToString();
			}
		}

		private void RemoveItem(object sender, RoutedEventArgs e)
		{
			if (TaskListBox.SelectedIndex != -1)
			{
				CommonData.Tasks.RemoveAt(TaskListBox.SelectedIndex);
			}
		}
	}
}
