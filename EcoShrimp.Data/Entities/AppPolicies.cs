using EcoShrimp.Data.Entities.Base;
using EcoShrimp.Share.Enums;

namespace EcoShrimp.Data.Entities
{
	public class AppPolicies : AppEntityBase
	{
		public string Name { get; set; }
		public string Path { get; set; }
		public string Summary { get; set; }
		public string Content { get; set; }
		public Status Status { get; set; }
		public int SortOrder { get; set; }
	}
}
