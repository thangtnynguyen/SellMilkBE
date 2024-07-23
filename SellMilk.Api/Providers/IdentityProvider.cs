using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Reflection;
using SellMilk.Api.Constants;
using Microsoft.AspNetCore.Authorization;

namespace SellMilk.Api.Providers
{
    public static class IdentityProvider
    {
        public static IServiceCollection AddIdentityProvider(this IServiceCollection services, WebApplicationBuilder builder)
        {
            
            var jwtKey = builder.Configuration.GetValue<string>("Jwt:Key");

            var key = Encoding.ASCII.GetBytes(jwtKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            #region nâng cao phân quyền
            //var permissionType = typeof(Constants.Permission);
            //var permissionFields = permissionType.GetFields(BindingFlags.Public | BindingFlags.Static)
            //.Where(f => f.IsLiteral && !f.IsInitOnly)
            //.Select(f => (string)f.GetValue(null))
            //.ToList();

            //services.AddAuthorization(options =>
            //{
            //    foreach (var permission in permissionFields)
            //    {
            //        options.AddPolicy(permission, policy =>
            //        {
            //            // Đối với mỗi chính sách, yêu cầu một claim có tên "Permission" và giá trị là quyền tương ứng
            //            policy.RequireClaim("Permission", permission);
            //        });
            //    }
            //});
            #endregion
            return services;
        }
    }
}





#region ví dụ nâng cao
//var createPolicy = new AuthorizationPolicyBuilder()
//    .RequireAssertion(context =>
//        context.User.HasClaim("Permission", Permission.ManageKhoaHocCreate) &&
//        context.User.HasClaim("Permission", Permission.ManageKhoaHocUpdate))
//    .Build();
#endregion