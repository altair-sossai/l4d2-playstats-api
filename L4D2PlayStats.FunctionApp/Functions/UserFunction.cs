using System;
using System.Linq;
using L4D2PlayStats.FunctionApp.Errors;
using L4D2PlayStats.FunctionApp.Extensions;
using L4D2PlayStats.Modules.Auth.Users.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace L4D2PlayStats.FunctionApp.Functions;

public class UserFunction
{
    private readonly IUserService _userService;

    public UserFunction(IUserService userService)
    {
        _userService = userService;
    }

    [FunctionName(nameof(UserFunction) + "_" + nameof(Me))]
    public IActionResult Me([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "me")] HttpRequest httpRequest)
    {
        try
        {
            var user = _userService.EnsureAuthentication(httpRequest.AuthorizationToken());

            return new OkObjectResult(user.Info());
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [FunctionName(nameof(UserFunction) + "_" + nameof(Get))]
    public IActionResult Get([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")] HttpRequest httpRequest)
    {
        try
        {
            _userService.EnsureAuthentication(httpRequest.AuthorizationToken());

            var users = _userService.GetUsers().Select(user => user.Info()).ToList();

            return new OkObjectResult(users);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [FunctionName(nameof(UserFunction) + "_" + nameof(GetById))]
    public IActionResult GetById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/{userId}")] HttpRequest httpRequest,
        string userId)
    {
        try
        {
            _userService.EnsureAuthentication(httpRequest.AuthorizationToken());

            var user = _userService.GetUser(userId);
            if (user == null)
                return new NotFoundResult();

            return new OkObjectResult(user.Info());
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }
}