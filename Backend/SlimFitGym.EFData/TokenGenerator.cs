using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace SlimFitGymBackend
{
    public class TokenGenerator
    {
        private readonly IConfiguration config;
        private readonly JwtSecurityTokenHandler tokenHandler;
        public TokenGenerator(IConfiguration config)
        {
            this.config = config;
            this.tokenHandler = new JwtSecurityTokenHandler();
        }
        public string GenerateToken(int id,string email, bool rememberMe, string role)
        {

            string keyFromConfig = config["Auth:Key"]!;
            Byte[] key = System.Text.Encoding.UTF8.GetBytes(keyFromConfig);

            List<Claim> claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub,id.ToString()),
                new(JwtRegisteredClaimNames.Email,email),
                new("role",role)
            };

            DateTime expiration = DateTime.Now.AddDays(1);
            if (rememberMe)
                expiration = expiration.AddDays(364);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiration,
                Issuer = config["Auth:Issuer"],
                Audience = config["Auth:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256),
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int GetAccountIdFromToken(string token)
        {
            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (int.Parse(userId!)>0)
                return int.Parse(userId!);
            return 0;
        }

    }
}
