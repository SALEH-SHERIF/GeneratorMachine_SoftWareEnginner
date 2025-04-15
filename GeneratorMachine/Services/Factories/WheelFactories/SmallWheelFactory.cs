

using System.Runtime.InteropServices;
using GeneratorMachine.Models;
using GeneratorMachine.Services.Helpers;
using MachinesWrapper;
namespace GeneratorMachine.Services.Factories.WheelFactories
{
    public class SmallWheelFactory : IComponentFactory
    {
        public string Type => "Wheel";
        public string SubType => "SmallWheel";

        public Component CreateComponent()
        {
            var details =MachineDriverWrapper.CreateSmallWheel();
            return new Component
            {
                Type = Type,
                SubType = SubType,
                Instructions = details
            };
        }
    }
}
