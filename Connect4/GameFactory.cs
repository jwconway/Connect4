using Connect4.Interfaces;

namespace Connect4
{
	/// <summary>
	/// Helper factory for setting up a game
	/// </summary>
	public class GameFactory
	{
		/// <summary>
		/// Sets up a game
		/// </summary>
		/// <param name="gameSettings">Settings used to set up the game</param>
		/// <param name="messenger">Used to relay messages to client</param>
		/// <param name="renderer">Used to render grid on client</param>
		/// <returns></returns>
		public Game SetupGame(IGameSettings gameSettings, IMessenger messenger, IRenderer renderer)
		{
			return new Game(gameSettings, messenger, renderer)
			{
				Players = PlayerFactory.GetPlayers(gameSettings)
			};
		}
	}
}