using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CNPMNC.Models
{
    public class CartItem
    {
        public DIENTHOAI sanpham { get; set; }
        public int soluong { get; set; }
        public string ErrorMessage { get; set; } // add this property
    }
    public class Cart
    {
        List<CartItem> items =new List<CartItem>();
        public IEnumerable<CartItem> Items 
        { 
            get { return items; } 
        }
        public void Add(DIENTHOAI sp,int sl = 1) 
        {
            var item = items.FirstOrDefault(s => s.sanpham.DIENTHOAIID == sp.DIENTHOAIID);
            if (item == null)
            items.Add(new CartItem { sanpham = sp, soluong = sl });
            else
                item.soluong += sl;
            
        }
        public int Tongsoluong()
        {
            return items.Sum(s => s.soluong);
        }
        public void Update_Sl(int id, int sl, ControllerContext controllerContext)
        {
            var item = items.Find(s => s.sanpham.DIENTHOAIID == id);
            bool error = false;
            if (item != null)
            {
               
                if (item.sanpham.SOLUONGTON < sl)
                {
                    
                    item.ErrorMessage = "Số lượng sản phẩm trong kho không đủ";
                    error = true;
                    
                }
                else if (sl <= 0)
                {
                    items.Remove(item);
                }
                else
                {
                    
                    item.soluong = sl;
                }
            }
            if (!error)
            {

                item.ErrorMessage = null;

            }
            //if (error)
            //{
            //    controllerContext.Controller.TempData["Message"] = item.ErrorMessage;
            //}
            //else
            //{
            //    controllerContext.Controller.TempData["Message"] = "Cập nhật số lượng sản phẩm thành công";
            //}
        }
        public double Tongtien()
        {
            var tongtien=items.Sum(s=>s.sanpham.GIAGIAM*s.soluong);
            return (double)tongtien;
        }
        public void Xoasp(int id)
        {
            items.RemoveAll(s => s.sanpham.DIENTHOAIID == id);
        }
        public void Xoagh()
        {
            items.Clear();
        }
    }
}