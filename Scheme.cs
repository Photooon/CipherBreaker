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
		Pause,
		Finish,
		Over
	}

	abstract class Scheme
	{
		private SchemeType type;
		private SchemeState state;
		private string plain;
		private string cipher;
		private string encodeKey;
		private string decodeKey;
		private ConcurrentQueue<string> processLog;
		public static Dictionary<SchemeType, int> schemeCount = new Dictionary<SchemeType, int>();

		public Scheme(string plain="",string cipher="",string encodeKey="",string decodeKey="")
		{
			this.state = SchemeState.Ready;
			this.plain = plain;
			this.cipher = cipher;
			this.encodeKey = encodeKey;
			this.decodeKey = decodeKey;
			this.ShouldOutput = false;
		}

		~Scheme()
		{
			// Do nothing
		}

		public SchemeType Type { get => type; set => type = value; }
		public SchemeState State { get => state; set => state = value; }
		public string Plain { get => plain; set => plain = value; }
		public string Cipher { get => cipher; set => cipher = value; }
		public string EncodeKey { get => encodeKey; set => encodeKey = value; }
		public string DecodeKey { get => decodeKey; set => decodeKey = value; }

		public bool ShouldOutput;



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

		public int SchemeCount(SchemeType type)
		{
			return schemeCount[type];
		}
	}
}
