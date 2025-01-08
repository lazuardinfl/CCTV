using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CCTV.Services;

public class StreamToken(IConfiguration configuration, ILogger<StreamToken> logger)
{
    private readonly ILogger logger = logger;
    private readonly SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["STREAM_SECRET"] ??
        RandomNumberGenerator.GetString("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", 512)));

    public string GenerateToken(string cctv, DateTime expires)
    {
        logger.LogInformation("token generated");
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = expires,
            Subject = new ClaimsIdentity([new Claim("cctv", cctv)]),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        };
        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }

    public async Task<bool> ValidateToken(string token)
    {
        logger.LogInformation("token validated");
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.Zero
        };
        var result = await tokenHandler.ValidateTokenAsync(token, validationParameters);
        return result.IsValid;
    }
}
