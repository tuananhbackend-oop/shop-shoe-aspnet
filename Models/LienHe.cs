using System;
using System.Collections.Generic;

namespace LVTNWEBGIAYDEP.Models
{
    public partial class LienHe
    {
        public int? Id { get; set; } = null;
        public string? NoiDung { get; set; }
        public string TaiKhoan { get; set; } = null!;
        public string? trangThai { get; set; }
        public string? noiDungPhanHoi { get; set; }
        
        public virtual TaiKhoan TaiKhoanNavigation { get; set; } = null!;
    }
}
