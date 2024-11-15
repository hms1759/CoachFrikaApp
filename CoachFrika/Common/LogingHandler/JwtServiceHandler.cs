using coachfrikaaaa.APIs.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoachFrika.Common.LogingHandler
{
    public class JwtServiceHandler
    {
        public interface IJwtService
        {
            Task<string> GenerateToken(CoachFrikaUsers user, IList<string> role);
        }
        public class JwtService : IJwtService
        {
            private readonly string _secretKey;
            private readonly string _issuer;
            private readonly string _audience;

            public JwtService(IConfiguration configuration)
            {
                _secretKey = configuration["Jwt:SecretKey"];
                _issuer = configuration["Jwt:Issuer"];
                _audience = configuration["Jwt:Audience"];
            }

            public async Task<string> GenerateToken(CoachFrikaUsers user, IList<string> roles)
            {

                // Create claims for the user
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.NameIdentifier, user.Id)
                    };

                // Add each role as a claim
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));  // Add role as a claim
                }

                // Define the secret key and signing credentials
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Generate the JWT token
                var token = new JwtSecurityToken(
                    issuer: _issuer,
                    audience: _audience,
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                );

                // Return the JWT token as a string
                return new JwtSecurityTokenHandler().WriteToken(token);
            }

        }
    }
}
