using System.Text.Json;
using Imagine.Core.Configurations;
using Imagine.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace Imagine.Infrastructure.Persistence.TypeConfigurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    private const string UsersSeedDataFile = "Users.json";
    private readonly AppSettings _appSettings;

    public UserEntityTypeConfiguration(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }

    public void Configure(EntityTypeBuilder<User> builder)
    {
        EnsureUserDataCreated(builder);
    }
    
    // Works during database update.
    private void EnsureUserDataCreated(EntityTypeBuilder<User> builder)
    {
        var usersDataFile = Path.Join(_appSettings.ExecutionDirectory, _appSettings.SeedFilesDirectory,
            UsersSeedDataFile);
        using var reader = new StreamReader(usersDataFile);
        var usersJson = reader.ReadToEnd();
        var users = JsonSerializer.Deserialize<List<User>>(usersJson);
        builder.HasData(users);
    }
}
