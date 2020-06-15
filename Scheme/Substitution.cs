using System;
using System.Collections.Generic;
using System.Text;

namespace CipherBreaker
{
    class Substitution : SymmetricScheme
    {
        private int[] permutation;
        private bool permutationIsValid;
        private bool calPermutation()
        {
            int cur = 0, pos = 0;
            for (int i = 0; i < this.key.Length; i++)
            {
                if (this.key[i] == ',')
                {
                    if (pos == Scheme.LetterSetSize)
                    {
                        return false;
                    }
                    permutation[pos++] = cur;
                    cur = 0;
                }
                else
                {
                    cur = 10 * cur + this.key[i] - '0';
                }
            }
            permutation[pos++] = cur;

            return permutationIsValid = (pos == Scheme.LetterSetSize);
        }
        protected override bool keyIsValid(string key = null)
        {
            if (permutationIsValid)
                return true;
            return calPermutation();
        }

        public Substitution(string plain = null, string cipher = null, string key = null) : base(plain, cipher, key)
        {
            this.Type = SchemeType.Substitution;
            permutation = new int[Scheme.LetterSetSize];
            calPermutation();
        }

        ~Substitution()
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
                if (value.Length == Scheme.LetterSetSize)
                {
                    value = value.ToLower();
                    bool[] isMarked = new bool[Scheme.LetterSetSize];
                    foreach (char c in value)
                    {
                        if (isMarked[c - 'a'])
                        {
                            return;
                        }
                        isMarked[c - 'a'] = true;
                    }
                }
            }
        }

        public override (string, bool) Encode(string plain = null, string encodeKey = null)
        {
            if (plain != null)
            {
                this.Plain = plain;
            }
            if (encodeKey != null)
            {
                this.key = encodeKey;
            }

            if (keyIsValid())
            {
                StringBuilder cipher = new StringBuilder();
                for (int i = 0; i < this.Plain.Length; i++)
                {
                    if (char.IsLetter(this.Plain[i]))
                    {
                        char bottom = char.IsLower(this.Plain[i]) ? 'a' : 'A';
                        int index = this.Plain[i] - bottom;
                        char subLetter = (char)(bottom + permutation[index]);
                        cipher.Append(subLetter);
                    }
                }
                this.Cipher = cipher.ToString();
                return (cipher.ToString(),true);
            }

            return ("",false);
        }

        public override (string, bool) Decode(string cipher = null, string decodeKey = null)
        {
            if (cipher != null)
            {
                this.Cipher = cipher;
            }

            if (keyIsValid())
            {
                StringBuilder plain = new StringBuilder();
                int[] reverseKey = new int[Scheme.LetterSetSize];
                for (int i = 0; i < Scheme.LetterSetSize; i++)
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
                return (plain.ToString(),true);
            }

            return ("",false);
        }

        public override (string, double) Break(string cipher = null)
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

        public override string GenerateKey()
        {
            throw new NotImplementedException();
        }
    }
}
