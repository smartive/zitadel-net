using Microsoft.AspNetCore.Mvc;

namespace Zitadel.WebApi.Controller;

[Route("unauthed")]
public class UnauthorizedApi : ControllerBase
{
    [HttpGet]
    public object UnAuthedGet()
        => new { Ping = "Pong", Timestamp = DateTime.Now };
}
