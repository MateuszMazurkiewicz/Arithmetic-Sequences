using System.Collections.Generic;


namespace Game
{
    public class Number
    {
        public int Value;
        public short Owner = 0;
        public List<Sequence> Sequences = new List<Sequence>();
    }
}
