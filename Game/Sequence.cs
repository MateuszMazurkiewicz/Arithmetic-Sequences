using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class Sequence
    {
        public Sequence(List<int> numbers)
        {
            Numbers = numbers;
        }
        public List<int> Numbers = new List<int>();
        public int Progress = 0;
        public short Owner = 0;
        public int IntersectionsCount = 0;
        public bool Blocked = false;

        public static void UpdateSequences(Game game, int selected, short playerId)
        {
            var sequences = game.GeneratedNumbers[selected].Sequences;
            var blockedSequences = new List<Sequence>();

            foreach (var sequence in sequences)
            {
                if (sequence.Owner == 0)
                {
                    sequence.Owner = playerId;
                }
                if (sequence.Owner == playerId)
                {
                    sequence.Progress ++;
                }
                else
                {
                    //game.GeneratedNumbers[selected].Sequences.Remove(sequence);
                    sequence.Blocked = true;
                    blockedSequences.Add(sequence);
                }
            }

            sequences.RemoveAll(x => x.Blocked);

            var affectedSequences = new List<Sequence>();
            
            foreach(var blocked in blockedSequences)
            {
                foreach(var number in blocked.Numbers)
                {
                    affectedSequences.AddRange(game.GeneratedNumbers[number].Sequences);
                }
            }

            foreach(var affectedSequence in affectedSequences.Distinct())
            {
                affectedSequence.IntersectionsCount--;
            }
        }
    }
}
