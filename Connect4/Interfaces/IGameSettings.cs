namespace Connect4.Interfaces
{
	public interface IGameSettings
	{
		int GridWidth { get; }
		int GridHeight { get; }
		int NumberOfPlayers { get; }
		int WinningConnectCount { get; }
	}
}