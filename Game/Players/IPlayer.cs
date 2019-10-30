using System.Collections.Generic;

namespace Game
{
    public interface IPlayer
    {
        short Id { get; set; }
        string Name { get; set; }

        List<int> PlayerNumbers { get; set; }

        void Play(Game game);
    }
}
