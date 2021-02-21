using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IOptions<AuthOptions> authOptions;

        public AuthController(IOptions<AuthOptions> options)
        {
            this.authOptions = options;
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] User request)
        {
            var user = AuthenticatedUser(request.Login, request.Password);

            if (user != null)
            {
                var token = GenerateJWT(user);
                return Ok(new { access_token = token, useremail = user.Email });
            }
            return Unauthorized();
        }

        [Route("registration")]
        [HttpPost]
        public IActionResult Registration([FromBody] User request)
        {
            try
            {
                SqliteHelper.AddUser(request);
            }
            catch
            {
                return BadRequest(500);
            }
            return Ok();
        }

        private User AuthenticatedUser(string login, string password)
        {
            
            return SqliteHelper.FindUser(login, password).FirstOrDefault();
        }
        private string GenerateJWT(User user)
        {
            var authParams = authOptions.Value;
            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString())
            };

            claims.Add(new Claim("role", user.Role.ToString()));

            var token = new JwtSecurityToken(authParams.Issuer,
                authParams.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(authParams.TokenLifeTime),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
