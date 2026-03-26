using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LVTNWEBGIAYDEP.Models
{
    public partial class ThuongHieu
    {
        public ThuongHieu()
        {
            LoaiSanPhams = new HashSet<LoaiSanPham>();
        }

        
    
        public int MaTh { get; set; }
        public string? TenTh { get; set; }

        public virtual ICollection<LoaiSanPham> LoaiSanPhams { get; set; }
    }
}
