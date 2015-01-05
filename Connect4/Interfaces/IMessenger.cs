namespace Connect4.Interfaces
{
	public interface IMessenger
	{
		void Message(string message);
		string GetInput();
	}
}