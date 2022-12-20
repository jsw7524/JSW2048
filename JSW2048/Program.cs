﻿using System;

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

    public class Grid
    {
        private Tile[,] _tiles;
        public int RemainedTiles
        {
            get
            {
                int counter = 0;
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        if (this._tiles[y, x] == null)
                        {
                            counter += 1;
                        }
                    }
                }
                return counter;
            }
        }

        public Tile this[int y, int x]
        {
            get
            {
                return _tiles[y, x];
            }
            set
            {
                _tiles[y, x] = value;
            }
        }


        public Grid(Tile[,] tiles)
        {
            if (tiles.GetLength(0) != 4 || tiles.GetLength(1) != 4)
            {
                throw new ArgumentException();
            }
            this._tiles = tiles;
        }

        public Grid():this(new Tile[4,4])
        {

        }
    }


    public class GameManager
    {
        private Random random;
        public Grid grid;
        //public int remainedTiles;
        public GameManager(Grid grid, Random random)
        {
            this.grid = grid;
            this.random = random;
        }
        public GameManager(Random random) : this(new Grid(new Tile[4, 4]), random)
        {
            SetRandomTile(this.grid);
            SetRandomTile(this.grid);
        }
        public GameManager() : this(new Random())
        {

        }


        public Tile SetRandomTile(Grid grid)
        {
            while (grid.RemainedTiles > 0)
            {
                int x = random.Next(4);
                int y = random.Next(4);
                if (grid[y, x] == null)
                {
                    Tile tile = new Tile(y, x, random.NextDouble() > 0.9 ? 4 : 2);
                    grid[y, x] = tile;
                    return tile;
                }
            }
            return null;
        }

        public Grid RotateRight(Grid grid)
        {
            Tile[,] newTiles = new Tile[4, 4];
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (grid[y, x] != null)
                    {
                        //grid[y, x].y = x;
                        //grid[y, x].x = 3 - y;
                        //newTiles[x, 3 - y] = grid[y, x];
                        newTiles[x, 3 - y] = new Tile(x, 3 - y, grid[y, x].value);
                    }
                }
            }
            return new Grid(newTiles);
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
                    tmp.Insert(i - 1, newTile);
                    return tmp;
                }
            }
            return column;
        }

        public Grid MergeGrid(Grid grid)
        {
            Tile[,] newTiles = new Tile[4, 4];
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
                    newTiles[start--, t.x] = t;
                }
            }
            return new Grid(newTiles);
        }


        public Grid MoveDown(Grid grid)
        {
            return MergeGrid(grid);
        }

        public Grid MoveRight(Grid grid)
        {
            Grid tmp;
            tmp = RotateRight(grid);
            tmp = MergeGrid(tmp);
            tmp = RotateRight(tmp);
            tmp = RotateRight(tmp);
            tmp = RotateRight(tmp);
            return tmp;
        }
        public Grid MoveUp(Grid grid)
        {
            Grid tmp;
            tmp = RotateRight(grid);
            tmp = RotateRight(tmp);
            tmp = MergeGrid(tmp);
            tmp = RotateRight(tmp);
            tmp = RotateRight(tmp);
            return tmp;
        }

        public Grid MoveLeft(Grid grid)
        {
            Grid tmp;
            tmp = RotateRight(grid);
            tmp = RotateRight(tmp);
            tmp = RotateRight(tmp);
            tmp = MergeGrid(tmp);
            tmp = RotateRight(tmp);
            return tmp;
        }

        public bool IsGameOver(Tile[,] grid)
        {
            return false;
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