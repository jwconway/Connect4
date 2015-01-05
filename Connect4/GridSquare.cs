using System;
using Connect4.Interfaces;

namespace Connect4
{
	/// <summary>
	/// Represents a square on the connect4 grid
	/// </summary>
	public class GridSquare : IGridSquare, IComparable<IGridSquare>, IComparable
	{
		/// <summary>
		/// Initializes a GridSquare
		/// </summary>
		/// <param name="indexInColumn">The index of the grid square in the column</param>
		public GridSquare(int indexInColumn)
		{
			IndexInColumn = indexInColumn;
		}

		public ICounter Counter { get; private set; }
		public int IndexInColumn { get; private set; }
		public void SetCounter(ICounter counter)
		{
			if (counter == null)
				throw new ArgumentException("Counter cannot be null", "counter");

			Counter = counter;
		}

		public int CompareTo(IGridSquare other)
		{
			return IndexInColumn.CompareTo(other.IndexInColumn);
		}

		public int CompareTo(object obj)
		{
			if (obj == null)
				return 0;

			var otherSquare = obj as IGridSquare;
			if (otherSquare != null)
			{
				return IndexInColumn.CompareTo(otherSquare.IndexInColumn);
			}

			throw new ArgumentException("obj is not IGridSquare", "obj");
		}
	}
}