﻿using System.Reflection;

namespace Imagine.Core.Configurations;

public class AppSettings
{
    public string LanAddress { get; set; }
    /// <summary>
    /// Thread safe stored value for ExecutionDirectory
    /// </summary>
    private static readonly Lazy<string> _executionDirectory = new(() =>
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
    public string ExecutionDirectory => _executionDirectory.Value;
    public string StorageDir { get; set; }
    public string SeedFilesDirectory { get; set; }
    public string InMemoryDatabaseProviderName { get; set; }
    public int TempFilesExpirationInDays { get; set; }
    public int QueueCapacity { get; set; }
}
