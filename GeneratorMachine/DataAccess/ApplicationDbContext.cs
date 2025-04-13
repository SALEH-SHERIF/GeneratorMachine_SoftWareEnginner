using Microsoft.EntityFrameworkCore;
namespace GeneratorMachine.DataAccess;

public class ApplicationDbContext : DbContext 
{
    public DbSet<Component> Components { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
          => options.UseSqlServer("Data Source = (localdb)\\ProjectModels; Initial Catalog =CarFactorySystem; Integrated Security = True; Connect Timeout = 30; Encrypt=False;Trust Server Certificate=true;");
}
