using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;

namespace CipherBreaker
{
	class Substitution : SymmetricScheme
	{
		protected override bool keyIsValid(string key = null)
		{
			if (key == null)
				key = this.Key;

			bool[] isMarked = new bool[Scheme.LetterSetSize];
			foreach (char c in key)
			{
				if (isMarked[c - 'a'])
				{
					return false;
				}
				isMarked[c - 'a'] = true;
			}
			return true;
		}

		public Substitution(string plain = null, string cipher = null, string key = null) : base(plain, cipher, key)
		{
			this.Type = SchemeType.Substitution;
			if (key == null)
			{
				this.Key = key = this.GenerateKey();
			}
			//calPermutation();
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
				if (value.Length == Scheme.LetterSetSize)
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
						if(char.IsLower(this.Plain[i]))
						{
							cipher.Append(key[index]);
						}
						else
						{
							cipher.Append(char.ToUpper(key[index]));
						}
					}
					else
					{
						cipher.Append(this.Plain[i]);
					}
				}
				this.Cipher = cipher.ToString();
				return (this.Cipher, true);
			}

			return ("", false);
		}

		public override (string, bool) Decode(string cipher = null, string decodeKey = null)
		{
			if (cipher != null)
			{
				this.Cipher = cipher;
			}
			if(decodeKey!=null)
			{
				this.Key = decodeKey;
			}
			StringBuilder reverseKey = new StringBuilder(this.Key.Clone() as string);
			for(int i = 0;i<this.Key.Length;i++)
			{
				reverseKey[(int)(this.Key[i] - 'a')] = (char)('a' + i);
			}
			
			if (keyIsValid())
			{
				StringBuilder plain = new StringBuilder();

				for (int i = 0; i < this.Cipher.Length; i++)
				{
					if (char.IsLetter(this.Cipher[i]))
					{
						char bottom = char.IsLower(this.Cipher[i]) ? 'a' : 'A';
						int index = this.Cipher[i] - bottom;
						if (char.IsLower(this.Cipher[i]))
						{
							plain.Append(reverseKey[index]);
						}
						else
						{
							plain.Append(char.ToUpper(reverseKey[index]));
						}
					}
					else
					{
						plain.Append(this.Cipher[i]);
					}
				}
				this.Plain = plain.ToString();
				return (this.Plain, true);
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
			var result = iga.Break(cipher);
			this.Plain = result.Item1;
			return result;
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
			StringBuilder key = new StringBuilder();

			for (int i = 0; i < Scheme.LetterSetSize; i++)
				key.Append((char)(i + 'a'));

			Random rand = new Random();

			for (int i = 0; i < Scheme.LetterSetSize; i++)
			{
				int j = rand.Next(Scheme.LetterSetSize);
				(key[i], key[j]) = (key[j], key[i]);
			}

			return key.ToString();
		}
	}
}
