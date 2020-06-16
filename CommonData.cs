using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;

namespace CipherBreaker
{
	static class CommonData
	{
		public static ObservableCollection<Task> Tasks = new ObservableCollection<Task>();
		public static void AddAndSelect(Task task,ListBox taskList)
		{
			Tasks.Add(task);
			taskList.SelectedIndex = taskList.Items.Count - 1;
		}
	}
}
