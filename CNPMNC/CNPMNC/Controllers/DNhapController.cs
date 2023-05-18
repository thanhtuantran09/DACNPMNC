using System;
using System.Collections.Generic;
using System.IO;
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
                    Session["Hinh"] = objUserGet.HINH;
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
        public ActionResult EditUser(int id)
        {
            //Lấy thông tin khách hàng từ database
            var email = Session["Email"] as string;
            return View(db.KHACHHANGs.Where(s => s.KHACHHANGID == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult EditUser(KHACHHANG model)
        {
            if (ModelState.IsValid)
            {
                var objDoiBong = db.KHACHHANGs.Find(model.KHACHHANGID);
                //var edituser = db.KHACHHANGs.Where(x => x.KHACHHANGID == kh.KHACHHANGID).FirstOrDefault();
                if (model.UploadImage1 != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(model.UploadImage1.FileName);
                    string extension = Path.GetExtension(model.UploadImage1.FileName);
                    filename = filename + extension;
                    model.HINH = "~/Content/Hinh/" + filename;
                    model.UploadImage1.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename));
                    // gan cac du lieu vao cai lay len

                }
                objDoiBong.HINH = model.HINH;
                objDoiBong.KHACHHANGID = model.KHACHHANGID;
                objDoiBong.EMAIL = model.EMAIL;
                objDoiBong.HOTEN = model.HOTEN;
                objDoiBong.SDT = model.SDT;
                objDoiBong.DIACHI = model.DIACHI;
                objDoiBong.NHAPLAIMK = model.NHAPLAIMK;
                Session["Email"] = model.EMAIL;
                Session["Ten"] = model.HOTEN;
                Session["Hinh"] = model.HINH;
                db.SaveChanges();
                return RedirectToAction("DetailUser", "DNhap");
            }

            return View(model);
        }
        public ActionResult LichSuDonHang(int? trangThaiId)
        {
            // Lấy thông tin khách hàng từ session
            var email = Session["Email"] as string;
            var hinh = Session["Hinh"] as string;
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
        public ActionResult Khongduochuy()
        {
            // Thực hiện các logic xử lý hoặc trả về view tương ứng
            return View();
        }
     
        public ActionResult Huydonhang(int id)
        {
            var donHang = db.DONHANGs.SingleOrDefault(dh => dh.DONHANGID == id);

            // Kiểm tra nếu TRANGTHAIID là 5 hoặc 6, không cho phép hủy đơn hàng
            if (donHang.TRANGTHAIID == 5 || donHang.TRANGTHAIID == 6)
            {
                // Redirect hoặc hiển thị thông báo lỗi cho khách hàng
                return RedirectToAction("Khongduochuy", "Dnhap");
            }

            // Kiểm tra nếu TRANGTHAIID là từ 1 đến 4, chuyển TRANGTHAIID thành 7
            if (donHang.TRANGTHAIID >= 1 && donHang.TRANGTHAIID <= 4)
            {
                donHang.TRANGTHAIID = 7;
                db.SaveChanges();
            }

            var chiTietDonHangs = db.CTDONHANGs.Where(ct => ct.DONHANGID == id).ToList();
            ViewBag.DonHang = donHang;

            return View(chiTietDonHangs);

        }
    }
}