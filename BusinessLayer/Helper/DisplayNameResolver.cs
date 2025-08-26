using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BusinessLayer.Helper
{
    public static class DisplayNameResolver
    {
        private static readonly Regex Indexer = new(@"\[\d+\]", RegexOptions.Compiled);

        public static string Resolve(Type rootType, string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return path;
            // Locations[2].City -> Locations.City
            var clean = Indexer.Replace(path, "");
            var parts = clean.Split('.');

            Type? currentType = rootType;
            PropertyInfo? lastProp = null;

            foreach (var part in parts)
            {
                if (currentType == null) break;

                var prop = currentType.GetProperty(part, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop == null) break;

                lastProp = prop;
                currentType = GetElementType(prop.PropertyType);
            }

            if (lastProp == null) return path;

            var disp = lastProp.GetCustomAttribute<DisplayAttribute>()?.GetName();
            if (!string.IsNullOrWhiteSpace(disp)) return disp!;
            var dispName = lastProp.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
            return string.IsNullOrWhiteSpace(dispName) ? lastProp.Name : dispName!;
        }

        private static Type GetElementType(Type t)
        {
            var nt = Nullable.GetUnderlyingType(t) ?? t;

            if (nt.IsArray) return nt.GetElementType() ?? nt;

            if (nt.IsGenericType)
            {
                var gen = nt.GetGenericArguments();
                if (typeof(System.Collections.IEnumerable).IsAssignableFrom(nt) && gen.Length == 1)
                    return gen[0];
            }
            return nt;
        }
    }
}
