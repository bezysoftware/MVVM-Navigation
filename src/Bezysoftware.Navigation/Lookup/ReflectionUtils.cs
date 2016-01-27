namespace Bezysoftware.Navigation.Lookup
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// The reflection utils.
    /// </summary>
    public static class ReflectionUtils
    {
        /// <summary>
        /// Returns all types which have an associated specified attribute
        /// </summary>
        /// <typeparam name="T"> Type of attribute </typeparam>
        public static IEnumerable<KeyValuePair<Type, T>> GetTypesWithAttribute<T>(IEnumerable<Assembly> assemblies)
            where T : Attribute
        {
            return
                from a in assemblies
                from t in a.DefinedTypes
                from attrs in t.GetCustomAttributes(typeof(T), true)
                where attrs != null
                select new KeyValuePair<Type, T>(t.AsType(), attrs as T);
        }
    }
}