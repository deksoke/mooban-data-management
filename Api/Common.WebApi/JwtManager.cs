using Common.DTO.AuthDTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Common.WebApi
{
    public static class JwtManager
    {
        private static readonly byte[] AccessSecret;
        private static readonly byte[] RefreshSecret;
        private static readonly string Issuer;
        private static readonly string Audience;
        private static readonly int AccessExpirationTime;
        private static readonly int RefreshExpirationTime;
        private static readonly IConfiguration _config;

        static JwtManager()
        {
            string EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (EnvironmentName == "Production")
            {
                _config = new ConfigurationBuilder()
                    .SetBasePath(Environment.CurrentDirectory)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                    .Build();
            }
            else
            {
                _config = new ConfigurationBuilder()
                    .SetBasePath(Environment.CurrentDirectory)
                    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false)
                    .Build();
            }

            SymmetricSecurityKey SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AppSecurity:Secret"]));

            Issuer = _config["AppSecurity:issuer"];
            Audience = _config["AppSecurity:audience"];
            AccessSecret = Convert.FromBase64String(_config["AppSecurity:accessSecret"]);
            RefreshSecret = Convert.FromBase64String(_config["AppSecurity:refreshSecret"]);
            AccessExpirationTime = Int32.Parse(_config["AppSecurity:accessExpire"]);
            RefreshExpirationTime = Int32.Parse(_config["AppSecurity:refreshExpire"]);
        }

        public static Token GenerateToken(ClaimsIdentity claims)
        {
            var issued = DateTime.UtcNow;
            var accessExpires = issued.AddMinutes(AccessExpirationTime);
            var refreshExpires = issued.AddMinutes(RefreshExpirationTime);

            var token = new Token
            {
                Expires_in = TimeSpan.FromMinutes(AccessExpirationTime).TotalMilliseconds,
                Access_token = CreateToken(claims, issued, accessExpires, AccessSecret),
                Refresh_token = CreateToken(claims, issued, refreshExpires, RefreshSecret)
            };

            return token;
        }

        private static string CreateToken(ClaimsIdentity claims, DateTime issuedAt, DateTime expireIn, byte[] secretKey)
        {
            var signingKey = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256);

            var roleClaim = claims.FindFirst(claims.RoleClaimType);
            if (roleClaim != null)
            {
                claims.AddClaim(new Claim("role", roleClaim.Value, roleClaim.ValueType, roleClaim.Issuer, roleClaim.OriginalIssuer, roleClaim.Subject));
                claims.TryRemoveClaim(roleClaim);
            }

            var token = new JwtSecurityToken(Issuer, Audience, claims.Claims, issuedAt, expireIn, signingKey);
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.WriteToken(token);

            return jwt;
        }

        public static ClaimsPrincipal GetPrincipal(string token, bool isAccessToken = true)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                if (!(tokenHandler.ReadToken(token) is JwtSecurityToken jwtToken))
                    return null;

                var validationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Audience,
                    ValidIssuer = Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(isAccessToken ? AccessSecret : RefreshSecret)
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);

                return principal;
            }

            catch (Exception)
            {
                return null;
            }
        }
    }

}
