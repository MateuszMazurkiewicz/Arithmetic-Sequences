using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class RandomPlayer : IPlayer
    {
        private Random r = new Random(Guid.NewGuid().GetHashCode());
        public short Id { get; set; }

        public string Name { get; set; } = "Random Player";
        public List<int> PlayerNumbers { get; set; } = new List<int>();

        public void Play(Game game)
        {
            int selected = -1;
            if (game.Sequences.Exists(x => x.Progress == game.SequenceLength - 1 && x.Owner == this.Id && !x.Blocked))
            {
                var numbers =  game.Sequences.Where(x => x.Progress == game.SequenceLength - 1 && x.Owner == this.Id && !x.Blocked).First().Numbers;

                foreach(var num in numbers)
                {
                    if(game.GeneratedNumbers[num].Owner == 0)
                    {
                        selected = num;
                        break;
                    }
                }
            }
            else
            {
                var availableNumbers = game.GeneratedNumbers.Values.Where(x => x.Owner == 0).ToList();
                var tmp = r.Next(availableNumbers.Count);

                selected = availableNumbers[tmp].Value;
            }
            this.PlayerNumbers.Add(selected);
            game.GeneratedNumbers[selected].Owner = this.Id;
            Sequence.UpdateSequences(game, selected, this.Id);
        }
    }
}
