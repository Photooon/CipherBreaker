using System;
using System.Collections.Generic;
using System.Text;

namespace CipherBreaker
{
    class Substitution : Scheme
    {
        public Substitution(string plain = "", string cipher = "", string encodeKey = "", string decodeKey = "")
        {
            this.Type = SchemeType.Substitution;
            this.Plain = plain;
            this.Cipher = cipher;

        }
        public override string EncodeKey
        {
            get
            {
                return encodeKey;
            }
            set
            {
                if(value.Length != 26)
                {
                    throw new FormatException("长度不匹配");
                }

               foreach (var ch in value)
                {
                    if (!char.IsDigit(ch))
                    {
                        throw new FormatException("存在非数字");
                    }
                }

                encodeKey = value;
            }
        }
        public override string DecodeKey { get => base.DecodeKey; set => base.DecodeKey = value; }

        public override bool Decode(string cipher = "", string decodeKey = "")
        {
            throw new NotImplementedException();
        }

        public override bool Encode(string plain = "", string encodeKey = "")
        {
            throw new NotImplementedException();
        }

        public override bool Load(string fileName)
        {
            throw new NotImplementedException();
        }

        public override bool Save(string fileName)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
