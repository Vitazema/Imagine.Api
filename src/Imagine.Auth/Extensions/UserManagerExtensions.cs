﻿using System.Security.Claims;
using Imagine.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Imagine.Auth.Extensions;

public static class UserManagerExtensions
{
    public static async Task<User?> FindUserByClaimsPrincipleWithSubscriptionAsync(this UserManager<User> input, ClaimsPrincipal user)
    {
        var userName = user.FindFirstValue(ClaimTypes.Name);
        if (userName == null) throw new InvalidOperationException("User not found or unauthorized");
        
        return await input.Users.Include(x => x.Subscription)
            .FirstOrDefaultAsync(x => x.UserName == userName);
    }

    public static async Task<User?> FindUserByNameWithSubscriptionAsync(this UserManager<User> input,
        string userName)
    {
        if (userName == null) throw new InvalidOperationException("User not found or unauthorized");
        var user = await input.Users.Include(x => x.Subscription)
            .FirstOrDefaultAsync(x => x.UserName == userName);
        return user;
    }
}