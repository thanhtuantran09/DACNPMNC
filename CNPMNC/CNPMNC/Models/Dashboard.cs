using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CNPMNC.Models
{
    public class Dashboard
    {
        public int TotalUsers { get; set; }
        public int TotalAdmin{ get; set; }
        public int TotalNewOrders { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        [DisplayFormat(DataFormatString = "{0:0,0 đ}", ApplyFormatInEditMode = false)]
        public Nullable<decimal> TotalRevenue { get; set; }
    }
}