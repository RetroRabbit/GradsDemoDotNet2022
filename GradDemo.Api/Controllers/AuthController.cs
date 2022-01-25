using GradDemo.Api.Entities;
using GradDemo.Api.Helpers;
using GradDemo.Api.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace GradDemo.Api.Controllers
{
    [Route("/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AuthTokenHelper _authHelper;

        public AuthController(UserManager<IdentityUser> userManager, AuthTokenHelper authhelper)
        {
            _userManager = userManager;
            _authHelper = authhelper;
        }

        /// <summary>
        /// Authenticates a user
        /// </summary>
        /// <param name="user">Username and password of the user</param>
        [HttpPost("token")]
        public async Task<ActionResult<TokenResult>> AuthorizeUser(DeviceCredentials user)
        {
            if (!string.IsNullOrEmpty(user.ClientId) && !string.IsNullOrEmpty(user.ClientSecret))
            {
                var currentUser = await _userManager.FindByNameAsync(user.ClientId);
                if (await _userManager.CheckPasswordAsync(currentUser, user.ClientSecret))
                {
                    return new TokenResult()
                    {
                        Token = await _authHelper.GenerateJWTAsync(currentUser)
                    };
                }
            }

            return BadRequest("Invalid Credentials");
        }

        /// <summary>
        /// Authenticates a user
        /// </summary>
        /// <param name="user">Username and password of the user</param>
        [HttpPost("register")]
        public async Task<ActionResult<DeviceCredentials>> RegisterUser()
        {
            string password = GenerateRandomClientSecret();
            string username = GenerateRandomClientId();

            var currentUser = await _userManager.CreateAsync(new IdentityUser()
            {
                UserName = username,
                Email = $"{username}@grademoapi.co.za",
                AccessFailedCount = 0,
                EmailConfirmed = true
            }, password);

            return new DeviceCredentials
            {
                ClientId = username,
                ClientSecret = password
            };
        }

        private string GenerateRandomClientId()
        {
            return Guid.NewGuid().ToString("N");
        }

        private string GenerateRandomClientSecret()
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[32];
                rng.GetBytes(bytes);
                string converted = Convert.ToBase64String(bytes)
                    .TrimEnd('=')
                    .Replace('/', 'a')
                    .Replace('+', 'c');
                return converted;
            }
        }
    }
}
