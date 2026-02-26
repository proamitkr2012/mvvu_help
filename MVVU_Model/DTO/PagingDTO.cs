using MVVU_Data.Entities;
//using X.PagedList;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace MVVU_Model.DTO
{
    public class PagingListDTO<T> where T : class
    {      
        //public StaticPagedList<T> Data { get; set; }
        public int TotalRows { get; set; }
        public long? TotalCounts { get; set; }
        public string TViews { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }

        public bool ShowPrevious => Page > 1;
        public bool ShowNext => Page < TotalPages;
        public bool ShowFirst => Page != 1;
        public bool ShowLast => Page != TotalPages;
        public decimal TotalAmount { get; set; }
        public bool IsDisplayMoreButton { get; set; }
      
    }

    public class PagingDTO<T> where T : class
    {
        public PagingDTO()
        {
            Data = new List<T>();
        }
        public IList<T> Data { get; set; }
        public int TotalRows { get; set; }
        public long? TotalCounts { get; set; }
        public string TViews { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }        
        public int TotalPages { get; set; }
       
        public bool ShowPrevious => Page > 1;
        public bool ShowNext => Page < TotalPages;
        public bool ShowFirst => Page != 1;
        public bool ShowLast => Page != TotalPages;
        public decimal TotalAmount { get; set; }

        //public List<CourseLabDTO> CourseLabData { get; set; }
    }
}
