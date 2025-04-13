using GeneratorMachine.DataAccess;
using GeneratorMachine.Services;
using GeneratorMachine.Services.Factories;
using GeneratorMachine.Services.Factories.DoorFactories;
using GeneratorMachine.Services.Factories.WheelFactories;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GeneratorMachine
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var services = new ServiceCollection();


            var dbContext = new ApplicationDbContext(); 

            services.AddScoped<IComponentRepository, ComponentRepository>();

            services.AddSingleton<IComponentFactory, SmallWheelFactory>();

            services.AddSingleton<IComponentFactory, BigWheelFactory>();

            services.AddSingleton<IComponentFactory, ThinWheelFactory>();

            services.AddSingleton<IComponentFactory, NormalWheelFactory>();

            services.AddSingleton<IComponentFactory>(new DoorFactory(0, "Steel"));
            services.AddSingleton<IComponentFactory>(new DoorFactory(1, "Titanium"));
            services.AddSingleton<IComponentFactory>(new DoorFactory(2, "Aluminum"));
            services.AddSingleton<IComponentFactory>(new DoorFactory(3, "Plastic"));

            services.AddSingleton<IComponentFactory>(new MotorFactory(0, "10 Horse"));
            services.AddSingleton<IComponentFactory>(new MotorFactory(1, "50 Horse"));
            services.AddSingleton<IComponentFactory>(new MotorFactory(2, "100 Horse"));

            services.AddSingleton<IComponentFactory>(new GlassFactory(0, "Thin"));
            services.AddSingleton<IComponentFactory>(new GlassFactory(1, "Thick"));
            services.AddSingleton<IComponentFactory>(new GlassFactory(2, "5 Layers"));
            services.AddSingleton<IComponentFactory>(new GlassFactory(3, "Bullet Proof"));

            services.AddScoped<IGeneratorService, GeneratorService>();

            services.AddScoped<MainForm>(provider =>
            {
                var creatorService = provider.GetRequiredService<IGeneratorService>();
                return new MainForm(creatorService);
            });

            using var serviceProvider = services.BuildServiceProvider();

            ApplicationConfiguration.Initialize();
            // Retrieve MainForm from DI container
            var mainForm = serviceProvider.GetRequiredService<MainForm>();
            Application.Run(mainForm);
        }
    }
}