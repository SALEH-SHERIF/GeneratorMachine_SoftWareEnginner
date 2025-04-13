using GeneratorMachine.DataAccess;
using GeneratorMachine.Services.Helpers;
using MachinesWrapper;
namespace GeneratorMachine.Services.Factories.WheelFactories
{

    public class ThinWheelFactory : IComponentFactory
    {
        public string Type => "Wheel";
        public string SubType => "ThinWheel";

        public Component CreateComponent()
        {
            var details = MachineDriverWrapper.CreateThinWheel();
            return new Component
            {
                Type = Type,
                SubType = SubType,
                Instructions = details
            };
        }
    }
}
