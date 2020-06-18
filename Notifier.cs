using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Resources;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CipherBreaker
{
	class Notifier
	{
		private Window mainwindow;
		public Notifier(Window window)
		{
			mainwindow = window;
		}
		public void Notify()
		{
			mainwindow.Icon = new BitmapImage(new System.Uri("assets/notify.png",UriKind.Relative));
		}

		public void MarkRead()
		{
			//mainwindow.Icon = new BitmapImage(new System.Uri("assets/logo.png",UriKind.Relative));
		}
	}
}
