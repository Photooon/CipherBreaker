using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CipherBreaker.Store;
using Microsoft.Data.Sqlite;

namespace CipherBreaker
{
	static class FrequencyAnalyst
	{
		private static Dictionary<string, long> frequencyDict = new Dictionary<string, long>();
		private static long totalCount;

		public static void Init()
		{
			SqliteClient freqDB = new SqliteClient("Data Source=cipher_breaker.db");
			freqDB.Open();

			(var wordList, var frequencyList) = freqDB.QueryAll();
			for (int i = 0; i < wordList.Count; i++)
			{
				frequencyDict[wordList[i]] = frequencyList[i];
				totalCount += frequencyList[i];
			}

			freqDB.Close();
		}

		public static void Flush()
		{
			var frequencyDB = new SqliteConnection("Data Source=cipher_breaker.db");
			frequencyDB.Open();

			List<Task<int>> taskList = new List<Task<int>>();
			foreach (var word_frequency in frequencyDict)
			{
				var writeCommand = frequencyDB.CreateCommand();
				writeCommand.CommandText =
					@"
					insert into word_frequency
					values ($word, $frequency)
				";
				writeCommand.Parameters.AddWithValue("$word", word_frequency.Key);
				writeCommand.Parameters.AddWithValue("$frequency", word_frequency.Value);
				//writeCommand.ExecuteNonQuery();
				var task = writeCommand.ExecuteNonQueryAsync();
				taskList.Add(task);
				if (taskList.Count >= 8)
				{
					Task.WaitAll(taskList.ToArray());
					taskList.Clear();
				}
			}
			Task.WaitAll(taskList.ToArray());
		}

		public static double Analyze(string str)
		{
			double prob = 0.0;
			if (totalCount == 0)
			{
				Init();
			}
			for (int i = 0; i < str.Length - 4; i++)
			{
				string quad = str.Substring(i, 4).ToUpper();
				long frequency = frequencyDict.GetValueOrDefault(quad);
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
