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

			if (!TestCaesarEncode())
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
			Caesar caesar = new Caesar(plain: "hello world", key: "2");
			(var cipher, _) = caesar.Encode();
			Print(caesar.Plain, caesar.Key, cipher);
			return cipher == "jgnnq yqtnf";
		}

		public bool TestCaesarDecode()
		{
			Caesar caesar = new Caesar(cipher: "jgnnq yqtnf", key: "2");
			(var plain, _) = caesar.Decode();
			Print(caesar.Cipher, caesar.Key, plain);
			return plain == "hello world";
		}

		public bool TestCaesarBreak()
		{
			Caesar caesar = new Caesar(cipher: "jgnnq yqtnf");
			(var plain, var prob) = caesar.Break();
			Print(caesar.Cipher, caesar.Key, plain, prob);
			return plain == "hello world";
		}

		private void Print(params object[] objs)
		{
			DebugInfo.Content += "\n";
			foreach (object obj in objs)
			{
				DebugInfo.Content += obj.ToString() + " ";
			}
		}

		private void ClearDebugInfo(object sender, RoutedEventArgs e)
		{
			DebugInfo.Content = "";
		}
	}
}
