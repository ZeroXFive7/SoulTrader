using System;

namespace SoulTrader
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SoulTrader game = new SoulTrader())
            {
                game.Run();
            }
        }
    }
#endif
}

