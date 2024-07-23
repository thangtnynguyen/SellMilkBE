using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SellMilk.Api.Constants;
using SellMilk.Api.Models.Common;
using SellMilk.Api.Models.File;
using SellMilk.Api.Models.Sua;
using SellMilk.Api.Services;

namespace SellMilk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuaController : ControllerBase
    {
        private SuaService _SuaService;
        private string _path;
        private IWebHostEnvironment _env;
        private readonly FileService _fileService;
        //private readonly FileApiClientService _fileApiClientService;


        public SuaController(SuaService SuaService, IConfiguration configuration, IWebHostEnvironment env, FileService fileService)
        {
            _SuaService = SuaService;
            _path = configuration["AppSettings:PATH"];
            _env = env;
            _fileService = fileService;
            //_fileApiClientService = fileApiClientService;
        }

        [Route("get")]
        [HttpGet]
        //[AllowAnonymous]
        public ApiResult<PagingResult<Sua>> Search([FromQuery] SuaTimKiem request)
        {
            try
            {
                var data = _SuaService.Search(request);
                return new ApiResult<PagingResult<Sua>>()
                {
                    Status = true,
                    Message = "Danh sách sữa đã được lấy thành công!",
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
        public ApiResult<Sua> GetDatabyID([FromQuery] int id)
        {
            return new ApiResult<Sua>
            {
                Message = "Thành công",
                Status = true,
                Data = _SuaService.GetSuaByID(id)
            };
        }

        [Route("create")]
        [HttpPost]
        public async Task<ApiResult<SuaThem>> CreateItem([FromForm] SuaThem model)
        {
            if (model.AnhFile?.Length > 0)
            {
                model.Anh = await _fileService.UploadFileAsync(model.AnhFile, PathFolder.Sua);
            }
            if (model.AnhKhacFile?.Count() > 0)
            {
                UploadMultipleFileRequest request = new UploadMultipleFileRequest();
                request.Files = model.AnhKhacFile;
                List<string> AnhKhacList = await _fileService.UploadMultipleFilesAsync(request, PathFolder.Sua);
                model.AnhKhac = string.Join(",", AnhKhacList);
            }
            if (_SuaService.Create(model) == true)
            {
                return new ApiResult<SuaThem>()
                {
                    Message = "Thêm sữa thành công nhé",
                    Status = true,
                    Data = model
                };
            }
            else
            {
                return new ApiResult<SuaThem>()
                {
                    Message = "Lỗi rồi",
                    Status = false,
                    Data = null
                };
            }
        }
        [Route("update")]
        [HttpPost]
        public async Task<ApiResult<SuaSua>> Update([FromForm] SuaSua model)
        {
            if (model.AnhFile?.Length > 0)
            {
                model.Anh = await _fileService.UploadFileAsync(model.AnhFile, PathFolder.Sua);
            }
            if (model.AnhKhacFile?.Count() > 0)
            {
                UploadMultipleFileRequest request = new UploadMultipleFileRequest();
                request.Files = model.AnhKhacFile;
                List<string> AnhKhacList = await _fileService.UploadMultipleFilesAsync(request, PathFolder.Sua);
                model.AnhKhac = string.Join(",", AnhKhacList);
            }
            if (_SuaService.Update(model) == true)
            {
                return new ApiResult<SuaSua>()
                {
                    Message = "Cập nhập sữa thành công nhé",
                    Status = true,
                    Data = model
                };
            }
            else
            {
                return new ApiResult<SuaSua>()
                {
                    Message = "Lỗi rồi",
                    Status = false,
                    Data = null
                };
            }
        }
        [Route("update-tonkho")]
        [HttpPost]
        public ApiResult<SuaCapNhapTonKho> UpdateTonKho([FromBody] SuaCapNhapTonKho request)
        {
            if (_SuaService.UpdateTonKho(request))
            {
                return new ApiResult<SuaCapNhapTonKho>
                {
                    Message = "Cập nhập tồn kho  thành công rồi nhé",
                    Status = true,
                    Data = request
                };
            }
            else
            {
                return new ApiResult<SuaCapNhapTonKho>
                {
                    Message = "Lỗi, không có sữa có id như vậy huhu",
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
            if (_SuaService.Delete(id))
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
                    Message = "Lỗi, không có sữa có id như vậy",
                    Status = false,
                    Data = id
                };
            }

        }
    }
}
