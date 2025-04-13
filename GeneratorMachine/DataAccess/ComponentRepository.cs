using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GeneratorMachine.DataAccess;

public class ComponentRepository : IComponentRepository 
{
    private readonly ApplicationDbContext _context = new ApplicationDbContext();

    public void AddComponent(Component component)
    {
         _context.Components.Add(component);

         _context.SaveChanges();
    }
}
