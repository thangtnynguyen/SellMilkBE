namespace SellMilk.Api.Models.NguoiDung
{
    public class NguoiDungDangKi
    {

        public string Name { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public int Role { get; set; } = 0;
    }
}
