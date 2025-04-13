

using GeneratorMachine.DataAccess;
using MachinesWrapper;

namespace GeneratorMachine.Services.Factories.DoorFactories;

public class DoorFactory : IComponentFactory
{
    private readonly int _doorType;
    private readonly string _subTypeName;

    public DoorFactory(int doorType, string subTypeName)
    {
        _doorType = doorType;
        _subTypeName = subTypeName;
    }

    public string Type => "Door";
    public string SubType => _subTypeName;

    public Component CreateComponent()
    {
        var details = MachineDriverWrapper.CreateDoor(_doorType) ;
        return new Component
        {
            Type = Type,
            SubType = SubType,
            Instructions = details
        };
    }
}