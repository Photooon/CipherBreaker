using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherBreaker
{
	class Affine : SymmetricScheme
	{
		private bool gcdIsValid;	//标记
		//用辗转相除法求最大公约数  
		private int gcdCalculate(int x,int y)
		{
			while (x * y!=0)    //当其中一个为0时，终止循环
			{
				if (x > y)  //将较大数模较小数的结果（余数）赋给较大的值，直到两个数相等  
				{
					x %= y;
				}
				else if (x < y)	//==的情况不考虑，==则为最大公约数
				{
					y %= x;
				}
			}
			return x > y ? x : y;
		}


		protected override bool keyIsValid(string key = null)
		{
			if (key == null)
			{
				key = this.Key;
			}

			string[] ab = key.Split(',');   //"a,b"，根据逗号分割字符串key，第一个数为乘数，第二个数为加数0
			string aString = ab[0];
			int aInt = int.Parse(aString);
			if (gcdCalculate(aInt, Scheme.LetterSetSize) == 1)
			{
				gcdIsValid = true;
			}
			return gcdIsValid;
		}

		public Affine(string plain = null, string cipher = null, string key = null) : base(plain, cipher, key)
		{

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
			string aString = ab[0];
			string bString = ab[1];
			int aInt = int.Parse(aString);
			int bInt = int.Parse(bString);
			string cipher = "";
			foreach (char p in plain)
			{
				int c = p;

				if (p >= 'a' && p <= 'z')
				{
					c = (((p - 96) * aInt + bInt) % Scheme.LetterSetSize) + 96;
				}
				else if (p >= 'A' && p <= 'Z')
				{
					c = (((p - 64) * aInt + bInt) % Scheme.LetterSetSize) + 64;
				}
				cipher.Append(Convert.ToChar(c));
			}
			this.Key = key;
			this.Cipher = cipher;
			this.Plain = plain;

			return (cipher, true);
		}

		//求逆元
		private int modularInversion(int a,int b)
		{
			int inverse = 1;
			while ((a * inverse) % b != 1)
			{
				inverse += 1;
			}
			return inverse;
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
			string aString = ab[0];
			string bString = ab[1];
			int aInt = int.Parse(aString);
			int bInt = int.Parse(bString);
			//int keyInt = int.Parse(key);

			string plain = "";
			foreach (char c in cipher)
			{
				int p = c;
				if (c >= 'a' && c <= 'z')
				{
					if (gcdCalculate(aInt, Scheme.LetterSetSize) == 1)  //只有当 a 与 n 互素的时候, a 才是有模逆的
					{
						int cInt = modularInversion(aInt, Scheme.LetterSetSize);
						p = (((c - 96 - bInt) * cInt) % Scheme.LetterSetSize) + 96;
					}
				}
				else if (c >= 'A' && c <= 'Z')
				{
					if (gcdCalculate(aInt, Scheme.LetterSetSize) == 1)  //只有当 a 与 n 互素的时候, a 才是有模逆的
					{
						int cInt = modularInversion(aInt, Scheme.LetterSetSize);
						p = (((c - 64 - bInt) * cInt) % Scheme.LetterSetSize) + 64;
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
