namespace SellMilk.Api.Models.NguoiDung
{
    public class NguoiDungSua
    {

        public int ID { get; set; }

        public string Name { get; set; }

        public IFormFile? AvatarFile { get; set; }

        public string Avatar { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }


    }
}
