using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CCTV.Services;

public class StreamToken(IConfiguration configuration)
{
    private readonly SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
        Regex.IsMatch(configuration["App:Secret"]!, "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{256,}$") ?
        configuration["App:Secret"]! : RandomNumberGenerator.GetString("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", 512)));

    public string GenerateToken(DateTime expires, ClaimsIdentity claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = expires,
            Subject = claims,
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        };
        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }

    public async Task<bool> ValidateToken(string token)
    {
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

    public string? GetTokenClaim(string token, string claim)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.ReadJwtToken(token);
            return jwt.Claims.First(c => c.Type == claim).Value;
        }
        catch
        {
            return null;
        }
    }
}
