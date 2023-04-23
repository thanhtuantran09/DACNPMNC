using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CNPMNC.Models;
namespace CNPMNC.Controllers
{
    public class ShoppingCartController : Controller
    {
        CNPMNCEntities db = new CNPMNCEntities();
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        // GET: ShoppingCart
        public ActionResult AddToCart(int id)
        {
            var pro = db.DIENTHOAIs.SingleOrDefault(s => s.DIENTHOAIID == id);
            if (pro != null)
            {
                GetCart().Add(pro);

            }
            //return RedirectToAction("ShowToCart", "ShoppingCart");
            return RedirectToAction("Trangchu","Trangchu");
        }
        public ActionResult ShowToCart()
        {
            if (Session["Cart"] == null)
            {
                 return RedirectToAction("ShowToCart", "ShoppingCart");
            }
               

            Cart cart = Session["Cart"] as Cart;
            ViewBag.Message = TempData["Message"];
            

            return View(cart);
        }
            public ActionResult Update_Sl(FormCollection form)
            {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(form["ID_dienthoai"]);
            int sl = int.Parse(form["Soluong"]);
            cart.Update_Sl(id_pro, sl, this.ControllerContext);

            return RedirectToAction("ShowToCart", "ShoppingCart");

             }
        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Xoasp(id);
            if (cart.Tongsoluong() == 0)
            {
                return RedirectToAction("Chucohang", "ShoppingCart");
            }
            else
            {
                return RedirectToAction("ShowToCart", "ShoppingCart");
            }
           
        }
        public PartialViewResult GioHang()
        {
            int tongsl = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
            {
                tongsl = cart.Tongsoluong();
            }
            ViewBag.infoCart = tongsl;
            return PartialView("GioHang");
        }
       public ActionResult Chucohang()
        {
            return View();
        }
        public ActionResult Complete()
        {
            return View();
        }
        public ActionResult Checkout(FormCollection form)
        {
            try
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
                    Cart cart = Session["Cart"] as Cart;
                    // Lấy thông tin khách hàng từ CSDL
                    var khachHang = db.KHACHHANGs.SingleOrDefault(kh => kh.EMAIL == email);
                    var donHang = new DONHANG();

                    // Thêm thông tin khách hàng vào đơn hàng
                    donHang.KHACHHANGID = khachHang.KHACHHANGID;
                    donHang.TENKH = (form["Hovaten"]);

                    donHang.DIACHI = (form["Diachi"]);
                    donHang.SDT = (form["SDT"]);
                    donHang.NGAYTAO = DateTime.Now;
                    donHang.THANHTIEN = (decimal?)cart.Tongtien();
                    // Lấy trạng thái đơn hàng mặc định (ví dụ: 1 - Chờ xử lý)
                    var trangThai = db.TRANGTHAIDHs.SingleOrDefault(tt => tt.TRANGTHAIID == 1);

                    // Thêm thông tin trạng thái vào đơn hàng
                    donHang.TRANGTHAIID = trangThai.TRANGTHAIID;
                    // Lưu đơn hàng vào CSDL
                    db.DONHANGs.Add(donHang);


                    // Thêm sản phẩm vào chi tiết đơn hàng


                    foreach (var item in cart.Items)
                    {
                        CTDONHANG chiTietDonHang = new CTDONHANG();


                        chiTietDonHang.DONHANGID = donHang.DONHANGID;
                        chiTietDonHang.DIENTHOAIID = item.sanpham.DIENTHOAIID;
                        chiTietDonHang.SOLUONGMUA = item.soluong;


                        chiTietDonHang.TONGTIEN = (item.soluong * item.sanpham.GIAGIAM);

                        db.CTDONHANGs.Add(chiTietDonHang);
                        foreach (var p in db.DIENTHOAIs.Where(s=>s.DIENTHOAIID==chiTietDonHang.DIENTHOAIID))
                        {
                            var soluongtonmoi = p.SOLUONGTON - item.soluong;
                            p.SOLUONGTON = soluongtonmoi;
                        }
                    }

                    db.SaveChanges();

                    // Xóa thông tin giỏ hàng
                    cart.Xoagh();

                    // Hiển thị thông báo đặt hàng thành công
                    ViewBag.ThongBao = "Đặt hàng thành công!";

                    return RedirectToAction("Complete", new { id = donHang.DONHANGID });

                }
            }
            catch
            {
                return View();
            }
           




        }


    }
}