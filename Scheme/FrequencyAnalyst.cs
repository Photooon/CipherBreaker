using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using CipherBreaker.Store;
using Microsoft.Data.Sqlite;

namespace CipherBreaker
{
	class FrequencyAnalyst
	{
		private static Dictionary<string, long>[] freqs = null;
		private static long[] totalCounts = null;

		public static void Init()
		{
			totalCounts = new long[4];
			freqs = new Dictionary<string, long>[4];
			for (int i = 0; i < 4; i++)
			{
				freqs[i] = new Dictionary<string, long>();
			}

			SqliteClient freqDB = new SqliteClient(CommonData.DbSource);
			freqDB.Open();

			(var wordList, var frequencyList) = freqDB.QueryAllWordFreq();
			for (int i = 0; i < wordList.Count; i++)
			{
				(var word, var freq) = (wordList[i], frequencyList[i]);
				if (word.Length >= 4)
				{
					for (int j = 0; j < word.Length - 4; j++)
					{
						string sub = word.Substring(j, 4);
						if (freqs[0].ContainsKey(sub))
						{
							freqs[0][sub] += freq;
						}
						else
						{
							freqs[0][sub] = freq;
						}
						totalCounts[0] += freq;
					}
				}
				else
				{
					freqs[word.Length][word] = freq;
					totalCounts[word.Length] += freq;
				}
			}

			freqDB.Close();
		}

		public static void Flush()
		{
			//var frequencydb = new sqliteconnection("data source=cipher_breaker.db");
			//frequencydb.open();

			//using (var transaction = frequencydb.begintransaction())
			//{
			//	var command = frequencydb.createcommand();
			//	command.commandtext =
			//		@"
			//		insert into word_frequency
			//		values ($word, $frequency)
			//	";

			//	(var wordparam, var freqparam) = (command.createparameter(), command.createparameter());
			//	wordparam.parametername = "$word";
			//	freqparam.parametername = "$frequency";
			//	command.parameters.add(wordparam);
			//	command.parameters.add(wordparam);

			//	foreach (var wordfrequency in frequencydict)
			//	{
			//		wordparam.value = wordfrequency.key;
			//		freqparam.value = wordfrequency.value;
			//		command.executenonquery();
			//	}

			//	transaction.commit();
			//}

			//frequencydb.close();
		}

		public static double Analyze(string str)
		{
			double prob = 0.0;

			if (totalCounts == null)
			{
				Init();
			}

			str = Regex.Replace(str, "[^a-zA-Z]", "");

			if (str.Length >= 4)
			{
				for (int i = 0; i < str.Length - 4; i++)
				{
					string quad = str.Substring(i, 4).ToUpper();
					long frequency = freqs[0].GetValueOrDefault(quad);
					if (frequency == 0)
					{
						prob += Math.Log(1.0 / totalCounts[0] / 2.0);
					}
					else
					{
						prob += Math.Log((double)frequency / totalCounts[0]);
					}
				}
			}
			else
			{
				long frequency = freqs[str.Length].GetValueOrDefault(str);
				if (frequency == 0)
				{
					prob += Math.Log(1.0 / totalCounts[str.Length] / 2.0);
				}
				else
				{
					prob += Math.Log((double)frequency / totalCounts[str.Length]);
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