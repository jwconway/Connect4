using Connect4.Interfaces;

namespace Connect4
{
	/// <summary>
	/// Represents a counter that can be dropped into the grid
	/// </summary>
	public class Counter : ICounter
	{
		public Counter(CounterColour counterColour)
		{
			Colour = counterColour;
		}

		public CounterColour Colour { get; private set; }
	}
}