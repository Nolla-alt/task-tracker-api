using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Business.Services.Interfaces;

namespace TaskManager.Business.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expireMinutes;

        public TokenService(string key, string issuer, string audience, int expireMinutes)
        {
            _key = key;
            _issuer = issuer;
            _audience = audience;
            _expireMinutes = expireMinutes;
        }

        public string GenerateToken(int userId, string name, string email)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _issuer,
                Audience = _audience,
                Expires = DateTime.UtcNow.AddMinutes(_expireMinutes),
                SigningCredentials = credentials,
                Claims = new Dictionary<string, object>
                {
                    ["sub"] = userId.ToString(),
                    ["name"] = name,
                    ["email"] = email
                }
            };

            return new JsonWebTokenHandler().CreateToken(descriptor);
        }
    }
}