namespace GeneratorMachine.Services.Helpers
{
	public interface IComponentJob
	{
		QueueItem ParentItem { get; set; }
		string SubType { get; set; }
		string Type { get; set; }
	}
}