using GeneratorMachine.DataAccess;
using GeneratorMachine.Services.Helpers;
using MachinesWrapper;
namespace GeneratorMachine.Services.Factories.WheelFactories
{
    public class BigWheelFactory : IComponentFactory
    {
        public string Type => "Wheel";
        public string SubType => "BigWheel";

        public Component CreateComponent()
        {
            var details = MachineDriverWrapper.CreateBigWheel();
            return new Component
            {
                Type = Type,
                SubType = SubType,
                Instructions = details
            };
        }
    }
}
