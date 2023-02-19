namespace L4D2PlayStats.Core.Modules.Auth.Commands;

public class AuthenticationCommand
{
    public AuthenticationCommand(string? token)
    {
        if (string.IsNullOrEmpty(token))
            return;

        var segments = token.Split(':', 2);

        ServerId = segments.FirstOrDefault();
        ServerSecret = segments.LastOrDefault();
    }

    public string? ServerId { get; }
    public string? ServerSecret { get; }
    public bool Valid => !string.IsNullOrEmpty(ServerId) && !string.IsNullOrEmpty(ServerSecret);
}