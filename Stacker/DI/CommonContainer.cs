using Autofac;
using Stacker.Services;

namespace Stacker.DI
{
    public class CommonContainer
    {
        public static IContainer Container { get; private set; }
        public static void Build()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<BluetoothService>().As<IBluetoothService>().SingleInstance();
            builder.RegisterType<NotificationService>().As<INotificationService>().SingleInstance();
            builder.RegisterType<UserSettingsService>().As<IUserSettingsService>().SingleInstance();
            builder.RegisterType<ApplicationWindowsService>().As<IApplicationWindowsService>().SingleInstance();
            builder.RegisterType<AutomationService>().As<IAutomationService>().SingleInstance();
            builder.RegisterType<DayDataService>().As<IDayDataService>().SingleInstance();
            builder.RegisterType<DayDataSavingService>().As<IDayDataSavingService>().SingleInstance();
            builder.RegisterType<ComputerModeMonitoringService>().As<IComputerModeMonitoringService>().SingleInstance();

            Container = builder.Build();
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
