using Imagine.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imagine.Infrastructure.Data.TypeConfigurations;

public class ArtEntityTypeConfiguration : IEntityTypeConfiguration<Art>
{
    public void Configure(EntityTypeBuilder<Art> builder)
    {
    }
}