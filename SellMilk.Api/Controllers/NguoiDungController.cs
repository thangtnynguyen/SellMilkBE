using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SellMilk.Api.Constants;
using SellMilk.Api.Models.Common;
using SellMilk.Api.Models.NguoiDung;
using SellMilk.Api.Services;
using System.Reflection;

namespace SellMilk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungController : ControllerBase
    {

        private NguoiDungService _nguoiDungService;
        private XacThucService _xacThucService;
        private string _path;
        private IWebHostEnvironment _env;
        private readonly FileService _fileService;
        //private readonly FileApiClientService _fileApiClientService;


        public NguoiDungController(NguoiDungService nguoiDungService, XacThucService xacThucService, IConfiguration configuration, IWebHostEnvironment env, FileService fileService)
        {
            _nguoiDungService = nguoiDungService;
            _xacThucService = xacThucService;
            _path = configuration["AppSettings:PATH"];
            _env = env;
            _fileService = fileService;
            //_fileApiClientService = fileApiClientService;
        }


        [Route("get")]
        [HttpGet]
        //[AllowAnonymous]
        public ApiResult<List<NguoiDung>> Search()
        {
            try
            {
                var data = _nguoiDungService.LayTatCaNguoiDung();
                return new ApiResult<List<NguoiDung>>()
                {
                    Status = true,
                    Message = "Danh sách người dùng đã được lấy thành công!",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Route("sign-up")]
        [HttpPost]
        public ApiResult<NguoiDungDangKi> SignUp([FromBody] NguoiDungDangKi request)
        {
            if (_nguoiDungService.DangKi(request))
            {
                return new ApiResult<NguoiDungDangKi>()
                {
                    Status = true,
                    Message = "Đăng kí tài khoản thành công",
                    Data = request
                };
            }
            else
                return new ApiResult<NguoiDungDangKi>()
                {
                    Status = false,
                    Message = "Đăng kí thất bại!",
                    Data = null
                };
        }

        [Route("get-thongtin-nguoidung")]
        [HttpGet]
        public ApiResult<NguoiDung> ThongTinNguoiDung()
        {
            var user = _xacThucService.ThongTinNguoiDung(HttpContext);

            if (user == null)
            {
                return new ApiResult<NguoiDung>()
                {
                    Status = false,
                    Message = "Không tìm thấy thông tin người dùng hợp lệ!",
                    Data = null
                };
            }

            return new ApiResult<NguoiDung>()
            {
                Status = true,
                Message = "Lấy thông tin người dùng thành công!",
                Data = user
            };
        }

        [Route("update")]
        [HttpPost]
        public async Task<ApiResult<NguoiDungSua>> CapNhapThongTin([FromForm] NguoiDungSua request)
        {
            if (request.AvatarFile?.Length > 0)
            {
                request.Avatar = await _fileService.UploadFileAsync(request.AvatarFile, PathFolder.Avatar);
            }
            if (_nguoiDungService.CapNhapThongTin(request))
            {
                return new ApiResult<NguoiDungSua>()
                {
                    Status = true,
                    Message = "Cập nhập tài khoản thành công",
                    Data = request
                };
            }
            else
                return new ApiResult<NguoiDungSua>()
                {
                    Status = false,
                    Message = "Cập nhập thất bại!",
                    Data = null
                };
        }
    }
}
