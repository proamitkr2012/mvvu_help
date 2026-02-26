using MVVU_Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVVU_Model.DTO
{
    public class CommonResultDataDTO
    {
        
        public StudentMasterDTO StudentMaster { get; set; }
        public List<STUDENT_MARKS_AMDTO> STUDENT_MARKS { get; set; }
        public List<STUDENT_RESULT_AMDTO> STUDENT_RESULT { get; set; }

    }
}