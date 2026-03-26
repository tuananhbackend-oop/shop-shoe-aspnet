using System;
using System.Collections.Generic;

namespace LVTNWEBGIAYDEP.Models
{
    public partial class TaiKhoan
    {
        public TaiKhoan()
        {
            DonHangs = new HashSet<DonHang>();
            LienHes = new HashSet<LienHe>();
        }

        public string TaiKhoan1 { get; set; } = null!;
        public string MatKhau { get; set; } = null!;
		public string HoTen { get; set; } = null!;
        public string? trangThai { get; set; } 
		public string? phanQuyen { get; set; }


		public virtual ICollection<DonHang> DonHangs { get; set; }
        public virtual ICollection<LienHe> LienHes { get; set; }
    }
}
