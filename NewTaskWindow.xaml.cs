using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CipherBreaker
{
	/// <summary>
	/// NewTaskWindow.xaml 的交互逻辑
	/// </summary>
	public partial class NewTaskWindow : Window
	{
		private MainWindow mainWindow;
		public NewTaskWindow()
		{
			InitializeComponent();
		}

		public NewTaskWindow(MainWindow mainWindow)
		{
			InitializeComponent();
			this.mainWindow = mainWindow;
		}

		private SchemeType GetSchemeTypeBySelectedIndex(int index)
		{
			switch (index)
			{
				case 0:
					return SchemeType.Caesar;
				case 1:
					return SchemeType.Substitution;
				case 2:
					return SchemeType.RailFence;
				case 3:
					return SchemeType.Affine;
			}
			return SchemeType.RailFence;
		}

		private void EncodeButton_Click(object sender, RoutedEventArgs e)
		{
			Task task = new Task();

			task.Name = EncodeTitle.Text;
			task.OriginText = Plain.Text;
			task.ResultText = null;
			task.OptType = OperationType.Encode;
			task.type = GetSchemeTypeBySelectedIndex(EncodeScheme.SelectedIndex);
			task.Date = DateTime.Now;
			task.Key = EncodeKey.Text;
			CommonData.AddAndSelect(task, mainWindow.TaskListBox);
			this.Close();
		}

		private void DecodeButton_Click(object sender, RoutedEventArgs e)
		{
			Task task = new Task();
			task.Name = DecodeTitle.Text;
			task.ResultText = Cipher.Text;
			task.OriginText = null;
			task.OptType = OperationType.Decode;
			task.type = GetSchemeTypeBySelectedIndex(DecodeScheme.SelectedIndex);
			task.Date = DateTime.Now;
			task.Key = DecodeKey.Text;

			CommonData.AddAndSelect(task, mainWindow.TaskListBox);
			this.Close();
		}

		private void BreakButton_Click(object sender, RoutedEventArgs e)
		{
			Task task = new Task();

			task.Name = BreakTitle.Text;
			task.OriginText = BreakPlain.Text;
			task.ResultText = null;
			task.OptType = OperationType.Break;
			task.Date = DateTime.Now;

			CommonData.AddAndSelect(task, mainWindow.TaskListBox);
			this.Close();
		}

		private void EncodeGenerateKey(object sender, RoutedEventArgs e)
		{
			var type = GetSchemeTypeBySelectedIndex(EncodeScheme.SelectedIndex);
			if (type == SchemeType.RailFence)
			{
				Random rand = new Random();
				if (Plain.Text.Length == 0)
				{
					MessageBox.Show("明文不能为空");
					return;
				}
				EncodeKey.Text = (rand.Next(Plain.Text.Length) + 1).ToString();
			}
			else
			{
				EncodeKey.Text = (Scheme.NewScheme(type) as SymmetricScheme).GenerateKey();
			}
		}

		private void ChooseFileButton_Click(object sender, MouseButtonEventArgs e)
		{
			
		}
	}
}
