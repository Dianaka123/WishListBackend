using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using WishListBackend.JwtAuthentication;
using WishListBackend.Utils.Interfaces;

namespace WishListBackend.Utils.Implementation
{
    public class JwtLoginService : IJwtLoginService
    {
        private readonly JwtOptions _jwtOptions;

        public JwtLoginService(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }

        public string CreateJwt(string id)
        {
            var signingCredentials = CreateSigningCredentials();
            var expiredDate = DateTime.Now.Add(TimeSpan.FromSeconds(_jwtOptions.ExpirationSeconds));

            var identity = GetIdentity(id);

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: identity.Claims,
                expires: expiredDate,
                signingCredentials: signingCredentials
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        private SigningCredentials CreateSigningCredentials()
        {
            var keyBytes = Encoding.UTF8.GetBytes(_jwtOptions.SigningKey);
            var symmetricKey = new SymmetricSecurityKey(keyBytes);

            return new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
        }

        private ClaimsIdentity GetIdentity(string id)
        {
            var claims = new List<Claim>
            {
                new Claim("id", id),
            };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
