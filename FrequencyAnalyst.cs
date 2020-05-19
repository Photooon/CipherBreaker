using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Animation;

namespace CipherBreaker
{
	static class FrequencyAnalyst
	{
		private static Dictionary<string, long> frequencyDict = new Dictionary<string, long>();
		private static long totalCount;

		public static void Init()
		{
			StreamReader quadgramFile = new StreamReader("./Resource/word_quadgrams.txt");
			string line;
			while ((line = quadgramFile.ReadLine()) != null)
			{
				string[] args = line.Split(' ');
				string quad = args[0];
				long frequency = long.Parse(args[1]);
				frequencyDict[quad] = frequency;
				totalCount += frequency;
			}
			quadgramFile.Close();
		}
		public static double Analyze(string str)
		{
			double prob = 0.0;
			if (totalCount == 0)
			{
				Init();
			}
			for (int i = 0;i<str.Length-4;i++)
			{
				string quad = str.Substring(i, 4);
				long frequency = frequencyDict[quad];
				if (frequency == 0)
				{
					prob += Math.Log(1.0 / totalCount / 2.0);
				}
				else
				{
					prob += Math.Log((double)frequency / totalCount);
				}
			}
			return prob;
		}

		public static string[] Split(string str)
		{
			return null;
		}

	}
}
