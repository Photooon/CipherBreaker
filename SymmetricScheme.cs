using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CipherBreaker
{
    abstract class SymmetricScheme : Scheme
    {
        protected string key;
        public SymmetricScheme(string plain = null, string cipher = null, string key = null) : base(plain, cipher)
        {
            this.key = key;
        }
        public virtual string Key { get => key; set => key = value; }
        protected abstract bool keyIsValid(string key = null);
        public override abstract bool Break(string cipher = null);

        public override abstract bool Decode(string cipher = null, string decodeKey = null);

        public override abstract bool Encode(string plain = null, string encodeKey = null);

        public override abstract bool Load(string fileName);

        public override abstract bool Save(string fileName);

        public override abstract string ToString();
    }
}
