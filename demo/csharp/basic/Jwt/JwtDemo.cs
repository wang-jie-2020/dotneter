using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace basic.Jwt
{
    internal class JwtDemo
    {
        void Method1()
        {
            CreateJwtToken(CreateClaims());
        }

        /*
         * 只是写一个东西证明可以写一个对称加密的jwt token
         * 网上一堆helper
         */
        private IEnumerable<Claim> CreateClaims()
        {
            return new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "123"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };
        }

        private string CreateJwtToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {

            DateTime now = DateTime.UtcNow;
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("Secret--C421AAEE0D114E9C"));

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: "test issuer",
                audience: "test audience",
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? TimeSpan.FromHours(1)),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}
