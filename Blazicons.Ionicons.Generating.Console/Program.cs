using Blazicons.Generating;

if (args.Length == 0)
{
    Console.WriteLine("Error: Output directory path is required as the first argument.");
    Environment.Exit(1);
    return;
}

var outputDirectory = args[0];

Console.WriteLine($"Generating Ionicons to: {outputDirectory}");

var downloader = new RepoDownloader(new Uri("https://github.com/ionic-team/ionicons/archive/refs/heads/main.zip"));

try
{
    await downloader.Download(@"^src\/svg\/.*.svg$").ConfigureAwait(false);

    var svgFolder = Path.Combine(downloader.ExtractedFolder, $"{downloader.RepoName}-{downloader.BranchName}", "src", "svg");

    // Generate the icons class file
    GenerateIconsClass(svgFolder, outputDirectory);
}
finally
{
    downloader.CleanUp();
}

Console.WriteLine("Icon generation completed successfully.");

static void GenerateIconsClass(string svgFolder, string outputDirectory)
{
    // Maintain the same directory structure as the source generator
    var generatorPath = Path.Combine(
        outputDirectory,
        "Blazicons.Ionicons.Generating",
        "Blazicons.Ionicons.Generating.IoniconsGenerator");

    var outputFilePath = Path.Combine(generatorPath, "Ionicon.g.cs");

    // Generate the code
    BlaziconsClassGenerator.GenerateClassFile(outputFilePath, "Ionicon", svgFolder);
}
