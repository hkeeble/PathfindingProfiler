using System;


namespace Pathfinder
{
    static class Program
    {
        [STAThreadAttribute]
        static void Main(string[] args)
        {
            using (Main game = new Main())
            {
                game.Run();
            }
        }
    }
}

