using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace CipherBreaker
{
	enum SchemeType
	{
		Caesar,
		Vigenere,
		Substitution,
		Transposition,
		Columnar
	}

	enum SchemeState
	{
		Ready,
		Running,
		Pause,
		Finish,
		Over
	}

	abstract class Scheme
	{
		protected static Dictionary<SchemeType, int> schemeCount = new Dictionary<SchemeType, int>();

		protected SchemeState state;
		protected string encodeKey;
		protected string decodeKey;
		protected ConcurrentQueue<string> processLog;

		public Scheme(string plain="",string cipher="",string encodeKey="",string decodeKey="")
		{
			this.state = SchemeState.Ready;
			this.Plain = plain;
			this.Cipher = cipher;
			this.EncodeKey = encodeKey;
			this.DecodeKey = decodeKey;
			this.ShouldOutput = false;
		}

		~Scheme()
		{
			// Do nothing
		}

		public SchemeType Type { get; protected set; }
		public SchemeState State { get => state;}
		public string Plain { get; set; }
		public string Cipher { get; set; }
		public virtual string EncodeKey { get => encodeKey; set => encodeKey = value; }
		public virtual string DecodeKey { get => decodeKey; set => decodeKey = value; }

		public bool ShouldOutput { get; set; }

		public abstract bool Encode(string plain = "", string encodeKey = "");
		public abstract bool Decode(string cipher = "", string decodeKey = "");
		public SchemeState Pause()
		{
			state = SchemeState.Pause;
			return state;
		}

		public SchemeState End()
		{
			state = SchemeState.Over;
			return state;
		}

		public abstract bool Save(string fileName);
		public abstract bool Load(string fileName);

		public int SchemeCount()
		{
			int sum = 0;
			foreach(int val in schemeCount.Values)
			{
				sum += val;
			}
			return sum;
		}
		public int SchemeCount(SchemeType type)
		{
			return schemeCount[type];
		}

		public new abstract string ToString();
	}

	static class FrequencyAnalyst
	{
		private static Dictionary<string, long> frequencyDict = new Dictionary<string, long>();

		public static double Analyze(string str)
		{
			return 0.0;
		}

		public static string[] Split(string str)
		{
			return null;
		}

	}
}
