using System;
using System.Collections.Generic;
using System.Text;

namespace CipherBreaker
{
	class Caesar : Scheme
	{
		public Caesar()
		{
			// Do nothing
		}

		~Caesar()
		{
			// Do nothing
		}
		public override bool Encode(string plain = "", string encodeKey = "")
		{
			throw new NotImplementedException();
		}
		public override bool Decode(string cipher = "", string decodeKey = "")
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
	}
}
