using MVVU_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MVVU_HELP_UI.Helpers;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using MVVU_HELP_UI.Areas.Admin.Filters;
using MVVU_Repo;

namespace MVVU_HELP_UI.Areas.Admin.Controllers
{
    [CustomAdminAuthorize(Roles= "Admin, Operator,SuperAdmin,Department")]
    [Area("Admin")]
    public class BaseController : Controller
    {
        protected readonly IHttpContextAccessor httpContextAccessor;
        protected ISession Session => httpContextAccessor.HttpContext.Session;
        protected BreadCrumbHelper BreadCrumb;
        protected IConfiguration config;
        protected IUnitOfWork UOF;
        //protected string TimeOffSet
        //{
        //    get
        //    {
        //        var defaultzone = "-330";

        //       // defaultzone = Session.GetString("timezoneoffset") != null ? Session.GetString("timezoneoffset").ToString() : "-330";

        //        double minutes2 = Convert.ToDouble(defaultzone);
        //        IEnumerable<TimeZoneInfo> zones = TimeZoneInfo.GetSystemTimeZones();
        //        var zone = zones.Where(p => p.BaseUtcOffset.TotalMinutes == minutes2 * -1).FirstOrDefault();
        //        return zone.Id;
        //    }
        //}
        public BaseController(IHttpContextAccessor _httpContextAccessor, IConfiguration _config, IUnitOfWork _UOF)
        {
            httpContextAccessor = _httpContextAccessor;
            BreadCrumb = new BreadCrumbHelper(_httpContextAccessor);
            UOF = _UOF;
            config = _config;
        }

        public CustomPrincipal CurrentUser
        {
            get
            {
                if (User.Claims.Count() > 0)
                {
                    string userData = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData).Value;
                    var user = JsonConvert.DeserializeObject<CustomPrincipal>(userData);
                    return user;
                }
                return null;
            }
        }
    }
}
