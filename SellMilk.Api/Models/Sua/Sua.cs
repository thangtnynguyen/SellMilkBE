namespace SellMilk.Api.Models.Sua
{
    public class Sua
    {
        public int ID { get; set; }

        public int LoaiID { get; set; }

        public string Ten { get; set; }

        public string MoTa { get; set; }

        public string NguyenLieu { get; set; }

        public string KhoiLuong { get; set; }

        public string Anh { get; set; }

        public string AnhKhac { get; set; }

        public bool Status { get; set; } = true;

        public int TonKho { get; set; } = 0;

        // Navigation properties
        //public SellMilk.Api.Models.LoaiSua.LoaiSua LoaiSua { get; set; }
        public ICollection<SellMilk.Api.Models.ThongSo.ThongSo> ThongSos { get; set; }
        public SellMilk.Api.Models.Gia.Gia Gia { get; set; }
    }

}
