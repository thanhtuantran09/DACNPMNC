using CNPMNC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CNPMNC.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        CNPMNCEntities db = new CNPMNCEntities();
        // GET: Admin/Dashboard
        public ActionResult Dashboard()
        {
            var model = new Dashboard();
            model.TotalUsers = db.KHACHHANGs.Count();
            model.TotalAdmin = db.ADMINs.Count();
            model.TotalNewOrders = db.DONHANGs.Where(x => x.TRANGTHAIID == 1).Count();
            model.TotalOrders = db.DONHANGs.Where(x => x.TRANGTHAIID == 2).Count();
            model.TotalProducts = db.DIENTHOAIs.Count();
            model.TotalRevenue = db.DONHANGs.Where(x => x.TRANGTHAIID == 2)
                                     .Sum(x => (decimal?)x.THANHTIEN) ?? 0;

            return View(model);
        }
    }
}