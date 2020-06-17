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
		const string TaskTableName = "task";

		public SqliteClient(string connectionString) : base(connectionString) { }

		public void InsertWordFreq(string word, long frequency)
		{
			var command = CreateCommand();
			command.CommandText = $"insert into {WordFrequencyTableName} values({word},{frequency})";
			command.ExecuteNonQuery();
		}

		public void UpdateWordFreq(string word, long frequency)
		{
			var command = CreateCommand();
			command.CommandText = $"update {WordFrequencyTableName} set frequency={frequency} where word={word}";
			command.ExecuteNonQuery();
		}

		public long QueryWordFreq(string word)
		{
			var command = CreateCommand();
			command.CommandText = $"select frequency from {WordFrequencyTableName} where word={word}";
			return (long)command.ExecuteScalar();
		}

		public (List<string>, List<long>) QueryAllWordFreq()
		{
			var command = CreateCommand();
			command.CommandText = $"select * from {WordFrequencyTableName}";
			var reader = command.ExecuteReader();

			List<string> wordList = new List<string>();
			List<long> frequencyList = new List<long>();
			while (reader.Read())
			{
				wordList.Add(reader.GetString(0));
				frequencyList.Add(reader.GetInt64(1));
			}
			reader.Close();

			return (wordList, frequencyList);
		}

		public void DeleteWordFreq(string word)
		{
			var command = CreateCommand();
			command.CommandText = $"delete from {WordFrequencyTableName} where word={word}";
			command.ExecuteNonQuery();
		}

		public void InsertTask(Task task)
		{
			var command = CreateCommand();
			command.CommandText = $"insert into {TaskTableName}" +
				$" values('{task.Name}',{(int)task.type},{(int)task.OptType},'{task.OriginText}','{task.Key}','{task.ResultText}',{task.Date.Ticks})";
			command.ExecuteNonQuery();
		}

		public string QueryTask(string originText,string key)
		{
			var command = CreateCommand();
			command.CommandText = $"select result_text from {TaskTableName} where origin_text={originText} and key={key}";
			return (string)command.ExecuteScalar();
		}

		public List<Task> QueryAllTask()
		{
			var command = CreateCommand();
			command.CommandText = $"select * from {TaskTableName}";
			var reader = command.ExecuteReader();

			List<Task> optRecordList = new List<Task>();
			while (reader.Read())
			{
				Task task = new Task();
				task.Name = reader.GetString(0);
				task.type = (SchemeType)reader.GetInt32(1);
				task.OptType = (OperationType)reader.GetInt32(2);
				task.OriginText = reader.GetString(3);
				task.Key = reader.GetString(4);
				task.ResultText = reader.GetString(5);
				task.Date = new DateTime(reader.GetInt64(6));
				optRecordList.Add(task);
			}
			reader.Close();

			return optRecordList;
		}

		public void ClearTask()
		{
			var command = CreateCommand();
			command.CommandText = $"delete from {TaskTableName}";
			command.ExecuteNonQuery();
		}
	}
}
