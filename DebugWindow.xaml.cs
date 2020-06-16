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

			if (!TestRailFenceEncode())
			{
				Print("TestRailFenceEncode failed");
			}
			if (!TestRailFenceDecode())
			{
				Print("TestRailFenceDecode failed");
			}
			if (!TestRailFenceBreak())
			{
				Print("TestRailFenceBreak failed");
			}
			if (!TestRailFenceBreak())
			{
				Print("TestRailFenceBreak failed");
			}

			if (!TestAffineEncode())
			{
				Print("TestAffineEncode failed");
			}
			if (!TestAffineDecode())
			{
				Print("TestAffineDecode failed");
			}
			if (!TestAffineBreak())
			{
				Print("TestAffineBreak failed");
			}
			if (!TestAffineGenerateKey())
			{
				Print("TestAffineGenerateKey failed");
			}
			if (!TestFileScheme())
			{
				Print("TestFileScheme failed");
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

		public bool TestRailFenceEncode()
		{
			RailFence railFence = new RailFence(plain: "one two three four", key: "4");
			(var cipher, _) = railFence.Encode();
			Print(railFence.Plain, railFence.Key, cipher);
			return cipher == "otteunwh reorf  eo";
		}

		public bool TestRailFenceDecode()
		{
			RailFence railFence = new RailFence(cipher: "otteunwh reorf  eo", key: "4");
			(var plain, _) = railFence.Decode();
			Print(railFence.Cipher, railFence.Key, plain);
			return plain == "one two three four";
		}

		public bool TestRailFenceBreak()
		{
			RailFence railFence = new RailFence(cipher : "otteunwh reorf  eo");
			(var plain, var prob) = railFence.Break();
			Print(railFence.Cipher, railFence.Key, plain, prob);
			return plain == "one two three four";
		}
		
		public bool TestAffineEncode()
		{
			Affine affine = new Affine(plain: "HELLO hello", key: "3,1");
			(var cipher, _) = affine.Encode();
			Print(affine.Plain, affine.Key, cipher);
			return cipher == "WNIIR wniir";
		}

		public bool TestAffineDecode()
		{
			Affine affine = new Affine(cipher: "WNIIR wniir", key: "3,1");
			(var plain, _) = affine.Decode();
			Print(affine.Cipher, affine.Key, plain);
			return plain == "HELLO hello";
		}

		public bool TestAffineBreak()
		{
			Affine affine = new Affine(cipher: "WNIIR wniir");
			(var plain, var prob) = affine.Break();
			Print(affine.Cipher, affine.Key, plain, prob);
			return plain == "HELLO hello";
		}
		public bool TestFileScheme()
		{
			FileScheme filescheme = new FileScheme();
			filescheme.File2Bytes("test.txt", "outEncoding.txt");
			Print("txt文件加密完成");
			filescheme.Bytes2File("outEncoding.txt", "outDecoding.txt");
			Print("txt文件解密完成");

			/*filescheme.File2Bytes("test.docx", "outEncoding.docx");
			Print("docx文件加密完成");
			filescheme.Bytes2File("outEncoding.docx", "outDecoding.docx");
			Print("docx文件解密完成");*/
			return true;
		}

		public bool TestAffineGenerateKey()
		{
			Affine affine = new Affine();
			string KeyGenerated = affine.GenerateKey();
			Print(KeyGenerated);
			return true;
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
