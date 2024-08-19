using Microsoft.EntityFrameworkCore;
using reciever.Core.Entities;
namespace reciever.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }

    public DbSet<MessageModel> Messages { get; set; } = null!;


}


