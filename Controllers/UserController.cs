using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pinterest.Data;
using Pinterest.DTOs.User;
using Pinterest.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Pinterest.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserController(AppDbContext appDbContext, IHttpContextAccessor contextAccessor)
        {
            _appDbContext = appDbContext;
            _contextAccessor = contextAccessor;
        }
        [HttpGet]
        [Route("getAdminDetails")]
        public IActionResult AdminDetails()
        {
            var accessToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.ReadJwtToken(accessToken);
            if (token is null) return BadRequest("An error occurred in generating the token");

            var userIdClaim = token.Claims.FirstOrDefault(x => x.Type == "UserID");

            var userId = userIdClaim.Value;

            var admin = _appDbContext.AppUsers.FirstOrDefault(x => x.Id == userId);
            if (admin == null) return NotFound();

            var details = _appDbContext.UserDetails.FirstOrDefault(x => x.AppUserId == userId);
            if (details == null) return NotFound();

            var dto = new GetAdminDetailsDto()
            {
                Username = admin.UserName,
                Email = admin.Email,
                Firstname = admin.FirstName,
                Lastname = admin.LastName,
                Photo = details.ProfilePicUrl
            };

            return Ok(dto);
        }

        [HttpGet]
        [Route("getUsers")]
        public IActionResult Index()
        {
            var users = _appDbContext.UserDetails.Where(x => x.AppUserId != "5b539870-feb9-494a-bdd1-746832ebbea6").ToList();

            var list = new List<GetUsersDto>();

            foreach (var user in users)
            {
                var dto = new GetUsersDto()
                {
                    Id = user.Id,
                    UserName = user.Username,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Gender = user.Gender,
                    Image = user.ProfilePicUrl,
                    UserId = user.AppUserId,
                    FollowerCount = _appDbContext.FollowerUsers.Where(x => x.AppUserId == user.AppUserId).Count(),
                };
                list.Add(dto);
            }
            return Ok(list);
        }

        [HttpDelete]
        [Route("deleteUser/{id}")]
        public IActionResult DeleteUser(string id)
        {
            var user = _appDbContext.AppUsers.FirstOrDefault(x => x.Id == id);
            if (user == null) return NotFound("User not found!");

            _appDbContext.AppUsers.Remove(user);
            _appDbContext.SaveChanges();

            return Ok("User deleted successfully!");
        }
        [HttpGet]
        [Route("searchUsers")]
        public IActionResult Search([FromQuery] string input)
        {
            if (input is null) return BadRequest("Input is null");

            var users =  _appDbContext.AppUsers
                .Where(x => x.UserName.ToLower()
                .StartsWith(input.ToLower()))
                .ToList();
            if (users.Count == 0) return BadRequest("No matches!");

            var list = new List<GetSearchResultDto>();

            foreach (var user in users)
            {
                var dto = new GetSearchResultDto()
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Image = _appDbContext.UserDetails.FirstOrDefault(x => x.AppUserId == user.Id).ProfilePicUrl
                };
                list.Add(dto);
            }

            return Ok(list);
        }
    }
}
