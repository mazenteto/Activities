using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Persistence;

public class AppDBContext(DbContextOptions Options) : IdentityDbContext<User>(Options)
{
    public required DbSet<Activity> Activities { get; set; }
}
