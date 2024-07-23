using SellMilk.Api.Helper.Interfaces;
using SellMilk.Api.Helper;
using SellMilk.Api.Services;
using SellMilk.Api.Providers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});



builder.Services.AddTransient<ITruyVanDuLieu, TruyVanDuLieu>();


builder.Services.AddTransient<FileService>();
builder.Services.AddTransient<SuaService>();
builder.Services.AddTransient<XacThucService>();
builder.Services.AddTransient<NguoiDungService>();
builder.Services.AddTransient<LoaiSuaService>();
builder.Services.AddTransient<GioHangService>();
builder.Services.AddTransient<HoaDonBanService>();





// Add services to the container.
builder.Services.AddIdentityProvider(builder);

builder.Services.AddSwaggerProvider();
builder.Services.AddHttpContextAccessor();//add thêm  truy cap
builder.Services.AddMemoryCache();//nguoi truy cap
builder.Services.AddHttpClient();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors(
    x => x.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();

app.Run();
