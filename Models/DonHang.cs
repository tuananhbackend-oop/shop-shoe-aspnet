using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LVTNWEBGIAYDEP.Models
{
    public partial class DonHang
    {
        public DonHang()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
        }
        public int iDDonHang { get; set; }
        public string SoDienThoai { get; set; }
	
		public string? DiaChi { get; set; }
		
		public string? TaiKhoan { get; set; }
		public string? thanhToan { get; set; }
		public string? trangThai { get; set; }
		public string? noiDungHuy { get; set; }
        public DateTime? ngayLapDon { get; set; }


        public virtual TaiKhoan? TaiKhoanNavigation { get; set; }
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}
