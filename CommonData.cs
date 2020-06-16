using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CipherBreaker
{
	static class CommonData
	{
		public static ObservableCollection<Task> Tasks = new ObservableCollection<Task>();

		public static int[] String2intArray(string key)
		{
			string[] strAr = key.Split(',');
			int[] ar = new int[Scheme.LetterSetSize];
			for (int i = 0; i < ar.Length; i++)
			{
				ar[i] = int.Parse(strAr[i]);
			}
			return ar;
		}
	}
}
