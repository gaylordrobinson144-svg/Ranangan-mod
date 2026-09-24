#load "../CommonScripts.csx"

#nullable enable

using System.Collections.Immutable;
using System.IO.Compression;
using System.Text.RegularExpressions;

struct ZipArchiveDirectoryVisitor
{
	public ZipArchive Archive { get; init; }
	public ImmutableHashSet<string> FilesToExclude { get; init; }
	public string BaseName { get; init; }
	public string EntryName { get; init; }

	private void CreateEntryFromAny(string sourceName)
	{
		if (FilesToExclude.Contains(sourceName))
		{
			Console.WriteLine($"Skipping file or directory \"{sourceName}\"");
			return;
		}

		if (File.GetAttributes(sourceName).HasFlag(FileAttributes.Directory))
		{
			foreach (var file in Directory.GetFiles(sourceName).Concat(Directory.GetDirectories(sourceName)).ToArray())
			{
				CreateEntryFromAny(file);
			}
		}
		else
		{
			var entryName = EntryName != "" ? sourceName.Replace(BaseName, EntryName) : "";
			Archive.CreateEntryFromFile(sourceName, entryName);
		}
	}

	public static void CreateEntryFromAny(ZipArchive archive, ImmutableHashSet<string> filesToExclude, string sourceName, string entryName = "")
	{
		var visitor = new ZipArchiveDirectoryVisitor
		{
			Archive = archive,
			FilesToExclude = filesToExclude,
			BaseName = sourceName,
			EntryName = entryName
		};

		visitor.CreateEntryFromAny(sourceName);
	}
}

/// <summary>
/// Creates a mod package from a mod installation.
/// </summary>
sealed class Packager : IDisposable
{
	private const string PackageExtension = ".zip";
	private readonly ImmutableHashSet<string> _filesToExclude;
	private readonly ZipArchive _archive;

	public string PackageName { get; }

	public Packager(string packageName, IEnumerable<string> filesToExclude)
	{
		PackageName = packageName + PackageExtension;

		_filesToExclude = filesToExclude.ToImmutableHashSet();

		RemovePackageFile();

		Console.WriteLine($"Creating archive {PackageName}");
		_archive = ZipFile.Open(PackageName, ZipArchiveMode.Create);
	}

	public void Dispose() => _archive.Dispose();

	private void RemovePackageFile()
	{
		if (File.Exists(PackageName))
		{
			Console.WriteLine($"Removing archive {PackageName}");
			File.Delete(PackageName);
		}
	}

	public void AddFiles(IEnumerable<string> files)
	{
		foreach (var file in files)
		{
			var completePath = file;

			//If the file doesn't exist then it might be game-specific, so the packager should just ignore it.
			if (!File.Exists(completePath) && !Directory.Exists(completePath))
			{
				Console.WriteLine($"Skipping \"{completePath}\" because it does not exist");
				continue;
			}

			if (File.GetAttributes(completePath).HasFlag(FileAttributes.Directory))
			{
				Console.WriteLine($"Adding directory \"{completePath}\"");
			}
			else
			{
				Console.WriteLine($"Adding file \"{completePath}\"");
			}

			var newName = completePath;

			// Files ending with ".install" need to be renamed.
			newName = Regex.Replace(newName, "\\.install$", "");

			if (completePath != newName)
			{
				Console.WriteLine($"Renaming \"{completePath}\" to \"{newName}\"");
			}

			ZipArchiveDirectoryVisitor.CreateEntryFromAny(_archive, _filesToExclude, completePath, newName);
		}
	}
}
