using System.Reflection;

namespace Shared.Helper;

/// <summary>
/// Helper class for retrieving types that implement a specified interface from given assemblies.
/// </summary>
public static class AssembliesHelper
{
    /// <summary>
    /// Retrieves all non-abstract class types that implement the specified interface from the given assemblies.
    /// </summary>
    /// <typeparam name="TInterface">The interface type to search for.</typeparam>
    /// <param name="assemblies">The assemblies to scan for types.</param>
    /// <returns>An array of types that implement the specified interface.</returns>
    /// <exception cref="ArgumentException">Thrown when TInterface is not an interface.</exception>
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