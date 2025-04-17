using EcoShrimp.Data.Entities.Base;

namespace EcoShrimp.Data.Entities
{
	public class AppThresholds : AppEntityBase
	{
		public string Name { get; set; }
		public string Unit { get; set; }
		public double Min { get; set; }
		public double Max { get; set; }
		public string? Desc { get; set; }

		public int IdFarm { get; set; }
		public AppFarms appFarms { get; set; }
	}
}
