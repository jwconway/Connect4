using System;
using System.Collections.Generic;
using Connect4.Interfaces;

namespace Connect4
{
	/// <summary>
	/// Information about the full grid event
	/// </summary>
	internal class GridFullArgs
	{
		public List<Tuple<int, CounterColour>> MovesPlayed { get; set; }
	}
}