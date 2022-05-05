using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

public static class DerivedClassFinder
{
    public static List<Type> FindAllChildrenTypes<T>()
    {
        var superClass = typeof(T);
        var assembly = Assembly.GetAssembly(typeof(T));
        return assembly
            .GetTypes()
            .Where(t => t.BaseType == superClass)
            .ToList();
    }

    public static List<Type> FindAllDerivedTypes<T>()
    {
        var derivedType = typeof(T);
        var assembly = Assembly.GetAssembly(typeof(T));
        return assembly
            .GetTypes()
            .Where(t => t != derivedType && derivedType.IsAssignableFrom(t))
            .ToList();
    }

    public static List<Type> FindAllDerivedTypes(Type T)
    {
        var derivedType = T;
        var assembly = Assembly.GetAssembly(T);
        return assembly
            .GetTypes()
            .Where(t => t != derivedType && derivedType.IsAssignableFrom(t))
            .ToList();
    }

    // https://tmont.com/blargh/2011/3/determining-if-an-open-generic-type-isassignablefrom-a-type
    /// <summary>
    /// Determines whether the <paramref name="genericType"/> is assignable from
    /// <paramref name="givenType"/> taking into account generic definitions
    /// </summary>
    public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
    {
        if (givenType == null || genericType == null)
        {
            return false;
        }

        return givenType == genericType
               || givenType.MapsToGenericTypeDefinition(genericType)
               || givenType.HasInterfaceThatMapsToGenericTypeDefinition(genericType)
               || givenType.BaseType.IsAssignableToGenericType(genericType);
    }

    private static bool HasInterfaceThatMapsToGenericTypeDefinition(this Type givenType, Type genericType)
    {
        return givenType
            .GetInterfaces()
            .Where(it => it.IsGenericType)
            .Any(it => it.GetGenericTypeDefinition() == genericType);
    }

    private static bool MapsToGenericTypeDefinition(this Type givenType, Type genericType)
    {
        return genericType.IsGenericTypeDefinition
               && givenType.IsGenericType
               && givenType.GetGenericTypeDefinition() == genericType;
    }
}