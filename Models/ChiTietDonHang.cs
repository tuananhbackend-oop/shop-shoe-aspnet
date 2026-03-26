using System;
using System.Collections.Generic;

namespace LVTNWEBGIAYDEP.Models
{
    public partial class ChiTietDonHang
    {
      
        public int? SoLuong { get; set; }
        public string MaSp { get; set; } = null!;
        public int iDDonHang { get; set; }

        public virtual SanPham MaSpNavigation { get; set; } = null!;
        public virtual DonHang iDDonHangNavigation { get; set; } = null!;
    }
}
