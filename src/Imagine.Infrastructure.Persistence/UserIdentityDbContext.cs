using Imagine.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Imagine.Infrastructure.Persistence;

public class UserIdentityDbContext : IdentityDbContext<User>
{

    public UserIdentityDbContext(DbContextOptions<UserIdentityDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Identity");
    }
}
