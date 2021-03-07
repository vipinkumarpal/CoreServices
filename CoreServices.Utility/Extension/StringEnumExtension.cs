using System;
using System.ComponentModel;
using System.Globalization;

namespace CoreServices.Utility.Extension
{
    public static class StringEnumExtension
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            string description = null;
            if(e is Enum)
            {
                Type type = e.GetType();
                Array values = Enum.GetValues(type);

                foreach(int val in values)
                {
                    if(val == e. ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAtrribute = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if(descriptionAtrribute.Length > 0)
                        {
                            description = ((DescriptionAttribute)descriptionAtrribute[0]).Description;
                        }
                    }
                }

            }
            return description;
        }
    }
}
