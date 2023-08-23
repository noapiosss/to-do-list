using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Api.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class TokenHandler : ITokenHandler
    {
        private readonly string _secret;

        public TokenHandler(IOptionsMonitor<AppConfiguration> appConfiguration)
        {
            _secret = appConfiguration.CurrentValue.Secret;
        }

        public string GenerateToken(int userId)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, userId.ToString())
            };

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_secret));
            SigningCredentials cred = new(key, SecurityAlgorithms.HmacSha256Signature);
            JwtSecurityToken token = new(
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: cred
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool Validate(string token, out int userId)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_secret));

            try
            {
                _ = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                userId = int.Parse(jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
                return true;
            }
            catch
            {
                userId = -1;
                return false;
            }
        }
    }
}