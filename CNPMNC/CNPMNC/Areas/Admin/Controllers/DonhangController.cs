using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CNPMNC.Models;
using PagedList;

namespace CNPMNC.Areas.Admin.Controllers
{
    public class DonhangController : Controller
    {
        CNPMNCEntities db = new CNPMNCEntities();
        // GET: Admin/Donhang
        public ActionResult Donhang(int? size, int? page, string currenFilter, string SearchString)
        {
            var email = Session["Email"] as string;
            var admin = db.ADMINs.FirstOrDefault(c => c.EMAIL == email);
            if (admin != null && (admin.CHUCVUID == 1 || admin.CHUCVUID == 2))
            {
                var thongtin = new List<DONHANG>();
                if (SearchString != null)
                {
                    page = 1;
                }
                else
                {
                    SearchString = currenFilter;
                }
                if (!string.IsNullOrEmpty(SearchString))
                {
                    //lấy ds tên nv theo tu khóa tim kiếm
                    thongtin = db.DONHANGs.Where(n => n.TENKH.Contains(SearchString) || n.SDT.Contains(SearchString)).ToList();
                }
                else
                {
                    //lấy ds tên nv theo bảng NV
                    thongtin = db.DONHANGs.ToList();
                }
                ViewBag.CurrenFilter = SearchString;
                // 1. Tạo list pageSize để người dùng có thể chọn xem để phân trang
                // Bạn có thể thêm bớt tùy ý 
                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "5", Value = "5" });
                items.Add(new SelectListItem { Text = "10", Value = "10" });
                items.Add(new SelectListItem { Text = "20", Value = "20" });


                // 1.1. Giữ trạng thái kích thước trang được chọn trên DropDownList
                foreach (var item in items)
                {
                    if (item.Value == size.ToString()) item.Selected = true;
                }

                // 1.2. Tạo các biến ViewBag
                ViewBag.size = items; // ViewBag DropDownList
                ViewBag.currentSize = size; // tạo biến kích thước trang hiện tại

                // 2. Nếu page = null thì đặt lại là 1.
                page = page ?? 1; //if (page == null) page = 1;

                // 3. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
                // theo LinkID mới có thể phân trang.
                thongtin = thongtin.OrderBy(n => n.DONHANGID).ToList();

                // 4. Tạo kích thước trang (pageSize), mặc định là 5.
                int pageSize = (size ?? 5);

                // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
                // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
                int pageNumber = (page ?? 1);
                //4.2 Lấy tổng số record chia cho kích thuốc để biết bao nhiêu trang
                int checkTotal = (int)(thongtin.ToList().Count / pageSize) + 1;
                //Nếu trang vượt qua tổng số trang thì thiết lập là 1 hoặc tống số trang
                if (pageNumber > checkTotal) pageNumber = checkTotal;

                // 5. Trả về các Link được phân trang theo kích thước và số trang.
                return View(thongtin.ToPagedList(pageNumber, pageSize));
            }
            // Người dùng không có quyền truy cập, chuyển hướng đến trang lỗi hoặc xử lý khác
            return RedirectToAction("Khongcoquyen","Dienthoai");
        }
        public ActionResult SelectCateTT()
        {
            TRANGTHAIDH se_cate = new TRANGTHAIDH();

            se_cate.ListCateTT = db.TRANGTHAIDHs.ToList<TRANGTHAIDH>();
            return PartialView(se_cate);
        }
        public ActionResult SelectCatePTTT()
        {
            PTTHANHTOAN se_cate = new PTTHANHTOAN();
            se_cate.ListCatePTTT = db.PTTHANHTOANs.ToList<PTTHANHTOAN>();
            return PartialView(se_cate);
        }
        
        public ActionResult Edit(int id)
        {
            var editing = db.DONHANGs.Find(id);
            return View(editing);
        }

        
        [HttpPost]
        public ActionResult Edit(DONHANG model)
        {
            try
            {
                var sua = db.DONHANGs.Find(model.DONHANGID);
                sua.KHACHHANG = model.KHACHHANG;
                sua.TRANGTHAIID=model.TRANGTHAIID;
                sua.PTTHANHTOANID= model.PTTHANHTOANID;
                sua.TENKH=model.TENKH;
                sua.DIACHI=model.DIACHI;
                sua.SDT=model.SDT;
                db.SaveChanges();
                return RedirectToAction("Donhang");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Details(int id)
        {
            // Lấy thông tin đơn hàng từ database
            var donHang = db.DONHANGs.FirstOrDefault(dh => dh.DONHANGID == id);
            if (donHang == null)
            {
                return HttpNotFound();
            }

            // Lấy thông tin chi tiết đơn hàng từ database
            var ctDonHangs = db.CTDONHANGs.Include("DIENTHOAI").Where(ct => ct.DONHANGID == id).ToList();

            // Tạo một đối tượng DonHangModel để truyền dữ liệu vào view
            var model = new DONHANG()
            {
                DONHANGID = donHang.DONHANGID,
                TENKH = donHang.TENKH,
                DIACHI= donHang.DIACHI,
                SDT= donHang.SDT,
                NGAYTAO = donHang.NGAYTAO,
                TRANGTHAIID = donHang.TRANGTHAIID,
                PTTHANHTOANID = donHang.PTTHANHTOANID,
                GIAMGIA = donHang.GIAMGIA,
                THANHTIEN = donHang.THANHTIEN,
                CTDONHANGs = new List<CTDONHANG>()
               
            };

            // Duyệt danh sách chi tiết đơn hàng và thêm thông tin vào đối tượng model
            foreach (var ctDonHang in ctDonHangs)
            {
                var dienThoai = db.DIENTHOAIs.FirstOrDefault(dt => dt.DIENTHOAIID == ctDonHang.DIENTHOAIID);
                // Lấy thông tin điện thoại từ cơ sở dữ liệu

                var ctModel = new CTDONHANG()
                {
                    CTDONHANGID = ctDonHang.CTDONHANGID,
                    DONHANGID = ctDonHang.DONHANGID,
                    SOLUONGMUA = ctDonHang.SOLUONGMUA,
                    TONGTIEN = ctDonHang.TONGTIEN,
                    DIENTHOAIID =ctDonHang.DIENTHOAIID,
                    
                };

                model.CTDONHANGs.Add(ctModel);
            }

            // Trả về view với đối tượng model
            return View(model);
        }
        // GET: Admin/Hang/Delete/5
        public ActionResult Delete(int id)
        {
            var deleting = db.DONHANGs.Find(id);

            return View(deleting);
        }

        // POST: QLDoibong/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                var deleting = db.DONHANGs.Find(id);
                db.DONHANGs.Remove(deleting);
                db.SaveChanges();
                return RedirectToAction("Donhang");
            }
            catch
            {
                return Content("Thông tin này đã được sử dụng!");
            }
        }
    }
}