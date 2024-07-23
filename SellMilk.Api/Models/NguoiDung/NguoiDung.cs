namespace SellMilk.Api.Models.NguoiDung
{
    public class NguoiDung
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Avatar { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool? EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string PhoneNumber { get; set; }

        public int Role { get; set; } = 0;
    }

}
