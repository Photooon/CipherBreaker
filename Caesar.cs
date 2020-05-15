using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherBreaker
{
	class Caesar : SymmetricScheme
	{

		protected override bool keyIsValid(string key = null)
		{
			if (key == null)
			{
				key = this.Key;
			}

			int keyInt;
			return int.TryParse(key, out keyInt);
		}

		public Caesar(string plain = null, string cipher = null, string key = null) : base(plain, cipher, key)
		{

		}

		~Caesar()
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
				int result;
				if (int.TryParse(value, out result))
				{
					result = (result % 26 + 26) % 26;
					key = $"{result}";
				}
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
			int keyInt = int.Parse(key);
			string cipher = "";
			foreach (char p in plain)
			{
				int c = p;
				if (p >= 'a' && p <= 'z')
				{
					c = p + keyInt;
					if (c > 'z')
					{
						c -= Scheme.LetterSetSize;
					}
				}
				else if(p>='A'&&p<='Z')
				{
					c = p + keyInt;
					if (c > 'Z')
					{
						c -= Scheme.LetterSetSize;
					}
				}
				cipher.Append(Convert.ToChar(c));
			}
			this.Key = key;
			this.Cipher = cipher;
			this.Plain = plain;

			return (cipher, true);
			throw new NotImplementedException();
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

			int keyInt = int.Parse(key);

			string plain = "";
			foreach (char c in cipher)
			{
				int p = c;
				if (c >= 'a' && c <= 'z')
				{
					p = c - keyInt;
					if (p < 'a')
					{
						p += Scheme.LetterSetSize;
					}
				}
				else if (c >= 'A' && c <= 'Z')
				{
					p = c - keyInt;
					if (p < 'A')
					{
						p += Scheme.LetterSetSize;
					}
				}
				plain.Append(Convert.ToChar(p));
			}

			this.Key = key;
			this.Plain = plain;
			this.Cipher = cipher;

			return (plain, true);
		}
		public override (string, bool) Break(string cipher = null)
		{
			throw new NotImplementedException();
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
	}
}
