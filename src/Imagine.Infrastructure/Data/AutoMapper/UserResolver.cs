﻿using AutoMapper;
using Imagine.Core.Contracts;
using Imagine.Core.Entities;

namespace Imagine.Infrastructure.Data.AutoMapper;

public class UserResolver : IValueResolver<ArtDto, Art, User>
{
    private readonly ArtDbContext _context;

    public UserResolver(ArtDbContext context)
    {
        _context = context;
    }

    public User Resolve(ArtDto source, Art destination, User destMember, ResolutionContext context)
    {
        return new User()
        {
            FullName = source.User
        };
    }
}