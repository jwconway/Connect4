using System;
using System.Collections.Generic;
using Connect4.Interfaces;

namespace Connect4
{
	public class GameSummary
	{
		public int NumberOfMoves { get; internal set; }
		public long GameLengthMs { get; internal set; }
		public List<Tuple<int, CounterColour>> MovesPlayed { get; internal set; }
		public CounterColour WinningColour { get; set; }

		public override string ToString()
		{
			return string.Format("Number of moves:\t\t{0}\r\nWinning colour:\t\t{1}\r\nGame length (mins):\t\t{2}\r\n\r\n", NumberOfMoves, WinningColour, TimeSpan.FromMilliseconds(GameLengthMs));
		}
	}
}