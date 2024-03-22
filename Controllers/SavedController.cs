using Microsoft.AspNetCore.Mvc;

namespace Pinterest.Controllers
{
	public class SavedController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
