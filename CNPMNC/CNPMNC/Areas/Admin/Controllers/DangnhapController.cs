using System;
using System.Collections.Generic;
using System.IO;
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

                Session["Ten"]=objUserGet.TENAD;
                Session["Email"] = admin.EMAIL;
            return RedirectToAction("Dashboard", "Dashboard");

        }
        public ActionResult DangXuat()
        {
            Session["Email"] = null;
            return RedirectToAction("Dangnhap", "Dangnhap");
        }
     
        public ActionResult DetailUser()
        {
            // Lấy thông tin khách hàng từ database
            var email = Session["Email"] as string;
            var customer = db.ADMINs.FirstOrDefault(c => c.EMAIL == email);
            // Truyền thông tin khách hàng sang
            return View(customer);
        }
        [HttpPost]
        public ActionResult DetailUser(ADMIN model)
        {
            if (ModelState.IsValid)
            {
                //var objadmin = db.ADMINs.Find(model.ADMINID);
                var edituser = db.ADMINs.Where(x => x.ADMINID == model.ADMINID).FirstOrDefault();

                edituser.TENAD = model.TENAD;
                edituser.MATKHAU = model.MATKHAU;
                
                Session["Ten"] = model.TENAD;
                db.SaveChanges();
                return RedirectToAction("DetailUser", "Dangnhap");
            }

            return View(model);
        }
    }
}