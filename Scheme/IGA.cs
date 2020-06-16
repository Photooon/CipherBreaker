using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation.Text;

namespace CipherBreaker
{
	class IGA
	{
		protected class Individual : IEquatable<Individual>,IComparable
		{
			public int[] key;
			public double fitness;

			public Individual(int[] key,IGA iga)
			{
				this.key = key;
				this.fitness = FrequencyAnalyst.Analyze(iga.Decode(key));
			}

			public int CompareTo(object obj)
			{
				return fitness.CompareTo((obj as Individual).fitness);
			}

			public override bool Equals(object obj)
			{
				return this.Equals(obj as Individual);
			}

			public bool Equals(Individual other)
			{
				return other != null &&
					   EqualityComparer<int[]>.Default.Equals(this.key, other.key);
			}

			public static bool operator ==(Individual left, Individual right)
			{
				return EqualityComparer<Individual>.Default.Equals(left, right);
			}

			public static bool operator !=(Individual left, Individual right)
			{
				return !(left == right);
			}
		}


		public const double Alpha = 0.7;
		public const int Cap = 200;
		public const int InitNum = 100000;
		public const double MutationProb = 0.4;

		private double minFitness = double.MinValue;

		private Substitution sub;

		public IGA(Substitution sub)
		{
			this.sub = new Substitution(sub.Plain, sub.Cipher, sub.Key);
		}

		// 计算相似度
		private int CalSimilarity(int[] key1, int[] key2)
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

		// 计算浓度
		private double[] CalConcentration(List<Individual> ids)
		{
			double[] probs = new double[ids.Count];
			probs[0] = 0.0;
			for (int i = 1; i < Scheme.LetterSetSize; i++)
			{
				probs[i] = CalSimilarity(ids[i - 1].key, ids[i].key) / (double)Scheme.LetterSetSize;
			}
			return probs;
		}

		private List<Individual> CalMutation(Individual id)
		{
			int[] tmp = id.key.Clone() as int[];

			double fatherFitness = FrequencyAnalyst.Analyze(Decode(tmp));

			List<Individual> kids = new List<Individual>();

			Random rand = new Random();
			for (int i = 0; i < Scheme.LetterSetSize; i++)
			{
				for (int j = 0; j < i; j++)
				{
					(tmp[i], tmp[j]) = (tmp[j], tmp[i]);
					double r = rand.NextDouble();
					double newFitness = FrequencyAnalyst.Analyze(Decode(tmp));

					if (newFitness > fatherFitness)
					{
						kids.Add(new Individual(tmp.Clone() as int[],this));
					}
					else if (r <= MutationProb)
					{
						kids.Add(new Individual(tmp.Clone() as int[], this));
					}

					(tmp[i], tmp[j]) = (tmp[j], tmp[i]);
				}
			}

			return kids;
		}

		public (string, double) Break(string cipher)
		{
			List<Individual> group = new List<Individual>();
			Substitution sub = new Substitution();
			for (int i = 0; i < InitNum; i++)
			{
				group.Add(new Individual(CommonData.String2intArray(sub.GenerateKey()),this));
			}

			int cnt = 0, T = 0;
			double last = double.MinValue;

			while (cnt < 5 && T < 10)
			{
				T++;

				// 去重
				HashSet<Individual> set = new HashSet<Individual>();
				foreach (var id in group)
				{
					set.Add(id);
				}
				group.Clear();
				foreach (var id in set)
				{
					group.Add(id);
				}
				set.Clear();

				// 按照适应度降序排序
				group.Sort();
				group.Reverse();

				// 控制个体数量
				if(group.Count>Cap)
				{
					group = group.GetRange(0, Cap);
				}

				minFitness = group.Last().fitness;

				// 最大适应度是否与上次相同，
				if(group[0].fitness != last)
				{
					last = group[0].fitness;
					cnt = 1;
				}

				// 计算浓度与基于适应度排名的线性概率
				var concentrations = CalConcentration(group);
				double[] fitnessProbs = new double[group.Count];
				for(int i = 0;i<fitnessProbs.Length;i++)
				{
					fitnessProbs[i] = (double)(group.Count - i) / group.Count;
				}

				Random rand = new Random();
				List<Individual> newGroup = new List<Individual>();
				for(int i = 0;i<group.Count;i++)
				{
					// 选择
					if(rand.NextDouble()<=Alpha*fitnessProbs[i]+(1-Alpha)*concentrations[i])
					{
						var kids = CalMutation(group[i]);
						newGroup.AddRange(kids);
					}
				}

				group.AddRange(newGroup);
			}

			group.Sort();
			group.Reverse();

			return (Decode(group[0].key), group[0].fitness);
		}

		private string Decode(int[] key)
		{
			(var plain, _) = sub.Decode(decodeKey: string.Join(',', key));
			return plain;
		}
	}
}
