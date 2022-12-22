using System;
using JSW2048;
namespace JSW2048 // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameManager gm = new GameManager(new Random());
            Grid currentGrid = new Grid();
            gm.InitializeGrid(currentGrid);
            while (true)
            {
                //show
                Console.WriteLine(currentGrid.score);
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        Console.Write($"{((currentGrid[y, x]?.value) ?? 0),-5}");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                var ch = Console.ReadKey(false).Key;
                var direction = Direction.UP;
                switch (ch)
                {
                    case ConsoleKey.Escape:
                        return;
                    case ConsoleKey.UpArrow:
                        direction=Direction.UP;
                        break;
                    case ConsoleKey.DownArrow:
                        direction = Direction.DOWN;
                        break;
                    case ConsoleKey.LeftArrow:
                        direction = Direction.LEFT;
                        break;
                    case ConsoleKey.RightArrow:
                        direction = Direction.RIGHT;
                        break;
                }
                var result=gm.RunTurn(currentGrid, direction);
                if (false==result.Item1)
                {
                    Console.WriteLine("Game Over!");
                    return;
                }
                currentGrid = result.Item2;
            }
        }
    }
}