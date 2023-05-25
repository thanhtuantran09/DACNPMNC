using CNPMNC.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using Chart = System.Web.UI.DataVisualization.Charting.Chart;

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
            model.TotalOrders = db.DONHANGs.Where(x => x.TRANGTHAIID == 6).Count();
            model.TotalProducts = db.DIENTHOAIs.Count();
            model.TotalRevenue = db.DONHANGs.Where(x => x.TRANGTHAIID == 6)
                                     .Sum(x => (decimal?)x.THANHTIEN) ?? 0;

            return View(model);
        }
        
        public ActionResult Chart()
        {
            var data = db.DONHANGs.Where(x => x.TRANGTHAIID != 1).ToList();
            var chart = new Chart();
            var area = new ChartArea();
            area.AxisX.Minimum = 1;
            area.AxisX.Maximum = 12;
            area.AxisY.Minimum = 0;
            area.AxisY.Maximum = (double)data.Sum(x => x.THANHTIEN);
            area.AxisX.Interval = 1;
            area.AxisY.Interval = (double)(data.Sum(x => x.THANHTIEN) / 10);
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.Enabled = false;
            chart.Width = 800;
            chart.Height = 500;
            chart.ChartAreas.Add(area);
            var series = new Series();
            var year = DateTime.Now.Year;
            var ordersByMonth = data.GroupBy(x => x.NGAYTAO.Value.Month);
            foreach (var item in ordersByMonth)
            {
                var month = item.Key;
                var totalRevenue = item.Sum(x => x.THANHTIEN);
                series.Points.AddXY(month, totalRevenue);               
            }
            Color[] colors = new Color[] { Color.LightGreen, Color.MistyRose, Color.Blue };
            for (int i = 0; i < series.Points.Count; i++)
            {
                series.Points[i].Color = colors[i];
            }
            chart.Titles.Add($"Doanh thu theo tháng năm {year}");
            series.Label = "#PERCENT{P0}";
            series.Font = new Font("Arial", 8.0f, FontStyle.Bold);
            series.ChartType = SeriesChartType.Column;
            series["PieLabelStyle"] = "Outside";
            chart.Series.Add(series);
            area.AxisY.LabelStyle.Format = "{N0} đ"; // Add thousand separator
            var returnStream = new MemoryStream();
            chart.ImageType = ChartImageType.Png;
            chart.SaveImage(returnStream);
            returnStream.Position = 0;
            return new FileStreamResult(returnStream, "image/png");
        }
    }
}