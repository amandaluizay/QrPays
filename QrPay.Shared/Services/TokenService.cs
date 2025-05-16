using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QrPay.Domain.Entities;
using QrPay.Domain.Repository;
using QrPay.Shared.Configuration;
using QrPay.Shared.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QrPay.Shared.Services
{
    public class TokenService(IOptions<TokenConfig> config) : ITokenService
    {
        private readonly TokenConfig _config = config.Value;

        public async Task<string> GetTokenAsync(User user)
        {
            var handler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_config.Key);

            var credentials = new SigningCredentials(
                                                      new SymmetricSecurityKey(key),
                                                      SecurityAlgorithms.HmacSha256Signature
                                                    );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(user),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = credentials,
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        private static ClaimsIdentity GenerateClaims(User user)
        {
            var ci = new ClaimsIdentity();

            ci.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            ci.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            ci.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            return ci;
        }
    }
}
