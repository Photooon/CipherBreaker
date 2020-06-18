using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CipherBreaker
{
	static class CommonData
	{
		public static string DbSource = "Data Source=cipher_breaker.db";
		public static ObservableCollection<Task> Tasks = new ObservableCollection<Task>();
		public static Settings settings = new Settings();
		public static Notifier notifier = new Notifier(Application.Current.MainWindow);

		public static SymmetricScheme ClipScheme = null;
		public static bool PressDown = false;

		public static void AddAndSelect(Task task,ListBox taskList)
		{
			Tasks.Add(task);
			taskList.SelectedIndex = taskList.Items.Count - 1;
		}
	}
}
