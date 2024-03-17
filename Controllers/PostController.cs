using Microsoft.AspNetCore.Mvc;

namespace Pinterest.Controllers
{
	public class PostController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
