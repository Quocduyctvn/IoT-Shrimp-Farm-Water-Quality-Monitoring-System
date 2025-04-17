using EcoShrimp.Data.Entities.Base;

namespace EcoShrimp.Data.Entities
{
	public class AppTimeIntervals : AppEntityBase
	{
		public int Value { get; set; }
		public string Label { get; set; } = string.Empty;
	}
}
