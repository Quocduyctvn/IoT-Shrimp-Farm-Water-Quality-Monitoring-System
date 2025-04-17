using EcoShrimp.Data.Entities.Base;

namespace EcoShrimp.Data.Entities
{
	public class AppPermissions : AppEntityBase
	{
		public string Code { get; set; }
		public string Table { get; set; }
		public string GroupName { get; set; }
		public string Desc { get; set; }

		public ICollection<AppRolePermission> appRolePermissions { get; set; }
	}
}
