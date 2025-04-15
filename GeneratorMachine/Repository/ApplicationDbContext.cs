using GeneratorMachine.Models;
using Microsoft.EntityFrameworkCore;
namespace GeneratorMachine.DataAccess;

public class ApplicationDbContext : DbContext 
{
    // define this class to EF this is table in DB
    public DbSet<Component> Components { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
          => options.UseSqlServer("Data Source =.; Initial Catalog =CarFactorySystem; Integrated Security = True; Connect Timeout = 30; Encrypt=False;Trust Server Certificate=true;");
}
