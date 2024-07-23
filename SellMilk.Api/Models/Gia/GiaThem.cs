namespace SellMilk.Api.Models.Gia
{
    public class GiaThem
    {
        public float? GiaSanPham { get; set; }

        public float? GiaTruocGiam { get; set; }

        public DateTime? NgayBatDau { get; set; }

        public DateTime? NgayKetThuc { get; set; }

        public bool Status { get; set; } = true;
    }
}
