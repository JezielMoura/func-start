using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

public class TokenService 
{
    public string GetAsync()
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
		var tokenKey = Encoding.UTF8.GetBytes("hbfskbghisbgsbgdsijgerginergikxbghkbvkfbdrfibgrgierhgeruogr");
		var tokenDescriptor = new SecurityTokenDescriptor
		{
		    Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddMinutes(180),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
		};
        
		var token = tokenHandler.CreateToken(tokenDescriptor);
		
        return tokenHandler.WriteToken(token);
    }
}