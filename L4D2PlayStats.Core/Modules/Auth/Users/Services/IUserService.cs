namespace L4D2PlayStats.Core.Modules.Auth.Users.Services;

public interface IUserService
{
	User EnsureAuthentication(string token);
	User? GetUser(string userId);
	IEnumerable<User> GetUsers();
}