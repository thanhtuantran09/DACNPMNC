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
    public class HangController : Controller
    {
        CNPMNCEntities db=new CNPMNCEntities();
        // GET: Admin/Hang
        public ActionResult Hang(int? size, int? page, string currenFilter, string SearchString)
        {
            var email = Session["Email"] as string;
            var admin = db.ADMINs.FirstOrDefault(c => c.EMAIL == email);
            if (admin != null && (admin.CHUCVUID == 1 || admin.CHUCVUID == 3))
            {
                var thongtin = new List<HANG>();
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
                    thongtin = db.HANGs.Where(n => n.TENHANG.Contains(SearchString)).ToList();
                }
                else
                {
                    //lấy ds tên nv theo bảng NV
                    thongtin = db.HANGs.ToList();
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
                thongtin = thongtin.OrderBy(n => n.HANGID).ToList();

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


        // GET: Admin/Hang/Create
        public ActionResult Create()
        {
            HANG pro = new HANG();
            return View(pro);
        }

        // POST: Admin/Hang/Create
        [HttpPost]
        public ActionResult Create(HANG model)
        {
            try
            {
                if (model.UploadImage1 != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(model.UploadImage1.FileName);
                    string extension = Path.GetExtension(model.UploadImage1.FileName);
                    filename = filename + extension;
                    model.HINH = "~/Content/Hinh/" + filename;
                    model.UploadImage1.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename));

                }
                db.HANGs.Add(model);
                db.SaveChanges();
                return RedirectToAction("Hang");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Hang/Edit/5
        public ActionResult Edit(int id)
        {
            return View(db.HANGs.Where(s => s.HANGID == id).FirstOrDefault());
        }

        // POST: Admin/Hang/Edit/5
        [HttpPost]
        public ActionResult Edit(HANG model)
        {
            try
            {
                var objDoiBong = db.HANGs.Find(model.HANGID);
                // TODO: Add update logic here
                if (model.UploadImage1 != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(model.UploadImage1.FileName);
                    string extension = Path.GetExtension(model.UploadImage1.FileName);
                    filename = filename + extension;
                    model.HINH = "~/Content/Hinh/" + filename;
                    model.UploadImage1.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename));
                    // gan cac du lieu vao cai lay len

                }
                objDoiBong.TENHANG = model.TENHANG;
                objDoiBong.HINH = model.HINH;
                //db.Entry(model).State = System.Data.Entity.EntityState.Modified;


                db.SaveChanges();
                return RedirectToAction("Hang");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Hang/Delete/5
        public ActionResult Delete(int id)
        {
            var deleting = db.HANGs.Find(id);

            return View(deleting);
        }

       
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                var deleting = db.HANGs.Find(id);
                db.HANGs.Remove(deleting);
                db.SaveChanges();
                return RedirectToAction("Hang");
            }
            catch
            {
                return Content("Thông tin này đã được sử dụng!");
            }
        }
    }
}
