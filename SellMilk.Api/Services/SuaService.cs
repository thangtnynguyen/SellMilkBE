

using Newtonsoft.Json;
using SellMilk.Api.Helper;
using SellMilk.Api.Helper.Interfaces;
using SellMilk.Api.Models.Gia;
using SellMilk.Api.Models.Sua;
using SellMilk.Api.Models.ThongSo;
using System.Data.SqlClient;
using System.Data;
using SellMilk.Api.Models.Common;

namespace SellMilk.Api.Services
{
    public class SuaService
    {

        private ITruyVanDuLieu _truyvan;
        public SuaService(ITruyVanDuLieu dbHelper)
        {
            _truyvan = dbHelper;
        }
        public bool Create(SuaThem model)
        {
            string msgError = "";
            try
            {
                var thongSoKyThuatJson = model.ThongSos != null ? MessageConvert.SerializeObject(model.ThongSos) : null;
                var giaJson = model.Gia != null ? MessageConvert.SerializeObject(model.Gia) : null;

                // Log để xem chuỗi JSON trước khi gửi đi
                if (model.ThongSos == null || !model.ThongSos.Any())
                {
                    Console.WriteLine("Thong So is null or empty");
                }

                //Console.WriteLine("Thong So Ky Thuat: " + JsonConvert.SerializeObject(model.ThongSos));

                Console.WriteLine("Thong So  JSON: " + thongSoKyThuatJson);
                Console.WriteLine("Gia JSON: " + giaJson);

                object[] parameters = new object[] {
                    "@LoaiID", model.LoaiID,
                    "@Ten", model.Ten,
                    "@MoTa", model.MoTa,
                    "@NguyenLieu", model.NguyenLieu,
                    "@KhoiLuong", model.KhoiLuong,
                    "@Anh", model.Anh,
                    "@AnhKhac", model.AnhKhac,
                    "@TonKho", model.TonKho,
                    "@ThongSoNvarchar", model.ThongSos != null ? MessageConvert.SerializeObject(model.ThongSos) : null,
                    "@GiaNvarchar", model.Gia != null ? MessageConvert.SerializeObject(model.Gia) : null};
                var result = _truyvan.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_ThemSuaVaThongSo", parameters);

                if ((result != null && !string.IsNullOrEmpty(result.ToString())) || !string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(Convert.ToString(result) + msgError);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Add thất bại", ex);
            }
        }
        public bool Update(SuaSua model)
        {
            string msgError = "";
            try
            {
                string thongSoJson = model.ThongSos != null ? MessageConvert.SerializeObject(model.ThongSos) : null;
                string giaJson = model.Gia != null ? MessageConvert.SerializeObject(model.Gia) : null;

                Console.WriteLine("ThongSo JSON: " + thongSoJson);
                Console.WriteLine("Gia JSON: " + giaJson);


                object[] parameters = new object[] {
                    "@SuaID",model.ID,
                    "@LoaiID", model.LoaiID,
                    "@Ten", model.Ten,
                    "@MoTa", model.MoTa,
                    "@NguyenLieu", model.NguyenLieu,
                    "@KhoiLuong", model.KhoiLuong,
                    "@Anh", model.Anh,
                    "@AnhKhac", model.AnhKhac,
                    "@TonKho", model.TonKho,
                    "@ThongSoNvarchar", model.ThongSos != null ? MessageConvert.SerializeObject(model.ThongSos) : null,
                    "@GiaNvarchar", model.Gia != null ? MessageConvert.SerializeObject(model.Gia) : null};
                var result = _truyvan.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_SuaThongTinSua", parameters);

                //if (result == null || !string.IsNullOrEmpty(msgError))
                //{
                //    throw new Exception(msgError);
                //}
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Sửa thất bại", ex);
            }
        }

        public bool UpdateTonKho(SuaCapNhapTonKho request)
        {
            string msgError = "";
            try
            {
                object[] parameters = {
                    "@ID", request.ID,
                    "@TonKho", request.TonKho
                };

                _truyvan.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_CapNhatTonKhoSua",  parameters);
                if (!string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Sửa thất bại", ex);

            }
        }

        public PagingResult<Sua> Search(SuaTimKiem request)
        {
            string msgError = "";
            try
            {
                object[] parameters = new object[] {
                    "@page_index", request.PageIndex,
                    "@page_size", request.PageSize,
                    "@ten_Sua_tinh", request.Ten ?? null};
                var dt = _truyvan.ExecuteSProcedureReturnDataTable(out msgError, "sp_TimKiemSua", parameters);
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                if (dt.Rows.Count > 0)
                {
                    var Suas = new List<Sua>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var Sua = new Sua
                        {
                            ID = row["ID"] != DBNull.Value ? Convert.ToInt32(row["ID"]) : 0,
                            LoaiID = row["LoaiID"] != DBNull.Value ? Convert.ToInt32(row["LoaiID"]) : 0,
                            Ten = row["Ten"] != DBNull.Value ? row["Ten"].ToString() : string.Empty,
                            MoTa = row["MoTa"] != DBNull.Value ? row["MoTa"].ToString() : string.Empty,
                            NguyenLieu = row["NguyenLieu"] != DBNull.Value ? row["NguyenLieu"].ToString() : string.Empty,
                            KhoiLuong = row["KhoiLuong"] != DBNull.Value ? row["KhoiLuong"].ToString() : string.Empty,
                            Anh = row["Anh"] != DBNull.Value ? row["Anh"].ToString() : string.Empty,
                            AnhKhac = row["AnhKhac"] != DBNull.Value ? row["AnhKhac"].ToString() : string.Empty,
                            Status = row["Status"] != DBNull.Value ? Convert.ToBoolean(row["Status"]) : false,
                            TonKho = row["TonKho"] != DBNull.Value ? Convert.ToInt32(row["TonKho"]) : 0,
                            Gia = new Gia
                            {
                                GiaSanPham = row["Gia"] != DBNull.Value ? Convert.ToSingle(row["Gia"]) : 0,
                                GiaTruocGiam = row["GiaTruocGiam"] != DBNull.Value ? Convert.ToSingle(row["GiaTruocGiam"]) : 0,
                                NgayBatDau = row["NgayBatDau"] != DBNull.Value ? Convert.ToDateTime(row["NgayBatDau"]) : DateTime.MinValue,
                                NgayKetThuc = row["NgayKetThuc"] != DBNull.Value ? Convert.ToDateTime(row["NgayKetThuc"]) : DateTime.MinValue
                            }
                        };
                        Suas.Add(Sua);
                    }

                    long totalRecords = Convert.ToInt64(dt.Rows[0]["RecordCount"]);
                    long totalPages = (long)totalRecords / ((int)request.PageSize);
                    if ((long)dt.Rows[0]["RecordCount"] % ((int)request.PageSize) != 0)
                    {
                        totalPages++;
                    }
                    ////long totalPages = (long)Math.Ceiling((double)totalRecords / request.PageSize);

                    return new PagingResult<Sua>()
                    {
                        Items = Suas,
                        PageIndex = request.PageIndex,
                        PageSize = request.PageSize,
                        TotalRecords = totalRecords,
                        TotalPages = totalPages,
                    };
                }
                else
                {
                    return new PagingResult<Sua>()
                    {
                        Items = new List<Sua>(), // Trả về danh sách rỗng thay vì null
                        PageIndex = 0,
                        PageSize = 0,
                        TotalRecords = 0,
                        TotalPages = 0,
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public Sua GetSuaByID(int id)
        {
            string msgError = "";
            DataSet ds = _truyvan.ExecuteSProcedureReturnDataSet(out msgError, "sp_LayThongTinSuaTheoID", "@SuaID", id);

            if (!string.IsNullOrEmpty(msgError))
            {
                throw new Exception(msgError);
            }

            Sua Sua = null;

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // Xử lý bảng thông tin cơ bản của máy tính
                var dtSua = ds.Tables[0];
                DataRow row = dtSua.Rows[0];
                Sua = new Sua
                {
                    ID = Convert.ToInt32(row["ID"]),
                    LoaiID = Convert.ToInt32(row["LoaiID"]),
                    Ten = row["Ten"].ToString(),
                    MoTa = row["MoTa"].ToString(),
                    NguyenLieu = row["NguyenLieu"].ToString(),
                    KhoiLuong = row["KhoiLuong"].ToString(),
                    Anh = row["Anh"].ToString(),
                    AnhKhac = row["AnhKhac"].ToString(),
                    Status = Convert.ToBoolean(row["Status"]),
                    TonKho = Convert.ToInt32(row["TonKho"])
                };

                // Xử lý bảng thông tin giá (giả định là Table[1])
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    DataRow giaRow = ds.Tables[1].Rows[0];
                    Sua.Gia = new Gia
                    {   
                        ID= Convert.ToInt32(giaRow["ID"]),
                        GiaSanPham = Convert.ToSingle(giaRow["Gia"]),
                        GiaTruocGiam = Convert.ToSingle(giaRow["GiaTruocGiam"]),
                        NgayBatDau = Convert.ToDateTime(giaRow["NgayBatDau"]),
                        NgayKetThuc = Convert.ToDateTime(giaRow["NgayKetThuc"]),
                        Status = Convert.ToBoolean(giaRow["Status"])
                    };
                }

                // Xử lý bảng thông số kỹ thuật (giả định là Table[2])
                if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                {
                    Sua.ThongSos = ds.Tables[2].AsEnumerable().Select(ts => new ThongSo
                    {   ID= Convert.ToInt32(ts["ID"]),
                        TenThongSo = ts.Field<string>("TenThongSo"),
                        MoTa = ts.Field<string>("MoTa")
                    }).ToList();
                }
            }

            return Sua;
        }




        public bool Delete(SuaXoa id)
        {
            try
            {
                object[] parameters = new object[] { "@SuaID", id.ID };
                var loi = _truyvan.ExecuteSProcedureNonQuery("sp_AnSua", parameters);
                if (loi.ToString() == "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
