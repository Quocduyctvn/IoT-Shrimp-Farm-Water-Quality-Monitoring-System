using EcoShrimp.Data.Entities.Base;
using EcoShrimp.Share.Enums;

namespace EcoShrimp.Data.Entities
{
	/// <summary>
	/// Represents a shrimp farm in the system.
	/// </summary>
	public class AppFarms : AppEntityBase
	{
		public AppFarms()
		{
			appPonds = new HashSet<AppPonds>();
			appProInstances = new HashSet<AppProInstances>();
		}
		public string Code { get; set; }
		public string FarmName { get; set; }
		public string OwnerName { get; set; }
		public string? NumberHouse { get; set; }
		public string? Ward { get; set; }
		public string? District { get; set; }
		public string? City { get; set; }
		public string? Location { get; set; }
		public string Phone { get; set; }
		public string? Email { get; set; }
		public string? Avatar { get; set; }
		public string PassWord { get; set; }
		public string? Desc { get; set; }
		public Status Status { get; set; }
		public int SortOrder { get; set; }
		public bool IsNotify { get; set; }
		// ----------------------
		public int? IdTime { get; set; }
		//----------------------  
		public ICollection<AppPonds> appPonds { get; set; }
		public ICollection<AppProInstances> appProInstances { get; set; }
	}
}