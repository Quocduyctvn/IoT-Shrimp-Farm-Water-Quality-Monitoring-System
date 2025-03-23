using EcoShrimp.Data.Entities.Base;
using EcoShrimp.Share.Enums;

namespace EcoShrimp.Data.Entities
{
	/// <summary>
	/// Represents the AppConnects entity
	/// </summary>
	public class AppConnects : AppEntityBase
	{
		public AppConnects()
		{
			appDataSensors = new HashSet<AppDataSensor>();
		}
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string? Desc { get; set; }
		public ConnectStatus Status { get; set; }
		// ----------------------
		public int IdProInstances { get; set; }
		public AppProInstances appProInstances { get; set; }
		public int IdSeason { get; set; }
		public AppSeasons appSeasons { get; set; }
		// ----------------------
		public ICollection<AppDataSensor> appDataSensors { get; set; }

	}
}
