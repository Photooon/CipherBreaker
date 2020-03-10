using System;
using System.Collections.Generic;
using System.Text;

namespace CipherBreaker
{
    class Substitution : Scheme
    {
        private int[] permutation;
        private bool permutationIsValid;
        private bool calPermutation()
        {
            int cur = 0, pos = 0;
            for(int i = 0;i<this.EncodeKey.Length;i++)
            {
                if(this.EncodeKey[i]==',')
                {
                    if(pos==Scheme.LetterSetSize)
                    {
                        return false;
                    }
                    permutation[pos++] = cur;
                    cur = 0;
                }
                else
                {
                    cur = 10 * cur + this.EncodeKey[i] - '0';
                }
            }
            permutation[pos++] = cur;

            return permutationIsValid = (pos == Scheme.LetterSetSize);
        }
        protected override bool keyIsValid(string key)
        {
            if (permutationIsValid)
                return true;
            return calPermutation();
        }

        public Substitution(string plain = null, string cipher = null, string key = null):base(plain,cipher,key,key)
        {
            if(this.EncodeKey != this.DecodeKey)
            {
                if (this.EncodeKey == null)
                    this.EncodeKey = this.DecodeKey;
                else if (this.DecodeKey == null)
                    this.DecodeKey = this.EncodeKey;
            }

            this.Type = SchemeType.Substitution;
            permutation = new int[Scheme.LetterSetSize];
            calPermutation();
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
                        char subLetter = (char)(bottom+permutation[index]);
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
                int[] reverseKey = new int[Scheme.LetterSetSize];
                for(int i = 0;i<Scheme.LetterSetSize;i++)
                {
                    reverseKey[this.permutation[i]] = i;
                }

                for (int i = 0; i < this.Cipher.Length; i++)
                {
                    if (char.IsLetter(this.Cipher[i]))
                    {
                        char bottom = char.IsLower(this.Cipher[i]) ? 'a' : 'A';
                        int index = this.Cipher[i] - bottom;
                        char subLetter = (char)(bottom + reverseKey[index]);
                        plain.Append(subLetter);
                    }
                }
                this.Plain = plain.ToString();
                return true;
            }

            return false;
        }

        public override bool Break(string cipher = null)
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
