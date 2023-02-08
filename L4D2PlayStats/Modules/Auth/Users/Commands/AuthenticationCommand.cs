namespace L4D2PlayStats.Modules.Auth.Users.Commands;

public class AuthenticationCommand
{
    public AuthenticationCommand(string? token)
    {
        if (string.IsNullOrEmpty(token))
            return;

        var segments = token.Split(':', 2);

        UserId = segments.FirstOrDefault();
        UserSecret = segments.LastOrDefault();
    }

    public string? UserId { get; }
    public string? UserSecret { get; }
    public bool Valid => !string.IsNullOrEmpty(UserId) && !string.IsNullOrEmpty(UserSecret);
}