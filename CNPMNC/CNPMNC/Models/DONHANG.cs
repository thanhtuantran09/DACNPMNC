//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CNPMNC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DONHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DONHANG()
        {
            this.CTDONHANGs = new HashSet<CTDONHANG>();
        }
    
        public int DONHANGID { get; set; }
        public Nullable<int> KHACHHANGID { get; set; }
        public Nullable<int> TRANGTHAIID { get; set; }
        public Nullable<System.DateTime> NGAYTAO { get; set; }
        public string GIAMGIA { get; set; }
        public Nullable<decimal> THANHTIEN { get; set; }
        public string TENKH { get; set; }
        public string DIACHI { get; set; }
        public string SDT { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTDONHANG> CTDONHANGs { get; set; }
        public virtual KHACHHANG KHACHHANG { get; set; }
        public virtual TRANGTHAIDH TRANGTHAIDH { get; set; }
    }
}