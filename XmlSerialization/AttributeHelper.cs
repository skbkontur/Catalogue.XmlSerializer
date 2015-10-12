using System.Linq;
using System.Reflection;

namespace SKBKontur.Catalogue.XmlSerialization
{
    public static class AttributeHelper
    {
        public static T GetSingleAttribute<T>(this object obj, bool inherit = false)
            where T : class
        {
            return GetSingleAttribute<T>(obj.GetType(), inherit);
        }

        public static T[] GetAttributes<T>(this object obj, bool inherit = false)
            where T : class
        {
            return GetAttributes<T>(obj.GetType(), inherit);
        }

        public static T GetSingleAttribute<T>(this ICustomAttributeProvider prov, bool inherit = false)
            where T : class
        {
            return prov.GetCustomAttributes(typeof(T), inherit).Cast<T>().Single();
        }

        public static T[] GetAttributes<T>(this ICustomAttributeProvider prov, bool inherit = false)
            where T : class
        {
            return prov.GetCustomAttributes(typeof(T), inherit).Cast<T>().ToArray();
        }
    }
}