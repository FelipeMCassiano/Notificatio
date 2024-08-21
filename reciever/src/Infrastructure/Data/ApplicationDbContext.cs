using Microsoft.EntityFrameworkCore;
using reciever.Core.Entities;
namespace reciever.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    public DbSet<MessageDbModel> Messages { get; set; } = null!;


}


