using System;
using System.Linq;
using Connect4.Interfaces;

namespace Connect4.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var gameSettings = new GameSettings()
			{
				GridHeight = 6,
				GridWidth = 7,
				NumberOfPlayers = 2,
				WinningConnectCount = 4
			};
			var game = new GameFactory().SetupGame(gameSettings, new Messenger(), new ConsoleRenderer());
			var summary = game.Run();

			System.Console.WriteLine("{0}", summary);

			System.Console.WriteLine("Game finished press any key to exit...");
			System.Console.ReadLine();
		}
	}

	public class Messenger : IMessenger
	{
		public void Message(string message)
		{
			System.Console.WriteLine(message);
		}

		public string GetInput()
		{
			return System.Console.ReadLine();
		}
	}

	public class ConsoleRenderer : IRenderer
	{
		public void Render(Grid grid)
		{
			System.Console.WriteLine();
			ICounter counter;
			for (int rowIndex = grid.Height-1; rowIndex >= 0; rowIndex--)
			{
				foreach (var gridColumn in grid.GridColumns)
				{
					counter=gridColumn.ColumnSquares.ElementAt(rowIndex).Counter;
					System.Console.Write("|");
					if (counter != null)
					{
						System.Console.ForegroundColor = CounterColourToConsoleColor(counter.Colour);
						System.Console.Write("X");
						System.Console.ResetColor();
					}
					else
					{
						System.Console.Write(" ");
					}
					System.Console.Write(" ");
				}
				System.Console.WriteLine();
			}
			System.Console.WriteLine();
		}

		private ConsoleColor CounterColourToConsoleColor(CounterColour counterColour)
		{
			switch (counterColour)
			{
				case CounterColour.Yellow:
					return ConsoleColor.Yellow;
				case CounterColour.Red:
					return ConsoleColor.Red;
				default:
					return ConsoleColor.White;
			}
		}
	}
}
