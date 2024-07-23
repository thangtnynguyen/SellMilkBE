using SellMilk.Api.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using SellMilk.Api.Constants;
using SellMilk.Api.Models.NguoiDung;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SellMilk.Api.Services
{
    public class XacThucService
    {
        private NguoiDungService _nguoiDungService;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public XacThucService(IConfiguration config, NguoiDungService nguoiDungService,IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _nguoiDungService = nguoiDungService;
            _httpContextAccessor = httpContextAccessor;

        }

        public string CreateToken(NguoiDungDangNhap request)
        {
            var key = _config["Jwt:Key"];

            var role = _nguoiDungService.LayRole(request.UserName);

            var claims = new List<Claim>
            {
                new Claim(ClaimType.UserName, request.UserName),
                new Claim(ClaimType.Role,role.ToString())
            };

            //foreach (var role in roles)
            //{
            //    claims.Add(new Claim(ClaimType.Role, role));
            //}

            var subject = new ClaimsIdentity(claims);
            var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature);
            var expires = DateTime.UtcNow.AddDays(30);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = expires,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            return token;
        }



        public NguoiDung ThongTinNguoiDung(HttpContext httpContext)
        {

            var userNameClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimType.UserName);

            if (userNameClaim != null)
            {
                NguoiDung nguoi = new NguoiDung
                {
                    UserName = userNameClaim.Value,
                };
                var user = _nguoiDungService.LayThongTinNguoiDung(nguoi.UserName);
                //var role = _nguoiDungService.LayRole(nguoi.UserName);
                //user.Role = role;
                return user;

            }
            else
            { return null; }
        }

        public async Task<NguoiDung> ThongTinNguoiDungAsync()
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimType.UserName);

                if ( userName != null)
                {
                    NguoiDung nguoi = new NguoiDung
                    {
                        UserName = userName.Value,
                    };
                    var user = _nguoiDungService.LayThongTinNguoiDung(nguoi.UserName);
                    //var role = _nguoiDungService.LayRole(nguoi.UserName);
                    //user.Role = role;
                    return user;
                }
                else
                { return null; }
            }
            catch (Exception ex)
            {
                throw new Exception("Thất bại", ex);
            }
        }
    }
}
