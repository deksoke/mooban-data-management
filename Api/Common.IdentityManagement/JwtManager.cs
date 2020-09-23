using Common.DTO.AuthDTO;
using Common.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Common.IdentityManagement
{
    public interface IJwtManager
    {
        Token GenerateToken(User user);
        ClaimsPrincipal GetPrincipal(string token, bool isAccessToken = true);
    }

    public class JwtManager: IJwtManager
    {
        private readonly byte[] AccessSecret;
        private readonly byte[] RefreshSecret;
        private readonly string Issuer;
        private readonly string Audience;
        private readonly int AccessExpirationTime;
        private readonly int RefreshExpirationTime;
        private readonly SymmetricSecurityKey SecretKey;
        private readonly IConfiguration configuration;

        public JwtManager(IConfiguration configuration)
        {
            this.configuration = configuration;
            //string EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            //if (EnvironmentName == "Production")
            //{
            //    _config = new ConfigurationBuilder()
            //        .SetBasePath(Environment.CurrentDirectory)
            //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            //        .Build();
            //}
            //else
            //{
            //    _config = new ConfigurationBuilder()
            //        .SetBasePath(Environment.CurrentDirectory)
            //        .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false)
            //        .Build();
            //}

            SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSecurity:Secret"]));

            Issuer = configuration["AppSecurity:issuer"];
            Audience = configuration["AppSecurity:audience"];
            AccessSecret = Convert.FromBase64String(configuration["AppSecurity:accessSecret"]);
            RefreshSecret = Convert.FromBase64String(configuration["AppSecurity:refreshSecret"]);
            AccessExpirationTime = Int32.Parse(configuration["AppSecurity:accessExpire"]);
            RefreshExpirationTime = Int32.Parse(configuration["AppSecurity:refreshExpire"]);
        }

        //public string GenerateTokenTest(User user)
        //{
        //    List<Claim> ObjClaims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //            new Claim(ClaimTypes.Name, user.Login),
        //            new Claim(ClaimTypes.Email, user.Email)
        //        };

        //    var signingKey = new SigningCredentials(new SymmetricSecurityKey(AccessSecret), SecurityAlgorithms.HmacSha256);

        //    JwtSecurityToken token = new JwtSecurityToken(
        //        issuer: Issuer,
        //        audience: Audience,
        //        claims: ObjClaims,
        //        expires: DateTime.UtcNow.AddMinutes(AccessExpirationTime),
        //        signingCredentials: signingKey
        //    );

        //    var handler = new JwtSecurityTokenHandler();

        //    string jwt = handler.WriteToken(token);
        //    return jwt;
        //}

        public Token GenerateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                };

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

        private string CreateToken(
            List<Claim> claims, 
            DateTime issuedAt, 
            DateTime expireIn, 
            byte[] secretKey)
        {
            var signingKey = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(Issuer, Audience, claims, issuedAt, expireIn, signingKey);
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.WriteToken(token);

            return jwt;
        }

        public ClaimsPrincipal GetPrincipal(string token, bool isAccessToken = true)
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
