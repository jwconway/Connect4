using System.Collections.Generic;
using Connect4.Interfaces;

namespace Connect4
{
	/// <summary>
	/// A factory helper for setting up the games players
	/// </summary>
	public class PlayerFactory
	{
		/// <summary>
		/// Sets up and returns the collection of players dictated by teh game settings
		/// </summary>
		/// <param name="gameSettings"></param>
		/// <returns>Collection of players</returns>
		public static IEnumerable<Player> GetPlayers(IGameSettings gameSettings)
		{
			return new List<Player>()
			{
				new Player(PlayerType.Human, CounterColour.Red, number: 1), 
				new Player(PlayerType.Computer, CounterColour.Yellow, number: 2)
			};
		}
	}
}