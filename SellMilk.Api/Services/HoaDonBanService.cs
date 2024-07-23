using Newtonsoft.Json;
using SellMilk.Api.Helper;
using SellMilk.Api.Helper.Interfaces;
using SellMilk.Api.Models.HoaDonBan;
using SellMilk.Api.Models.LoaiSua;
using System.Data;
using System.Reflection;

namespace SellMilk.Api.Services
{
    public class HoaDonBanService
    {
        private ITruyVanDuLieu _truyvan;
        private XacThucService _xacThucService;
        public HoaDonBanService(ITruyVanDuLieu dbHelper, XacThucService xacThucService)
        {
            _truyvan = dbHelper;
            _xacThucService = xacThucService;
        }

        public List<HoaDonBan> getHoaDonBan()
        {
            string msgError = "";
            try
            {
                var dt = _truyvan.ExecuteSProcedureReturnDataTable(out msgError, "sp_LayThongTinHoaDonBan");
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);
                return dt.ConvertTo<HoaDonBan>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("lỗi rồi", ex);
            }

        }

        public List<HoaDonBan> GetHoaDonBanByNguoiDungID(int id)
        {
            string msgError = "";
            var dt = _truyvan.ExecuteSProcedureReturnDataTable(out msgError, "sp_LayThongTinHoaDonBanTheoNguoiDungID", "@NguoiDungID", id);

            if (!string.IsNullOrEmpty(msgError))
            {
                throw new Exception(msgError);
            }

            return dt.ConvertTo<HoaDonBan>().ToList();

        }



        public HoaDonBan GetHoaDonBanVaChiTiet(int hoaDonBanId)
        {
            string msgError = "";
            try
            {
                DataSet ds = _truyvan.ExecuteSProcedureReturnDataSet(out msgError, "sp_LayThongTinHoaDonBanTheoID",  "@HDBID", hoaDonBanId );
                if (!string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError);
                }

                HoaDonBan hoaDonBan = null;
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // Extract the main invoice data
                    DataRow row = ds.Tables[0].Rows[0];
                    hoaDonBan = new HoaDonBan
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        NguoiDungID = Convert.ToInt32(row["NguoiDungID"]),
                        Tong = Convert.ToDecimal(row["Tong"]),
                        Status = row["Status"] != DBNull.Value ? Convert.ToBoolean(row["Status"]) : null,
                        CreatedAt = Convert.ToDateTime(row["CreatedAt"])
                    };

                    // Extract the invoice details
                    if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    {
                        hoaDonBan.CTHoaDonBans = new List<CTHoaDonBan>();
                        foreach (DataRow detailRow in ds.Tables[1].Rows)
                        {
                            var Sua = new SellMilk.Api.Models.Sua.Sua
                            {
                                ID = Convert.ToInt32(detailRow["SuaID"]),
                                Ten = detailRow["TenSua"].ToString(),
                                MoTa = detailRow["MoTa"].ToString(),
                                NguyenLieu = detailRow["NguyenLieu"].ToString(),
                                KhoiLuong = detailRow["KhoiLuong"].ToString(),
                                Anh = detailRow["Anh"].ToString(),
                                AnhKhac = detailRow["AnhKhac"].ToString(),
                                Status = Convert.ToBoolean(detailRow["Status"]),
                                TonKho = Convert.ToInt32(detailRow["TonKho"])
                            };
                            hoaDonBan.CTHoaDonBans.Add(new CTHoaDonBan
                            {
                                //ID = Convert.ToInt32(detailRow["ID"]),
                                HDBID = Convert.ToInt32(detailRow["HDBID"]),
                                SuaID = Convert.ToInt32(detailRow["SuaID"]),
                                TenSua = detailRow["TenSua"].ToString(),
                                SoLuong = Convert.ToInt32(detailRow["SoLuong"]),
                                Gia = Convert.ToDecimal(detailRow["Gia"]),
                                Tong = Convert.ToDecimal(detailRow["Tong"]),
                                Sua=Sua,
                            }) ;

                        }

                    }


                }

                return hoaDonBan;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi rồi - thất bại", ex);

            }
        }






        public async Task<bool> ThemHoaDonBan(HoaDonBanThem hoaDonBan)
        {
            string msgError = "";
            try
            {
                var nguoidung = await  _xacThucService.ThongTinNguoiDungAsync();
                // Serialize the order details list to JSON
                string chiTietHoaDonJson = hoaDonBan.CTHoaDonBans != null ? MessageConvert.SerializeObject(hoaDonBan.CTHoaDonBans) : null;

                object[] parameters = new object[]
                {
                    "@NguoiDungID", nguoidung.ID,
                    "@Tong", hoaDonBan.Tong,
                    "@ChiTietHoaDon", chiTietHoaDonJson
                };

                var result = _truyvan.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_ThemHoaDonBanVaChiTiet", parameters);
                if (!string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CapNhapTrangThai(HoaDonBanSua request)
        {
            string msgError = "";
            try
            {
                object[] parameters = new object[] { "@ID", request.ID, "@Status", request.Status };
                _truyvan.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_CapNhatTrangThaiHoaDon", parameters);

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

    }
}

