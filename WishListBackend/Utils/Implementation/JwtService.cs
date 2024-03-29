﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WishListBackend.Models;
using WishListBackend.Utils.Interfaces;
using WishListBackend.Views;

namespace WishListBackend.Utils.Implementation
{
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _jwtOptions;

        public int DefaultExperationTimeMin => _jwtOptions.ExpirationMin;

        public JwtService(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }

        public string CreateConfirmationEmailJwt(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.EmailAddress)
            };

            return CreateJwtToken(claims);
        }

        public string CreateLoginJwt(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
            };

            return CreateJwtToken(claims);
        }

        public string GetEmailByToken(string token)
        {
            var claims = new JwtSecurityTokenHandler().ReadJwtToken(token).Claims;
            var emailClaim = claims.FirstOrDefault(e => e.Type == ClaimTypes.Email);
            if(emailClaim == null)
            {
                return "";
            }

            return emailClaim.Value;
        }

        private string CreateJwtToken(List<Claim> claims)
        {
            var signingCredentials = CreateSigningCredentials();
            var expiredDate = DateTime.Now.Add(TimeSpan.FromMinutes(_jwtOptions.ExpirationMin));
            var identity = new ClaimsIdentity(claims, "Token");

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
    }
}
