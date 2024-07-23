﻿namespace SellMilk.Api.Models.HoaDonBan
{
    public class HoaDonBanLay
    {
        public int ID { get; set; }
        public int NguoiDungID { get; set; }
        public decimal Tong { get; set; }
        public bool? Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public SellMilk.Api.Models.NguoiDung.NguoiDung NguoiDung { get; set; }
        public ICollection<CTHoaDonBanLay> CTHoaDonBans { get; set; }

    }

    public class CTHoaDonBanLay
    {
        public int ID { get; set; }

        public int HDBID { get; set; }

        public int SuaID { get; set; }

        public string Anh { get; set; }

        public string TenSua { get; set; }

        public int SoLuong { get; set; }

        public decimal Gia { get; set; }

        public decimal Tong { get; set; }

        // Navigation properties
        public HoaDonBan HoaDonBan { get; set; }
        public SellMilk.Api.Models.Sua.Sua Sua { get; set; }
    }
}
