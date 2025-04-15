using GeneratorMachine.Models;
using GeneratorMachine.Services.Helpers;
using MachinesWrapper;

namespace GeneratorMachine.Services.Factories.DoorFactories;

public class GlassFactory : IComponentFactory
{
    private readonly int _glassType;
    private readonly string _subTypeName;

    public GlassFactory(int glassType, string subTypeName)
    {
        _glassType = glassType;
        _subTypeName = subTypeName;
    }

    public string Type => "Glass";
    public string SubType => _subTypeName;

    public Component CreateComponent()
    {
        var details = MachineDriverWrapper.CreateGlass(_glassType);

        return new Component
        {
            Type = Type,
            SubType = SubType,
            Instructions = details
        };
    }
}