using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SellMilk.Api.Models.Common;
using SellMilk.Api.Models.GioHang;
using SellMilk.Api.Models.HoaDonBan;
using SellMilk.Api.Models.Sua;
using SellMilk.Api.Services;

namespace SellMilk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GioHangController : ControllerBase
    {
        private GioHangService _gioHangService;
        private XacThucService _xacThucService;
        private string _path;
        private IWebHostEnvironment _env;
        private readonly FileService _fileService;


        public GioHangController(GioHangService gioHangService, XacThucService xacThucService, IConfiguration configuration, IWebHostEnvironment env, FileService fileService)
        {
            _gioHangService = gioHangService;
            _xacThucService = xacThucService;
            _path = configuration["AppSettings:PATH"];
            _env = env;
            _fileService = fileService;
        }


        [Route("get-by-nguoidung-id")]
        [HttpGet]
        public ApiResult<List<GioHang>> GetDatabyNguoiDungID([FromQuery] int id)
        {
            return new ApiResult<List<GioHang>>
            {
                Message = "Thành công",
                Status = true,
                Data = _gioHangService.GetGioHangByNguoiDungID(id)
            };
        }


        [HttpPost("create")]
        public async Task<ApiResult<bool>> Create([FromBody] GioHangThem request)
        {
            var result = await _gioHangService.ThemGioHang(request);

            return new ApiResult<bool>()
            {
                Status = true,
                Message = "Thêm giỏ hàng thành công!",
                Data = result
            };
        }

        [HttpPost("update")]
        public ApiResult<bool> Update([FromBody] GioHangSua request)
        {
            var result = _gioHangService.SuaGioHang(request);

            return new ApiResult<bool>()
            {
                Status = true,
                Message = "Sửa giỏ hàng thành công!",
                Data = result
            };
        }

        [Route("delete")]
        [HttpPost]
        //[AllowAnonymous]
        public ApiResult<SuaXoa> Delete([FromBody] SuaXoa id)
        {
            if (_gioHangService.XoaGioHang(id))
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
                    Message = "Lỗi, không có giỏ hàng có id như vậy",
                    Status = false,
                    Data = id
                };
            }

        }
        [Route("delete-multiple")]
        [HttpPost]
        //[AllowAnonymous]
        public ApiResult<SuaXoaNhieu> DeleteMultiple([FromBody] SuaXoaNhieu ids)
        {
            if (_gioHangService.XoaNhieuGioHang(ids))
            {
                return new ApiResult<SuaXoaNhieu>
                {
                    Message = "Xóa thành công rồi nhé",
                    Status = true,
                    Data = ids
                };
            }
            else
            {
                return new ApiResult<SuaXoaNhieu>
                {
                    Message = "Lỗi, không có giỏ hàng có id như vậy",
                    Status = false,
                    Data = ids
                };
            }

        }
    }
}
