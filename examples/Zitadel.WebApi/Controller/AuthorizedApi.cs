using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Zitadel.Authentication;

namespace Zitadel.WebApi.Controller;

[Route("authed")]
public class AuthorizedApi : ControllerBase
{
    [HttpGet("jwt")]
    [Authorize(AuthenticationSchemes = "ZITADEL_JWT")]
    public object JetGet()
        => Result();

    [HttpGet("basic")]
    [Authorize(AuthenticationSchemes = "ZITADEL_BASIC")]
    public object BasicGet()
        => Result();

    [HttpGet("mock")]
    [Authorize(AuthenticationSchemes = "ZITADEL_FAKE")]
    public object FakeGet()
        => Result();

    private object Result() => new
    {
        Ping = "Pong",
        Timestamp = DateTime.Now,
        AuthType = User.Identity?.AuthenticationType,
        UserName = User.Identity?.Name,
        UserId = User.FindFirstValue(OidcClaimTypes.Subject),
        Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList(),
        IsInAdminRole = User.IsInRole("Admin"),
        IsInUserRole = User.IsInRole("User"),
    };
}
