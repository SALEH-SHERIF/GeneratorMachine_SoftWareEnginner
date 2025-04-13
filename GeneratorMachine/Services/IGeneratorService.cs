namespace GeneratorMachine.Services
{
    public interface IGeneratorService
    {
        void CreateComponent(string type, string subType);
        IEnumerable<string> GetAllComponentTypes();
        public IEnumerable<string> GetSubTypesForType(string type);
    }
}