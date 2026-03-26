using System;
using System.Collections.Generic;

namespace LVTNWEBGIAYDEP.Models
{
    public partial class LoaiSanPham
    {
        public LoaiSanPham()
        {
            SanPhams = new HashSet<SanPham>();
        }

        public int MaLoai { get; set; }
        public string? TenLoai { get; set; }
        public int? MaTh { get; set; }

        public virtual ThuongHieu? MaThNavigation { get; set; }
        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
