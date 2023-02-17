using Azure.Data.Tables;
using L4D2PlayStats.Core.Contexts.AzureTableStorage;
using L4D2PlayStats.Core.Modules.Auth.Users.Commands;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Core.Modules.Auth.Users.Services;

public class UserService : IUserService
{
    private readonly IAzureTableStorageContext _context;
    private readonly IMemoryCache _memoryCache;
    private TableClient? _userTable;

    public UserService(IAzureTableStorageContext context,
        IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }

    private TableClient UserTable => _userTable ??= _context.GetTableClientAsync("Users").Result;

    private List<User> Users => _memoryCache.GetOrCreate(nameof(Users), factory =>
    {
        factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

        return UserTable.Query<User>().ToList();
    });

    public User EnsureAuthentication(string token)
    {
        var command = new AuthenticationCommand(token);
        if (!command.Valid)
            throw new UnauthorizedAccessException();

        var user = Users.FirstOrDefault(user => user.RowKey == command.UserId && user.Secret == command.UserSecret);
        if (user == null)
            throw new UnauthorizedAccessException();

        return user;
    }

    public User? GetUser(string userId)
    {
        return Users.FirstOrDefault(user => user.RowKey == userId);
    }

    public IEnumerable<User> GetUsers()
    {
        return Users;
    }
}