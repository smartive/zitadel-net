using System.Security.Claims;

using NSubstitute;

using Xunit;

using Zitadel.Extensions;

namespace Zitadel.Test.Extensions;

public class ClaimsPrincipalExtensionsTest
{
    private readonly ClaimsPrincipal claimsPrincipal;

    public ClaimsPrincipalExtensionsTest()
    {
        claimsPrincipal = Substitute.For<ClaimsPrincipal>();
        claimsPrincipal.IsInRole("negative").Returns(false);
        claimsPrincipal.IsInRole("positive").Returns(true);
    }

    [Fact]
    public void IsInSingleRole()
    {
        bool actual = ClaimsPrincipalExtensions.IsInRole(claimsPrincipal, new[] { "positive" });

        Assert.True(actual);
        claimsPrincipal.Received(1).IsInRole("positive");
        Assert.Single(claimsPrincipal.ReceivedCalls());
    }

    [Fact]
    public void IsInOneOfTheGivenRoles()
    {
        bool actual = ClaimsPrincipalExtensions.IsInRole(claimsPrincipal, new[] { "negative", "positive" });

        Assert.True(actual);
        claimsPrincipal.Received(1).IsInRole("positive");
        claimsPrincipal.Received(1).IsInRole("negative");
        Assert.Equal(2, claimsPrincipal.ReceivedCalls().Count());
    }

    [Fact]
    public void IsNotInRole()
    {
        bool actual = ClaimsPrincipalExtensions.IsInRole(claimsPrincipal, new[] { "negative" });

        Assert.False(actual);
        claimsPrincipal.Received(1).IsInRole("negative");
        Assert.Single(claimsPrincipal.ReceivedCalls());
    }

    [Fact]
    public void IsNotInNoneOfTheGivenRoles()
    {
        bool actual =
            ClaimsPrincipalExtensions.IsInRole(claimsPrincipal, new[] { "negative", "negative", "negative" });

        Assert.False(actual);
        claimsPrincipal.Received(3).IsInRole("negative");
        Assert.Equal(3, claimsPrincipal.ReceivedCalls().Count());
    }

    [Fact]
    public void IsFalseForNoGivenRoles()
    {
        bool actual = ClaimsPrincipalExtensions.IsInRole(claimsPrincipal, Array.Empty<string>());

        Assert.False(actual);
        Assert.Empty(claimsPrincipal.ReceivedCalls());
    }
}
