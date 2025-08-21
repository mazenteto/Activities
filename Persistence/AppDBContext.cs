using Domain;
using Microsoft.EntityFrameworkCore;
namespace Persistence;

public class AppDBContext(DbContextOptions Options) : DbContext(Options)
{
    public required DbSet<Activity> Activities { get; set; }
}
