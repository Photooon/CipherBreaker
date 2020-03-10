using System;
using System.Collections.Generic;
using System.Text;

namespace CipherBreaker
{
    class Substitution : Scheme
    {
        protected override bool keyIsValid(string key)
        {
            if (key.Length != Scheme.LetterSetSize)
                return false;
            bool[] count = new bool[26];
            foreach(char ch in key)
            {
                int index = ch - '0';
                if (count[index])
                    return false;
                count[index] = true;
            }
            return true;
        }

        public Substitution(string plain = "", string cipher = "", string key = ""):base(plain,cipher,key,key)
        {
            this.Type = SchemeType.Substitution;
        }

        ~Substitution()
        {

        }

        public override string EncodeKey
        {
            get
            {
                return encodeKey;
            }
            set
            {
                decodeKey = encodeKey = value;
            }
        }
        public override string DecodeKey
        {
            get
            {
                return decodeKey;
            }
            set
            {
                decodeKey = encodeKey = value;
            }
        }

        public override bool Encode(string plain = null, string encodeKey = null)
        {
            if (plain != null)
            {
                this.Plain = plain;
            }
            if (encodeKey != null)
            {
                this.EncodeKey = encodeKey;
            }

            if (encodeKeyIsValid())
            {
                StringBuilder cipher = new StringBuilder();
                for(int i = 0;i<this.Plain.Length;i++)
                {
                    if(char.IsLetter(this.Plain[i]))
                    {
                        char bottom = char.IsLower(this.Plain[i]) ? 'a' : 'A';
                        int index = this.Plain[i] - bottom;
                        char subLetter = (char)(this.EncodeKey[index] - '0' + bottom);
                        cipher.Append(subLetter);
                    }
                }
                this.Cipher = cipher.ToString();
                return true;
            }

            return false;
        }

        public override bool Decode(string cipher = null, string decodeKey = null)
        {
            if (cipher != null)
            {
                this.Cipher = cipher;
            }
            if (decodeKey != null)
            {
                this.DecodeKey = decodeKey;
            }

            if (decodeKeyIsValid())
            {
                StringBuilder plain = new StringBuilder();
                char[] reverseKey = new char[26];
                for(int i = 0;i<Scheme.LetterSetSize;i++)
                {
                    reverseKey[this.DecodeKey[i] - '0'] = (char)(i + '0');
                }

                for (int i = 0; i < this.Cipher.Length; i++)
                {
                    if (char.IsLetter(this.Cipher[i]))
                    {
                        char bottom = char.IsLower(this.Cipher[i]) ? 'a' : 'A';
                        int index = this.Cipher[i] - bottom;
                        char subLetter = (char)(this.DecodeKey[index] - '0' + bottom);
                        plain.Append(subLetter);
                    }
                }
                this.Plain = plain.ToString();
                return true;
            }

            return false;
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
