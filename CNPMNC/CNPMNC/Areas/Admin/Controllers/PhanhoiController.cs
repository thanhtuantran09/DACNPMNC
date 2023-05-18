using CNPMNC.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CNPMNC.Areas.Admin.Controllers
{
    public class PhanhoiController : Controller
    {
        CNPMNCEntities db = new CNPMNCEntities();
        // GET: Admin/Phanhoi
        public ActionResult PhanHoi(int? size, int? page, string currenFilter, string SearchString)
        {
            var email = Session["Email"] as string;
            var admin = db.ADMINs.FirstOrDefault(c => c.EMAIL == email);
            if (admin != null && (admin.CHUCVUID == 1))
            {
                var thongtin = new List<PHANHOI>();
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
                    thongtin = db.PHANHOIs.Where(n => n.KHACHHANG.HOTEN.Contains(SearchString) || n.EMAIL.Contains(SearchString)).ToList();
                }
                else
                {
                    //lấy ds tên nv theo bảng NV
                    thongtin = db.PHANHOIs.ToList();
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
                thongtin = thongtin.OrderBy(n => n.KHACHHANG.HOTEN).ToList();

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
            return RedirectToAction("Khongcoquyen", "Dienthoai");
        }
        public ActionResult Delete(int id)
        {
            var deleting = db.PHANHOIs.Find(id);

            return View(deleting);
        }


        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                var deleting = db.PHANHOIs.Find(id);
                db.PHANHOIs.Remove(deleting);
                db.SaveChanges();
                return RedirectToAction("PhanHoi");
            }
            catch
            {
                return Content("Thông tin này đã được sử dụng!");
            }
        }
    }
}