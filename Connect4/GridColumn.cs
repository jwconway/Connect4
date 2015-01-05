using System.Collections.Generic;
using Connect4.Interfaces;

namespace Connect4
{
	/// <summary>
	/// This class represents a column on the connect4 grid
	/// </summary>
	public class GridColumn : IGridColumn
	{
		/// <summary>
		/// Initializes a new GridColumn
		/// </summary>
		/// <param name="columnIndex">The index of the column on the grid</param>
		/// <param name="columnHeight">The height of the column</param>
		public GridColumn(int columnIndex, int columnHeight)
		{
			ColumnIndex = columnIndex;
			ColumnHeight = columnHeight;

			InitializeColumnSquares();
		}

		public IEnumerable<IGridSquare> ColumnSquares { get; private set; }
		public int ColumnIndex { get; private set; }
		public int ColumnHeight { get; private set; }

		/// <summary>
		/// Initializes the column with squares
		/// </summary>
		private void InitializeColumnSquares()
		{
			var squares = new List<GridSquare>();
			for (int indexInColumn = 0; indexInColumn < ColumnHeight; indexInColumn++)
			{
				squares.Add(new GridSquare(indexInColumn));
			}

			ColumnSquares = squares;
		}
	}
}