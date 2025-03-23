using EcoShrimp.Data.Entities.Base;
using EcoShrimp.Share.Enums;

namespace EcoShrimp.Data.Entities
{
	/// <summary>
	/// Represents a shrimp pond in the system.
	/// </summary>
	public class AppPonds : AppEntityBase
	{
		public AppPonds()
		{
			appSeasons = new HashSet<AppSeasons>();
		}

		public string Name { get; set; }
		public string? Address { get; set; }
		// diện tích ao
		public decimal? Area { get; set; }
		// Active, Inactive
		public Status Status { get; set; }
		public string? Desc { get; set; }
		public int SortOrder { get; set; }

		//-----------------
		public int IdFarm { get; set; }
		public AppFarms appFarm { get; set; }
		//-----------------
		public ICollection<AppSeasons> appSeasons { get; set; }

	}
}
