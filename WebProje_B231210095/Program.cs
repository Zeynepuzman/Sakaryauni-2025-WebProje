using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebProje_B231210095.Data;
using WebProje_B231210095.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//  Identity sistemini ekliyoruz
// (Kullanýcý yönetimi: giriþ, þifre, roller vb.)

builder.Services.AddIdentity<Uye, IdentityRole>(options =>
{
    // E-posta doðrulamasý gerektirmesin (ödev/test için ideal)
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline./ MVC desteðini ekliyoruz
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Identity’nin çalýþmasý için gerekli
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
