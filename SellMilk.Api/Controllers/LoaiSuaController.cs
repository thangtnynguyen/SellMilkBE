using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SellMilk.Api.Constants;
using SellMilk.Api.Models.Common;
using SellMilk.Api.Models.File;
using SellMilk.Api.Models.LoaiSua;
using SellMilk.Api.Models.Sua;
using SellMilk.Api.Services;

namespace SellMilk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiSuaController : ControllerBase
    {
        private LoaiSuaService _loaiSuaService;
        private string _path;
        private IWebHostEnvironment _env;
        private readonly FileService _fileService;


        public LoaiSuaController(LoaiSuaService loaiSuaService, IConfiguration configuration, IWebHostEnvironment env, FileService fileService)
        {
            _loaiSuaService = loaiSuaService;
            _path = configuration["AppSettings:PATH"];
            _env = env;
            _fileService = fileService;
        }

        [Route("get")]
        [HttpGet]
        //[AllowAnonymous]
        public ApiResult<List<LoaiSua>> Search()
        {
            try
            {
                var data = _loaiSuaService.LayLoai();
                return new ApiResult<List<LoaiSua>>()
                {
                    Status = true,
                    Message = "Danh sách loại sữa đã được lấy thành công!",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Route("get-by-id")]
        [HttpGet]
        public ApiResult<LoaiSua> GetDatabyID([FromQuery] int id)
        {
            return new ApiResult<LoaiSua>
            {
                Message = "Thành công",
                Status = true,
                Data = _loaiSuaService.GetLoaiSuaByID(id)
            };
        }


        [Route("create")]
        [HttpPost]
        public async Task<ApiResult<LoaiSuaThem>> CreateItem([FromBody] LoaiSuaThem model)
        {

            if (_loaiSuaService.Create(model) != null)
            {
                return new ApiResult<LoaiSuaThem>()
                {
                    Message = "Thêm loại sữa thành công nhé",
                    Status = true,
                    Data = model
                };
            }
            else
            {
                return new ApiResult<LoaiSuaThem>()
                {
                    Message = "Lỗi rồi",
                    Status = false,
                    Data = null
                };
            }
        }

        [Route("update")]
        [HttpPost]
        public async Task<ApiResult<LoaiSua>> UpdateItem([FromBody] LoaiSua model)
        {

            if (_loaiSuaService.Update(model) != null)
            {
                return new ApiResult<LoaiSua>()
                {
                    Message = "Thêm loại sữa thành công nhé",
                    Status = true,
                    Data = model
                };
            }
            else
            {
                return new ApiResult<LoaiSua>()
                {
                    Message = "Lỗi rồi",
                    Status = false,
                    Data = null
                };
            }
        }

        [Route("delete")]
        [HttpPost]
        //[AllowAnonymous]
        public ApiResult<SuaXoa> Delete([FromBody] SuaXoa id)
        {
            if (_loaiSuaService.Delete(id))
            {
                return new ApiResult<SuaXoa>
                {
                    Message = "Xóa thành công rồi nhé",
                    Status = true,
                    Data = id
                };
            }
            else
            {
                return new ApiResult<SuaXoa>
                {
                    Message = "Lỗi, không xóa được loại do có sữa trong loại này!!",
                    Status = false,
                    Data = id
                };
            }

        }
    }
}
