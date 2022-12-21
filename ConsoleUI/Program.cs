using System;
using JSW2048;
namespace JSW2048 // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameManager gm = new GameManager();
            Grid currentGrid = gm.grid;
            Grid nextGrid = gm.grid;
            while (true)
            {
                //show
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        Console.Write($"{((currentGrid[y, x]?.value )?? 0):d}");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();

                var ch = Console.ReadKey(false).Key;
                switch (ch)
                {
                    case ConsoleKey.Escape:
                        return;
                    case ConsoleKey.UpArrow:
                        nextGrid = gm.MoveUp(currentGrid);
                        break;
                    case ConsoleKey.DownArrow:
                        nextGrid = gm.MoveDown(currentGrid);
                        break;
                    case ConsoleKey.LeftArrow:
                        nextGrid = gm.MoveLeft(currentGrid);
                        break;
                    case ConsoleKey.RightArrow:
                        nextGrid = gm.MoveRight(currentGrid);
                        break;
                }
                if (gm.IsGameOver(nextGrid))
                {
                    Console.WriteLine("Game Over!");
                    return;
                }
                else
                {
                    gm.SetRandomTile(nextGrid);
                }
                currentGrid = nextGrid;

            }
        }
    }
}