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
using System.IO;
using System.Xml.Serialization;
using System.Xml;

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

			/*右键快捷加密解密部分*/
			var args = Environment.GetCommandLineArgs();
			if (args.Length > 2)
			{
				string originPath = args[2];    //获取加密文件路径

				FileScheme fileScheme = new FileScheme();

				if (args[1] == "encrypt")       //加密文件
				{
					fileScheme.File2Bytes(originPath, originPath += ".cb");
				}
				else if (args[1] == "decrypt" && originPath.Contains(".cb"))     //解密文件，且检查是否为加密过的文件
				{
					int index = originPath.IndexOf(".cb");
					fileScheme.Bytes2File(originPath, originPath.Remove(index, 3));
				}
			}

			/*创建或读入设置文件部分*/
			string xmlPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
			xmlPath += "settings.xml";
			if (!File.Exists(xmlPath))
			{
				XMLSaveAndRead xmlHandle = new XMLSaveAndRead();

				//存储信息
				List<Settings> info = new List<Settings>();
				Settings settings = new Settings(false, true, ("crtl", "E"), SchemeType.Caesar, SchemeType.RailFence, SchemeType.Substitution);	//默认配置
				info.Add(settings);
				//将info的类型List<Test>和自身info传入
				string xmlInfo = xmlHandle.SerializeObject<List<Settings>>(info);
				xmlHandle.CreateXML(xmlPath, xmlInfo);
			}
			else
			{
				XMLSaveAndRead xmlHandle = new XMLSaveAndRead();
				string doc = xmlHandle.LoadXML(xmlPath);
				List<Settings> info1 = (List<Settings>)xmlHandle.DeserializeObject<List<Settings>>(doc);
				for (int i = 0; i < info1.Count; i++)
				{
					CommonData.settings.isAutoStart = info1[i].isAutoStart;
					CommonData.settings.isUsingServer = info1[i].isUsingServer;
					CommonData.settings.shortCutKey = info1[i].shortCutKey;
					CommonData.settings.encryptType = info1[i].encryptType;
					CommonData.settings.decryptType = info1[i].decryptType;
					CommonData.settings.breakType = info1[i].breakType;
				}
			}

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
			//debugWindow.Show();

			//Task testTask = new Task();
			//testTask.Name = "test";                                       //测试用
			//testTask.type = SchemeType.Caesar;
			//testTask.OptType = OperationType.Encode;                        //测试用
			//testTask.OriginText = "jmpwfzpv";      //测试用
			//testTask.Key = "2";
			//testTask.Date = DateTime.Now;                                  //测试用
			//CommonData.Tasks.Add(testTask);
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
			if (task.OptType == OperationType.Encode)
			{
				EncodePage encodePage = new EncodePage(task);
				ContentControl.Content = new Frame() { Content = encodePage };
				encodePage.TaskTitle.Content = task.ToString();
				encodePage.SchemeType.Content = task.type.ToString();
				encodePage.Key.Content = task.Key;
				encodePage.Text.Text = task.OriginText;
				encodePage.Date.Text = task.Date.ToString();
			}
			else if (task.OptType == OperationType.Decode)
			{
				DecodePage decodePage = new DecodePage(task);
				ContentControl.Content = new Frame() { Content = decodePage };
				decodePage.TaskTitle.Content = task.ToString();
				decodePage.SchemeType.Content = task.type.ToString();
				decodePage.Key.Content = task.Key;
				decodePage.Text.Text = task.ResultText;
				decodePage.Date.Text = task.Date.ToString();
			}
			else if (task.OptType == OperationType.Break)
			{
				BreakPage breakPage = new BreakPage(task);
				ContentControl.Content = new Frame() { Content = breakPage };
				breakPage.TaskTitle.Content = task.ToString();
				breakPage.SchemeType.Content = task.type.ToString();
				breakPage.Text.Text = task.ResultText;
				breakPage.Date.Text = task.Date.ToString();
			}
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
