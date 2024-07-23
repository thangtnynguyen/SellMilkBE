namespace SellMilk.Api.Models.HoaDonBan
{
  
    public class HoaDonBanThem
    {
        public int NguoiDungID { get; set; }

        public decimal Tong { get; set; }

        public ICollection<CTHoaDonBanThem> CTHoaDonBans { get; set; }

    }
    public class CTHoaDonBanThem
    {
        public int SuaID { get; set; }

        public string TenSua { get; set; }

        public int SoLuong { get; set; }

        public decimal Gia { get; set; }


    }
}
