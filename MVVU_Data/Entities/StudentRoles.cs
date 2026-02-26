using System;
using System.Collections.Generic;
using System.Text;

namespace MVVU_Data.Entities
{
    public class StudentRoles
    {
        public long StudId { get; set; }
        public int RoleId { get; set; }
        public virtual Student Student { get; set; }
        public virtual Role Role { get; set; }
    }

    public class AdminMasterRoles
    {
        public int AdminId { get; set; }
        public int RoleId { get; set; }
        public virtual AdminMaster Admin { get; set; }
        public virtual Role Role { get; set; }
    }

}
