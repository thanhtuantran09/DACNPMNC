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
            const int pageSize = 5;
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
            return View(dienthoai);
        }
    }
}