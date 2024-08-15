using Microsoft.EntityFrameworkCore;
using reciever.Models;
namespace reciever.Db;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }

    public DbSet<MessageModel> Messages { get; set; } = null!;


}


