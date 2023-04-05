using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CNPMNC.Models;
using PagedList;

namespace CNPMNC.Areas.Admin.Controllers
{
    public class TSKTController : Controller
    {
        CNPMNCEntities db = new CNPMNCEntities();
        // GET: Admin/TSKT
        public ActionResult TSKT(int? size, int? page, string currenFilter, string SearchString)
        {
            var thongtin = new List<THONGSOKT>();
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
                thongtin = db.THONGSOKTs.Where(n => n.PIN.Contains(SearchString)).ToList();
            }
            else
            {
                //lấy ds tên nv theo bảng NV
                thongtin = db.THONGSOKTs.ToList();
            }
            ViewBag.CurrenFilter = SearchString;
            //1.Tạo list pageSize để người dùng có thể chọn xem để phân trang
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
            thongtin = thongtin.OrderBy(n => n.THONGSOKTID).ToList();

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

        // GET: Admin/TSKT/Details/5
        public ActionResult Details(int id)
        {
            var Details = db.THONGSOKTs.Find(id);

            return View(Details);
        }

        public ActionResult SelectCateDT()
        {
            DIENTHOAI se_cate = new DIENTHOAI();
            se_cate.ListCateDT = db.DIENTHOAIs.ToList<DIENTHOAI>();
            return PartialView(se_cate);
        }
        // GET: Admin/TSKT/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/TSKT/Create
        [HttpPost]
        public ActionResult Create(THONGSOKT model)
        {
            try
            {
                db.THONGSOKTs.Add(model);
                db.SaveChanges();
                return RedirectToAction("TSKT");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/TSKT/Edit/5
        public ActionResult Edit(int id)
        {
            var editing = db.THONGSOKTs.Find(id);
            return View(editing);
        }

        // POST: Admin/TSKT/Edit/5
        [HttpPost]
        public ActionResult Edit(THONGSOKT model)
        {
            try
            {
                var sua = db.THONGSOKTs.Find(model.THONGSOKTID);
                sua.DIENTHOAIID = model.DIENTHOAIID;
                sua.MANHINH = model.MANHINH;
                sua.DOPHANGIAI = model.DOPHANGIAI;
                sua.CAMERA = model.CAMERA;
                sua.HEDH = model.HEDH;
                sua.CHIPXULY = model.CHIPXULY;
                sua.ROM = model.ROM;
                sua.RAM = model.RAM;
                sua.MANGDIDONG = model.MANGDIDONG;
                sua.SOKESIM = model.SOKESIM;
                sua.PIN = model.PIN;
                db.SaveChanges();
                return RedirectToAction("TSKT");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/TSKT/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/TSKT/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
