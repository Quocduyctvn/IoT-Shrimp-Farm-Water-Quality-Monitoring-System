using EcoShrimp.Data.Entities.Base;

namespace EcoShrimp.Data.Entities
{
	public class AppRoles : AppEntityBase
	{
		public string Name { get; set; }
		public string Desc { get; set; }
		public int? SortOrder { get; set; }

		public ICollection<AppUsers> appUsers { get; set; }

		public ICollection<AppRolePermission> appRolePermissions { get; set; }
	}
}
