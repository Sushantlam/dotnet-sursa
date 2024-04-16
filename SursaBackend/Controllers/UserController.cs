using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SursaBackend.AppDbContext; // Assuming this is your DbContext namespace
using SursaBackend.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SursaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SursaBackendDbContext _dbContext;

        public UserController(IConfiguration configuration, SursaBackendDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpPost("register")]
        public ActionResult<User> Register(User request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var existingUser = _dbContext.Users.FirstOrDefault(u => u.Email == request.Email);
            if (existingUser != null)
            {
                return BadRequest("Email already exists");
            }
            // Create a new User entity
            var newUser = new User
            {
                Email = request.Email,
                
                Password = passwordHash
            };

            // Add the new user to the database
            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            return Ok(newUser);
        }

        [HttpPost("login")]
        public ActionResult<User> Login(User request)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Email == request.Email);
            if (user == null)
            {
                return BadRequest("Wrong Email.");

            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return BadRequest("Wrong password.");
            }

            

            string token = CreateToken(user);

            var response = new LoginResponse
            {
                
                Email = user.Email,
                Token = token
            };

            return Ok(response);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Email, user.Email),
                // You can add more claims here if needed
            };

            var key = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }

            // Convert the byte array key to a Base64-encoded string for storage and usage
            var base64Key = Convert.ToBase64String(key);

            // Use the base64Key in your configuration or directly in code
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(base64Key));

            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
