using System;
using System.Collections.Generic;
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

        public override bool Encode(string plain = "", string encodeKey = "")
        {
            throw new NotImplementedException();
        }
        public override bool Decode(string cipher = "", string decodeKey = "")
        {
            throw new NotImplementedException();
        }
        public override bool Break(string cipher = "")
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
