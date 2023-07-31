﻿using Imagine.Core.Entities;

namespace Imagine.Core.Contracts;

public record UserDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Token { get; set; }
    public UserPermission Permission { get; set; }
    public SubscriptionDto Subscription { get; set; }
    public Role Role { get; set; }
}