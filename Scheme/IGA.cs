using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Automation.Text;

namespace CipherBreaker
{
	class IGA
	{
        public const double Alpha = 0.7;
        public const int Cap = 200;
        public const int InitNum = 100000;
        public const double MutationProb = 0.4;

        private Substitution sub;

        public IGA(Substitution sub)
        {
            this.sub = new Substitution(sub.Plain, sub.Cipher, sub.Key);
        }
        public int CalSimilarity(int[] key1, int[] key2)
        {
            int count = 0;
            for (int i = 0; i < Scheme.LetterSetSize; i++)
            {
                if (key1[i] == key2[i])
                {
                    count++;
                }
            }
            return count;
        }

        public double[] CalConcentration(int[][] keys)
        {
            double[] probs = new double[keys.Length];
            probs[0] = 0.0;
            for(int i = 1;i<Scheme.LetterSetSize;i++)
            {
                probs[i] = CalSimilarity(keys[i - 1], keys[i]) / (double)Scheme.LetterSetSize;
            }
            return probs;
        }

        public List<int[]> CalMutation(int[] key)
        {
            int[] tmp = key.Clone() as int[];

            double fatherFitness = FrequencyAnalyst.Analyze(Decode(tmp));

            List<int[]> kids = new List<int[]>();

            Random rand = new Random();
            for(int i = 0;i<Scheme.LetterSetSize;i++)
            {
                for(int j = 0;j<i;j++)
                {
                    (tmp[i], tmp[j]) = (tmp[j], tmp[i]);
                    double r = rand.NextDouble();
                    double newFitness = FrequencyAnalyst.Analyze(Decode(tmp));

                    if (newFitness > fatherFitness)
                    {
                        kids.Add(tmp.Clone() as int[]);
                    }
                    else if(r<=MutationProb)
                    {
                        kids.Add(tmp.Clone() as int[]);
                    }

                    (tmp[i], tmp[j]) = (tmp[j], tmp[i]);
                }
            }

            return kids;
        }

        public string Decode(int[] key)
        {
            (var plain, _) = sub.Decode(decodeKey: string.Join(',', key));
            return plain;
        }

    }
}
