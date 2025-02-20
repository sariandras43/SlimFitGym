using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace SlimFitGymBackend
{
    public class TokenGenerator
    {
        private readonly IConfiguration config;
        public TokenGenerator(IConfiguration config)
        {
            this.config = config;
        }
        public string GenerateToken(string email, bool rememberMe, string role)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            string keyFromConfig = config["Auth:Key"]!;
            Byte[] key = System.Text.Encoding.UTF8.GetBytes(keyFromConfig);

            List<Claim> claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub,email),
                new(JwtRegisteredClaimNames.Email,email),
                new("role",role)
            };

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = config["Auth:Issuer"],
                Audience = config["Auth:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256),
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
