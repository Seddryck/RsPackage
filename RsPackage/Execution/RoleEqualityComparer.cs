using RsPackage.ReportingService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RsPackage.Execution
{
    class RoleEqualityComparer : IEqualityComparer<Role>
    {
        public bool Equals(Role x, Role y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null | y == null)
                return false;
            else if (x.Name == y.Name)
                return true;
            else
                return false;
        }

        public int GetHashCode(Role obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
