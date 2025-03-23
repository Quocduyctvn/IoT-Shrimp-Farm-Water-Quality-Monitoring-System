using EcoShrimp.Data.Entities.Base;
using EcoShrimp.Share.Enums;

namespace EcoShrimp.Data.Entities
{
	public class AppRequests : AppEntityBase
	{
		public AppRequests() { }
		public string CompanyName { get; set; }
		public string? Address { get; set; }
		public string? Phone { get; set; }
		public string Email { get; set; }
		public string Content { get; set; }
		public string? ContentFeedback { get; set; }
		public string? TitleFeedback { get; set; }
		public DateTime? FeedbackDate { get; set; }
		public RequestStatus Status { get; set; }
	}
}
