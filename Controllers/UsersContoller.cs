using Microsoft.AspNetCore.Mvc;
using blog.Models;
using blog.Data;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace blog.Controllers {
    public class UsersController : Controller {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly CookieOptions _cookieOptions; 

        public UsersController(AppDbContext db, IConfiguration configuration) {
            _db = db;
            _configuration = configuration;
            this._cookieOptions = new CookieOptions {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddHours(1)
            };
        }
        public IActionResult Index() {
            return View();
        }
        public IActionResult Register() {
            return View();
        }
        

        [HttpPost]
        public IActionResult RegisterJson([FromBody] User user) {
            if (ModelState.IsValid) {

                if (_db.Users.Any(dbUser => dbUser.Username == user.Username)) {
                    ModelState.AddModelError("Username", "This username is already exits.");
                    return BadRequest(ModelState);
                }
                _db.Users.Add(user);
                _db.SaveChanges();
                return Ok("User registered successfully");
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (_db.Users.Any(dbUser => dbUser.Username == user.Username))
            {
                ModelState.AddModelError("Username", "This username is already taken.");
                return View(user); 
            }

            _db.Users.Add(user);
            _db.SaveChanges();
            return RedirectToAction("Index", "Users");
        }
        
        [HttpPost]
        public IActionResult LoginJson([FromBody] User user) {
            var resultUser = _db.Users.FirstOrDefault(dbUser => dbUser.Username == user.Username && dbUser.Password == user.Password);
            if (resultUser == null) {
                return Unauthorized("Invalid username or password");
            }
            return Ok($"{resultUser.Username}");
        }

        [HttpPost]
        public IActionResult Login([FromForm] User user) {
            var resultUser = _db.Users.FirstOrDefault(dbUser => dbUser.Username == user.Username && dbUser.Password == user.Password);
            if (resultUser == null) {
                return Unauthorized("Invalid username or password");
            }
            
            // Create Claims
            var claims = new[] {
                new Claim(ClaimTypes.Name, resultUser.Username),
                new Claim(ClaimTypes.Role, "User")
            };

            // Create Key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ?? "default_secret_key_which_is_long_enough"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            // Set token to cookie
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            Response.Cookies.Append("AuthToken", tokenString, this._cookieOptions);

            // Write username and id to Cookie
            Response.Cookies.Append("Username", resultUser.Username, this._cookieOptions);
            Response.Cookies.Append("UserId", resultUser.Id.ToString(), this._cookieOptions);


            return RedirectToAction("Index", "Blogs");

        }

        public IActionResult Logout() {
            Response.Cookies.Delete("AuthToken");
            Response.Cookies.Delete("Username");
            Response.Cookies.Delete("UserId");

            return RedirectToAction("Index", "Blogs");

        }

    }
}