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

			
			
			RailFence scheme = new RailFence(cipher: "ooentfehtrwe", key: "5");
            (string plain, _) = scheme.Decode();
			/*StringBuilder key = new StringBuilder();
			for(int i = 25;i>=0;i--)
			{
				key.Append(i.ToString());
				if (i > 0)
					key.Append(',');
			}
			Substitution sub = new Substitution("hello", key: key.ToString());
			sub.Encode();
			sub.Decode();*/

			/*Affine affine = new Affine("Hello", "", "3,1");
			affine.Encode();
			Affine affines = new Affine("", "Wniir", "3,1");
			affines.Decode();*/

			return;
		}
	}
}
