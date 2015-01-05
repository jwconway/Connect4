using System;
using System.Collections.Generic;

namespace Connect4.Interfaces
{
	public class ConnectArgs
	{
		public CounterColour WinningColour { get; set; }
		public int NumberOfMoves { get; set; }
		public List<Tuple<int, CounterColour>> MovesPlayed { get; set; }
	}
}