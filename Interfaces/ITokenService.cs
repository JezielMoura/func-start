public interface ITokenService
{
    string Get();
    CurrentUserFeature GetCurrentUser(string token);
}