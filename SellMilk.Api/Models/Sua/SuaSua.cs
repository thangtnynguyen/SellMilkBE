using SellMilk.Api.Models.ThongSo;
using SellMilk.Api.Models.Gia;
namespace SellMilk.Api.Models.Sua
{
    public class SuaSua
    {
        public int ID { get; set; }

        public int LoaiID { get; set; }

        public string Ten { get; set; }

        public string MoTa { get; set; }

        public string NguyenLieu { get; set; }

        public string KhoiLuong { get; set; }

        public IFormFile? AnhFile { get; set; }

        public string Anh { get; set; }

        public List<IFormFile>? AnhKhacFile { get; set; }

        public string AnhKhac { get; set; }

        public bool Status { get; set; } = true;

        public int TonKho { get; set; } = 0;

        // Navigation properties

        //public SellComputer.Api.Models.LoaiSua.LoaiSua LoaiSua { get; set; }

        public ICollection<ThongSoThem> ThongSos { get; set; }

        public GiaThem Gia { get; set; }


    }
}
