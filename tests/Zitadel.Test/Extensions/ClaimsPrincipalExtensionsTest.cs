using System.Security.Claims;
using Moq;
using Xunit;
using Zitadel.Extensions;

namespace Zitadel.Test.Extensions;

public class ClaimsPrincipalExtensionsTest
{
    private Mock<ClaimsPrincipal> claimsPrincipal;

    public ClaimsPrincipalExtensionsTest()
    {
        claimsPrincipal = new();
        claimsPrincipal.Setup(c => c.IsInRole("negative")).Returns(false);
        claimsPrincipal.Setup(c => c.IsInRole("positive")).Returns(true);
    }

    [Fact]
    public void IsInSingleRole()
    {
        bool actual = ClaimsPrincipalExtensions.IsInRole(claimsPrincipal.Object, new[] { "positive" });

        Assert.True(actual);
        claimsPrincipal.Verify(c => c.IsInRole("positive"), Times.Once);
        claimsPrincipal.VerifyNoOtherCalls();
    }

    [Fact]
    public void IsInOneOfTheGivenRoles()
    {
        bool actual = ClaimsPrincipalExtensions.IsInRole(claimsPrincipal.Object, new[] { "negative", "positive" });

        Assert.True(actual);
        claimsPrincipal.Verify(c => c.IsInRole("positive"), Times.Once);
        claimsPrincipal.Verify(c => c.IsInRole("negative"), Times.Once);
        claimsPrincipal.VerifyNoOtherCalls();
    }

    [Fact]
    public void IsNotInRole()
    {
        bool actual = ClaimsPrincipalExtensions.IsInRole(claimsPrincipal.Object, new[] { "negative" });

        Assert.False(actual);
        claimsPrincipal.Verify(c => c.IsInRole("negative"), Times.Once);
        claimsPrincipal.VerifyNoOtherCalls();
    }

    [Fact]
    public void IsNotInNoneOfTheGivenRoles()
    {
        bool actual = ClaimsPrincipalExtensions.IsInRole(claimsPrincipal.Object, new[] { "negative", "negative", "negative" });

        Assert.False(actual);
        claimsPrincipal.Verify(c => c.IsInRole("negative"), Times.Exactly(3));
        claimsPrincipal.VerifyNoOtherCalls();
    }
}
