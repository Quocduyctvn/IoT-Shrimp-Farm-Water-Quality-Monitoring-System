using EcoShrimp.Data.Entities.Base;
using EcoShrimp.Share.Enums;

namespace EcoShrimp.Data.Entities
{
	public class AppCategories : AppEntityBase
	{
		public string Name { get; set; }
		public string? Desc { get; set; }
		public Status Status { get; set; }
		public int SortOrder { get; set; }
		public ICollection<AppProducts> appProducts { get; set; }
	}
}
