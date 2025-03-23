using EcoShrimp.Data.Entities.Base;
using EcoShrimp.Share.Enums;

namespace EcoShrimp.Data.Entities
{
	/// <summary>
	/// Represents the AppProducts entity
	/// </summary>
	public class AppProducts : AppEntityBase
	{

		public AppProducts() { }
		public string Code { get; set; }
		public string Name { get; set; }
		public double OriginalPrice { get; set; } // giá gốc
		public double SalePrice { get; set; } // giá bán
		public int TotalQuantity { get; set; }
		public int StockQuantity { get; set; }
		public string? Video { get; set; }
		public string Summary { get; set; }
		public string Desc { get; set; }
		public Status Status { get; set; }
		public int SortOrder { get; set; }
		//-----------------------
		public int IdCategory { get; set; }
		public AppCategories appCategory { get; set; }
		// ----------------------
		public ICollection<AppImges> appImges { get; set; }
		public ICollection<AppProInstances> appProInstances { get; set; }
	}
}
