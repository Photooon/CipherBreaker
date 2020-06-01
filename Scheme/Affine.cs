using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherBreaker
{
	class Affine : SymmetricScheme
	{
		protected override bool keyIsValid(string key = null)
		{
			if (key == null)
			{
				key = this.Key;
			}

			string[] ab = key.Split(',');   //"a,b"，根据逗号分割字符串key，第一个数为乘数，第二个数为加数0
			int aInt = int.Parse(ab[0]);
			return NumberTheory.Gcd(aInt, Scheme.LetterSetSize) == 1;
		}

		public Affine(string plain = null, string cipher = null, string key = null) : base(plain, cipher, key)
		{
			Type = SchemeType.Affine;
		}

		~Affine()
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
				key = value;
			}
		}

		public override (string, bool) Encode(string plain = null, string key = null)
		{
			if (plain == null)
			{
				plain = Plain;
			}
			if (key == null)
			{
				key = Key;
			}
			if (!keyIsValid(key))
			{
				return (null, false);
			}

			string[] ab = key.Split(',');   //"a,b"，根据逗号分割字符串key，第一个数为乘数，第二个数为加数0
			int aInt = int.Parse(ab[0]);
			int bInt = int.Parse(ab[1]);
			string cipher = "";  
			foreach (char p in plain)
			{
				int c = p;

				if (p >= 'a' && p <= 'z')
				{
					c = (((p - 'a') * aInt + bInt) % Scheme.LetterSetSize) + 'a';
				}
				else if (p >= 'A' && p <= 'Z')
				{
					c = (((p - 'A') * aInt + bInt) % Scheme.LetterSetSize) + 'A';
				}
				cipher += Convert.ToChar(c);
			}
			this.Key = key;
			this.Cipher = cipher;
			this.Plain = plain;

			return (cipher, true);
		}

		public override (string, bool) Decode(string cipher = null, string key = null)
		{
			if (cipher == null)
			{
				cipher = Cipher;
			}
			if (key == null)
			{
				key = Key;
			}

			if (!keyIsValid(key))
			{
				return (null, false);
			}

			string[] ab = key.Split(',');   //"a,b"，根据逗号分割字符串key，第一个数为乘数，第二个数为加数0
			int aInt = int.Parse(ab[0]);
			int bInt = int.Parse(ab[1]);
			string plain = "";
			foreach (char c in cipher)
			{
				int p = c;
				if (c >= 'a' && c <= 'z')
				{
					if (NumberTheory.Gcd(aInt, Scheme.LetterSetSize) == 1)  //只有当 a 与 n 互素的时候, a 才是有模逆的
					{
						int cInt = (int)NumberTheory.Inverse(aInt, Scheme.LetterSetSize);
						p = (((c - 'a' - bInt) * cInt) % Scheme.LetterSetSize) + 'a';
					}
				}
				else if (c >= 'A' && c <= 'Z')
				{
					if (NumberTheory.Gcd(aInt, Scheme.LetterSetSize) == 1)  //只有当 a 与 n 互素的时候, a 才是有模逆的
					{
						int cInt = (int)NumberTheory.Inverse(aInt, Scheme.LetterSetSize);
						p = (((c - 'A' - bInt) * cInt) % Scheme.LetterSetSize) + 'A';
					}
				}
				plain += Convert.ToChar(p);
			}

			this.Key = key;
			this.Plain = plain;
			this.Cipher = cipher;

			return (plain, true);
		}

		public override (string, double) Break(string cipher = null)
		{
			if (cipher == null)
			{
				cipher = this.Cipher;
			}

			string plain = cipher;
			double maxProb = FrequencyAnalyst.Analyze(cipher);

			for (int i = 0; i < LetterSetSize; i++)
			{
				if (NumberTheory.Gcd(i, LetterSetSize) == 1)
				{
					for (int j = 0; j < LetterSetSize; j++)
					{
						string a = i.ToString();
						string b = j.ToString();
						string ab = a + "," + b;
						(string str, bool ok) result = Decode(cipher, ab);
						if (result.ok)
						{
							double prob = FrequencyAnalyst.Analyze(result.str);
							if (prob > maxProb)
							{
								plain = result.str;
								maxProb = prob;
							}
						}
					}
				}
			}

			this.Plain = plain;
			return (plain, Math.Pow(Math.E, maxProb));
			//throw new NotImplementedException();
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
			throw new NotImplementedException();
		}
	}
}
