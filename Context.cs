using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;

namespace CipherBreaker
{
	class Context
	{
		private Scheme scheme;
		public double BestProb;
		public string BestKey;
		public Dictionary<string, object> ctx;

		Context(Scheme scheme)
		{
			this.scheme = scheme;
		}
	}
}
