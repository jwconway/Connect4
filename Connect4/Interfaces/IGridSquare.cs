namespace Connect4.Interfaces
{
	public interface IGridSquare
	{
		ICounter Counter { get; }
		int IndexInColumn { get; }

		void SetCounter(ICounter counter);
	}
}