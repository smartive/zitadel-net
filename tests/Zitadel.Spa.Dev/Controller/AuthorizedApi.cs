using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zitadel.Authorization;

namespace Zitadel.Spa.Dev.Controller
{
    [Route("authed")]
    public class AuthorizedApi : ControllerBase
    {
        [HttpGet("jwt")]
        [Authorize(AuthenticationSchemes = "ZitadelApiJWT", Policy = "CaosUser")]
        public object JwtAuthedGet()
            => Result();

        [HttpGet("bearer")]
        [Authorize(AuthenticationSchemes = "ZitadelApiBearer")]
        public object BearerAuthedGet()
            => Result();

        [HttpGet("hd-jwt")]
        [Authorize(AuthenticationSchemes = "ZitadelApiJWTHostedDomain")]
        public object HdJwtAuthedGet()
            => Result();

        [HttpGet("hd-bearer")]
        [Authorize(AuthenticationSchemes = "ZitadelApiBearerHostedDomain")]
        public object HdBearerAuthedGet()
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
            IsInCaosAdminRole = User.IsInRole("69234230193872955", "Admin"),
            IsInCaosUserRole = User.IsInRole("69234230193872955", "User"),
            IsInOtherAdminRole = User.IsInRole("69234230193871337", "Admin"),
        };
    }
}
