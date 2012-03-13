using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public partial class RoleType
    {
        public static RoleType GetById(int id)
        {
            return Context.RoleTypes.SingleOrDefault(entity => entity.Id == id);
        }

        public static RoleType GetByName(string name)
        {
            return Context.RoleTypes.SingleOrDefault(entity => entity.Name == name);
        }

        public static RoleType GetType(RoleType parentRole, RoleType subRole)
        {
            return Context.RoleTypes.SingleOrDefault(entity => entity.Name == subRole.Name && entity.ParentRoleTypeId == parentRole.Id);
        }
    }
}
