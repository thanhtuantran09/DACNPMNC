using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CNPMNC.Models
{
    public class CartItem
    {
        public DIENTHOAI sanpham { get; set; }
        public int soluong { get; set; }
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
        public void Update_Sl(int id, int sl)
        {
            var item=items.Find(s=>s.sanpham.DIENTHOAIID==id);
            if (item != null)
            {
                if (sl > 0) // Kiểm tra số lượng lớn hơn 0
                {
                    item.soluong = sl;
                }
                else // Nếu số lượng <= 0, xóa sản phẩm khỏi giỏ hàng
                {
                    items.Remove(item);
                }
            }
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