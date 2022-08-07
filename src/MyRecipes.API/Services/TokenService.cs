using Microsoft.IdentityModel.Tokens;
using MyRecipes.Application.Common.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyRecipes.API.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly string _tokenKey;
    private readonly string _tokenIssuer;
    private readonly string _tokenAudience;

    public TokenService(IConfiguration config)
    {
        _config = config;
        _tokenKey = _config.GetValue<string>("Authentication:SecretKey");
        _tokenIssuer = _config.GetValue<string>("Authentication:Issuer");
        _tokenAudience = _config.GetValue<string>("Authentication:Audience");
    }

    public string GenerateToken(string username, string userId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenKey));

        var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new()
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.UniqueName, username),
        };

        // TODO: Change the expire date, add refresh token.
        var token = new JwtSecurityToken(
            _tokenIssuer,
            _tokenAudience,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7),
            signinCredentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
