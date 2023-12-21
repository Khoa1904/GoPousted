using Microsoft.Extensions.DependencyInjection;

namespace GoShared
{
    public static class ModuleConfigure
    {
        public static void RegisterDependencyInjection(IServiceCollection serviceCollection, System.Reflection.Assembly sourceAssembly)
        {
            // singleton 등록
            sourceAssembly.GetTypes().Where(a => a.IsDefined(typeof(SingletonClassAttribute), true)).ToList().ForEach(a => serviceCollection.AddSingleton(a));
            sourceAssembly.GetTypes().Where(a => a.IsDefined(typeof(TransientClassAttribute), true)).ToList().ForEach(a => serviceCollection.AddTransient(a));

        }

        public static void Initialize(IServiceCollection serviceCollection)
        {
            RegisterDependencyInjection(serviceCollection, typeof(ModuleConfigure).Assembly);
        }
    }
}