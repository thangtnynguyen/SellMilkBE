using Newtonsoft.Json;
using SellMilk.Api.Helper;
using SellMilk.Api.Helper.Interfaces;
using SellMilk.Api.Models.Gia;
using SellMilk.Api.Models.LoaiSua;
using SellMilk.Api.Models.Sua;
using SellMilk.Api.Models.NguoiDung;
using SellMilk.Api.Models.ThongSo;
using System.Data;
using System.Linq;

namespace SellMilk.Api.Services
{
    public class LoaiSuaService
    {
        private ITruyVanDuLieu _truyvan;
        public LoaiSuaService(ITruyVanDuLieu dbHelper)
        {
            _truyvan = dbHelper;
        }


        public List<LoaiSua> LayLoai()
        {
            string msgError = "";
            try
            {
                var dt = _truyvan.ExecuteSProcedureReturnDataTable(out msgError, "sp_LayLoaiSua");
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);
                return dt.ConvertTo<LoaiSua>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("lỗi rồi",ex);
            }

        }

        public LoaiSua GetLoaiSuaByID(int id)
        {
            string msgError = "";
            var dt = _truyvan.ExecuteSProcedureReturnDataTable(out msgError, "sp_LayThongTinLoaiSuaTheoID", "@LoaiSuaID", id);

            if (!string.IsNullOrEmpty(msgError))
            {
                throw new Exception(msgError);
            }

            return dt.ConvertTo<LoaiSua>().FirstOrDefault();

        }

        public bool Create(LoaiSuaThem model)
        {
            string msgError = "";
            try
            {
                object[] parameters = new object[] {
                "@Ten", model.Ten,
                "@MoTa", model.MoTa,
                "@Status", model.Status,
                "@ShowMenu", model.ShowMenu };
                var result = _truyvan.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_ThemLoaiSua", parameters);

               
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Add thất bại", ex);
            }
        }

        public bool Update(LoaiSua model)
        {
            string msgError = "";
            try
            {
                object[] parameters = new object[] {
                "@ID", model.ID,
                "@Ten", model.Ten,
                "@MoTa", model.MoTa,
                "@Status", model.Status,
                "@ShowMenu", model.ShowMenu };
                var result = _truyvan.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_CapNhatLoaiSua", parameters);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Add thất bại", ex);
            }
        }



        public bool Delete(SuaXoa id)
        {
            try
            {
                string msgError;
                object[] parameters = new object[] { "@Id", id.ID };
                var dt = _truyvan.ExecuteSProcedureReturnDataTable(out msgError, "sp_XoaLoaiSua", parameters);
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    int result = Convert.ToInt32(row["Result"]);
                    if(result == 0)
                    {
                        return false;
                    }
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
