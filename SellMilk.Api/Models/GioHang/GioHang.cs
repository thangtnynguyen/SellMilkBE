namespace SellMilk.Api.Models.GioHang
{
    public class GioHang
    {
        public int ID { get; set; }
        public int NguoiDungID { get; set; }
        public int SuaID { get; set; }
        public string TenSua { get; set; }
        public int SoLuong { get; set; }
        public decimal Gia { get; set; }
        public decimal Tong { get; set; }

        // Navigation properties
        public SellMilk.Api.Models.NguoiDung.NguoiDung NguoiDung { get; set; }
        public SellMilk.Api.Models.Sua.Sua Sua { get; set; }
    }

}
