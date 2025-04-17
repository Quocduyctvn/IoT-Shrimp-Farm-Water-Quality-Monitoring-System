using EcoShrimp.Data.Entities.Base;
using EcoShrimp.Share.Enums;

namespace EcoShrimp.Data.Entities
{
	public class AppUsers : AppEntityBase
	{
		public string Name { get; set; }
		public string? Address { get; set; }
		public string? Email { get; set; }
		public string Phone { get; set; }
		public string Pass { get; set; }
		public string? Avatar { get; set; }
		public Status Status { get; set; }
		public int? SortOrder { get; set; }
		//---------------------------------------
		public int IdRole { get; set; }
		public AppRoles appRole { get; set; }
	}
}
