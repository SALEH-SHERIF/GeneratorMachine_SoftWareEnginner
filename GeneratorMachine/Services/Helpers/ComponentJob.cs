using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorMachine.Services.Helpers
{
	public class ComponentJob : IComponentJob
	{
		public string Type { get; set; }
		public string SubType { get; set; }
		public QueueItem ParentItem { get; set; }
	}
}
