using System;
using System.Collections.Generic;
using System.Linq;
using Connect4.Interfaces;

namespace Connect4
{
	/// <summary>
	/// Represents a player in the game
	/// </summary>
	public class Player
	{
		/// <summary>
		/// Constructs a nerw Player
		/// </summary>
		/// <param name="playerType">The type of player</param>
		/// <param name="counterColour">The counter colour</param>
		/// <param name="number">The players number in the game</param>
		public Player(PlayerType playerType, CounterColour counterColour, int number)
		{
			PlayerType = playerType;
			CounterColour = counterColour;
			Number = number;
		}

		public PlayerType PlayerType { get; private set; }
		public CounterColour CounterColour { get; private set; }
		public int Number { get; private set; }

		/// <summary>
		/// Contains the logic for a player taking a turn
		/// </summary>
		/// <param name="game">The game in which the player is participating</param>
		/// <param name="messenger">The messenger</param>
		internal void TakeTurn(Game game, IMessenger messenger)
		{
			messenger.Message(string.Format("Player {0}'s go...\r\n\r\n", Number));
			bool turnTaken = false;
			//Make this a loop so we dont exit until a succesful move has been made
			while (!turnTaken)
			{
				messenger.Message(string.Format("Please choose a column to drop your counter into:", Number));
				var columnNumberString = messenger.GetInput();
				int columnNumber;
				//validate input
				if (!int.TryParse(columnNumberString, out columnNumber))
				{
					messenger.Message(string.Format("Please try again. Input must be a number representing the column number i.e. 1, 2, 3, etc"));
					continue;
				}
				if (columnNumber < 1 || columnNumber > game.Grid.GridColumns.Count())
				{
					messenger.Message(string.Format("Please try again. There is no column with that number. Input must be between 1 and {0}", game.Grid.GridColumns.Count()));
					continue;
				}

				var columnIndex = columnNumber - 1;
				//validate position
				if (!game.Grid.CanDropCounterAt(columnIndex))
				{
					messenger.Message("That column is full please try again");
					continue;
				}
				try
				{
					game.Grid.DropCounterAt(columnIndex, new Counter(CounterColour));
					turnTaken = true;
				}
				catch (Exception ex)
				{
					messenger.Message(string.Format("Invalid move - {0}", ex.Message));
				}
			}
		}
	}
}