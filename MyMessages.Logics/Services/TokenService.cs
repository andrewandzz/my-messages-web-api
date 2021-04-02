using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyMessages.Logics.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyMessages.Logics.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSecurityTokenHandler tokenHandler;
        private readonly byte[] sequrityKey;

        public TokenService(IConfiguration configuration)
        {
            tokenHandler = new JwtSecurityTokenHandler();
            sequrityKey = Encoding.UTF8.GetBytes(configuration.GetValue<string>("SecurityKey"));
        }

        public string GenerateNewIncludingUserId(int userId)
        {
            var tokerDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, userId.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(sequrityKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokerDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }
    }
}
