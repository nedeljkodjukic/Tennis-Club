using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TennisClub.Api.Models.Cosmos.Containers;
using TennisClub.Api.Models.Identity;

namespace TennisClub.Api.Extensions;

public static class JwtAuthExtensions
{
    public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var key = Encoding.ASCII.GetBytes(configuration["Authorization:SecretKey"]);

        return services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });
    }

    public static TokenOutputModel GenerateToken(ApplicationUser user, IEnumerable<string> roles, string secret, double expiration)
    {
        var claims = new ClaimsIdentity(new[]
            {
                new Claim(type:ClaimTypes.Name, user.UserName),
                new Claim(type : ClaimTypes.NameIdentifier,user.Id.ToString())
            }); ;

        foreach (var role in roles)
        {
            claims.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secret);

        var expires = DateTime.UtcNow.AddDays(expiration);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = expires,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var encryptedToken = tokenHandler.WriteToken(token);

        return new TokenOutputModel
        {
            AccessToken = encryptedToken,
            Expires = expires,
            Name = user.FirstName,
            Surname = user.LastName
        };
    }
}
