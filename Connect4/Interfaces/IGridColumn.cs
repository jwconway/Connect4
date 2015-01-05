using System.Collections.Generic;

namespace Connect4.Interfaces
{
	public interface IGridColumn
	{
		IEnumerable<IGridSquare> ColumnSquares { get; }
		int ColumnIndex { get; }
		int ColumnHeight { get; }
	}
}