using EcoShrimp.Data.Entities.Base;

namespace EcoShrimp.Data.Entities
{
	public class AppRolePermission : AppEntityBase
	{
		public int IdRole { get; set; }
		public int IdPermission { get; set; }
		public AppRoles appRole { get; set; }
		public AppPermissions appPermission { get; set; }
	}
}