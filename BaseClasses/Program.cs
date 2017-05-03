using System;

// All of below is provided by Monogame Framework
namespace Anti_Latency
{
#if WINDOWS || LINUX

    /// The main class
    public static class Program
    {
        [STAThread]
        /// The main entry point for the application
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
#endif
}
