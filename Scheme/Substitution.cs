using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;

namespace CipherBreaker
{
	class Substitution : SymmetricScheme
	{
		private int[] permutation;
		private bool permutationIsValid;
		private bool calPermutation()
		{
			string[] perString = key.Split(',');
			if (perString.Length != Scheme.LetterSetSize)
				return false;

			for (int i = 0; i < Scheme.LetterSetSize; i++)
			{
				if (!int.TryParse(perString[i], out permutation[i]))
				{
					return false;
				}
			}

			return true;
		}

		public static int[] CalPermutation(string key)
		{
			string[] perString = key.Split(',');
			if (perString.Length != Scheme.LetterSetSize)
				return null;

			int[] per = new int[Scheme.LetterSetSize];

			for (int i = 0; i < Scheme.LetterSetSize; i++)
			{
				if (!int.TryParse(perString[i], out per[i]))
				{
					return null;
				}
			}

			return per;
		}

		protected override bool keyIsValid(string key = null)
		{
			if (permutationIsValid)
				return true;
			return calPermutation();
		}

		public Substitution(string plain = null, string cipher = null, string key = null) : base(plain, cipher, key)
		{
			this.Type = SchemeType.Substitution;
			permutation = new int[Scheme.LetterSetSize];
			if (key == null)
			{
				this.Key = key = this.GenerateKey();
			}
			calPermutation();
		}

		~Substitution()
		{

		}

		public override string Key
		{
			get
			{
				return key;
			}
			set
			{
				if (value.Split(',').Length == Scheme.LetterSetSize)
				{
					value = value.ToLower();
					bool[] isMarked = new bool[Scheme.LetterSetSize];
					foreach (char c in value)
					{
						if (c == ',')
							continue;

						if (isMarked[c - 'a'])
						{
							return;
						}
						isMarked[c - 'a'] = true;
					}
					key = value;
				}
			}
		}

		public override (string, bool) Encode(string plain = null, string encodeKey = null)
		{
			if (plain != null)
			{
				this.Plain = plain;
			}
			if (encodeKey != null)
			{
				this.key = encodeKey;
			}

			if (keyIsValid())
			{
				StringBuilder cipher = new StringBuilder();
				for (int i = 0; i < this.Plain.Length; i++)
				{
					if (char.IsLetter(this.Plain[i]))
					{
						char bottom = char.IsLower(this.Plain[i]) ? 'a' : 'A';
						int index = this.Plain[i] - bottom;
						char subLetter = (char)(bottom + permutation[index]);
						cipher.Append(subLetter);
					}
				}
				this.Cipher = cipher.ToString();
				return (cipher.ToString(), true);
			}

			return ("", false);
		}

		public override (string, bool) Decode(string cipher = null, string decodeKey = null)
		{
			if (cipher != null)
			{
				this.Cipher = cipher;
			}

			if (keyIsValid())
			{
				StringBuilder plain = new StringBuilder();
				int[] reverseKey = new int[Scheme.LetterSetSize];
				for (int i = 0; i < Scheme.LetterSetSize; i++)
				{
					reverseKey[this.permutation[i]] = i;
				}

				for (int i = 0; i < this.Cipher.Length; i++)
				{
					if (char.IsLetter(this.Cipher[i]))
					{
						char bottom = char.IsLower(this.Cipher[i]) ? 'a' : 'A';
						int index = this.Cipher[i] - bottom;
						char subLetter = (char)(bottom + reverseKey[index]);
						plain.Append(subLetter);
					}
				}
				this.Plain = plain.ToString();
				return (plain.ToString(), true);
			}

			return ("", false);
		}

		public override (string, double) Break(string cipher = null)
		{
			if (cipher == null)
			{
				cipher = this.Cipher;
			}
			else
			{
				this.Cipher = cipher;
			}

			IGA iga = new IGA(this);
			return iga.Break(cipher);
		}

		public override bool Save(string fileName)
		{
			throw new NotImplementedException();
		}
		public override bool Load(string fileName)
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			throw new NotImplementedException();
		}

		public override string GenerateKey()
		{
			char[] key = new char[Scheme.LetterSetSize];

			for (int i = 0; i < Scheme.LetterSetSize; i++)
				key[i] = (char)(i + 'a');

			Random rand = new Random();

			for (int i = 0; i < Scheme.LetterSetSize; i++)
			{
				int j = rand.Next(Scheme.LetterSetSize);
				(key[i], key[j]) = (key[j], key[i]);
			}

			return string.Join(',', key);
		}
	}
}
