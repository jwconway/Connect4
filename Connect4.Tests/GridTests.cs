using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connect4.Interfaces;
using Moq;
using NUnit.Framework;

namespace Connect4.Tests
{
	[TestFixture]
    public class GridTests
    {
		private Grid _grid;

		[SetUp]
		public void Setup()
		{
			var factory = new GameFactory();
			var game = factory.SetupGame(new GameSettings() { GridHeight = 6, GridWidth = 7, NumberOfPlayers = 2, WinningConnectCount = 4}, Mock.Of<IMessenger>(), Mock.Of<IRenderer>());
			_grid = game.Grid;
		}

		[Test]
		public void Grid_has_7_columns()
		{
			Assert.IsTrue(_grid.GridColumns.Count() == 7);
		}

		[Test]
		public void Grid_is_7_wide()
		{
			Assert.IsTrue(_grid.Width == 7);
		}

		[Test]
		public void Grid_is_6_high()
		{
			Assert.IsTrue(_grid.Height == 6);
		}

		[Test]
		public void Each_column_has_6_squares()
		{
			foreach(var column in _grid.GridColumns)
			{
				Assert.IsTrue(column.ColumnSquares.Count() == 6);
			}
		}

		[Test]
		public void Try_put_counter_in_negative_one_index_column_throws_ArgumentOutOfRangeException()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => _grid.DropCounterAt(-1, new Counter(CounterColour.Yellow)));
		}

		[Test]
		public void Try_put_counter_in_seven_index_column_throws_ArgumentOutOfRangeException()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => _grid.DropCounterAt(7, new Counter(CounterColour.Yellow)));
		}

		[Test]
		public void Try_put_counter_in_full_column_throws_AInvalidOperationException()
		{
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Red));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Red));
			Assert.Throws<InvalidOperationException>(() => _grid.DropCounterAt(0, new Counter(CounterColour.Yellow)));
		}

		
    }
}
