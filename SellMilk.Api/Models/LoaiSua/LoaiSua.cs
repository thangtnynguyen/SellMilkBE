namespace SellMilk.Api.Models.LoaiSua
{
    public class LoaiSua
    {
        public int ID { get; set; }

        public string Ten { get; set; }

        public string MoTa { get; set; }

        public string Anh { get; set; }

        public bool Status { get; set; } = true;

        public bool ShowMenu { get; set; } = false;
    }

}
