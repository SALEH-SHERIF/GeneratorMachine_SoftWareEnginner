using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorMachine.Services.Helpers
{
	public class QueueItem : IQueueItem
	{
		public string Type { get; }
		public string SubType { get; }
		public int Quantity { get; }
		public int Completed { get; set; }

		public string DisplayText => $"{Quantity}x {SubType} {Type} ({Completed}/{Quantity})";

		public QueueItem(string type, string subType, int quantity)
		{
			Type = type;
			SubType = subType;
			Quantity = quantity;
		}
	}
}
