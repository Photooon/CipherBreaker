using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.TextFormatting;

namespace CipherBreaker.Store
{
	class SqliteClient : SqliteConnection
	{
		const string WordFrequencyTableName = "word_frequency";

		public SqliteClient(string connectionString) : base(connectionString)
		{
		}

		public void Insert(string word, long frequency)
		{
			var command = CreateCommand();
			command.CommandText = $"insert into {WordFrequencyTableName} values({word},{frequency})";
			command.ExecuteNonQuery();
		}

		public void Updata(string word, long frequency)
		{
			var command = CreateCommand();
			command.CommandText = $"update {WordFrequencyTableName} set frequency={frequency} where word={word}";
			command.ExecuteNonQuery();
		}

		public long Query(string word)
		{
			var command = CreateCommand();
			command.CommandText = $"select frequency from {WordFrequencyTableName} where word={word}";
			return (long)command.ExecuteScalar();
		}

		public (List<string>, List<long>) QueryAll()
		{
			var command = CreateCommand();
			command.CommandText = $"select * from {WordFrequencyTableName}";
			var reader = command.ExecuteReader();

			List<string> wordList = new List<string>();
			List<long> frequencyList = new List<long>();
			while (reader.Read())
			{
				wordList.Add(reader.GetString(0));
				frequencyList.Add(reader.GetInt32(1));
			}

			return (wordList, frequencyList);
		}

		void Delete(string word)
		{
			var command = CreateCommand();
			command.CommandText = $"delete from {WordFrequencyTableName} where word={word}";
			command.ExecuteNonQuery();
		}
	}
}
