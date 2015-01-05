using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connect4.Interfaces;

namespace Connect4
{
	/// <summary>
	/// Represents a connect4 grid
	/// </summary>
	public class Grid
	{
		private readonly Game _game;
		
		/// <summary>
		/// Initialises a new Grid
		/// </summary>
		/// <param name="gameSettings">gamesettings that contain grid settings</param>
		/// <param name="game">The implmentation of IGame</param>
		public Grid(IGameSettings gameSettings, Game game)
		{
			_game = game;
			Height = gameSettings.GridHeight;
			Width = gameSettings.GridWidth;
			WinningConnectCount = gameSettings.WinningConnectCount;
			InitializeGridColumns();
			Moves = new List<Tuple<int, CounterColour>>();
		}

		public int WinningConnectCount { get; private set; }
		public IEnumerable<IGridColumn> GridColumns { get; private set; }
		public int Height { get; private set; }
		public int Width { get; private set; }
		internal Action<GridFullArgs> OnGridFull { get; set; }
		internal Action<ConnectArgs> OnConnect { get; set; }
		private List<Tuple<int, CounterColour>> Moves { get; set; } 

		/// <summary>
		/// Tests wether a counter can be dropped in the column specified at columnIndex
		/// </summary>
		/// <param name="columnIndex">The index of the column to test</param>
		/// <returns></returns>
		internal bool CanDropCounterAt(int columnIndex)
		{
			if (columnIndex < 0 || columnIndex >= GridColumns.Count())
				throw new ArgumentOutOfRangeException(string.Format("columnindex must be between 0 and {0} and was {1}", GridColumns.Count() - 1, columnIndex));

			var column = GridColumns.ElementAt(columnIndex);

			//return true if any column squares dont have a counter
			return column.ColumnSquares.Any(columnSquare => columnSquare.Counter == null);
		}

		/// <summary>
		/// Drops counter into the column at the specified index
		/// </summary>
		/// <param name="columnIndex">The index of the column to drop the counter into</param>
		/// <param name="counter">The counter to drop</param>
		internal void DropCounterAt(int columnIndex, ICounter counter)
		{
			if (columnIndex < 0 || columnIndex >= GridColumns.Count())
				throw new ArgumentOutOfRangeException(string.Format("columnindex must be between 0 and {0} and was {1}", GridColumns.Count() - 1, columnIndex));

			if(!CanDropCounterAt(columnIndex))
				throw new InvalidOperationException(string.Format("Column {0} is full", columnIndex));

			var column = GridColumns.ElementAt(columnIndex);

			//Get the gridsquare that the counter will fall to.
			//This will be the gridsquare with the minimum index that doesnt have a counter
			var lowestEmptyColumnSquare = column.ColumnSquares
											.First(columnSquare => columnSquare.Counter == null);

			lowestEmptyColumnSquare.SetCounter(counter);

			//Track move
			Moves.Add(new Tuple<int, CounterColour>(columnIndex, counter.Colour));

			if(HasConnectBeenDetected())
			{
				if (OnConnect != null)
					OnConnect.Invoke(new ConnectArgs(){ WinningColour = counter.Colour, MovesPlayed = Moves, NumberOfMoves = Moves.Count});
			}

			if(IsFull())
			{
				if (OnGridFull != null)
					OnGridFull.Invoke(new GridFullArgs(){ MovesPlayed = Moves });
			}
		}

		/// <summary>
		/// Rteurns true if Grid is full
		/// </summary>
		/// <returns></returns>
		private bool IsFull()
		{
			return !GridColumns.Any(column => CanDropCounterAt(column.ColumnIndex));
		}

		/// <summary>
		/// Returns true if a Connect has been detected
		/// </summary>
		/// <returns></returns>
		private bool HasConnectBeenDetected()
		{
			return CheckRows()
				   || CheckColumns()
				   || CheckDiagonalRows();
		}

