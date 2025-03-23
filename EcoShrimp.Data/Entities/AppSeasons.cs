using EcoShrimp.Data.Entities.Base;
using EcoShrimp.Share.Enums;

namespace EcoShrimp.Data.Entities
{
	/// <summary>
	/// Represents a shrimp season in the system.
	/// </summary>
	public class AppSeasons : AppEntityBase
	{
		public AppSeasons()
		{
			appConnects = new HashSet<AppConnects>();
		}
		public string Name { get; set; }
		public string? Desc { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		// Mật độ tôm nuôi trong ao (số lượng tôm/m²)
		public int? Density { get; set; }
		public Status Status { get; set; }
		public int SortOrder { get; set; }
		//----------------------
		public int IdPond { get; set; }
		public AppPonds appPond { get; set; }
		//----------------------

		public ICollection<AppConnects> appConnects { get; set; }

	}
}
