using SellMilk.Api.Models.Sua;
using System.Text.Json.Serialization;

namespace SellMilk.Api.Models.Gia
{
    public class Gia
    {
        public int ID { get; set; }
        public int? SuaID { get; set; }
        public float? GiaSanPham { get; set; }
        public float? GiaTruocGiam { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public bool Status { get; set; } = true;

        // Navigation property
        //[JsonIgnore]
        //public SellComputer.Api.Models.MayTinh.MayTinh MayTinh { get; set; }
    }

}
