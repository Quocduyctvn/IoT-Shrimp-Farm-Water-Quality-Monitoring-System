using Microsoft.AspNetCore.Mvc;

namespace EcoShrimp.Client.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
