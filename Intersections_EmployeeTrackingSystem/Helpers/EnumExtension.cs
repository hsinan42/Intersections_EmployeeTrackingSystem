using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Intersections_EmployeeTrackingSystem.Helpers
{
    public static class EnumExtension
    {
        public static string GetDisplayName(this Enum e)
        {
            var mem = e.GetType().GetMember(e.ToString()).First();
            var attr = mem.GetCustomAttribute<DisplayAttribute>();
            return attr?.GetName() ?? e.ToString();
        }
    }
}
