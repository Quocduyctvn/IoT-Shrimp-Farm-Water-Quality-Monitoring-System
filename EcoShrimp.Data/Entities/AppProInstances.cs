using EcoShrimp.Data.Entities.Base;
using EcoShrimp.Share.Enums;

namespace EcoShrimp.Data.Entities
{
	/// <summary>
	/// Represents the AppProInstances entity
	/// </summary>
	public class AppProInstances : AppEntityBase
	{
		public AppProInstances()
		{
			appConnects = new HashSet<AppConnects>();
		}
		public string SeriNumber { get; set; }
		public string Name { get; set; }
		public string? Desc { get; set; }
		public Status Status { get; set; }
		public string? IP { get; set; }
		public int? Port { get; set; }
		public DateTime? BuyDate { get; set; }
		// không tham chiếu
		public int SortOrder { get; set; }
		// ----------------------
		public int? IdFarm { get; set; }
		public AppFarms appFarm { get; set; }
		public int IdProduct { get; set; }
		public AppProducts appProducts { get; set; }
		// ----------------------
		public ICollection<AppConnects> appConnects { get; set; }
	}
}
