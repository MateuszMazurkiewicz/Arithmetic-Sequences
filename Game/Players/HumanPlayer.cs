using System.Collections.Generic;

namespace Game
{
    public class HumanPlayer : IPlayer
    {
        public short Id { get; set; }

        public string Name { get; set; } = "Human Player";

        public List<int> PlayerNumbers { get; set; } = new List<int>();

        public delegate int GetNumber();

        public GetNumber GetValue;
        public void Play(Game game)
        {
            int selected = this.GetValue();
            this.PlayerNumbers.Add(selected);
            game.GeneratedNumbers[selected].Owner = this.Id;
            Sequence.UpdateSequences(game, selected, this.Id);
        }

        
    }
}
