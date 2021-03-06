using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Models.JwtToken;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestWebAppl.Models;

namespace ClassLibrary.Models
{
    public class JwtGenerator:IJwtGenerator
    {
        private readonly SymmetricSecurityKey _key;

        public JwtGenerator(IConfiguration _configuration)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenKey"])); 
        }

        public string CreateToken(ApplicationUser _user)
        {
            var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.NameId, _user.UserName) };

            var credentails = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = credentails
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
