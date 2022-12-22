using System;
using JSW2048;
namespace JSW2048 // Note: actual namespace depends on the project name.
{

    public interface IDirectionProvider
    {
        public Direction GetDirection(Grid grid);
    }

    public class GaAI : IDirectionProvider
    {
        public Random random = new Random(7893);
        public double[,] inputLayer = new double[16, 1];
        public double[,] hidenLayer1;
        public double[,] outputLayer;
        public double[,] SetRandomLayer(int row, int col)
        {
            double[,] layer = new double[row, col];
            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < col; x++)
                {
                    layer[y, x] = (random.Next(2) == 0 ? -1 : 1) * random.NextDouble();
                }
            }
            return layer;
        }

        public double[,] Relu(double[,] layer)
        {
            for (int y = 0; y < layer.GetLength(0); y++)
            {
                for (int x = 0; x < layer.GetLength(1); x++)
                {
                    layer[y, x] = layer[y, x] > 0 ? layer[y, x] : 0.0;
                }
            }
            return layer;
        }

        public double[,] SoftMax(double[,] layer)
        {
            double sumExp = 0.0;
            for (int i = 0; i < layer.GetLength(0); i++)
            {
                sumExp += Math.Exp(layer[i, 0]);
            }
            for (int i = 0; i < layer.GetLength(0); i++)
            {
                layer[i, 0] = Math.Exp(layer[i, 0]) / sumExp;
            }
            return layer;
        }

        public double[,] MultiplyMatrix(double[,] A, double[,] B)
        {
            int rA = A.GetLength(0);
            int cA = A.GetLength(1);
            int rB = B.GetLength(0);
            int cB = B.GetLength(1);

            if (cA != rB)
            {
                throw new Exception();
            }
            else
            {
                double temp = 0.0;
                double[,] result = new double[rA, cB];
                for (int i = 0; i < rA; i++)
                {
                    for (int j = 0; j < cB; j++)
                    {
                        temp = 0.0;
                        for (int k = 0; k < cA; k++)
                        {
                            temp += A[i, k] * B[k, j];
                        }
                        result[i, j] = temp;
                    }
                }
                return result;
            }
        }

        public Direction GetDirection(Grid grid)
        {
            if (hidenLayer1 == null)
            {
                hidenLayer1 = SetRandomLayer(8, 16);
            }
            if (outputLayer == null)
            {
                outputLayer = SetRandomLayer(4, 8);
            }
            //flaten
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    inputLayer[y * 4 + x, 0] = 0.0;
                    if (grid[y, x]?.value > 0)
                    {
                        inputLayer[y * 4 + x, 0] = Math.Log2(grid[y, x].value);
                    }

                }
            }
            //
            double[,] tmp;
            tmp = MultiplyMatrix(hidenLayer1, inputLayer);
            tmp = Relu(tmp);
            tmp = MultiplyMatrix(outputLayer, tmp);

            double[,] result = SoftMax(tmp);
            double maxTmp = 0.0;
            int maxDir = 0;
            for (int d = 0; d < 4; d++)
            {
                if (result[d, 0] > maxTmp)
                {
                    maxTmp = result[d, 0];
                    maxDir = d;
                }
            }
            switch (maxDir)
            {
                case 0:
                    return Direction.UP;
                    break;
                case 1:
                    return Direction.LEFT;
                    break;
                case 2:
                    return Direction.DOWN;
                    break;
                case 3:
                    return Direction.RIGHT;
                    break;
            }
            return Direction.UP;
        }
    }

    public class RandomAI : IDirectionProvider
    {
        Random random = new Random(7524);
        public Direction GetDirection(Grid grid)
        {
            double p = random.NextDouble();
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

    public class Benchmarker
    {
        Random random;
        public Benchmarker(Random random)
        {
            this.random = random;
        }

        public void Run(IDirectionProvider AI, int loopTimes = 30)
        {
            List<long> results = new List<long>();
            for (int i = 0; i < loopTimes; i++)
            {
                results.Add(Benchmark(AI));

            }
            Console.WriteLine(results.Average());
        }

        public long Benchmark(IDirectionProvider AI)
        {
            GameManager gm = new GameManager(random);
            Grid currentGrid = new Grid();
            gm.InitializeGrid(currentGrid);
            while (true)
            {
                //show
                Direction direction = AI.GetDirection(currentGrid);
                var result = gm.RunTurn(currentGrid, direction);
                if (false == result.Item1)
                {
                    break;
                }
                currentGrid = result.Item2;
            }
            return currentGrid.score;
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            //Random random = new Random(7524);
            //Benchmarker benchmarker = new Benchmarker(random);
            //IDirectionProvider AI = new GaAI();
            //benchmarker.Run(AI);
            //////////////////////////////////////
            Random random = new Random(7524);
            IDirectionProvider AI = new GaAI();
            GameManager gm = new GameManager(random);
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

                Direction direction = AI.GetDirection(currentGrid);
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