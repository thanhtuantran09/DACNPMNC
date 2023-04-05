using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CNPMNC.Models;

namespace CNPMNC.Controllers
{
    public class DNhapController : Controller
    {
        CNPMNCEntities db=new CNPMNCEntities();
        // GET: DNhap
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(KHACHHANG user)
        {

            var objUserGet = db.KHACHHANGs.Where(n => n.EMAIL.Equals(user.EMAIL) && n.MATKHAU.Equals(user.MATKHAU)).FirstOrDefault();


            if (objUserGet == null)
            {
                ViewBag.error = "Email hay Mật khẩu không đúng vui lòng nhập lại!";
                return View();
            }
            else


                    Session["Email"] = user.EMAIL;
                    return RedirectToAction("Trangchu", "Trangchu");

        }
        public ActionResult Dangky()
        {
           
            return View();
        }
       
        [HttpPost]
        public ActionResult Dangky(KHACHHANG user)
        {
            if (ModelState.IsValid)
            {
                if (db.KHACHHANGs.Any(u => u.EMAIL == user.EMAIL))
                {
                    ViewBag.error = "Email đã tồn tại!";
                    return View();
                }

                if (user.MATKHAU != user.NHAPLAIMK)
                {
                    ViewBag.error1 = "Mật khẩu không trùng khớp!";
                    return View();
                }

                db.KHACHHANGs.Add(user);
                db.SaveChanges();

                return RedirectToAction("Dangnhap", "DNhap");
            }

            return View(user);
        }


    }
}