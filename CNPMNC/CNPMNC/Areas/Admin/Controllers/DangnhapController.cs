using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CNPMNC.Models;
namespace CNPMNC.Areas.Admin.Controllers
{
    
    public class DangnhapController : Controller
    {
        CNPMNCEntities db=new CNPMNCEntities(); 
        // GET: Admin/Dangnhap
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(ADMIN admin)
        {

            var objUserGet = db.ADMINs.Where(n => n.EMAIL.Equals(admin.EMAIL) && n.MATKHAU.Equals(admin.MATKHAU)).FirstOrDefault();


            if (objUserGet == null)
            {
                ViewBag.error = "Email hay Mật khẩu không đúng vui lòng nhập lại!";
                return View();
            }
            else


                Session["Email"] = admin.EMAIL;
            return RedirectToAction("Dienthoai", "Dienthoai");

        }
    }
}