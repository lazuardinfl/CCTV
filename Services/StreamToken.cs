using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CCTV.Services;

public class StreamToken
{
    private readonly ILogger logger;
    private readonly SecurityKey key;

    public StreamToken(IConfiguration configuration, ILogger<StreamToken> logger)
    {
        key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["STREAM_SECRET"] ??
            RandomNumberGenerator.GetString("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", 512)));
        this.logger = logger;
    }

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
}
