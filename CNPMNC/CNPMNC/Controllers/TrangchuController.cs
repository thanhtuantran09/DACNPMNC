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
    public class TrangchuController : Controller
    {

        CNPMNCEntities db=new CNPMNCEntities();
        // GET: Trangchu
        public ActionResult Trangchu(int? page)
        {
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            var products = db.DIENTHOAIs.OrderBy(p => p.TENDT).ToPagedList(pageNumber, pageSize);
            return View(products);
        }
        public ActionResult Hang()
        {
            var hangs = db.HANGs.ToList();
            return PartialView(hangs);
        }
        
        public ActionResult DienThoai(int id, int page = 1)
        {
            var dienthoais = db.DIENTHOAIs.Where(d => d.HANGID == id).ToList().ToPagedList(page, 10); // 10 là số phần tử trên mỗi trang
            return View(dienthoais);
        }
       

        public ActionResult Details(int id)
        {
            var dienthoai = db.DIENTHOAIs.Find(id);
            if (dienthoai == null)
            {
                return HttpNotFound();
            }
            var thongSoKiThuat = db.THONGSOKTs.FirstOrDefault(t => t.DIENTHOAIID == dienthoai.DIENTHOAIID);

            ViewBag.ThongSoKiThuat = thongSoKiThuat;
            
            var email = Session["Email"] as string;
            // Check if user already reviewed this product
            var existingReview = db.DANHGIASANPHAMs.FirstOrDefault(r => r.KHACHHANGID.ToString() == email && r.DIENTHOAIID == id);
            if (existingReview != null)
            {
                ViewBag.CanReview = false;
                ViewBag.UserReview = existingReview;
            }
            else
            {
                ViewBag.CanReview = true;
            }
            return View(dienthoai);
        }
        [HttpPost]
        public ActionResult AddReview(int id, string review)
        {
            // Check if user is logged in
            var email = Session["Email"] as string;
            if (Session["Email"] == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Dangnhap", "DNhap");
            }
            // Get logged in user id


            // Check if user already reviewed this product
            var existingReview = db.DANHGIASANPHAMs.FirstOrDefault(r => r.KHACHHANGID.ToString() == email && r.DIENTHOAIID == id);

            if (existingReview != null)
            {
                ModelState.AddModelError("", "Bạn đã đánh giá sản phẩm này rồi!");
            }
            else
            {
                // Add new review
                var khachHang = db.KHACHHANGs.SingleOrDefault(kh => kh.EMAIL == email);
                var newReview = new DANHGIASANPHAM();


                    newReview.KHACHHANGID = khachHang.KHACHHANGID;
                    newReview.DIENTHOAIID = id;
                    newReview.DANHGIA = review;
                    newReview.NGAYTAO = DateTime.Now;
                

                db.DANHGIASANPHAMs.Add(newReview);
                db.SaveChanges();
            }

            // Redirect to product details page
            return RedirectToAction("Details", new { id = id });
        }
        public ActionResult Search(string searching)
        {
             
            return View(db.DIENTHOAIs.Where(x => x.TENDT.StartsWith(searching) || x.TENDT == null).ToList());
            
        }
        public ActionResult Lienhe()
        {
            return View();
        }
    }
}