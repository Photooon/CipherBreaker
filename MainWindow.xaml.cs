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
		public MainWindow()//一条注释
		{
			InitializeComponent();

			DebugWindow debugWindow = new DebugWindow();
			debugWindow.Show();
			this.TaskListBox.ItemsSource = CommonData.Tasks;
			Task task1 = new Task();
			task1.Name = "first";                                    //测试用
			task1.OptType = OperationType.Decode;					 //测试用
			task1.type = SchemeType.Caesar;							 //测试用
			task1.Key = "3";										 //测试用
			task1.ResultText = "dddtttttttttttttttttttttttttttttt";	 //测试用
			task1.Date = DateTime.Now;								 //测试用
			CommonData.Tasks.Add(task1);

			Task task2 = new Task();
			task2.Name = "second";                                                 //测试用
			task2.OptType = OperationType.Encode;								   //测试用
			task2.type = SchemeType.Caesar;										   //测试用
			task2.Key = "5";													   //测试用
			task2.OriginText = "iiiiiiiiiiiizzzzzzzzzzzzzoooooooonnnnnnnnneeeeee"; //测试用
			task2.Date = DateTime.Now;											   //测试用
			CommonData.Tasks.Add(task2);

			Task task3 = new Task();
			task3.Name = "third";                                       //测试用
			task3.OptType = OperationType.Break;						//测试用
			task3.OriginText = "jmpwfzpv";  	//测试用
			task3.Date = DateTime.Now;									//测试用
			CommonData.Tasks.Add(task3);
		}
		private void NewTaskButton_Click(object sender, RoutedEventArgs e)
		{
			NewTaskWindow newTaskWindow = new NewTaskWindow();
			newTaskWindow.Show();
		}

		private void OptionButton_Click(object sender, RoutedEventArgs e)
		{
			OptionWindow optionWindow = new OptionWindow();
			optionWindow.Show();
		}

		private void TaskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Task task = TaskListBox.SelectedItem as Task;
			if(task.OptType==OperationType.Encode)
			{
				EncodePage encodePage = new EncodePage(task);
				ContentControl.Content = new Frame() { Content = encodePage };
				encodePage.TaskTitle.Content += task.ToString();
				encodePage.SchemeType.Content += task.type.ToString();
				encodePage.Key.Content += task.Key;
				encodePage.Text.Text += "\n" + task.OriginText;
				encodePage.Date.Text += "\n" + task.Date.ToString();
			}
			else if(task.OptType==OperationType.Decode)
			{
				DecodePage decodePage = new DecodePage(task);
				ContentControl.Content = new Frame() { Content = decodePage };
				decodePage.TaskTitle.Content += task.ToString();
				decodePage.SchemeType.Content += task.type.ToString();
				decodePage.Key.Content += task.Key;
				decodePage.Text.Text += "\n" + task.ResultText;
				decodePage.Date.Text += "\n" + task.Date.ToString();
			}
			else if(task.OptType==OperationType.Break)
			{
				BreakPage breakPage = new BreakPage(task);
				ContentControl.Content = new Frame() { Content = breakPage };
				breakPage.TaskTitle.Content += task.ToString();
				breakPage.Text.Text += "\n" + task.OriginText;
				breakPage.Date.Text += "\n" + task.Date.ToString();
			}
		}

	}
}
