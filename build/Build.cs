using System.Linq;
using Nuke.Common.Tools.DotNet;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Compile);

    AbsolutePath SourceDir => RootDirectory / "src";
    AbsolutePath TestDir => RootDirectory / "tests";
    AbsolutePath OutDir => RootDirectory / "out";
    
    Target Clean => _ => _
        .Executes(() =>
        {
            DeleteDirectories(GlobDirectories(SourceDir, "*/wwwroot/*"));
            DeleteDirectories(GlobDirectories(SourceDir, "*/bin/*", "*/obj/*"));
            DeleteDirectories(GlobDirectories(TestDir, "*/bin", "*/obj"));
        });
    
    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s);
        });

    /// <summary>
    /// Building solution with version information included
    /// </summary>
    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s.AddProperty("nodeReuse", "false").SetConfiguration("Release"));
        });

    /// <summary>
    /// Running all tests with coverlet params 
    /// </summary>
    /// <seealso>
    ///     https://github.com/tonerdo/coverlet
    /// </seealso>
    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            GlobFiles("tests", "*.Tests.*proj").NotEmpty()
                .ForEach(testProjectFile =>
                    DotNetTest(s => s
                            .SetProjectFile(testProjectFile)
                            .SetArgumentConfigurator(args => args
                                    .Add("/p:CollectCoverage=true")
                                    .Add("/p:CoverletOutputFormat=opencover"))));
        });

    /// <summary>
    /// Target used inside Dockerfile to create signed asseblies
    /// </summary>
    Target Publish => _ => _
        .Executes(() =>
        {
            var globFiles = GlobFiles(SourceDir, "*.fsproj");
            var projectFile = globFiles.First();
            Logger.Info($"Project File: {projectFile}");
            Logger.Info($"Output Dir: {OutDir}");

            DotNetPublish(s =>
                s
                .SetOutput(OutDir)
                .SetProject(projectFile));
        });

    /// <summary>
    /// Removes node_modules folder in src/
    /// </summary>
    Target nr => _ => _
        .Executes(() =>
        {
            DeleteDirectories(GlobDirectories(SourceDir, "*/node*"));
        });
}
