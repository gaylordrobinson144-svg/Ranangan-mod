#r "nuget: HalfLife.UnifiedSdk.Utilities, 0.1.3"
#r "nuget: System.CommandLine, 2.0.0-beta3.22114.1"

#load "GameContentInstaller.csx"

#nullable enable

using HalfLife.UnifiedSdk.Utilities.Games;
using System.CommandLine;

// Installs game content into a mod installation.
const string InstallAllGamesName = "all";

static readonly string RootDirectory = Path.GetDirectoryName(Path.GetDirectoryName(GetScriptFolder()))
		?? throw new InvalidOperationException("Couldn't get root directory");

//List of games whose content can be installed with this tool.
static readonly IEnumerable<GameInstallData> Games = new[]
{
	new GameInstallData(ValveGames.HalfLife1),
	new GameInstallData(ValveGames.OpposingForce, MapEntFiles: "op4_map_ent_files.zip"),
	new GameInstallData(ValveGames.BlueShift, MapEntFiles: "bs_map_ent_files.zip")
};

// Command line setup.
static string GetAvailableGamesText()
{
	var builder = new StringBuilder();

	builder.AppendLine("Games supported by this installer:");
	builder.AppendLine("<Game name>: <name to specify>");

	foreach (var game in Games)
	{
		var installed = GameContentInstaller.IsGameInstalled(RootDirectory, game.Info.ModDirectory);
		builder.AppendLine($"{game.Info.Name}: {game.Info.ModDirectory} (original game {(installed ? "installed" : "not installed")})");
	}

	return builder.ToString();
}

var gameOption = new Option<string>(
		"--game",
		description: "The name of a game's mod directory to install that game's content, "
		+ $"or \"{InstallAllGamesName}\" to install all game content\n" +
		GetAvailableGamesText())
{
	IsRequired = true
};

gameOption.AddValidator((result) =>
{
	var game = result.GetValueForOption(gameOption)!;

	if (game != InstallAllGamesName && !Games.Any(g => g.Info.ModDirectory == game))
	{
		result.ErrorMessage = $"Invalid game \"{game}\" specified.";
	}
});

var dryRunOption = new Option<bool>("--dry-run", description: "If provided no file changes will be written to disk");

var rootCommand = new RootCommand("Half-Life game content installer")
{
	gameOption,
	dryRunOption
};

rootCommand.SetHandler((string game, bool dryRun) =>
{
	if (dryRun)
	{
		Console.WriteLine("Performing dry run.");
	}

	var gamesToInstall = Games;

	if (game != InstallAllGamesName)
	{
		gamesToInstall = gamesToInstall.Where(g => g.Info.ModDirectory == game);
	}

	var installer = new GameContentInstaller
	{
		IsDryRun = dryRun
	};

	installer.Install(RootDirectory, gamesToInstall);
}, gameOption, dryRunOption);

return rootCommand.Invoke(Args.ToArray());
