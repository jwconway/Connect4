using Connect4.Interfaces;

namespace Connect4
{
	public class GameSettings : IGameSettings
	{
		public int GridWidth { get; set; }
		public int GridHeight { get; set; }
		public int NumberOfPlayers { get; set; }
		public int WinningConnectCount { get; set; }
	}
}