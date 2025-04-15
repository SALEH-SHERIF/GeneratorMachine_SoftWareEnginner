using GeneratorMachine.Models;
using GeneratorMachine.Services.Helpers;
using MachinesWrapper;
namespace GeneratorMachine.Services.Factories.WheelFactories
{
    public class NormalWheelFactory : IComponentFactory
    {
        public string Type => "Wheel";
        public string SubType => "NormalWheel";
        public Component CreateComponent()
        {
            var details = MachineDriverWrapper.CreateNormalWheel();
            return new Component
            {
                Type = Type,
                SubType = SubType,
                Instructions = details
            };
        }
    }
}
