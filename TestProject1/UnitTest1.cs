using JSW2048;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            GameManager gameManager = new GameManager(new Random(7524));
            Grid grid = new Grid();
            gameManager.InitializeGrid(grid);
            Assert.AreEqual(2, grid[0, 2].value);
            Assert.AreEqual(4, grid[1, 1].value);
            Assert.AreEqual(14, grid.RemainedTiles);
        }

        [TestMethod]
        public void TestMethod2()
        {
            Grid grid = new Grid(new Tile[4, 4]);
            grid[0, 2] = new Tile(0, 2, 512);
            grid[1, 1] = new Tile(1, 1, 8);
            GameManager gameManager = new GameManager( new Random(7524));
            Assert.AreEqual(512, grid[0, 2].value);
            Assert.AreEqual(8, grid[1, 1].value);
            Assert.AreEqual(14, grid.RemainedTiles);
        }

        [TestMethod]
        public void TestMethod3()
        {
            GameManager gameManager = new GameManager(new Random(7524));
            List<Tile> data = new List<Tile>() { new Tile(0, 2, 8), new Tile(1, 2, 8), new Tile(2, 2, 8) };
            List<Tile> result = gameManager.MergeColumn(data).Item1.ToList();
            Assert.AreEqual(16, result[0].value);
            Assert.AreEqual(8, result[1].value);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void TestMethod4()
        {
            GameManager gameManager = new GameManager(new Random(7524));
            List<Tile> data = new List<Tile>() { new Tile(0, 3, 4), new Tile(1, 3, 2), new Tile(2, 3, 2), new Tile(3, 3, 2) };
            List<Tile> result = gameManager.MergeColumn(data).Item1.ToList();
            Assert.AreEqual(4, result[0].value);
            Assert.AreEqual(4, result[1].value);
            Assert.AreEqual(2, result[2].value);
            Assert.AreEqual(3, result.Count);
        }


        [TestMethod]
        public void TestMethod5()
        {
            GameManager gameManager = new GameManager(new Random(7524));
            List<Tile> data = new List<Tile>() { new Tile(0, 1, 2), new Tile(1, 1, 32), new Tile(2, 1, 2) };
            List<Tile> result = gameManager.MergeColumn(data).Item1.ToList();
            Assert.AreEqual(2, result[0].value);
            Assert.AreEqual(32, result[1].value);
            Assert.AreEqual(2, result[2].value);
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void TestMethod6()
        {
            Grid grid = new Grid(new Tile[4, 4]);
            grid[0, 0] = null;
            grid[1, 0] = null;
            grid[2, 0] = null;
            grid[3, 0] = new Tile(3, 0, 8);
            //
            grid[0, 1] = null;
            grid[1, 1] = new Tile(1, 1, 2);
            grid[2, 1] = new Tile(2, 1, 32);
            grid[3, 1] = new Tile(3, 1, 2);
            //
            grid[0, 2] = null;
            grid[1, 2] = new Tile(1, 2, 8);
            grid[2, 2] = new Tile(2, 2, 8);
            grid[3, 2] = new Tile(3, 2, 8);
            //
            grid[0, 3] = new Tile(0, 3, 2);
            grid[1, 3] = new Tile(1, 3, 2);
            grid[2, 3] = new Tile(2, 3, 2);
            grid[3, 3] = new Tile(3, 3, 4);
            GameManager gameManager = new GameManager( new Random(7524));

            Grid result = gameManager.MergeGrid(grid);

            Assert.AreEqual(16, result[3, 2].value);
            Assert.AreEqual(4, result[2, 3].value);
            Assert.AreEqual(8, result[3, 0].value);
            Assert.IsNull(result[0, 3]);
            Assert.AreEqual(7, result.RemainedTiles);
        }


        [TestMethod]
        public void TestMethod7()
        {
            Grid grid = new Grid();
            grid[0, 0] = null;
            grid[1, 0] = null;
            grid[2, 0] = null;
            grid[3, 0] = new Tile(3, 0, 8);
            //
            grid[0, 1] = null;
            grid[1, 1] = new Tile(1, 1, 2);
            grid[2, 1] = new Tile(2, 1, 32);
            grid[3, 1] = new Tile(3, 1, 2);
            //
            grid[0, 2] = null;
            grid[1, 2] = new Tile(1, 2, 8);
            grid[2, 2] = new Tile(2, 2, 8);
            grid[3, 2] = new Tile(3, 2, 8);
            //
            grid[0, 3] = new Tile(0, 3, 2);
            grid[1, 3] = new Tile(1, 3, 2);
            grid[2, 3] = new Tile(2, 3, 2);
            grid[3, 3] = new Tile(3, 3, 4);
            GameManager gameManager = new GameManager(new Random(7524));

            Grid result = gameManager.RotateRight(grid);

            Assert.AreEqual(8, result[0, 0].value);
            Assert.AreEqual(2, result[1, 0].value);
            Assert.AreEqual(8, result[2, 0].value);
            Assert.AreEqual(4, result[3, 0].value);
            //
            Assert.IsNull(result[0, 1]);
            Assert.AreEqual(32, result[1, 1].value);
            Assert.AreEqual(8, result[2, 1].value);
            Assert.AreEqual(2, result[3, 1].value);
            Assert.IsNull(result[0, 3]);
            Assert.AreEqual(5, result.RemainedTiles);
        }

        [TestMethod]
        public void TestMethod8()
        {
            Grid grid = new Grid();
            grid[0, 0] = null;
            grid[1, 0] = null;
            grid[2, 0] = null;
            grid[3, 0] = new Tile(3, 0, 8);
            //
            grid[0, 1] = null;
            grid[1, 1] = new Tile(1, 1, 2);
            grid[2, 1] = new Tile(2, 1, 32);
            grid[3, 1] = new Tile(3, 1, 2);
            //
            grid[0, 2] = null;
            grid[1, 2] = new Tile(1, 2, 8);
            grid[2, 2] = new Tile(2, 2, 8);
            grid[3, 2] = new Tile(3, 2, 8);
            //
            grid[0, 3] = new Tile(0, 3, 2);
            grid[1, 3] = new Tile(1, 3, 2);
            grid[2, 3] = new Tile(2, 3, 2);
            grid[3, 3] = new Tile(3, 3, 4);
            GameManager gameManager = new GameManager( new Random(7524));

            Grid result = gameManager.MoveRight(grid);

            Assert.AreEqual(null, result[0, 0]?.value);
            Assert.AreEqual(null, result[1, 0]?.value);
            Assert.AreEqual(null, result[2, 0]?.value);
            Assert.AreEqual(8, result[3, 0]?.value);
            //
            Assert.AreEqual(null, result[0, 1]?.value);
            Assert.AreEqual(2, result[1, 1]?.value);
            Assert.AreEqual(32, result[2, 1]?.value);
            Assert.AreEqual(2, result[3, 1]?.value);
            Assert.IsNull(result[0, 2]);
            Assert.AreEqual(5, result.RemainedTiles);
        }

        [TestMethod]
        public void TestMethod9()
        {
            Grid grid = new Grid();
            grid[0, 0] = null;
            grid[1, 0] = null;
            grid[2, 0] = null;
            grid[3, 0] = new Tile(3, 0, 8);
            //
            grid[0, 1] = null;
            grid[1, 1] = new Tile(1, 1, 2);
            grid[2, 1] = new Tile(2, 1, 32);
            grid[3, 1] = new Tile(3, 1, 2);
            //
            grid[0, 2] = null;
            grid[1, 2] = new Tile(1, 2, 8);
            grid[2, 2] = new Tile(2, 2, 8);
            grid[3, 2] = new Tile(3, 2, 8);
            //
            grid[0, 3] = new Tile(0, 3, 2);
            grid[1, 3] = new Tile(1, 3, 2);
            grid[2, 3] = new Tile(2, 3, 2);
            grid[3, 3] = new Tile(3, 3, 4);
            GameManager gameManager = new GameManager( new Random(7524));

            Grid result = gameManager.MoveUp(grid);

            Assert.AreEqual(8, result[0, 0]?.value);
            Assert.AreEqual(null, result[1, 0]?.value);
            Assert.AreEqual(null, result[2, 0]?.value);
            Assert.AreEqual(null, result[3, 0]?.value);
            //
            Assert.AreEqual(2, result[0, 1]?.value);
            Assert.AreEqual(32, result[1, 1]?.value);
            Assert.AreEqual(2, result[2, 1]?.value);
            Assert.AreEqual(null, result[3, 1]?.value);
            //
            Assert.AreEqual(16, result[0, 2]?.value);
            Assert.AreEqual(8, result[1, 2]?.value);
            Assert.AreEqual(null, result[2, 2]?.value);
            Assert.AreEqual(null, result[3, 2]?.value);
            //
            Assert.AreEqual(4, result[0, 3]?.value);
            Assert.AreEqual(2, result[1, 3]?.value);
            Assert.AreEqual(4, result[2, 3]?.value);
            Assert.AreEqual(null, result[3, 3]?.value);
            //
            Assert.AreEqual(7, result.RemainedTiles);
        }

        [TestMethod]
        public void TestMethod10()
        {
            Grid grid = new Grid();
            grid[0, 0] = null;
            grid[1, 0] = null;
            grid[2, 0] = null;
            grid[3, 0] = new Tile(3, 0, 8);
            //
            grid[0, 1] = null;
            grid[1, 1] = new Tile(1, 1, 2);
            grid[2, 1] = new Tile(2, 1, 32);
            grid[3, 1] = new Tile(3, 1, 2);
            //
            grid[0, 2] = null;
            grid[1, 2] = new Tile(1, 2, 8);
            grid[2, 2] = new Tile(2, 2, 8);
            grid[3, 2] = new Tile(3, 2, 8);
            //
            grid[0, 3] = new Tile(0, 3, 2);
            grid[1, 3] = new Tile(1, 3, 2);
            grid[2, 3] = new Tile(2, 3, 2);
            grid[3, 3] = new Tile(3, 3, 4);
            GameManager gameManager = new GameManager( new Random(7524));

            Grid result = gameManager.MoveLeft(grid);

            Assert.AreEqual(2, result[0, 0]?.value);
            Assert.AreEqual(2, result[1, 0]?.value);
            Assert.AreEqual(32, result[2, 0]?.value);
            Assert.AreEqual(8, result[3, 0]?.value);
            //
            Assert.AreEqual(null, result[0, 1]?.value);
            Assert.AreEqual(8, result[1, 1]?.value);
            Assert.AreEqual(8, result[2, 1]?.value);
            Assert.AreEqual(2, result[3, 1]?.value);
            //
            Assert.AreEqual(null, result[0, 2]?.value);
            Assert.AreEqual(2, result[1, 2]?.value);
            Assert.AreEqual(2, result[2, 2]?.value);
            Assert.AreEqual(8, result[3, 2]?.value);
            //
            Assert.AreEqual(null, result[0, 3]?.value);
            Assert.AreEqual(null, result[1, 3]?.value);
            Assert.AreEqual(null, result[2, 3]?.value);
            Assert.AreEqual(4, result[3, 3]?.value);
            //
            Assert.AreEqual(5, result.RemainedTiles);
        }

        [TestMethod]
        public void TestMethod11()
        {
            Grid grid = new Grid();
            grid[0, 0] = null;
            grid[1, 0] = null;
            grid[2, 0] = null;
            grid[3, 0] = new Tile(3, 0, 8);
            //
            grid[0, 1] = null;
            grid[1, 1] = new Tile(1, 1, 2);
            grid[2, 1] = new Tile(2, 1, 32);
            grid[3, 1] = new Tile(3, 1, 2);
            //
            grid[0, 2] = null;
            grid[1, 2] = new Tile(1, 2, 8);
            grid[2, 2] = new Tile(2, 2, 8);
            grid[3, 2] = new Tile(3, 2, 8);
            //
            grid[0, 3] = new Tile(0, 3, 2);
            grid[1, 3] = new Tile(1, 3, 2);
            grid[2, 3] = new Tile(2, 3, 2);
            grid[3, 3] = new Tile(3, 3, 4);
            GameManager gameManager = new GameManager( new Random(7524));
            bool result = gameManager.IsGameOver(grid);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestMethod12()
        {
            Grid grid = new Grid();
            grid[0, 0] = new Tile(0, 0, 4);
            grid[1, 0] = new Tile(1, 0, 16);
            grid[2, 0] = new Tile(2, 0, 4); 
            grid[3, 0] = new Tile(3, 0, 2);
            //
            grid[0, 1] = new Tile(0, 1, 8);
            grid[1, 1] = new Tile(1, 1, 256);
            grid[2, 1] = new Tile(2, 1, 32);
            grid[3, 1] = new Tile(3, 1, 4);
            //
            grid[0, 2] = new Tile(0, 2, 64);
            grid[1, 2] = new Tile(1, 2, 32);
            grid[2, 2] = new Tile(2, 2, 64);
            grid[3, 2] = new Tile(3, 2, 16);
            //
            grid[0, 3] = new Tile(0, 3, 2);
            grid[1, 3] = new Tile(1, 3, 4);
            grid[2, 3] = new Tile(2, 3, 16);
            grid[3, 3] = new Tile(3, 3, 2);
            GameManager gameManager = new GameManager( new Random(7524));
            bool result = gameManager.IsGameOver(grid);
            Assert.IsTrue(result);
        }

    }
}