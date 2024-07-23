using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SellMilk.Api.Models.Common;
using SellMilk.Api.Models.NguoiDung;
using SellMilk.Api.Services;

namespace SellMilk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XacThucController : ControllerBase
    {
        private NguoiDungService _nguoiDungService;
        private XacThucService _xacThucService;
        private string _path;
        private IWebHostEnvironment _env;
        private readonly FileService _fileService;
        //private readonly FileApiClientService _fileApiClientService;

        public XacThucController(XacThucService xacThucService, NguoiDungService nguoiDungService, IConfiguration configuration, IWebHostEnvironment env, FileService fileService)
        {
            _xacThucService = xacThucService;
            _nguoiDungService = nguoiDungService;
            _path = configuration["AppSettings:PATH"];
            _env = env;
            _fileService = fileService;
            //_fileApiClientService = fileApiClientService;
        }

        [Route("login")]
        [HttpPost]
        public ApiResult<TokenRole> Login([FromBody] NguoiDungDangNhap request)
        {

            if (_nguoiDungService.DangNhap(request.UserName, request.PasswordHash) == true)
            {
                var token = _xacThucService.CreateToken(request);
                var nguoidung = _nguoiDungService.LayThongTinNguoiDung(request.UserName);

                return new ApiResult<TokenRole>()
                {
                    Status = true,
                    Message = "Đăng nhập thành công!",
                    Data = new TokenRole()
                    {
                        Role = nguoidung.Role.ToString(),
                        Token = token,
                        NguoiDung = nguoidung,
                    }
                };
            }
            else
            {
                return new ApiResult<TokenRole>()
                {
                    Status = false,
                    Message = "Tạo mã token thất bại- đăng nhập thất bại!",
                    Data = null,
                };
            }

        }

        [Route("get-thongtin-nguoidung")]
        [HttpGet]
        //[AllowAnonymous]
        public ApiResult<NguoiDung> LayThongTinNguoiDung()
        {
            try
            {
                var data = _xacThucService.ThongTinNguoiDung(HttpContext);
                return new ApiResult<NguoiDung>()
                {
                    Status = true,
                    Message = "Thông tin người dùng đã được lấy thành công!",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Route("get-thongtin-nguoidung-async")]
        [HttpGet]
        //[AllowAnonymous]
        public async Task<ApiResult<NguoiDung>> LayThongTinNguoiDungAsync()
        {
            try
            {
                var data = await _xacThucService.ThongTinNguoiDungAsync();
                return new ApiResult<NguoiDung>()
                {
                    Status = true,
                    Message = "Thông tin người dùng đã được lấy thành công!",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
