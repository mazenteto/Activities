using System;
using System.Data.Common;
using System.Security.Claims;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security;

public class UserAccessor(IHttpContextAccessor httpContextAccessor, AppDBContext dBContext) : IUserAccessor
{
    public async Task<User> GetUserAsync()
    {
        return await dBContext.Users.FindAsync(GetUserID())
            ?? throw new UnauthorizedAccessException("No user is logged in") ;
    }

    public string GetUserID()
    {
        return httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) 
            ?? throw new Exception("No User found");
    }

    public async Task<User> GetUserWithPhotosAsync()
    {
        var UserId = GetUserID();
        return await dBContext.Users
            .Include(x=>x.Photos)
            .FirstOrDefaultAsync(x=>x.Id == UserId)
            ?? throw new UnauthorizedAccessException("No user is logged in") ;
    }
}
