using System.Linq;
using GlobExpressions;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
    const short MaxReleaseNoteLength = 30000;

    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("Version of the nuget package to build.")] readonly string Version = string.Empty;

    [Parameter("Optional release notes to append.")] readonly string ReleaseNotes = string.Empty;

    [Parameter("ApiKey to publish the packages.")] readonly string NugetApiKey = string.Empty;

    [Parameter("Nuget source to publish to.")] readonly string NugetSource = string.Empty;

    [Solution] readonly Solution Solution;

    Project MainProject =>
        Solution.AllProjects.First(p => p.Name == "Zitadel" && p.SolutionFolder.Name == "src");

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    string PackageReleaseNotes => (ReleaseNotes.Length > MaxReleaseNoteLength
        ? ReleaseNotes.Substring(0, MaxReleaseNoteLength)
        : ReleaseNotes)
        .Replace(",", "%2c")
        .Replace(";", "%3b");

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .Executes(() => DotNetRestore(s => s
            .SetProjectFile(Solution)));

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() => DotNetBuild(s => s
            .SetProjectFile(Solution)
            .SetConfiguration(Configuration)
            .EnableNoRestore()));

    Target Test => _ => _
        .DependsOn(Clean, Compile)
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Solution)
            .SetConfiguration(Configuration)
            .SetProperty("CollectCoverage", true)
            .EnableNoRestore()
            .EnableNoBuild()));

    Target Pack => _ => _
        .DependsOn(Clean, Restore)
        .Requires(() => !string.IsNullOrWhiteSpace(Version))
        .Executes(() => DotNetPack(s => s
            .SetConfiguration(Configuration.Release)
            .SetVersion(Version)
            .SetPackageReleaseNotes(PackageReleaseNotes)
            .SetOutputDirectory(ArtifactsDirectory)
            .SetProject(MainProject)));

    Target Publish => _ => _
        .Requires(
            () => !string.IsNullOrWhiteSpace(NugetApiKey) &&
                  !string.IsNullOrWhiteSpace(NugetSource))
        .Executes(() => DotNetNuGetPush(s => s
            .SetSource(NugetSource)
            .SetApiKey(NugetApiKey)
            .CombineWith(Glob.Files(ArtifactsDirectory, "*.nupkg"), (ss, package) => ss
                .SetTargetPath(ArtifactsDirectory / package))));
}
