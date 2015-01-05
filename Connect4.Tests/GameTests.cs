using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connect4.Interfaces;
using Moq;
using NUnit.Framework;

namespace Connect4.Tests
{
	[TestFixture]
	public class GameTests
	{
		private Grid _grid;
		private Game _game;
		private bool _connect4Fired;
		private bool _gridFullFired;
		private ConnectArgs _connect4Args;
		private GridFullArgs gridFullArgs;

		[SetUp]
		public void Setup()
		{
			var factory = new GameFactory();
			_game = factory.SetupGame(new GameSettings() { GridHeight = 6, GridWidth = 7, NumberOfPlayers = 2, WinningConnectCount = 4}, Mock.Of<IMessenger>(), Mock.Of<IRenderer>());
			_grid = _game.Grid;

			_connect4Fired = false;
			_gridFullFired = false;
			_connect4Args = null;
			gridFullArgs = null;

			_grid.OnConnect = args =>
			{
				_connect4Fired = true;
				_connect4Args = args;
			};

			_grid.OnGridFull = args =>
			{
				_gridFullFired = true;
				gridFullArgs = args;
			};
		}

		[Test]
		public void Part_Fill_grid_No_connect4_events_shouldnt_Fire()
		{
			//============================================
			//|__|__|__|__|__|__|__|
			//|__|__|__|__|__|__|__|
			//|__|__|__|__|__|__|_ |
			//|R |Y |R |Y |Y |Y |R |
			//|R |Y |R |Y |Y |R |R |
			//|R |Y |R |Y |R |R |R |
			//============================================
			_grid.DropCounterAt(6, new Counter(CounterColour.Red));
			_grid.DropCounterAt(6, new Counter(CounterColour.Red));
			_grid.DropCounterAt(6, new Counter(CounterColour.Red));

			_grid.DropCounterAt(5, new Counter(CounterColour.Red));
			_grid.DropCounterAt(5, new Counter(CounterColour.Red));
			_grid.DropCounterAt(5, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(4, new Counter(CounterColour.Red));
			_grid.DropCounterAt(4, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(4, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(2, new Counter(CounterColour.Red));
			_grid.DropCounterAt(2, new Counter(CounterColour.Red));
			_grid.DropCounterAt(2, new Counter(CounterColour.Red));

			_grid.DropCounterAt(1, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(1, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(1, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(0, new Counter(CounterColour.Red));
			_grid.DropCounterAt(0, new Counter(CounterColour.Red));
			_grid.DropCounterAt(0, new Counter(CounterColour.Red));

			Assert.IsFalse(_connect4Fired);
			Assert.IsNull(_connect4Args);

			Assert.IsFalse(_gridFullFired);
			Assert.IsNull(gridFullArgs);
		}

		[Test]
		public void Make_Horizontal_Connect4_event_should_fire()
		{
			//============================================
			//|__|__|__|__|__|__|__|
			//|__|__|__|__|__|__|__|
			//|__|__|__|__|__|__|__|
			//|__|__|__|__|__|__|__|
			//|__|__|__|__|__|__|__|
			//|Y |Y |Y |Y |__|__|__|
			//============================================
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(1, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(2, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));

			Assert.IsTrue(_connect4Fired);
			Assert.IsNotNull(_connect4Args);

			Assert.IsFalse(_gridFullFired);
			Assert.IsNull(gridFullArgs);
		}

		[Test]
		public void Make_Vertical_Connect4_event_should_fire()
		{
			//============================================
			//|__|__|__|__|__|__|__|
			//|__|__|__|__|__|__|__|
			//|Y |__|__|__|__|__|__|
			//|Y |__|__|__|__|__|__|
			//|Y |__|__|__|__|__|__|
			//|Y |__|__|__|__|__|__|
			//============================================
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));

			Assert.IsTrue(_connect4Fired);
			Assert.IsNotNull(_connect4Args);

			Assert.IsFalse(_gridFullFired);
			Assert.IsNull(gridFullArgs);
		}

		[Test]
		public void Make_Diagonal_NWSE_Connect4_event_should_fire()
		{
			//============================================
			//|__|__|__|__|__|__|__|
			//|__|__|__|__|__|__|__|
			//|Y |__|__|__|__|__|__|
			//|R |Y |__|__|__|__|__|
			//|R |R |Y |__|__|__|__|
			//|R |R |R |Y |__|__|__|
			//============================================
			_grid.DropCounterAt(0, new Counter(CounterColour.Red));
			_grid.DropCounterAt(0, new Counter(CounterColour.Red));
			_grid.DropCounterAt(0, new Counter(CounterColour.Red));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(1, new Counter(CounterColour.Red));
			_grid.DropCounterAt(1, new Counter(CounterColour.Red));
			_grid.DropCounterAt(1, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(2, new Counter(CounterColour.Red));
			_grid.DropCounterAt(2, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));

			Assert.IsTrue(_connect4Fired);
			Assert.IsNotNull(_connect4Args);

			Assert.IsFalse(_gridFullFired);
			Assert.IsNull(gridFullArgs);
		}

		[Test]
		public void Make_Diagonal_SWNE_Connect4_event_should_fire()
		{
			//============================================
			//|__|__|__|__|__|__|__|
			//|__|__|__|__|__|__|__|
			//|__|__|__|__|__|__|Y |
			//|__|__|__|__|__|Y |R |
			//|__|__|__|__|Y |R |R |
			//|__|__|__|Y |R |R |R |
			//============================================
			_grid.DropCounterAt(6, new Counter(CounterColour.Red));
			_grid.DropCounterAt(6, new Counter(CounterColour.Red));
			_grid.DropCounterAt(6, new Counter(CounterColour.Red));
			_grid.DropCounterAt(6, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(5, new Counter(CounterColour.Red));
			_grid.DropCounterAt(5, new Counter(CounterColour.Red));
			_grid.DropCounterAt(5, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(4, new Counter(CounterColour.Red));
			_grid.DropCounterAt(4, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));

			Assert.IsTrue(_connect4Fired);
			Assert.IsNotNull(_connect4Args);

			Assert.IsFalse(_gridFullFired);
			Assert.IsNull(gridFullArgs);
		}

		[Test]
		public void Make_Diagonal_in_full_grid_NWSE_Connect4_event_should_fire()
		{
			//============================================
			//|R |Y |Y |R |R |Y |Y |
			//|Y |R |Y |Y |Y |R |R |
			//|Y |R |R |Y |R |Y |Y |
			//|Y |Y |R |Y |R |R |R |
			//|R |R |Y |R |Y |R |R |
			//|Y |R |Y |Y |R |R |R |
			//============================================
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Red));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Red));

			_grid.DropCounterAt(1, new Counter(CounterColour.Red));
			_grid.DropCounterAt(1, new Counter(CounterColour.Red));
			_grid.DropCounterAt(1, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(1, new Counter(CounterColour.Red));
			_grid.DropCounterAt(1, new Counter(CounterColour.Red));
			_grid.DropCounterAt(1, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(2, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(2, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(2, new Counter(CounterColour.Red));
			_grid.DropCounterAt(2, new Counter(CounterColour.Red));
			_grid.DropCounterAt(2, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(2, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(3, new Counter(CounterColour.Red));
			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(3, new Counter(CounterColour.Red));

			_grid.DropCounterAt(4, new Counter(CounterColour.Red));
			_grid.DropCounterAt(4, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(4, new Counter(CounterColour.Red));
			_grid.DropCounterAt(4, new Counter(CounterColour.Red));
			_grid.DropCounterAt(4, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(4, new Counter(CounterColour.Red));

			_grid.DropCounterAt(5, new Counter(CounterColour.Red));
			_grid.DropCounterAt(5, new Counter(CounterColour.Red));
			_grid.DropCounterAt(5, new Counter(CounterColour.Red));
			_grid.DropCounterAt(5, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(5, new Counter(CounterColour.Red));
			_grid.DropCounterAt(5, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(6, new Counter(CounterColour.Red));
			_grid.DropCounterAt(6, new Counter(CounterColour.Red));
			_grid.DropCounterAt(6, new Counter(CounterColour.Red));
			_grid.DropCounterAt(6, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(6, new Counter(CounterColour.Red));
			_grid.DropCounterAt(6, new Counter(CounterColour.Yellow));
			Assert.IsTrue(_connect4Fired);
			Assert.IsNotNull(_connect4Args);

			Assert.IsTrue(_gridFullFired);
			Assert.IsNotNull(gridFullArgs);
		}

		[Test]
		public void Make_Vertical_in_full_grid_Connect4_event_should_fire()
		{
			//============================================
			//|R |Y |Y |R |R |Y |Y |
			//|Y |R |Y |Y |Y |R |R |
			//|Y |R |R |Y |R |Y |Y |
			//|Y |Y |R |Y |R |R |R |
			//|Y |R |R |R |Y |R |R |
			//|Y |R |Y |Y |R |R |R |
			//============================================
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Red));

			_grid.DropCounterAt(1, new Counter(CounterColour.Red));
			_grid.DropCounterAt(1, new Counter(CounterColour.Red));
			_grid.DropCounterAt(1, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(1, new Counter(CounterColour.Red));
			_grid.DropCounterAt(1, new Counter(CounterColour.Red));
			_grid.DropCounterAt(1, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(2, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(2, new Counter(CounterColour.Red));
			_grid.DropCounterAt(2, new Counter(CounterColour.Red));
			_grid.DropCounterAt(2, new Counter(CounterColour.Red));
			_grid.DropCounterAt(2, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(2, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(3, new Counter(CounterColour.Red));
			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(3, new Counter(CounterColour.Red));

			_grid.DropCounterAt(4, new Counter(CounterColour.Red));
			_grid.DropCounterAt(4, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(4, new Counter(CounterColour.Red));
			_grid.DropCounterAt(4, new Counter(CounterColour.Red));
			_grid.DropCounterAt(4, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(4, new Counter(CounterColour.Red));

			_grid.DropCounterAt(5, new Counter(CounterColour.Red));
			_grid.DropCounterAt(5, new Counter(CounterColour.Red));
			_grid.DropCounterAt(5, new Counter(CounterColour.Red));
			_grid.DropCounterAt(5, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(5, new Counter(CounterColour.Red));
			_grid.DropCounterAt(5, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(6, new Counter(CounterColour.Red));
			_grid.DropCounterAt(6, new Counter(CounterColour.Red));
			_grid.DropCounterAt(6, new Counter(CounterColour.Red));
			_grid.DropCounterAt(6, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(6, new Counter(CounterColour.Red));
			_grid.DropCounterAt(6, new Counter(CounterColour.Yellow));
			Assert.IsTrue(_connect4Fired);
			Assert.IsNotNull(_connect4Args);

			Assert.IsTrue(_gridFullFired);
			Assert.IsNotNull(gridFullArgs);
		}

		[Test]
		public void Make_Horizontal_in_full_grid_Connect4_event_should_fire()
		{
			//============================================
			//|Y |Y |Y |Y |R |Y |Y |
			//|Y |R |Y |R |Y |R |R |
			//|Y |R |R |Y |R |Y |Y |
			//|R |Y |R |Y |R |R |R |
			//|Y |R |R |R |Y |R |R |
			//|Y |R |Y |Y |R |R |R |
			//============================================
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Red));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(1, new Counter(CounterColour.Red));
			_grid.DropCounterAt(1, new Counter(CounterColour.Red));
			_grid.DropCounterAt(1, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(1, new Counter(CounterColour.Red));
			_grid.DropCounterAt(1, new Counter(CounterColour.Red));
			_grid.DropCounterAt(1, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(2, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(2, new Counter(CounterColour.Red));
			_grid.DropCounterAt(2, new Counter(CounterColour.Red));
			_grid.DropCounterAt(2, new Counter(CounterColour.Red));
			_grid.DropCounterAt(2, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(2, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(3, new Counter(CounterColour.Red));
			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(3, new Counter(CounterColour.Red));
			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(4, new Counter(CounterColour.Red));
			_grid.DropCounterAt(4, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(4, new Counter(CounterColour.Red));
			_grid.DropCounterAt(4, new Counter(CounterColour.Red));
			_grid.DropCounterAt(4, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(4, new Counter(CounterColour.Red));

			_grid.DropCounterAt(5, new Counter(CounterColour.Red));
			_grid.DropCounterAt(5, new Counter(CounterColour.Red));
			_grid.DropCounterAt(5, new Counter(CounterColour.Red));
			_grid.DropCounterAt(5, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(5, new Counter(CounterColour.Red));
			_grid.DropCounterAt(5, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(6, new Counter(CounterColour.Red));
			_grid.DropCounterAt(6, new Counter(CounterColour.Red));
			_grid.DropCounterAt(6, new Counter(CounterColour.Red));
			_grid.DropCounterAt(6, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(6, new Counter(CounterColour.Red));
			_grid.DropCounterAt(6, new Counter(CounterColour.Yellow));
			Assert.IsTrue(_connect4Fired);
			Assert.IsNotNull(_connect4Args);

			Assert.IsTrue(_gridFullFired);
			Assert.IsNotNull(gridFullArgs);
		}

		[Test]
		public void Make_full_grid_no_Connect4_event_should_not_fire()
		{
			//============================================
			//|Y |R |Y |Y |R |Y |Y |
			//|Y |R |Y |R |Y |R |R |
			//|Y |R |R |Y |R |Y |Y |
			//|R |Y |R |Y |R |R |R |
			//|Y |R |R |R |Y |R |Y |
			//|Y |R |Y |Y |Y |R |R |
			//============================================
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Red));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(0, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(1, new Counter(CounterColour.Red));
			_grid.DropCounterAt(1, new Counter(CounterColour.Red));
			_grid.DropCounterAt(1, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(1, new Counter(CounterColour.Red));
			_grid.DropCounterAt(1, new Counter(CounterColour.Red));
			_grid.DropCounterAt(1, new Counter(CounterColour.Red));

			_grid.DropCounterAt(2, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(2, new Counter(CounterColour.Red));
			_grid.DropCounterAt(2, new Counter(CounterColour.Red));
			_grid.DropCounterAt(2, new Counter(CounterColour.Red));
			_grid.DropCounterAt(2, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(2, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(3, new Counter(CounterColour.Red));
			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(3, new Counter(CounterColour.Red));
			_grid.DropCounterAt(3, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(4, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(4, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(4, new Counter(CounterColour.Red));
			_grid.DropCounterAt(4, new Counter(CounterColour.Red));
			_grid.DropCounterAt(4, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(4, new Counter(CounterColour.Red));

			_grid.DropCounterAt(5, new Counter(CounterColour.Red));
			_grid.DropCounterAt(5, new Counter(CounterColour.Red));
			_grid.DropCounterAt(5, new Counter(CounterColour.Red));
			_grid.DropCounterAt(5, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(5, new Counter(CounterColour.Red));
			_grid.DropCounterAt(5, new Counter(CounterColour.Yellow));

			_grid.DropCounterAt(6, new Counter(CounterColour.Red));
			_grid.DropCounterAt(6, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(6, new Counter(CounterColour.Red));
			_grid.DropCounterAt(6, new Counter(CounterColour.Yellow));
			_grid.DropCounterAt(6, new Counter(CounterColour.Red));
			_grid.DropCounterAt(6, new Counter(CounterColour.Yellow));
			Assert.IsFalse(_connect4Fired);
			Assert.IsNull(_connect4Args);

			Assert.IsTrue(_gridFullFired);
			Assert.IsNotNull(gridFullArgs);
		}
	}
}
