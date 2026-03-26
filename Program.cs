using LVTNWEBGIAYDEP.Models;
using LVTNWEBGIAYDEP.Models.Momo;
using LVTNWEBGIAYDEP.Services.Momo;
using LVTNWEBGIAYDEP.Services.Vnpay;
using Microsoft.EntityFrameworkCore;
var cultureInfo = new System.Globalization.CultureInfo("vi-VN");
System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DBGiayDepContext>(options =>
    options.UseSqlite("Data Source=app.db"));
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});
//connect momoAPI
builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));
builder.Services.AddScoped<IMomoService, MomoService>();
//connect vnpay API



// Đảm bảo các dịch vụ được đăng ký ở đây
builder.Services.AddScoped<IVnPayService, VnPayService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DBGiayDepContext>();

    db.Database.ExecuteSqlRaw("PRAGMA journal_mode=DELETE;");
}

app.Run();

