using System;
using System.Reflection;

public static class TypeExtend 
{
    public static Type GetUType(string name)
    {
        foreach (Assembly assembly in System.AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (System.Type type in assembly.GetTypes())
            {
                if (type.Name == name)
                    return type;
            }
        }

        return null;
    }
}
