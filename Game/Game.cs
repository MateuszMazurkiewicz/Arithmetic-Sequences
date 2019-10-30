using System;
using System.Collections.Generic;
using System.Linq;


namespace Game
{
    public class Game
    {
        public int Lower;

        public int Upper;

        public int SetCount;

        public int SequenceLength;

        public Dictionary<int, Number> GeneratedNumbers;        

        public List<Sequence> Sequences = new List<Sequence>();

        public bool GameEnded
        {
            get
            {
                return this.Sequences.Where(x => !x.Blocked).Count() == 0;
            }
        }

        public bool Initialize()
        {
            var tmpList = this.GenerateSet(this.Lower, this.Upper, this.SetCount);
            tmpList.Sort();
            this.GeneratedNumbers = this.IntToNumber(tmpList);

            var sequences = ArithmeticSequences.ArithmeticSequences.Find(tmpList, this.SequenceLength);

            if (sequences.Count == 0)
            {
                return false;
            }

                foreach (var sequence in sequences)
                {
                    var s = new Sequence(sequence);
                    this.Sequences.Add(s);
                    foreach (var number in sequence)
                    {
                        this.GeneratedNumbers[number].Sequences.Add(s);
                    }
                }

            foreach (var sequence in this.Sequences)
            {
                List<Sequence> tmpSequence = new List<Sequence>();
                foreach (var number in sequence.Numbers)
                {
                    tmpSequence.AddRange(this.GeneratedNumbers[number].Sequences);
                }

                sequence.IntersectionsCount = (tmpSequence.Distinct()).Count();
            }

            return true;
        }

        public List<int> GenerateSet(int lower, int upper, int setCount)
        {
            List<int> generatedSet = new List<int>();

            List<int> helpList = new List<int>();

            for (int i = lower; i <= upper; i++)
            {
                helpList.Add(i);
            }

            Random random = new Random(Guid.NewGuid().GetHashCode());

            for (int i = 0; i < setCount; i++)
            {
                int selected = random.Next(0, helpList.Count);

                generatedSet.Add(helpList[selected]);
                helpList.RemoveAt(selected);
            }

            return generatedSet;
        }

        public Dictionary<int, Number> IntToNumber(List<int> intList)
        {
            Dictionary<int, Number> numbers = new Dictionary<int, Number>();
            foreach (var i in intList)
            {
                numbers.Add(i, new Number() { Value = i });
            }

            return numbers;
        }

        public bool Check(List<int> numbers)
        {
            return ArithmeticSequences.ArithmeticSequences.Find(numbers, this.SequenceLength).Count > 0;
        }
    }
}
