using Microsoft.AspNetCore.Mvc;

namespace Pinterest.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LikeController : ControllerBase
	{
		public IActionResult Index()
		{
			return Ok();
		}
	}
}
