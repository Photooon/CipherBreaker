using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CipherBreaker
{
	class Context
	{
		private Scheme scheme;
		public double BestProb;
		public string BestKey;
		private Dictionary<string, object> Ctx;

		Context(Scheme scheme)
		{
			this.scheme = scheme;
			this.BestKey = "";
			this.BestProb = 0.0;
			this.Ctx = new Dictionary<string, object>();
		}
		
		public object Get (string key)
		{
			return Ctx[key];
		}
		public void Set (string key,object value)
		{
			Ctx[key] = value;
		}
		
	}
}
