using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Players
{
    public enum PlayerType
    {
        Human,
        Random,
        Defensive,
        Aggressive,
    }
    public static class PlayerFactory
    {
        public static IPlayer GetPlayer(PlayerType type)
        {
            switch(type)
            {
                case PlayerType.Human:
                    {
                        return new HumanPlayer();
                    }
                case PlayerType.Random:
                    {
                        return new RandomPlayer();
                    }
                case PlayerType.Defensive:
                    {
                        return new AdvancedPlayer() { Name = "Defensive Advanced Player"};
                    }
                case PlayerType.Aggressive:
                    {
                        return new AdvancedPlayer() { Alpha = 0.5, Aggresive = true, Name = "Aggressive Advanced Player" };                        
                    }

                default:
                    return null;


            }
        }
    }
}
