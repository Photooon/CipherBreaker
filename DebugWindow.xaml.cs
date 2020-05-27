using System;
using System.Collections.Generic;
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
	/// DebugWindow.xaml 的交互逻辑
	/// </summary>
	public partial class DebugWindow : Window
	{
		public DebugWindow()
		{
			InitializeComponent();

			Print("Debug!");
			
			if(!TestCaesarEncode())
			{
				Print("TestCaesarEncode failed");
			}
			if (!TestCaesarDecode())
			{
				Print("TestCaesarDecode failed");
			}
			if (!TestCaesarBreak())
			{
				Print("TestCaesarBreak failed");
			}

			return;
		}

		public bool TestCaesarEncode()
		{
			return true;
		}

		public bool TestCaesarDecode()
		{
			return true;
		}

		public bool TestCaesarBreak()
		{
			return true;
		}

		private void Print(object obj)
		{
			DebugInfo.Content += "\n" + obj.ToString();
		}

		private void ClearDebugInfo(object sender, RoutedEventArgs e)
		{
			DebugInfo.Content = "";
		}
	}
}
