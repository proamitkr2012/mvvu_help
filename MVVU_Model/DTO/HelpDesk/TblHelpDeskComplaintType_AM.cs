using MVVU_Model.CustomAttribute;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MVVU_Model.DTO.HelpDesk
{
    public class TblHelpDeskComplaintType_AM
    {
        public int CTypeID { get; set; }
        public string? TypeName { get; set; }
        public string? Details { get; set; }       
        public string? AlertMsg { get; set; }
        public bool? IsActive { get; set; }
    }
}
