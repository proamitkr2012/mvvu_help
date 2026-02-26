using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MVVU_Data.Entities
{
    public class AdminWorkRoleMap_AM
    {
        [Key]
        public int EntryID { get; set; }
        public int? AdminId { get; set; }
        public int? CTypeID { get; set; }
        public bool? IsActive { get; set; }
        [NotMapped]
        public string? TypeName { get; set; }

    }
}
