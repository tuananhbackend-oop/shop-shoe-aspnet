using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LVTNWEBGIAYDEP.Models
{
    public partial class SanPham
    {
        public SanPham()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
        }

        public string MaSp { get; set; } = null!;
        public string? TenSp { get; set; }
        public string? HinhAnh { get; set; }
		[Range(0.01, double.MaxValue, ErrorMessage = "Giá bán phải lớn hơn 0.")]
		public int? GiaBan { get; set; }
        public string? TrangThai { get; set; }
        public string? MoTa { get; set; }
        public int? HangTonKho { get; set; }
        public int? KichCo { get; set; }
        public int MaLoai { get; set; }
        public DateTime? NgayThemSP { get; set; }



        public virtual LoaiSanPham MaLoaiNavigation { get; set; } = null!;
      
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}