		/// <summary>
		/// Checks diagonal rows for connect
		/// </summary>
		/// <returns></returns>
		internal bool CheckDiagonalRows()
		{
			//Diagonal check
			for (int diagColumnStart = 0; diagColumnStart < Width; diagColumnStart++)
			{
				for (int diagRowStart = Height - 1; diagRowStart >= 0; diagRowStart--)
				{
					var squaresToCheck = new List<IGridSquare>();
					for (int rowIndex = diagRowStart, columnIndex = diagColumnStart; rowIndex >= 0 && columnIndex < Width; rowIndex--, columnIndex++)
					{
						squaresToCheck.Add(GetGridSquares()[columnIndex][rowIndex]);
					}

					//got a row lets check it
					if (CheckSetForConnect(squaresToCheck))
						return true;
				}
			}

			var flippedGrid = GetGridSquares().Reverse().ToArray();

			for (int diagColumnStart = 0; diagColumnStart < Width; diagColumnStart++)
			{
				for (int diagRowStart = Height - 1; diagRowStart >= 0; diagRowStart--)
				{
					var squaresToCheck = new List<IGridSquare>();
					for (int rowIndex = diagRowStart, columnIndex = diagColumnStart; rowIndex >= 0 && columnIndex < Width; rowIndex--, columnIndex++)
					{
						squaresToCheck.Add(flippedGrid[columnIndex][rowIndex]);
					}

					//got a row lets check it
					if (CheckSetForConnect(squaresToCheck))
						return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Checks horizontal rows for connect
		/// </summary>
		/// <returns></returns>
		internal bool CheckRows()
		{
			//row check
			for (int rowIndex = 0; rowIndex < Height; rowIndex++)
			{
				var squaresToCheck = new List<IGridSquare>();
				for (int columnIndex = 0; columnIndex < Width; columnIndex++)
				{
					squaresToCheck.Add(GetGridSquares()[columnIndex][rowIndex]);
				}
				//got a row lets check it
				if (CheckSetForConnect(squaresToCheck))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Checks columns for connect
		/// </summary>
		/// <returns></returns>
		internal bool CheckColumns()
		{
			//column check
			for (int columnIndex = 0; columnIndex < Width; columnIndex++)
			{
				var squaresToCheck = new List<IGridSquare>();
				for (int rowIndex = 0; rowIndex < Height; rowIndex++)
				{
					squaresToCheck.Add(GetGridSquares()[columnIndex][rowIndex]);
				}
				//Got a column lets check it
				if (CheckSetForConnect(squaresToCheck))
					return true;
				
			}
			return false;
		} 

		/// <summary>
		/// Checks a given set of gridsquares for a connect
		/// </summary>
		/// <param name="squaresToCheck"></param>
		/// <returns></returns>
		private bool CheckSetForConnect(IEnumerable<IGridSquare> squaresToCheck)
		{
			if (squaresToCheck == null)
				throw new ArgumentException("squares to test cannot be null", "squaresToTest");

			//Lets discard empty squares
			var squaresWithCounters = squaresToCheck.Where(square => square.Counter != null);

			//if there arent [WinningConnectCount] squares with counters in there cant be connect4
			if (squaresWithCounters.Count() < WinningConnectCount)
				return false;

			int colourCount = 0;
			CounterColour initialColour = squaresWithCounters.First().Counter.Colour;

			foreach (var gridSquare in squaresWithCounters)
			{
				if (gridSquare.Counter == null)
					continue;
				if (gridSquare.Counter.Colour == initialColour)
				{
					//Counter is the same as the last one, increment count of colours in a row
					colourCount++;
				}
				else
				{
					//Counter is a different colour from the last one, reset colour and count
					initialColour = gridSquare.Counter.Colour;
					colourCount = 1;
				}

				if (colourCount == WinningConnectCount)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// A 2d array of grid squares
		/// </summary>
		/// <returns></returns>
		private IGridSquare[][] GetGridSquares()
		{
			return GridColumns.Select(column => column.ColumnSquares.ToArray()).ToArray();
		}

		/// <summary>
		/// Initializes the grids columns
		/// </summary>
		private void InitializeGridColumns()
		{
			var columns = new List<GridColumn>();
			for (int columnIndex = 0; columnIndex < Width; columnIndex++)
			{
				columns.Add(new GridColumn(columnIndex, Height));
			}

			GridColumns = columns;
		}

		/// <summary>
		/// Clears the grid
		/// </summary>
		internal void ClearGrid()
		{
			InitializeGridColumns(); 
		}
	}
}
