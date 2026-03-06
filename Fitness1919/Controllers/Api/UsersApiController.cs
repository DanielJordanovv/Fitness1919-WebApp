using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Data.Models.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Fitness1919.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [IgnoreAntiforgeryToken]
    [Authorize]
    public class UsersApiController : Controller
    {

        private readonly Fitness1919DbContext context;
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersApiController(Fitness1919DbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.configuration = configuration;
            this.userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var usersQuery = context.Users
                .OrderBy(u => u.UserName)
                .AsQueryable();

            var totalUsers = await usersQuery.CountAsync();

            var users = await usersQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email
                })
                .ToListAsync();

            var result = new
            {
                totalUsers,
                page,
                pageSize,
                users
            };

            return View(result);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }
        [HttpPost]
        public IActionResult Create([FromBody] ApplicationUser user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            context.Users.Add(user);
            context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        [HttpPut("update/{username}")]
        public async Task<IActionResult> UpdateUser(string username, [FromBody] UsersUpdateDto model)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound("User not found");
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Address = model.Address;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("User updated successfully");
        }
        [HttpDelete("delete/{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("User deleted successfully");
        }
        [HttpGet("search")]
        public IActionResult Search(string? firstName,string? lastName,string? address,int page = 1,int pageSize = 10)
        {
            var query = context.Users.AsQueryable();

            // Филтриране по FirstName
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                query = query.Where(u => u.FirstName.Contains(firstName));
            }

            // Филтриране по LastName
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                query = query.Where(u => u.LastName.Contains(lastName));
            }

            // Филтриране по Address
            if (!string.IsNullOrWhiteSpace(address))
            {
                query = query.Where(u => u.Address.Contains(address));
            }

            // Pagination
            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var users = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Връщаме JSON с мета информация за страниците
            var result = new
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Users = users
            };

            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await userManager.FindByNameAsync(request.Username);
            if (user == null)
                return Unauthorized();

            var passwordValid = await userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
                return Unauthorized();

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = configuration["JwtSettings:Issuer"],
                Audience = configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return Ok(new { token = jwtToken });
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
