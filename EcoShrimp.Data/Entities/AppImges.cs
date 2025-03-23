using EcoShrimp.Data.Entities.Base;

namespace EcoShrimp.Data.Entities
{
	/// <summary>
	/// Represents a image in the system.
	/// </summary>
	public class AppImges : AppEntityBase
	{
		public AppImges() { }
		public string Path { get; set; }

		// -----------------------
		public int IdProduct { get; set; }
		public AppProducts appProduct { get; set; }
	}
}
