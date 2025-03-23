using EcoShrimp.Data.Entities.Base;
using EcoShrimp.Share.Enums;

namespace EcoShrimp.Data.Entities
{
	public class AppNews : AppEntityBase
	{
		public string Title { get; set; }
		public string Path { get; set; }
		public string Summary { get; set; }
		public string Content { get; set; }
		public Status Status { get; set; }
		public int SortOrder { get; set; }

		// ------------------------------------
		public int IdCateNew { get; set; }
		public AppCateNews appCateNew { get; set; }
	}
}
