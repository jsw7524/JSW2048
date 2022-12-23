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
        public bool isSame(Grid a, Grid b)
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if ((a[y, x]?.value ?? 0) != (b[y, x]?.value ?? 0))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public GaAI(double[,] a = null, double[,] b = null)
        {
            hidenLayer1 = a ?? SetRandomLayer(8, 16);
            outputLayer = b ?? SetRandomLayer(4, 8);
        }

        Grid lastGrid = new Grid();
        public Direction GetDirection(Grid grid)
        {

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
            //forward 
            double[,] tmp;
            tmp = MultiplyMatrix(hidenLayer1, inputLayer);
            tmp = Relu(tmp);
            tmp = MultiplyMatrix(outputLayer, tmp);
            double[,] result = SoftMax(tmp);
            //choose direction
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
            if (isSame(lastGrid, grid))
            {
                maxDir = random.Next(4);
            }
            lastGrid = grid;
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

        public double Run(IDirectionProvider AI, int loopTimes = 30)
        {
            List<long> results = new List<long>();
            for (int i = 0; i < loopTimes; i++)
            {
                results.Add(Benchmark(AI));

            }
            double tmpScore = results.Average();
            Console.WriteLine(tmpScore);
            return tmpScore;
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

    class GeneScore
    {
        public double Score;
        public GaAI ai;
        public GeneScore(double Score, GaAI ai)
        {
            this.Score = Score;
            this.ai = ai;
        }
    }

    class GaFarm
    {

        Random random;

        Benchmarker benchmarker;
        int size;
        public GaFarm(int size)
        {
            random = new Random(7524);
            //geneScores = new List<GeneScore>();
            benchmarker = new Benchmarker(random);
            this.size = size;
        }
        public void Init(List<GeneScore> geneScores)
        {
            for (int i = 0; i < size; i++)
            {
                GaAI ai = new GaAI();
                geneScores.Add(new GeneScore(benchmarker.Run(ai), ai));
            }
        }

        public IEnumerable<GeneScore> SelectBest(int topN, List<GeneScore> geneScores)
        {
            return geneScores.OrderByDescending(g => g.Score).Take(topN);
        }

        public IEnumerable<GeneScore> Mate(List<GeneScore> pool, List<GeneScore> geneScores)
        {
            int poolSize = pool.Count();
            //double rate = random.NextDouble();
            double rate = 0.5;
            geneScores.Clear();
            for (int k = 0; k < size; k++)
            {
                int i = random.Next(poolSize);
                int j = random.Next(poolSize);
                double[,] newA = new double[8, 16];
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        newA[y, x] = pool[i].ai.hidenLayer1[y, x] * rate + pool[j].ai.hidenLayer1[y, x] * (1.0 - rate);
                    }
                }

                double[,] newB = new double[4, 8];
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        newB[y, x] = pool[i].ai.outputLayer[y, x] * rate + pool[j].ai.outputLayer[y, x] * (1.0 - rate);
                    }
                }
                GaAI ai = new GaAI(newA, newB);
                geneScores.Add(new GeneScore(benchmarker.Run(ai), ai));
            }
            return geneScores;
        }

    }


    internal class Program
    {
        static void Main(string[] args)
        {
            List<GeneScore> genes = new List<GeneScore>();
            GaFarm gaFarm = new GaFarm(50);
            gaFarm.Init(genes);

            for (int k=0;k<100;k++)
            {
                Console.WriteLine(k);
                var pool=gaFarm.SelectBest(5, genes).ToList();
                gaFarm.Mate(pool, genes);
            }
            genes=genes.OrderByDescending(g => g.Score).ToList();
            int yyy = 1;

            //Random random = new Random(7524);
            //Benchmarker benchmarker = new Benchmarker(random);
            //IDirectionProvider AI = new GaAI();
            //benchmarker.Run(AI);
            //////////////////////////////////////
            //Random random = new Random();
            //IDirectionProvider AI = new GaAI();
            //GameManager gm = new GameManager(random);
            //Grid currentGrid = new Grid();
            //gm.InitializeGrid(currentGrid);
            //while (true)
            //{
            //    //show
            //    Console.WriteLine(currentGrid.score);
            //    for (int y = 0; y < 4; y++)
            //    {
            //        for (int x = 0; x < 4; x++)
            //        {
            //            Console.Write($"{((currentGrid[y, x]?.value) ?? 0),-5}");
            //        }
            //        Console.WriteLine();
            //    }
            //    Console.WriteLine();

            //    Direction direction = AI.GetDirection(currentGrid);
            //    var result = gm.RunTurn(currentGrid, direction);
            //    if (false == result.Item1)
            //    {
            //        Console.WriteLine("Game Over!");
            //        return;
            //    }
            //    currentGrid = result.Item2;

            //    //Thread.Sleep(1000);

            //}
        }
    }
}