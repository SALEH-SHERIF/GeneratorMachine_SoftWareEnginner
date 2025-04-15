namespace GeneratorMachine.Services.Helpers
{
	public interface IQueueItem
	{
		int Completed { get; set; }
		string DisplayText { get; }
		int Quantity { get; }
		string SubType { get; }
		string Type { get; }
	}
}