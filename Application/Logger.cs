using System;
using System.IO;

namespace Application
{
    public static class Logger
    {
        private static string path = ".";


        public static void Initialize()
        {
            path = Path.Combine(".", Guid.NewGuid().ToString() + ".csv");

            File.Create(path);
        }

        public static void Log(string winner, int turnsCount)
        {
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine(winner + "," + turnsCount);
            }
        }
    }
}
