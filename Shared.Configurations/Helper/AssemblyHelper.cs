using System.Reflection;

namespace Shared.Configurations.Helper;

public static class AssembliesHelper
{
    public static Type[] GetInterfaceTypes<TInterface>(params Assembly[] assemblies)
    {
        if (!typeof(TInterface).IsInterface) throw new Exception("T must be an interface");

        var types = assemblies
            .Where(a => !a.IsDynamic)
            .SelectMany(a => a.GetTypes())
            .Where(x => x is { IsClass: true, IsAbstract: false, IsInterface: false } && x.IsAssignableTo(typeof(TInterface)))
            .ToArray();

        return types;
    }
}