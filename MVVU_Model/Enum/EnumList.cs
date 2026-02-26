using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MVVU_Model.Enum
{
    public enum ENUM_APP_SEX : int
    {
        Male = 1,
        Female = 2,
        Other = 0
    }


    public enum ENUM_HELP_STATUS
    {
        [Description("Open")]
        Open,
        [Description("Pending")]
        Pending,
        [Description("On Hold")]
        OnHold,
        //[Description("Solved")]
        //Solved,
        [Description("Closed")]
        Closed,
        [Description("Spam")]
        Spam
    }
}
