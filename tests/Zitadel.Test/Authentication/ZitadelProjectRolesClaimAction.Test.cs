using System.Security.Claims;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Xunit;
using Zitadel.Authentication;

namespace Zitadel.Test.Authentication;

public class ZitadelProjectRolesClaimActionTest
{
    private readonly ClaimAction _action = new ZitadelProjectRolesClaimAction();

    [Fact]
    public void Should_Not_Throw_On_Missing_Roles()
    {
        _action.Run(WithoutRoles, new(), string.Empty);
    }

    [Fact]
    public void Should_Not_Add_Something_On_Missing_Roles()
    {
        var identity = new ClaimsIdentity();
        _action.Run(WithoutRoles, identity, string.Empty);
        identity
            .Claims
            .Any(c => c.Type == ClaimTypes.Role)
            .Should()
            .BeFalse();
    }

    [Fact]
    public void Should_Add_Roles()
    {
        var identity = new ClaimsIdentity();
        _action.Run(WithRoles, identity, string.Empty);
        identity
            .Claims
            .Any(c => c.Type == ClaimTypes.Role)
            .Should()
            .BeTrue();
        var principal = new ClaimsPrincipal(identity);
        principal.IsInRole("Admin").Should().BeTrue();
        principal.IsInRole("User").Should().BeTrue();
    }

    private JsonElement WithRoles => JsonDocument.Parse(
            @"
{
  ""iss"": ""https://issuer.zitadel.ch"",
  ""urn:zitadel:iam:org:project:roles"": {
    ""Admin"": {
      ""foo"": ""bar""
    },
    ""User"": {
      ""foo"": ""bar""
    }
  }
}
")
        .RootElement;

    private JsonElement WithoutRoles => JsonDocument.Parse(
            @"{""iss"": ""https://issuer.zitadel.ch""}")
        .RootElement;
}
