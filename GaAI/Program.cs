using System;
using JSW2048;
namespace JSW2048 // Note: actual namespace depends on the project name.
{

    public interface IDirectionProvider
    {
        public Direction GetDirection();
    }

    public class GaAI : IDirectionProvider
    {
        public Direction GetDirection()
        {
            throw new NotImplementedException();
        }
    }

    public class RandomAI: IDirectionProvider
    {
        Random random = new Random();   
        public Direction GetDirection()
        {
            double p=random.NextDouble();
            if (p < 0.25)
            {
                return Direction.UP;
            }
            else if (p < 0.5)
            {
                return Direction.LEFT;
            }
            else if (p < 0.75)
            {
                return Direction.DOWN;
            }
            else
            {
                return Direction.RIGHT;
            }
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            IDirectionProvider AI = new RandomAI();
            GameManager gm = new GameManager();
            Grid currentGrid = gm.grid;
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

                Direction direction = AI.GetDirection();
                var result = gm.RunTurn(currentGrid, direction);
                if (false == result.Item1)
                {
                    Console.WriteLine("Game Over!");
                    return;
                }
                currentGrid = result.Item2;

                //Thread.Sleep(1000);

            }
        }
    }
}