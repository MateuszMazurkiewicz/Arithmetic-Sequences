using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class AdvancedPlayer : IPlayer
    {
        public short Id { get; set; }
        public string Name { get; set; } = "Advanced Player";

        public double Alpha = 0;//0.5;

        public bool Aggresive = false;

        public List<int> PlayerNumbers { get; set; } = new List<int>();

        private Random random = new Random(Guid.NewGuid().GetHashCode());

        public void Play(Game game)
        {
            
            int selected = -1;
            if (game.Sequences.Exists(x => x.Progress == game.SequenceLength - 1 && x.Owner == this.Id && !x.Blocked))
            {
                var numbers = game.Sequences.Where(x => x.Progress == game.SequenceLength - 1 && x.Owner == this.Id && !x.Blocked).First().Numbers;

                foreach (var num in numbers)
                {
                    if (game.GeneratedNumbers[num].Owner == 0)
                    {
                        selected = num;
                        break;
                    }
                }
            }
            else
            {

                int max = game.Sequences.Max(x => x.Progress);

                bool longestSequenceOwner = game.Sequences.Exists(x => x.Progress == max && x.Owner == this.Id);

                bool shouldBlock = game.Sequences.Any(x => x.Progress > (int)(game.SequenceLength * Alpha) && x.Owner != this.Id && !x.Blocked);

                if (Aggresive)
                {
                    shouldBlock &= !longestSequenceOwner;
                }

                if (!shouldBlock) //must block
                {
                    var toSelect = game.Sequences.Where(x => (x.Owner == this.Id || x.Owner == 0) && !x.Blocked);
                    if (toSelect.Any())
                    {
                        selected = SelectNumber(game, toSelect);
                    }

                    // from enemies
                    // select sequences with biggest progress
                    // select sequence with biggest intersectionCount
                    // select number with biggest number of sequences
                }
                if (shouldBlock || selected == -1)
                {
                    var toBlock = game.Sequences.Where(x => x.Owner != this.Id && x.Owner != 0 && !x.Blocked);
                    selected = SelectNumber(game, toBlock);
                    // from mine or available
                    // select sequences with biggest progress
                    // select sequence with biggest intersectionCount
                    // select number with biggest number of sequences
                }
            }
            this.PlayerNumbers.Add(selected);
            game.GeneratedNumbers[selected].Owner = this.Id;
            Sequence.UpdateSequences(game, selected, this.Id);
        }

        private int SelectNumber(Game game, IEnumerable<Sequence> sequences)
        {
            int maxProgress = sequences.Max(x => x.Progress);
            sequences = sequences.Where(x => x.Progress == maxProgress);
            int maxIntersectioCount = sequences.Max(x => x.IntersectionsCount);
            var tmpSequence = sequences.Where(x => x.IntersectionsCount == maxIntersectioCount);
            var sequence = tmpSequence.ElementAt(random.Next(tmpSequence.Count()));

            int tmpNumber = -1;
            int tmpSequencesCount = int.MinValue;

            foreach (var number in sequence.Numbers)
            {

                var tmp = game.GeneratedNumbers[number];
                if (tmp.Sequences.Count > tmpSequencesCount && tmp.Owner == 0)
                {
                    tmpNumber = number;
                    tmpSequencesCount = game.GeneratedNumbers[number].Sequences.Count;
                }
            }

            if (tmpNumber == -1)
            {
                throw new Exception("Selected bad number!");
            }

            return tmpNumber;

        }
    }
}
