using SellMilk.Api.Models.NguoiDung;

namespace SellMilk.Api.Models.Common
{
    public class TokenRole
    {

        public string? Token { get; set; }

        public string? Role { get; set; }

        public SellMilk.Api.Models.NguoiDung.NguoiDung NguoiDung { get; set; }

    }
}
