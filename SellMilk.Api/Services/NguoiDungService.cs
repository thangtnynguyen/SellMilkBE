using SellMilk.Api.Helper;
using SellMilk.Api.Helper.Interfaces;
using SellMilk.Api.Models.NguoiDung;
using System.Data;

namespace SellMilk.Api.Services
{
    public class NguoiDungService
    {

        private ITruyVanDuLieu _truyvan;
        public NguoiDungService(ITruyVanDuLieu dbHelper)
        {
            _truyvan = dbHelper;
        }

        public List<NguoiDung> LayTatCaNguoiDung()
        {
            string msgError = "";
            try
            {
                var dt = _truyvan.ExecuteSProcedureReturnDataTable(out msgError, "sp_LayTatNguoiDung");
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);
                return dt.ConvertTo<NguoiDung>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("lỗi rồi", ex);
            }

        }

        public bool DangKi(NguoiDungDangKi newUser)
        {
            string msgError = "";
            try
            {
                string password = newUser.PasswordHash;
                string passwoedHash = password.HashPassword();

                object[] parameters = new object[]
                {
                    "@Name", newUser.Name,
                    "@UserName", newUser.UserName,
                    "@PasswordHash", passwoedHash,
                    "@Role", newUser.Role ,
                };
                string ere = "";
                var result = _truyvan.ExecuteScalarSProcedureWithTransaction(out ere, "sp_DangKyNguoiDung", parameters);
                if (!string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool DangNhap(string username, string password)
        {
            string msgError = "";
            try
            {
                object[] parameters = new object[] { "@UserName", username };
                var dt = _truyvan.ExecuteSProcedureReturnDataTable(out msgError, "sp_DangNhapNguoiDung", parameters);
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                if (dt.Rows.Count > 0)
                {
                    string storedHash = dt.Rows[0]["PasswordHash"].ToString();
                    return MaHoaMatKhau.VerifyPassword(password, storedHash);
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int LayRole(string username)
        {
            string msgError = "";
            try
            {
                object[] parameters = new object[] { "@UserName", username };
                var dt = _truyvan.ExecuteSProcedureReturnDataTable(out msgError, "sp_LayRoleNguoiDung", parameters);
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                int role = int.Parse(dt.Rows[0]["Role"].ToString());
                return role;

            }
            catch (Exception)
            {
                throw new Exception(msgError);
            }
        }

        public NguoiDung LayThongTinNguoiDung(string username)
        {
            string msgError = "";
            try
            {
                object[] parameters = new object[] { "@UserName", username };
                var dt = _truyvan.ExecuteSProcedureReturnDataTable(out msgError, "sp_LayThongTinNguoiDung", parameters);
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    return new NguoiDung
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Name = row["Name"].ToString(),
                        Avatar = row["Avatar"].ToString(),
                        UserName = row["UserName"].ToString(),
                        Email = row["Email"].ToString(),
                        EmailConfirmed = row["EmailConfirmed"] as bool?,
                        PasswordHash = row["PasswordHash"].ToString(),
                        PhoneNumber = row["PhoneNumber"].ToString(),
                        Role = Convert.ToInt32(row["Role"])
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Thất bại rồi huhu", ex);
            }
        }

        public bool CapNhapThongTin(NguoiDungSua request)
        {
            try
            {
                string msgError = "";
                object[] parameters = {
                "@ID", request.ID,
                "@Name", request.Name,
                "@Avatar", request.Avatar, 
                "@Email", request.Email,
                "@PhoneNumber", request.PhoneNumber};

                _truyvan.ExecuteSProcedureReturnDataTable(out msgError, "sp_CapNhatThongTinNguoiDung", parameters);
                if (!string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError);
                }
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Thất bại rồi huhu", ex);

            }
        }









    }

}

