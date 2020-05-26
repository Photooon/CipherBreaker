using System;
using System.Collections.Generic;
using System.Text;

namespace CipherBreaker
{
	class NumberTheory
	{
		// 快速幂
		public static long Pow(long a, long b, long n)
		{
			a = Math.Abs(a);
			b = Math.Abs(b);
			long ans = 1;
			while (b > 0)
			{
				if ((b & 1) != 0)
				{
					ans *= a;
					ans %= n;
				}
				a *= a;
				a %= n;

				b >>= 1;
			}
			return ans;
		}

		// 最大公约数
		public static long Gcd(long a, long b)
		{
			a = Math.Abs(a);
			b = Math.Abs(b);
			while (b > 0)
			{
				a %= b;
				(a, b) = (b, a);
			}
			return a;
		}

		// 欧拉函数
		public static long Phi(long n)
		{
			if (n <= 0)
			{
				return 0;
			}

			long phi = n;
			for (long i = 2; i * i <= n; i++)
			{
				if (n % i == 0)
				{
					phi /= i;
					phi *= (i - 1);

					while (n % i == 0)
						n /= i;
				}
			}

			if (n > 1)
			{
				phi /= n;
				phi *= (n - 1);
			}

			return phi;
		}

		// 不存在逆元时返回0
		// 用于一般情况，若n为素数使用InversePrime()可获得最佳性能
		public static long Inverse(long a, long n)
		{
			if (n <= 0)
			{
				return 0;
			}

			a %= n;
			if (Gcd(a, n) > 1)
			{
				return 0;
			}

			return Pow(a, Phi(n) - 1, n);
		}

		// 针对素数模数的逆元
		public static long InversePrime(long a, long p)
		{
			if (p <= 0)
			{
				return 0;
			}

			a %= p;
			if (a == 0)
			{
				return 0;
			}

			return Pow(a, p - 2, p);
		}
	}
}
