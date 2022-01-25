using GradDemo.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GradDemo.Api.Helpers
{
    public class AuthTokenHelper
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AuthTokenHelper(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        internal async Task<string> GenerateJWTAsync(IdentityUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yWDIbXfnckoLSuCqquTSTBrNKdA7qvCM"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

            var token = new JwtSecurityToken(
                issuer: "GradDemo.Api",
                audience: "GradDemo.Api",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}