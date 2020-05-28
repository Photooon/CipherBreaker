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
		const string OperationRecordTableName = "operation_record";

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

		public void InsertOptRecord(OperationRecord record)
		{
			var command = CreateCommand();
			command.CommandText = $"insert into {OperationRecordTableName}" +
				$" values(null,{(int)record.Type},{record.OriginText},{record.Key},{record.ResultText},{DateTime.Now.ToString()})";
			command.ExecuteNonQuery();
		}

		public string QueryOptRecord(string originText,string key)
		{
			var command = CreateCommand();
			command.CommandText = $"select result_text from {OperationRecordTableName} where origin_text={originText} and key={key}";
			return (string)command.ExecuteScalar();
		}

		public List<OperationRecord> QueryAllOptRecord()
		{
			var command = CreateCommand();
			command.CommandText = $"select * from {OperationRecordTableName}";
			var reader = command.ExecuteReader();

			List<OperationRecord> optRecordList = new List<OperationRecord>();
			while (reader.Read())
			{
				optRecordList.Add(new OperationRecord(reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetString(4),
					reader.GetString(5)));
			}
			reader.Close();

			return optRecordList;
		}
	}
}
