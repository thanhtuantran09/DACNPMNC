using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CNPMNC.Models;
using PagedList;

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
                    Session["Ten"] = objUserGet.HOTEN;
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
        public ActionResult DangXuat()
        {
            Session["Email"] = null;
            //HttpCookie ckTaiKhoan = new HttpCookie("TenTaiKhoan"), ckMatKhau = new HttpCookie("MatKhau");
            //ckTaiKhoan.Expires = DateTime.Now.AddDays(-1);
            //ckMatKhau.Expires = DateTime.Now.AddDays(-1);
            //Response.Cookies.Add(ckTaiKhoan);
            //Response.Cookies.Add(ckMatKhau);
            return RedirectToAction("Trangchu", "Trangchu");
        }
        public ActionResult DetailUser()
        {
            // Lấy thông tin khách hàng từ database
            var email = Session["Email"] as string;
            var customer = db.KHACHHANGs.FirstOrDefault(c => c.EMAIL == email);
            // Truyền thông tin khách hàng sang
            return View(customer);
        }
        public ActionResult EditUser()
        {
            //Lấy thông tin khách hàng từ database
            var email = Session["Email"] as string;
            var customer = db.KHACHHANGs.FirstOrDefault(c => c.EMAIL == email);
            //Truyền thông tin khách hàng sang
            return View(customer);
        }
        [HttpPost]
        public ActionResult EditUser(KHACHHANG kh)
        {
            if (ModelState.IsValid)
            {
                var edituser = db.KHACHHANGs.Where(x => x.KHACHHANGID == kh.KHACHHANGID).FirstOrDefault();

                edituser.KHACHHANGID = kh.KHACHHANGID;
                edituser.EMAIL = kh.EMAIL;
                edituser.HOTEN = kh.HOTEN;
                edituser.SDT = kh.SDT;
                edituser.DIACHI = kh.DIACHI;
                edituser.NHAPLAIMK = kh.NHAPLAIMK;
                Session["Email"] = kh.EMAIL;
                db.SaveChanges();
                return RedirectToAction("DetailUser", "DNhap");
            }

            return View(kh);
        }
        public ActionResult LichSuDonHang(int? trangThaiId)
        {
            // Lấy thông tin khách hàng từ session
            var email = Session["Email"] as string;
            if (Session["Email"] == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Dangnhap", "DNhap");
            }
            else
            {
                // Lấy danh sách đơn hàng của khách hàng từ CSDL
                var khachHang = db.KHACHHANGs.SingleOrDefault(kh => kh.EMAIL == email);
                var donHangs = db.DONHANGs.Where(dh => dh.KHACHHANGID == khachHang.KHACHHANGID);

                // Lọc danh sách đơn hàng theo trạng thái được chọn (nếu có)
                if (trangThaiId.HasValue)
                {
                    donHangs = donHangs.Where(dh => dh.TRANGTHAIID == trangThaiId.Value);
                }

                return View(donHangs.ToList());
            }
        }

        public ActionResult ChiTietDonHang(int id)
        {
            var donHang = db.DONHANGs.SingleOrDefault(dh => dh.DONHANGID == id);
            var chiTietDonHangs = db.CTDONHANGs.Where(ct => ct.DONHANGID == id).ToList();
            ViewBag.DonHang = donHang;
            return View(chiTietDonHangs);
        }
      


    }
}