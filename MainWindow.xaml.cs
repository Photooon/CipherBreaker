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
using System.Diagnostics.SymbolStore;

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
				Settings settings = new Settings(false, true, ("crtl", "E"), SchemeType.Caesar, SchemeType.RailFence, SchemeType.Substitution); //默认配置
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
			foreach (var task in taskList)
			{
				CommonData.Tasks.Add(task);
			}
			dbClient.Close();

			//DebugWindow debugWindow = new DebugWindow();
			//debugWindow.Show();
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			SqliteClient dbClient = new SqliteClient(CommonData.DbSource);
			dbClient.Open();
			dbClient.ClearTask();
			foreach (var task in CommonData.Tasks)
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
				(page as EncodePage).DeleteButton.Click += RemoveItem;
			}
			else if (task.OptType == OperationType.Decode)
			{
				page = new DecodePage(task);
				(page as DecodePage).DeleteButton.Click += RemoveItem;
			}
			else if (task.OptType == OperationType.Break)
			{
				page = new BreakPage(task);
				(page as BreakPage).DeleteButton.Click += RemoveItem;
			}
			ContentControl.Content = new Frame() { Content = page };
		}

		private void RemoveItem(object sender, RoutedEventArgs e)
		{
			if (TaskListBox.SelectedIndex != -1)
			{
				CommonData.Tasks.RemoveAt(TaskListBox.SelectedIndex);
				ContentControl.Content = null;
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

		private void ClearTask(object sender, RoutedEventArgs e)
		{
			CommonData.Tasks.Clear();
			ContentControl.Content = null;
		}

		private void Window_LostFocus(object sender, RoutedEventArgs e)
		{
			CommonData.notifier.Notify();
		}

		private void Window_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			CommonData.notifier.MarkRead();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Hotkey.Regist(this, HotkeyModifiers.MOD_CONTROL, Key.E, () =>
			{
				CommonData.PressDown = !CommonData.PressDown;
				if (!CommonData.PressDown)
					return;

				if(CommonData.ClipScheme==null)
				{
					CommonData.ClipScheme = Scheme.ChooseScheme("", "", "") as SymmetricScheme;
				}
				else if(CommonData.ClipScheme.Plain == Clipboard.GetText())
				{
					return;
				}

				CommonData.ClipScheme.Plain = Clipboard.GetText();
				CommonData.ClipScheme.Key = CommonData.ClipScheme.GenerateKey();
				CommonData.ClipScheme.Encode();
				Clipboard.SetText(CommonData.ClipScheme.Cipher);
				if (!this.IsKeyboardFocused)
					CommonData.notifier.Notify();
			});

			Hotkey.Regist(this, HotkeyModifiers.MOD_CONTROL, Key.D, () =>
			{
				if (CommonData.ClipScheme == null) return;

				Clipboard.SetText(CommonData.ClipScheme.Plain);
				CommonData.notifier.MarkRead();
			});

			Hotkey.Regist(this, HotkeyModifiers.MOD_CONTROL, Key.R, () =>
			{
				CommonData.notifier.MarkRead();
			});
		}
	}
}
