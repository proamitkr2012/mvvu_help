using MVVU_Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVVU_Model.DTO
{
    public class ResultDTO
    {
        public string? CourseType { get; set; }
        public string? ExamTypeName { get; set; }
        public string? Course { get; set; }        
        public bool? IsPG { get; set; }
        public List<CourseMasterDTO_AM> CourseTypeList { get; set; }
        public List<CourseMasterDTO_AM> CourseList { get; set; }
        public List<CourseMasterDTO_AM> CourseAllList { get; set; }
        public List<ExamTyperDTO_AM> ExamTypeList { get; set; }

    }
}