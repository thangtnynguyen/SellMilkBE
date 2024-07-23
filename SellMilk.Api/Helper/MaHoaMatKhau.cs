using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellMilk.Api.Helper
{
    public static class MaHoaMatKhau
    {
        public static string HashPassword(this string password)
        {
            // Mã hóa mật khẩu bằng BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);          
        }
        public static bool VerifyPassword(this string candidatePassword, string hashedPassword)
        {
            // Sử dụng BCrypt để kiểm tra mật khẩu
            bool kq = BCrypt.Net.BCrypt.Verify(candidatePassword, hashedPassword);
            return kq;
        }
    }
}
