using System.ComponentModel;
using System.Reflection;

namespace Sales.Helpers
{
    public static class EnumHelpers
    {
        public static string GetDescription(this Enum enumerator)
        {
            var item = enumerator.GetType().GetField(enumerator.ToString());
            var field = item.GetCustomAttribute<DescriptionAttribute>();

            if (field == null)
            {
                return enumerator.ToString();
            }
            return field.Description.ToString();
        }
    }
}
