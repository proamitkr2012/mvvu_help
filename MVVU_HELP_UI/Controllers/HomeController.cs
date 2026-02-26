using MVVU_Model.DTO;
using MVVU_Model.DTO.HelpDesk;
using MVVU_Repo;
using MVVU_Repo.Utilities;
using MVVU_HELP_UI.Extensions;
using MVVU_HELP_UI.Helpers;
using MVVU_HELP_UI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Net.Http.Headers;

namespace MVVU_HELP_UI.Controllers
{
    [CanonicalActionFilter]
    public class HomeController : BaseController
    {
        protected readonly IHttpContextAccessor httpContextAccessor;
        SmsClient sms = new SmsClient();
        int pageSize;
        int pageSizeJobs;

        private IMailClient _mailClient;
        public HomeController(IConfiguration _config, IHttpContextAccessor _httpContextAccessor, IUnitOfWork uow, IMailClient mailClient) : base(_httpContextAccessor, _config, uow)
        {
            httpContextAccessor = _httpContextAccessor;
            _mailClient = mailClient;
        }

        public async Task<IActionResult> Index()
        {

            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            ViewBag.Typelist = null;
            return View();
        }

        public async Task<IActionResult> NewTicket()
        {
            List<TblHelpDeskComplaintType_AM> cData = await UOF.IAdminMaster.GetComplaintType();
            ViewBag.Typelist = cData;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewTicket(TblHelpDesk_AM model, string Message)
        {
            try
            {
                model.Remarks = Message;
                model.Subject = model.CTypeID==1?"IGRS":model.Subject;
                //model.Subject = model.CTypeID==14? "TN-No" : model.Subject;
                if (!string.IsNullOrEmpty(model.Subject) && (!string.IsNullOrEmpty(model.Roll)||model.CTypeID==1))
                {
                    if (Request.Form.Files.Count > 0)
                    {
                        List<TblHelpFile_AM> docfile = new List<TblHelpFile_AM>();
                        int j = 1;
                        foreach (IFormFile file in Request.Form.Files)
                        {
                            if (file.Name != "ProfilePic")
                            {
                                TblHelpFile_AM d = new TblHelpFile_AM();
                                var FullPath = "";

                                if ((file != null) && (file.Length > 0) && !string.IsNullOrEmpty(file.FileName))
                                {

                                    var filename = ContentDispositionHeaderValue
                                                    .Parse(file.ContentDisposition)
                                                    .FileName
                                                    .Trim('"');
                                    var name = ContentDispositionHeaderValue
                                                    .Parse(file.ContentDisposition)
                                                    .Name
                                                    .Trim('"');
                                    d.Name = filename;
                                    var ext = filename.Substring(filename.LastIndexOf('.'));
                                    string time = DateTime.Now.ToString("yyyyMMddHHmmss");
                                    string myfile = time + j.ToString() + ext;
                                    var path = "D:/DOCUMENTS/HELPDESK_FILE/";
                                    FullPath = path + myfile;
                                    var uploads = Path.Combine(path);

                                    var filePath = Path.Combine(uploads, myfile);
                                    string strpath = Path.GetExtension(file.FileName);
                                    if (strpath == ".jpg" || strpath == ".jpeg" || strpath == ".pdf" || strpath == ".png")
                                    {

                                        if (!Directory.Exists(uploads))
                                        {
                                            Directory.CreateDirectory(uploads);
                                        }

                                        if (System.IO.File.Exists(filePath) == true)
                                        {
                                            System.IO.File.Delete(filePath);
                                        }

                                        using (FileStream fs = System.IO.File.Create(filePath))
                                        {
                                            file.CopyTo(fs);
                                        }
                                        d.FilePath = FullPath.Replace("D:", "");
                                    }
                                    //  size += file.Length;
                                    j++;

                                }

                                docfile.Add(d);
                            }

                            else
                            {
                                var FullPath = "";

                                if ((file != null) && (file.Length > 0) && !string.IsNullOrEmpty(file.FileName))
                                {

                                    var filename = ContentDispositionHeaderValue
                                                    .Parse(file.ContentDisposition)
                                                    .FileName
                                                    .Trim('"');
                                    var name = ContentDispositionHeaderValue
                                                    .Parse(file.ContentDisposition)
                                                    .Name
                                                    .Trim('"');
                                    
                                    var ext = filename.Substring(filename.LastIndexOf('.'));
                                    string time = DateTime.Now.ToString("yyyyMMddHHmmss");
                                    string myfile = time + j.ToString() + ext;
                                    var path = "D:/DOCUMENTS/HELPDESK_FILE/";
                                    FullPath = path + myfile;
                                    var uploads = Path.Combine(path);

                                    var filePath = Path.Combine(uploads, myfile);
                                    string strpath = Path.GetExtension(file.FileName);
                                    if (strpath == ".jpg" || strpath == ".jpeg" || strpath == ".pdf" || strpath == ".png")
                                    {

                                        if (!Directory.Exists(uploads))
                                        {
                                            Directory.CreateDirectory(uploads);
                                        }

                                        if (System.IO.File.Exists(filePath) == true)
                                        {
                                            System.IO.File.Delete(filePath);
                                        }

                                        using (FileStream fs = System.IO.File.Create(filePath))
                                        {
                                            file.CopyTo(fs);
                                        }
                                        model.ProfilePic = FullPath.Replace("D:", "");



                                    }
                                    //  size += file.Length;
                                    j++;

                                }

                            }
                        }
                        model.FileList = docfile;
                    }
                    model.IsUser = CurrentUser == null ? true : false;
                    model.IpAddress = await Geoloc();

                    model.Subject = model.CTypeID == 7 ? model.Subject+ "     Application No:- "+ model.ApplicationNo : model.Subject;
                    FormResponse adminData = await UOF.IAdminMaster.SaveTicket(model);
                    if (adminData.ResponseCode == 1 && adminData.ResponseMessageCode != "-1")
                    {
                        string t = AESEncription.Base64Encode(adminData.ResponseMessageCode);
                        return RedirectToAction("Success", new { t = t });
                    }
                }
                List<TblHelpDeskComplaintType_AM> cData = await UOF.IAdminMaster.GetComplaintType();
                ViewBag.Typelist = cData;
                return View();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public async Task<string> Geoloc()
        {
            try
            {
                string _ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                if (_ipAddress.Contains("::1"))
                    _ipAddress = "127.0.0.1";

                return _ipAddress;
            }
            catch (Exception e)
            {

            }
            return string.Empty;
        }

        [HttpGet]
        public async Task<IActionResult> Success(string t)
        {
            ViewBag.ticket = AESEncription.Base64Decode(t);

            return View();
        }
        public IActionResult SignIn()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<ActionResult> SignOut()
        {
            if (CurrentUser != null)
            {
                if (CurrentUser.Roles.Contains("Student") == true)
                {
                    string[] Roles = CurrentUser != null ? CurrentUser.Roles : new string[] { "" };

                    await httpContextAccessor.HttpContext.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    string[] Roles = CurrentUser != null ? CurrentUser.Roles : new string[] { "" };

                    await httpContextAccessor.HttpContext.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    return RedirectToAction("Login", "Account", new { Area = "Admin" });

                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> SearchResult([FromBody] SearchDTO model)
        {
            try
            {
                Session.SetObject("resultdata", model);

                if (model.RollNumber.Trim() != "")
                {
                    if (string.IsNullOrEmpty(model.FName))
                    {
                        var datet = Convert.ToDateTime(model.DOBstr, CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                        model.DOB = datet;
                    }

                    var adminData = await UOF.IAdminMaster.CheckStudentData(model);
                    if (adminData.ROLL_NO == model.RollNumber)
                    {
                        adminData.EncrptedRoll = AESEncription.Base64Encode(adminData.ROLL_NO);


                        return Json(new { data = adminData, res = "success" });
                    }

                }
                else
                {
                    return Json("0");
                }
            }
            catch (Exception ex)
            {
                return Json("Internal error!");
            }
            return Json("Internal error!");
        }



        //[NonAction]
        //private static Byte[] BitmapToBytesCode(Bitmap image)
        //{
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        //        return stream.ToArray();
        //    }
        //}

        public async Task<IActionResult> DataSync()
        {
            try
            {
                CommonResultDataDTO d = new CommonResultDataDTO();

                d.StudentMaster = await UOF.IAdminMaster.GETDeLLServeMVVU_Data();
                //if (!string.IsNullOrEmpty(adminData))
                //{
                //    adminData.MarksList = await UOF.IAdminMaster.ResultMarksStudentData(s);
                //}



            }
            catch (Exception ex)
            {

            }
            return Json("ok");
        }


        /////////////////////////////////////////////
        [HttpGet]
        public async Task<IActionResult> Tickets(string Ticket)
        {
            Ticket = AESEncription.Base64Decode(Ticket);
            TblHelpDeskDTO_AM s = await UOF.IAdminMaster.GetTickets(Ticket);
            if (s != null)
            {
                // List<tb> s= await UOF.IAdminMaster.GetTickets(Ticket);
            }
            return View(s);
        }
        [HttpPost]
        public async Task<IActionResult> Tickets(string Ticket, string Message, string Status)
        {
            TblHelpDeskHistories_AM model = new TblHelpDeskHistories_AM();
            model.TicketID = Ticket;
            model.Status = Status;
            model.Remarks = Message;
            model.IpAddress = await Geoloc();

            if (Request.Form.Files.Count > 0)
            {
                List<TblHelpFile_AM> docfile = new List<TblHelpFile_AM>();
                int j = 1;
                foreach (IFormFile file in Request.Form.Files)
                {
                    TblHelpFile_AM d = new TblHelpFile_AM();
                    var FullPath = "";

                    if ((file != null) && (file.Length > 0) && !string.IsNullOrEmpty(file.FileName))
                    {

                        var filename = ContentDispositionHeaderValue
                                        .Parse(file.ContentDisposition)
                                        .FileName
                                        .Trim('"');
                        var name = ContentDispositionHeaderValue
                                        .Parse(file.ContentDisposition)
                                        .Name
                                        .Trim('"');
                        d.Name = filename;
                        var ext = filename.Substring(filename.LastIndexOf('.'));
                        string time = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string myfile = time + j.ToString() + ext;
                        var path = "D:/DOCUMENTS/HELPDESK_FILE/";
                        FullPath = path + myfile;
                        var uploads = Path.Combine(path);

                        var filePath = Path.Combine(uploads, myfile);
                        string strpath = Path.GetExtension(file.FileName);
                        if (strpath == ".jpg" || strpath == ".jpeg" || strpath == ".pdf" || strpath == ".png")
                        {
                            if (!Directory.Exists(uploads))
                            {
                                Directory.CreateDirectory(uploads);
                            }

                            if (System.IO.File.Exists(filePath) == true)
                            {
                                System.IO.File.Delete(filePath);
                            }

                            using (FileStream fs = System.IO.File.Create(filePath))
                            {
                                file.CopyTo(fs);
                            }
                            d.FilePath = FullPath.Replace("D:", "");
                        }

                        //  size += file.Length;
                        j++;
                    }

                    docfile.Add(d);
                }
                model.FileList = docfile;
            }

            FormResponse adminData = await UOF.IAdminMaster.SaveRemarkTicket(model);
            //if (adminData.ResponseCode == 1 && adminData.ResponseMessageCode != "-1")
            //   {
            //       string t = AESEncription.Base64Encode(adminData.ResponseMessageCode);
            //       return RedirectToAction("Success", new { t=t });
            //       }
            Ticket = AESEncription.Base64Encode(Ticket);
            return RedirectToAction("Tickets", new { Ticket = Ticket });
        }
        //


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckTicket([FromBody] TblHelpDesk_AM model)
        {
            try
            {

                if (!string.IsNullOrEmpty(model.TicketID))
                {
                    if (CurrentUser != null) {
                        model.Mobile = "-999";
                    }

                    FormResponse adminData = await UOF.IAdminMaster.CheckTicket(model.TicketID.Trim(), model.Mobile,model.Name);
                    if (adminData.ResponseCode == 1 && adminData.ResponseMessageCode != "-1")
                    {
                        string t = AESEncription.Base64Encode(adminData.ResponseMessageCode);
                        return Json(new { data = "success", res = t });
                    }
                    else
                    {
                        return Json(new { data = "Ticket provided is incorrect" });

                    }

                }
                else
                {
                    return Json(new { data = "Ticket is not provided!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { data = "Internal error!" });
            }

        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> FindTicket([FromBody] TblHelpDesk_AM model)
        {
            try
            {

                if (!string.IsNullOrEmpty(model.Enroll_Roll))
                {

                    List<TblHelpDesk_AM> adminData = await UOF.IAdminMaster.FindTicket(model);
                    if (adminData.Count > 0)
                    {
                        return Json(new { data = "success", res = adminData });
                    }
                    else
                    {
                        return Json(new { data = "Ticket Not Found" });

                    }

                }
                else
                {
                    return Json(new { data = "Data not provided!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { data = "Internal error!" });
            }

        }


        public async Task<IActionResult> Forget()
        {
            List<TblHelpDeskComplaintType_AM> cData = await UOF.IAdminMaster.GetComplaintType();
            ViewBag.Typelist = cData;
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckAlert([FromBody] TblHelpDesk_AM model)
        {
            try
            {

                if (!string.IsNullOrEmpty(model.CTypeID.ToString()))
                {


                    List<TblHelpDeskComplaintType_AM> cData = await UOF.IAdminMaster.GetComplaintType();
                    var s = cData.Where(x => x.CTypeID == model.CTypeID).Select(x=>x.AlertMsg).FirstOrDefault();
                    if (!string.IsNullOrEmpty(s))
                    {
                        return Json(new { data = "yes", msg = s });
                    }
                    else
                    {
                        return Json(new { data = "no" });

                    }

                }
                else
                {
                    return Json(new { data = "no" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { data = "Internal error!" });
            }

        }

    }
}