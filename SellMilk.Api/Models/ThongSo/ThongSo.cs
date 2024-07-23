using System.Text.Json.Serialization;

namespace SellMilk.Api.Models.ThongSo
{
    public class ThongSo
    {
        public int ID { get; set; }

        public int? SuaID { get; set; }

        public string TenThongSo { get; set; }

        public string MoTa { get; set; }

        // Navigation property
        //[JsonIgnore]
        //public SellComputer.Api.Models.MayTinh.MayTinh MayTinh { get; set; }
    }

}
