using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Zitadel.Authentication;

namespace Zitadel.AspNet.AuthN.Pages;

[Authorize]
public class Authenticated : PageModel
{
    public async Task OnPostAsync()
    {
        await HttpContext.SignOutAsync(
            "Identity.External"); // Options: signs you out of ZITADEL entirely, without this you may not be reprompted for your password.
        await HttpContext.SignOutAsync(
            ZitadelDefaults.AuthenticationScheme,
            new AuthenticationProperties { RedirectUri = "http://localhost:8080/loggedout" }
        );
    }
}
