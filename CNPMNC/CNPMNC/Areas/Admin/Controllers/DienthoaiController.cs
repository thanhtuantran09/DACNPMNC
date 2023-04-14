using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CNPMNC.Models;
using PagedList;
namespace CNPMNC.Areas.Admin.Controllers
{
    public class DienthoaiController : Controller
    {
        CNPMNCEntities db=new CNPMNCEntities();
        // GET: Admin/Dienthoai
        public ActionResult Dienthoai(int? size, int? page, string currenFilter, string SearchString)
        {
            var thongtin = new List<DIENTHOAI>();
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
                thongtin = db.DIENTHOAIs.Where(n => n.TENDT.Contains(SearchString)).ToList();
            }
            else
            {
                //lấy ds tên nv theo bảng NV
                thongtin = db.DIENTHOAIs.ToList();
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
            thongtin = thongtin.OrderBy(n => n.DIENTHOAIID).ToList();

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
        public ActionResult SelectCate()
        {
            HANG se_cate = new HANG();
            se_cate.ListCate = db.HANGs.ToList<HANG>();
            return PartialView(se_cate);
        }
        public ActionResult SelectCateNCC()
        {
            NHACUNGCAP se_cate = new NHACUNGCAP();
            se_cate.ListCateNCC = db.NHACUNGCAPs.ToList<NHACUNGCAP>();
            return PartialView(se_cate);
        }

        // GET: Admin/Dienthoai/Create
        public ActionResult Create()
        {
           
            DIENTHOAI pro = new DIENTHOAI();
            return View(pro);
        }

        // POST: Admin/Dienthoai/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DIENTHOAI model)
        {

            // Lưu ảnh vào thư mục ~/Content/Images/
            if (model.UploadImage1 != null && model.UploadImage2 != null && model.UploadImage3 != null)
            {
                string filename1 = Path.GetFileNameWithoutExtension(model.UploadImage1.FileName);
                string extension1 = Path.GetExtension(model.UploadImage1.FileName);
                filename1 = filename1 + extension1;
                string filename2 = Path.GetFileNameWithoutExtension(model.UploadImage2.FileName);
                string extension2 = Path.GetExtension(model.UploadImage2.FileName);
                filename2 = filename2 + extension2;
                string filename3 = Path.GetFileNameWithoutExtension(model.UploadImage3.FileName);
                string extension3 = Path.GetExtension(model.UploadImage3.FileName);
                filename3 = filename3 + extension3;

                model.HINHANH1 = "~/Content/Hinh/" + filename1;
                model.HINHANH2 = "~/Content/Hinh/" + filename2;
                model.HINHANH3 = "~/Content/Hinh/" + filename3;

                model.UploadImage1.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename1));
                model.UploadImage2.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename2));
                model.UploadImage3.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename3));
            }
                // Kiểm tra giá bán, số lượng tồn và phần trăm giảm
                if (model.GIABAN >= 0 && model.SOLUONGTON >= 0 && model.PHANTRAMGIAM >= 0 && model.PHANTRAMGIAM <= 100)
                {
                  
                    model.GIAGIAM = model.GIABAN - (model.GIABAN * model.PHANTRAMGIAM / 100);
                    db.DIENTHOAIs.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Dienthoai");
                }
                else
                {
                    if (model.GIABAN < 0)
                    {
                        ModelState.AddModelError("GIABAN", "Giá bán phải lớn hơn hoặc bằng 0.");
                    }
                    if (model.SOLUONGTON < 0)
                    {
                        ModelState.AddModelError("SOLUONGTON", "Số lượng tồn phải lớn hơn hoặc bằng 0.");
                    }
                    if (model.PHANTRAMGIAM < 0 || model.PHANTRAMGIAM > 100)
                    {
                        ModelState.AddModelError("PHANTRAMGIAM", "Phần trăm giảm phải trong khoảng từ 0 đến 100.");
                    }
                }
            
            return View(model);
        }

        // GET: Admin/Dienthoai/Edit/5
        public ActionResult Edit(int id)
        {
            return View(db.DIENTHOAIs.Where(s => s.DIENTHOAIID == id).FirstOrDefault());
        }

        // POST: Admin/Dienthoai/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DIENTHOAI model)
        {
            var objDoiBong = db.DIENTHOAIs.Find(model.DIENTHOAIID);
            // Lưu ảnh vào thư mục ~/Content/Images/
            if (model.UploadImage2 != null)
            {
                string filename2 = Path.GetFileNameWithoutExtension(model.UploadImage2.FileName);
                string extension2 = Path.GetExtension(model.UploadImage2.FileName);
                filename2 = filename2 + extension2;

                objDoiBong.HINHANH2 = "~/Content/Hinh/" + filename2;
                model.UploadImage2.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename2));
            }
            if (model.UploadImage1 != null)
            {
                string filename1 = Path.GetFileNameWithoutExtension(model.UploadImage1.FileName);
                string extension1 = Path.GetExtension(model.UploadImage1.FileName);
                filename1 = filename1 + extension1;

                objDoiBong.HINHANH1 = "~/Content/Hinh/" + filename1;
                model.UploadImage1.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename1));
            }
            if (model.UploadImage3 != null)
            {
                string filename3 = Path.GetFileNameWithoutExtension(model.UploadImage3.FileName);
                string extension3 = Path.GetExtension(model.UploadImage3.FileName);
                filename3 = filename3 + extension3;

                objDoiBong.HINHANH3 = "~/Content/Hinh/" + filename3;
                model.UploadImage3.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename3));
            }

            // Kiểm tra giá bán, số lượng tồn và phần trăm giảm
            if (model.GIABAN >= 0 && model.SOLUONGTON >= 0 && model.PHANTRAMGIAM >= 0 && model.PHANTRAMGIAM <= 100)
            {
                objDoiBong.TENDT = model.TENDT;
                objDoiBong.GIABAN = model.GIABAN;
                objDoiBong.GIAGIAM = model.GIAGIAM;
                objDoiBong.PHANTRAMGIAM = model.PHANTRAMGIAM;

                objDoiBong.SOLUONGTON = model.SOLUONGTON;
                objDoiBong.HANGID = model.HANGID;
                objDoiBong.NCCID = model.NCCID;
                objDoiBong.MAUSAC = model.MAUSAC;
                objDoiBong.GIAGIAM = objDoiBong.GIABAN - (objDoiBong.GIABAN * objDoiBong.PHANTRAMGIAM / 100);

                db.SaveChanges();
                return RedirectToAction("Dienthoai");
            }
            else
            {
                if (model.GIABAN < 0)
                {
                    ModelState.AddModelError("GIABAN", "Giá bán phải lớn hơn hoặc bằng 0.");
                }
                if (model.SOLUONGTON < 0)
                {
                    ModelState.AddModelError("SOLUONGTON", "Số lượng tồn phải lớn hơn hoặc bằng 0.");
                }
                if (model.PHANTRAMGIAM < 0 || model.PHANTRAMGIAM > 100)
                {
                    ModelState.AddModelError("PHANTRAMGIAM", "Phần trăm giảm phải trong khoảng từ 0 đến 100.");
                }
            }

            return View(model);
        }
        // GET: Admin/Dienthoai/Delete/5
        public ActionResult Delete(int id)
        {
            var deleting = db.DIENTHOAIs.Find(id);

            return View(deleting);
        }

        // POST: Admin/Dienthoai/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                var deleting = db.DIENTHOAIs.Find(id);
                db.DIENTHOAIs.Remove(deleting);
                db.SaveChanges();
                return RedirectToAction("Dienthoai");
            }
            catch
            {
                return Content("Thông tin này đã được sử dụng!");
            }
        }
        public ActionResult Details(int id)
        {
            var Details = db.DIENTHOAIs.Find(id);

            return View(Details);
        }
    }
}
