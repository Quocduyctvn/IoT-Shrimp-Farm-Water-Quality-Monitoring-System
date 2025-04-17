using EcoShrimp.Data.Entities.Base;

namespace EcoShrimp.Data.Entities
{
	public class AppShrimp : AppEntityBase
	{
		public AppShrimp() { }

		// Thông tin cơ bản
		public string WebsiteName { get; set; }
		public string? Logan { get; set; }
		public string LogoUrl { get; set; }

		// Thông tin liên hệ
		public string Phone { get; set; }
		public string? SubPhone { get; set; }
		public string? Email { get; set; }
		public string? Address { get; set; }
		public string? Map { get; set; }
		public string? OpentTime { get; set; }

		public string? FacebookUrl { get; set; }
		public string? TwitterUrl { get; set; }
		public string? InstagramUrl { get; set; }
		public string? YouTubeUrl { get; set; }
	}
}
