using EcoShrimp.Data.Entities.Base;

namespace EcoShrimp.Data.Entities
{
	/// <summary>
	/// Represents a sensor data in the system.
	/// </summary>
	public class AppDataSensor : AppEntityBase
	{
		public AppDataSensor() { }
		public double? PH { get; set; }
		public double Temp { get; set; }
		public double? DO { get; set; }
		public double? Nh4 { get; set; }
		public double? Sal { get; set; }
		public double? Tur { get; set; }
		// ---------------------
		public int IdConnect { get; set; }
		public AppConnects appConnect { get; set; }
	}
}
