using SellMilk.Api.Helper;
using SellMilk.Api.Helper.Interfaces;
using SellMilk.Api.Models.GioHang;
using SellMilk.Api.Models.Sua;

namespace SellMilk.Api.Services
{
    public class GioHangService
    {
        private ITruyVanDuLieu _truyvan;
        private XacThucService _xacThucService;
        public GioHangService(ITruyVanDuLieu dbHelper, XacThucService xacThucService)
        {
            _truyvan = dbHelper;
            _xacThucService = xacThucService;
        }


        public List<GioHang> GetGioHangByNguoiDungID(int id)
        {
            string msgError = "";
            var dt = _truyvan.ExecuteSProcedureReturnDataTable(out msgError, "sp_LayThongTinGioHangTheoNguoiDungID", "@NguoiDungID", id);

            if (!string.IsNullOrEmpty(msgError))
            {
                throw new Exception(msgError);
            }

            List<GioHang> gioHangs = dt.ConvertTo<GioHang>().ToList();
            for (int i = 0; i < gioHangs.Count; i++)
            {
                gioHangs[i].Sua = new Sua
                {
                    Anh = dt?.Rows[i]["Anh"].ToString(),
                };
            }

            return gioHangs;

        }

        public async Task<bool> ThemGioHang(GioHangThem request)
        {
            string msgError = "";
            try
            {
                var nguoidung = await _xacThucService.ThongTinNguoiDungAsync();

                object[] parameters = new object[]
                {
                    "@NguoiDungID", nguoidung.ID,
                    "@SuaID", request.SuaID,
                    "@TenSua", request.TenSua,
                    "@SoLuong", request.SoLuong,
                    "@Gia", request.Gia
                };

                var result = _truyvan.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_ThemVaoGioHang", parameters);
                if (!string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi rồi - thất bại", ex);
            }
        }

        public bool SuaGioHang(GioHangSua request)
        {
            string msgError = "";
            try
            {
                object[] parameters = new object[]
                {
                    "@ID", request.ID,
                    "@SoLuong", request.SoLuong
                };

                var result = _truyvan.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_CapNhatSoLuongGioHang", parameters);
                if (!string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi rồi - thất bại", ex);
            }
        }

        public bool XoaGioHang(SuaXoa id)
        {
            string msgError = "";
            try
            {
                object[] parameters = new object[] { "@ID", id.ID };
                _truyvan.ExecuteSProcedureNonQuery("sp_XoaMucGioHang", parameters);

                if (!string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi rồi - thất bại", ex);

            }
        }

        public bool XoaNhieuGioHang(SuaXoaNhieu ids)
        {
            string msgError = "";
            try
            {


                string idsString = string.Join(",", ids.IDs);                

                object[] parameters = new object[] { "@IDs", idsString };
                _truyvan.ExecuteSProcedureNonQuery("sp_XoaNhieuMucGioHang", parameters);

                if (!string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError);
                }

                //Console.Write("IDSSS LÀ LAFFFFFFFFF" + idsString);
                //Console.Write("IDSSS LÀ LAFFFFFFFFF" + ids.IDs);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi rồi - thất bại", ex);

            }
        }
    }
}

