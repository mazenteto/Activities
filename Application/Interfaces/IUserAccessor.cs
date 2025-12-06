using System;
using Domain;

namespace Application.Interfaces;

public interface IUserAccessor
{
    string GetUserID();
    Task<User> GetUserAsync();
    Task<User> GetUserWithPhotosAsync();

}
