#nullable enable

using System.Runtime.CompilerServices;

//From the dotnet script README.
public static string GetScriptPath([CallerFilePath] string path = "") => path;
public static string GetScriptFolder([CallerFilePath] string path = "") => Path.GetDirectoryName(path)!;
