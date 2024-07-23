using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SellMilk.Api.Models.Common;
using SellMilk.Api.Models.HoaDonBan;
using SellMilk.Api.Services;
using System.Xml.Linq;

namespace SellMilk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonBanController : ControllerBase
    {
        private HoaDonBanService _hoaDonBanService;
        private XacThucService _xacThucService;
        private string _path;
        private IWebHostEnvironment _env;
        private readonly FileService _fileService;


        public HoaDonBanController(HoaDonBanService hoaDonBanService, XacThucService xacThucService, IConfiguration configuration, IWebHostEnvironment env, FileService fileService)
        {
            _hoaDonBanService = hoaDonBanService;
            _xacThucService = xacThucService;
            _path = configuration["AppSettings:PATH"];
            _env = env;
            _fileService = fileService;
        }

        [Route("get")]
        [HttpGet]
        //[AllowAnonymous]
        public ApiResult<List<HoaDonBan>> Search()
        {
            try
            {
                var data = _hoaDonBanService.getHoaDonBan();
                return new ApiResult<List<HoaDonBan>>()
                {
                    Status = true,
                    Message = "Danh sách hóa đơn bán đã được lấy thành công!",
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
        public ApiResult<HoaDonBan> GetDatabyID([FromQuery] int id)
        {
            return new ApiResult<HoaDonBan>
            {
                Message = "Thành công",
                Status = true,
                Data = _hoaDonBanService.GetHoaDonBanVaChiTiet(id)
            };
        }
        [Route("get-by-nguoidung-id")]
        [HttpGet]
        public ApiResult<List<HoaDonBan>> GetDatabyNguoiDungID([FromQuery] int id)
        {
            return new ApiResult<List<HoaDonBan>>
            {
                Message = "Thành công",
                Status = true,
                Data = _hoaDonBanService.GetHoaDonBanByNguoiDungID(id)
            };
        }


        [HttpPost("create")]
        public async Task<ApiResult<bool>> Create([FromBody] HoaDonBanThem request)
        {
            var result = await _hoaDonBanService.ThemHoaDonBan(request);

            return new ApiResult<bool>()
            {
                Status = true,
                Message = "Thêm hóa đơn bán thành công!",
                Data = result
            };
        }

        [Route("update-status")]
        [HttpPost]
        public ApiResult<bool> UpdateItem([FromBody] HoaDonBanSua model)
        {

            if (_hoaDonBanService.CapNhapTrangThai(model) != null)
            {
                return new ApiResult<bool>()
                {
                    Message = "Thêm loại máy tính thành công nhé",
                    Status = true,
                    Data = true
                };
            }
            else
            {
                return new ApiResult<bool>()
                {
                    Message = "Lỗi rồi",
                    Status = false,
                    Data = true
                };
            }
        }
    }
}
