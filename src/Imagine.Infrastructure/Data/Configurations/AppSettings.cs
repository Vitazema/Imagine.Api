﻿using System.Reflection;

namespace Imagine.Infrastructure.Data.Configurations;

public class AppSettings
{
    public string ArtsSeedDataFile = "Arts.json";

    /// <summary>
    /// Thread safe stored value for ExecutionDirectory
    /// </summary>
    private static readonly Lazy<string> _executionDirectory = new(() =>
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
    public string ExecutionDirectory => _executionDirectory.Value;
    public string SeedFilesDirectory { get; set; }
    public string InMemoryDatabaseProviderName { get; set; }
    public string User { get; set; }
    public string Pass { get; set; }

}
