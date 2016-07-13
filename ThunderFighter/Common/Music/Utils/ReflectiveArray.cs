namespace ThunderFighter.Common.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    // http://stackoverflow.com/questions/5411694/get-all-inherited-classes-of-an-abstract-class
    public static class ReflectiveArray
    {
        public static Type[] GetTypeOfDerivedClasses<T>(params object[] constructorArgs) where T : class
        {
            List<Type> objects = new List<Type>();

            foreach (Type type in Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add(type);
            }

            return objects.ToArray();
        }
    }
}
