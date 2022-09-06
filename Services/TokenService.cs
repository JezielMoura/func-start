using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

public class TokenService : ITokenService
{
    private const string IdIdentifier = "id";
    private const string AccountIdentifier = "account";
    private const string NameIdentifier = "name";
    private const string EmailIdentifier = "email";
    private const string PermissionsIdentifier = "permissions";
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration) =>
        _configuration = configuration;

    public string Get()
    {
        var claims = new List<Claim>
        {
            new Claim("id", Guid.NewGuid().ToString()),
            new Claim("account", "1"),
            new Claim("name", "Jeziel Moura"),
            new Claim("email", "jeziel.moura@2020.com"),
            new Claim("permissions", "AddUser;EditUser;ReadUser;DeleteUser"),
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var tokenHandler = new JwtSecurityTokenHandler();
		var tokenKey = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtKey"));
		var tokenDescriptor = new SecurityTokenDescriptor
		{
		    Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddMinutes(180),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
		};
        
		var token = tokenHandler.CreateToken(tokenDescriptor);
		
        return tokenHandler.WriteToken(token);
    }

    public CurrentUserFeature GetCurrentUser(string token)
    {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JwtKey")));
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenOptions = new TokenValidationParameters { ValidateIssuerSigningKey = true, ValidateIssuer = false, ValidateAudience = false, IssuerSigningKey = key};
        
        tokenHandler.ValidateToken(token, tokenOptions, out SecurityToken validatedToken);

        var securityToken = validatedToken as JwtSecurityToken;
        var id = securityToken?.Claims.FirstOrDefault(c => c.Type == IdIdentifier)?.Value ?? string.Empty;
        var account = securityToken?.Claims.FirstOrDefault(c => c.Type == AccountIdentifier)?.Value ?? string.Empty;
        var name = securityToken?.Claims.FirstOrDefault(c => c.Type == NameIdentifier)?.Value ?? string.Empty;
        var email = securityToken?.Claims.FirstOrDefault(c => c.Type == EmailIdentifier)?.Value ?? string.Empty;
        var permissions = securityToken?.Claims.FirstOrDefault(c => c.Type == PermissionsIdentifier)?.Value.Split(";") ?? Enumerable.Empty<string>();
        
        return new CurrentUserFeature(Guid.Parse(id), int.Parse(account), name, email, permissions);
    }
}