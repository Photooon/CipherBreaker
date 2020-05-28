using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Markup;

namespace CipherBreaker
{
	class Context
	{
		private Scheme scheme;
		public double BestProb;
		public string BestKey;
		private Dictionary<string, object> ctx;

		Context(Scheme scheme)
		{
			this.scheme = scheme;
		}

		public object Get(string key)
		{
			return ctx.GetValueOrDefault(key);
		}

		public void Set(string key,object value)
		{
			ctx[key] = value;
		}
	}

	//class ContextJsonConvert : JsonConverter<Context>
	//{
	//	public override Context Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	//	{
			
	//	}

	//	public override void Write(Utf8JsonWriter writer, Context value, JsonSerializerOptions options)
	//	{
	//		writer.WriteString("");
	//	}
	//}
}
