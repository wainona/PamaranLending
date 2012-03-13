using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public partial class SystemSettingType
    {
        public static IQueryable<SystemSettingType> GetByName(SystemSettingType type)
        {
            return Context.SystemSettingTypes.Where(entity => entity.Name == type.Name);
        }

        public static SystemSettingType GetByName(string type)
        {
            return Context.SystemSettingTypes.FirstOrDefault(entity => entity.Name == type);
        }
    }
}
