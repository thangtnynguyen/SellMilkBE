namespace SellMilk.Api.Models.NguoiDung
{
    public class NguoiDungDangNhap
    {

        public string UserName {  get; set; }

        public string PasswordHash { get; set; }

        public int Role { get; set; } = 0;
    }
}
