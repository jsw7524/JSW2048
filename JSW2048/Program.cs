using System;

namespace JSW2048 // Note: actual namespace depends on the project name.
{
    public class Tile
    {
        public long value;
        public int x;
        public int y;
        public Tile(int y, int x, long value)
        {
            this.x = x;
            this.y = y;
            this.value = value;
        }
    }




    public class GameManager
    {
        private Random random;
        public Tile[,] grid;
        public int remainedTiles;
        public GameManager(Tile[,] grid, int remainedTile, Random random)
        {
            this.grid = grid;
            this.remainedTiles = remainedTile;
            this.random = random;
        }
        public GameManager(Random random) : this(new Tile[4, 4], 0, random)
        {
            if (grid.GetLength(0) != 4 || grid.GetLength(1) != 4)
            {
                throw new ArgumentException();
            }
            remainedTiles = 16;
            SetRandomTile();
            SetRandomTile();
        }
        public GameManager() : this(new Random())
        {

        }


        public Tile SetRandomTile()
        {
            while (remainedTiles > 0)
            {
                int x = random.Next(4);
                int y = random.Next(4);
                if (grid[y, x] == null)
                {
                    Tile tile = new Tile(y, x, random.NextDouble() > 0.9 ? 4 : 2);
                    grid[y, x] = tile;
                    remainedTiles -= 1;
                    return tile;
                }
            }
            return null;
        }

        public Tile[,] RotateRight(Tile[,] grid)
        {
            Tile[,] newGrid = new Tile[4, 4];
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (grid[y, x] != null)
                    {
                        grid[y, x].y = x;
                        grid[y, x].x = 3 - y;
                        newGrid[x, 3 - y] = grid[y, x];
                    }
                }
            }
            return newGrid;
        }

        public IEnumerable<Tile> MergeColumn(IEnumerable<Tile> column)
        {
            List<Tile> tmp = column.ToList();
            for (int i = 1; i < tmp.Count; i++)
            {
                if (tmp[i].value == tmp[i - 1].value)
                {
                    Tile newTile = new Tile(tmp[i - 1].y, tmp[i - 1].x, 2 * tmp[i - 1].value);
                    tmp.RemoveAt(i - 1);
                    tmp.RemoveAt(i - 1);
                    remainedTiles += 1;
                    tmp.Insert(i - 1, newTile);
                    return tmp;
                }
            }
            return column;
        }

        public Tile[,] MergeGrid(Tile[,] grid)
        {
            Tile[,] newGrid = new Tile[4, 4];
            for (int x = 0; x < 4; x++)
            {
                List<Tile> column = new List<Tile>();
                for (int y = 3; y >= 0; y--)
                {
                    if (grid[y, x] != null)
                    {
                        column.Add(grid[y, x]);
                    }
                }
                int start = 3;
                foreach (Tile t in MergeColumn(column))
                {
                    newGrid[start--, t.x] = t;
                }
            }
            return newGrid;
        }


        public Tile[,] MoveDown(Tile[,] grid)
        {
            return MergeGrid(grid);
        }

        public Tile[,] MoveRight(Tile[,] grid)
        {
            Tile[,] tmp;
            tmp = RotateRight(grid);
            tmp = MergeGrid(tmp);
            tmp = RotateRight(tmp);
            tmp = RotateRight(tmp);
            tmp = RotateRight(tmp);
            return tmp;
        }
        public Tile[,] MoveUp(Tile[,] grid)
        {
            Tile[,] tmp;
            tmp = RotateRight(grid);
            tmp = RotateRight(tmp);
            tmp = MergeGrid(tmp);
            tmp = RotateRight(tmp);
            tmp = RotateRight(tmp);
            return tmp;
        }

        public Tile[,] MoveLeft(Tile[,] grid)
        {
            Tile[,] tmp;
            tmp = RotateRight(grid);
            tmp = RotateRight(tmp);
            tmp = RotateRight(tmp);
            tmp = MergeGrid(tmp);
            tmp = RotateRight(tmp);
            return tmp;
        }

        public bool IsGameOver(Tile[,] grid)
        {
            MoveDown(grid)
        }



    }





    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}