
using GeneratorMachine.DataAccess;
using GeneratorMachine.Services.Helpers;
using MachinesWrapper;
namespace GeneratorMachine.Services.Factories.DoorFactories;

public class MotorFactory : IComponentFactory
{
    private readonly int _motorType;
    private readonly string _subTypeName;

    public MotorFactory(int motorType, string subTypeName)
    {
        _motorType = motorType;
        _subTypeName = subTypeName;
    }

    public string Type => "Motor";
    public string SubType => _subTypeName;

    public Component CreateComponent()
    {
        var details = MachineDriverWrapper.CreateMotorPower(_motorType);

        return new Component
        {
            Type = Type,
            SubType = SubType,
            Instructions = details
        };
    }
}