
using GeneratorMachine.DataAccess;
using GeneratorMachine.Services.Factories;

namespace GeneratorMachine.Services;
// solid -> s , d , i 
public class GeneratorService : IGeneratorService
{
    private readonly IComponentRepository _repository;
    private readonly IEnumerable<IComponentFactory> _factories;

    public GeneratorService(IComponentRepository repository, IEnumerable<IComponentFactory> factories)
    {
        _repository = repository;
        _factories = factories;
    }

    public void CreateComponent(string type, string subType)
    {
        var factory = _factories.FirstOrDefault(f => f.Type == type && f.SubType == subType);
        if (factory == null) throw new KeyNotFoundException("Component type not found");

        var component = factory.CreateComponent();
        _repository.AddComponent(component);
    }

    public IEnumerable<string> GetAllComponentTypes()
    {
        return _factories
            .Select(f => f.Type)
            .Distinct()
            .ToList();
    }

    public IEnumerable<string> GetSubTypesForType(string type)
    {
        return _factories
            .Where(f => f.Type == type)
            .Select(f => f.SubType)
            .ToList();
    }

}

