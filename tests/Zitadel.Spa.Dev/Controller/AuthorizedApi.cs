using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zitadel.Spa.Dev.Controller
{
    [Route("authed")]
    public class AuthorizedApi : ControllerBase
    {
        [HttpGet("jwt")]
        [Authorize(AuthenticationSchemes = "ZitadelAuthHandlerJWT")]
        public object JwtAuthedGet()
            => Result();

        [HttpGet("bearer")]
        [Authorize(AuthenticationSchemes = "ZitadelAuthHandlerBearer")]
        public object BearerAuthedGet()
            => Result();

        private object Result() => new
        {
            Ping = "Pong",
            Timestamp = DateTime.Now,
            AuthType = User.Identity?.AuthenticationType,
            UserName = User.Identity?.Name,
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList(),
            IsInAdminRole = User.IsInRole("Admin"),
            IsInUserRole = User.IsInRole("User"),
        };
    }
}
