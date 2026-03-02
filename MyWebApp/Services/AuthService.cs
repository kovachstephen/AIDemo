namespace MyWebApp.Services;

public interface IAuthService
{
    bool IsLoggedIn { get; }
    string? Username { get; }
    bool Login(string username, string password);
    void Logout();
}

public class AuthService : IAuthService
{
    private string? _username;
    private readonly Dictionary<string, string> _users = new()
    {
        ["admin"] = "whiskey123",
        ["user"] = "cocktail"
    };

    public bool IsLoggedIn => !string.IsNullOrEmpty(_username);
    public string? Username => _username;

    public bool Login(string username, string password)
    {
        if (_users.TryGetValue(username.ToLower(), out var storedPassword))
        {
            if (storedPassword == password)
            {
                _username = username;
                return true;
            }
        }
        return false;
    }

    public void Logout()
    {
        _username = null;
    }
}
