﻿using System;

namespace JSW2048 // Note: actual namespace depends on the project name.
{
    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
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
        public bool isGameOver;
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

        public long score;
        public Grid(Tile[,] tiles, long score = 0)
        {
            if (tiles.GetLength(0) != 4 || tiles.GetLength(1) != 4)
            {
                throw new ArgumentException();
            }
            this._tiles = tiles;
            this.score = score;
            this.isGameOver = false;
        }

        public Grid(long score = 0) : this(new Tile[4, 4], score)
        {

        }
    }


    public class GameManager
    {
        private Random random;
        public GameManager(Random random) 
        {
            this.random = random;
        }

        public void InitializeGrid(Grid grid)
        {
            SetRandomTile(grid);
            SetRandomTile(grid);
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
                        newTiles[x, 3 - y] = new Tile(x, 3 - y, grid[y, x].value);
                    }
                }
            }
            return new Grid(newTiles, grid.score);
        }
        public Tuple<List<Tile>, long> MergeColumn(IEnumerable<Tile> column)
        {
            long localScore = 0;
            List<Tile> tmp = column.ToList();
            for (int i = 1; i < tmp.Count; i++)
            {
                if (tmp[i].value == tmp[i - 1].value)
                {
                    Tile newTile = new Tile(tmp[i - 1].y, tmp[i - 1].x, 2 * tmp[i - 1].value);
                    localScore += newTile.value;
                    tmp.RemoveAt(i - 1);
                    tmp.RemoveAt(i - 1);
                    tmp.Insert(i - 1, newTile);
                    return Tuple.Create(tmp, localScore);
                }
            }
            return Tuple.Create(tmp, localScore);
        }


        public Grid MergeGrid(Grid grid)
        {
            long scoreTmp = grid.score;
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
                Tuple<List<Tile>, long> tuple = MergeColumn(column);
                scoreTmp += tuple.Item2;
                foreach (Tile t in tuple.Item1)
                {
                    newTiles[start--, x] = t;
                }
            }
            return new Grid(newTiles, scoreTmp);
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

        public bool IsGameOver(Grid grid)
        {
            if (grid.RemainedTiles == 0
                && MoveLeft(grid).score == grid.score
                && MoveDown(grid).score == grid.score
                && MoveRight(grid).score == grid.score
                && MoveUp(grid).score == grid.score)
            {
                grid.isGameOver = true;
                return true;
            }
            return false;
        }


        public Tuple<bool, Grid> RunTurn(Grid grid, Direction direction)
        {
            Grid nextTurnGrid = null;
            switch (direction)
            {
                case Direction.UP:
                    nextTurnGrid = MoveUp(grid);
                    break;
                case Direction.DOWN:
                    nextTurnGrid = MoveDown(grid);
                    break;
                case Direction.LEFT:
                    nextTurnGrid = MoveLeft(grid);
                    break;
                case Direction.RIGHT:
                    nextTurnGrid = MoveRight(grid);
                    break;
            }
            if (IsGameOver(nextTurnGrid))
            {
                return Tuple.Create(false, nextTurnGrid);
            }
            if (!isSame(nextTurnGrid, grid))
            {
                SetRandomTile(nextTurnGrid);
            }
            return Tuple.Create(true, nextTurnGrid);
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