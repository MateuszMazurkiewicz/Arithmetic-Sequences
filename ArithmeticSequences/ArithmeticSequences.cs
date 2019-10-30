using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticSequences
{
    public static class ArithmeticSequences
    {
        public static List<List<int>> Find(List<int> numbers, int k)
        {
            //Array.Sort(numbers);

            numbers.Sort();

            List<List<int>> list = new List<List<int>>();
            Dictionary<int, Dictionary<int, int>> dictionary = new Dictionary<int, Dictionary<int, int>>();

            int diff;

            for (int i = 0; i < numbers.Count - 1; i++)
            {
                for (int j = i + 1; j < numbers.Count; j++)
                {
                    diff = numbers[j] - numbers[i];
                    if (dictionary.ContainsKey(diff))
                    {
                        dictionary[diff].Add(numbers[i], numbers[j]);
                    }
                    else
                    {
                        dictionary.Add(diff, new Dictionary<int, int>());
                        dictionary[diff].Add(numbers[i], numbers[j]);
                    }
                }
            }

            List<int> keysToRemove = new List<int>();

            foreach (var dict in dictionary)
            {
                if (dict.Value.Count() < k - 1)
                {
                    keysToRemove.Add(dict.Key);
                }
            }

            foreach (var key in keysToRemove)
            {
                dictionary.Remove(key);
            }

            int[] tmpSequnce = new int[k];

            foreach (var dict in dictionary)
            {
                foreach (var pair in dict.Value)
                {
                    int counter;
                    int last;

                    tmpSequnce[0] = pair.Key;
                    tmpSequnce[1] = last = pair.Value;

                    for (counter = 2; counter < k; counter++)
                    {
                        if (dict.Value.TryGetValue(last, out last))
                        {
                            tmpSequnce[counter] = last;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if(counter == k)
                    {
                        list.Add(tmpSequnce.ToList());
                    }
                }
            }

            return list;
        }
    }
}
