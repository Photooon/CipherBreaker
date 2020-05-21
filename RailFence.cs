using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CipherBreaker
{
    class RailFence : SymmetricScheme
    {
        protected override bool keyIsValid(string key = null)
        {
            if (key == null)
            {
                key = this.key;
            }
            int keyInt;
            return int.TryParse(key, out keyInt);
        }

        
        public override (string, bool) Encode(string plain = null, string key = null)
        {
            if (plain == null)
            {
                plain = Plain;
            }
            if (key == null)
            {
                key = Key;
            }
            if (!keyIsValid(key))
            {
                return (null, false);
            }
            int keyInt = int.Parse(key);
            string cipher = "";
            string[] cipherKey = new string[keyInt];
            foreach (char p in plain) {
                for (int i = 0; i < plain.Length; i++)
                {
                    int j = i;
                    while (j >= keyInt) 
                    { 
                        j -= keyInt;
                    }
                        
                    cipherKey[j].Append(p);
                } }
            for(int k=0;k<keyInt;k++)
            {
                cipher = cipher + cipherKey[k];
            }
            this.Key = key;
            this.Cipher = cipher;
            this.Plain = plain;
            return (cipher, true);
        }
        
        public override (string, double) Break(string cipher = null)
        {
            throw new NotImplementedException();
        }

        public override (string, bool) Decode(string cipher = null, string key = null)
        {
            if (cipher == null)
            {
                cipher = Cipher;
            }
            if (key == null)
            {
                key = Key;
            }
            if (!keyIsValid(key))
            {
                return (null, false);
            }
            int keyInt = int.Parse(key);

            string plain = "";
            string[] plainKey = null;
            int i = cipher.Length / keyInt;
            int j = cipher.Length % keyInt;
            foreach (char c in cipher)
            { 
                for (int k = 0; k < j; k++)
                {
                    for (int m = 0; m <= i; m++)
                    {
                        plainKey[k].Append(c);
                    }
                }
                for (int k = j; k < keyInt; k++)
                {
                    for (int m = 0; m < i; m++)
                    {
                        plainKey[k].Append(c);
                    }
                }
            }
            for(int m = 0; m <= i; m++)
            {
               for(int k=0;k<keyInt;k++)
                {
                    plain = plain + plainKey[k].Substring(m, 1);
                }
            }
            this.Key = key;
            this.Plain = plain;
            this.Cipher = cipher;

            return (plain, true);
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

       public RailFence(string plain=null,string cipher=null,string key = null) : base(plain, cipher, key)
        {

        }
        ~RailFence()
        {

        } 
    }
}
