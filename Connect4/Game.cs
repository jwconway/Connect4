using System;
using System.Collections.Generic;
using System.Diagnostics;
using Connect4.Interfaces;

namespace Connect4
{
	/// <summary>
	/// Encapsulates the game loop and returns a summary to the client
	/// </summary>
	public class Game
	{
		protected readonly IGameSettings _gameSettings;
		protected readonly IMessenger _messenger;
		protected readonly IRenderer _renderer;
		protected bool _gameInProgress;

		/// <summary>
		/// Constructs a new Game and creates the Grid based on the gameSettings parameters
		/// </summary>
		/// <param name="gameSettings">The settings that will be used to construct the game</param>
		/// <param name="messenger">The messenger will relay message back to the client</param>
		/// <param name="renderer">The renderer will be called to initiate a render in the client</param>
		public Game(IGameSettings gameSettings, IMessenger messenger, IRenderer renderer)
		{
			_gameSettings = gameSettings;
			_messenger = messenger;
			_renderer = renderer;
			InitializeGrid(gameSettings);
		}

		public Grid Grid { get; private set; }
		public IEnumerable<Player> Players { get; internal set; }

		/// <summary>
		/// Builds the grid
		/// </summary>
		/// <param name="gameSettings">Constructs the Grid based on the gameSettings</param>
		private void InitializeGrid(IGameSettings gameSettings)
		{
			Grid = new Grid(gameSettings, this);
		}

		/// <summary>
		/// The game loop 
		/// </summary>
		/// <returns>A summary of the game that played out</returns>
		public GameSummary Run()
		{
			var stopWatch = new Stopwatch();
			var gameSummary = new GameSummary();
			var gameInProgress = true;
			Grid.OnConnect = args =>
			{
				gameInProgress = false;
				stopWatch.Stop();
				gameSummary.WinningColour = args.WinningColour;
				gameSummary.NumberOfMoves = args.NumberOfMoves;
				gameSummary.MovesPlayed = args.MovesPlayed;
				gameSummary.GameLengthMs = stopWatch.ElapsedMilliseconds;
			};

			Grid.OnGridFull = args =>
			{
				gameInProgress = false;
				stopWatch.Stop();
				gameSummary.MovesPlayed = args.MovesPlayed;
				gameSummary.GameLengthMs = stopWatch.ElapsedMilliseconds;
			};

			gameInProgress = true;
			stopWatch.Start();
			
			_messenger.Message(string.Format("Welcome to connect {0}!\r\n\r\n", _gameSettings.WinningConnectCount));

			while(gameInProgress)
			{
				foreach (var player in Players)
				{
					player.TakeTurn(this, _messenger);
					_renderer.Render(Grid);
					//If this players counter ended game dont bother giving other players a turn in this round
					if(!gameInProgress)
						break;
				}
			}

			return gameSummary;
		}
	}
}