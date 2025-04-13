using GeneratorMachine.DataAccess;

namespace GeneratorMachine.Services.Factories;

public interface IComponentFactory
{
    string Type { get; }
    string SubType { get; }
    Component CreateComponent();
}
