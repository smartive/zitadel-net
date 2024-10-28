using Zitadel.Authentication.Events.Context;
using Zitadel.Authentication.Handler;

namespace Zitadel.Authentication.Events;

public class LocalFakeZitadelEvents
{
    /// <summary>
    /// Invoked after a ClaimsIdentity has been generated in the <see cref="LocalFakeZitadelHandler"/>.
    /// </summary>
    public Func<LocalFakeZitadelAuthContext, Task> OnZitadelFakeAuth { get; set; } = context => Task.CompletedTask;
}
