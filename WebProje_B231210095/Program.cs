using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebProje_B231210095.Data;
using WebProje_B231210095.Models;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<Uye, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender, FakeEmailSender>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ADMİN OLUŞTURMAYI ÇALIŞTIR!
  await CreateDefaultAdminAsync(app);

app.Run();


static async Task CreateDefaultAdminAsync(WebApplication app)
{
    Console.WriteLine(" Admin oluşturma metodu çalıştı!");

    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Uye>>();

    // Admin rolü yoksa oluştur
    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    string adminEmail = "B231210095@hotmail.com";
    string adminPassword = "sau";   // zayıf şifre

    var existingUser = await userManager.FindByEmailAsync(adminEmail);

    if (existingUser == null)
    {
        Console.WriteLine(" Admin kullanıcısı oluşturuluyor...");

        var admin = new Uye
        {
            UserName = adminEmail,
            Email = adminEmail,
            AdSoyad = "Sistem Admini"
        };

        // Kullanıcıyı oluştur (şifresiz)
        var createUser = await userManager.CreateAsync(admin);
        if (createUser.Succeeded)
        {
            // Şifreyi manuel ekle → Validasyon devre dışı!
            var passwordHash = userManager.PasswordHasher.HashPassword(admin, adminPassword);
            admin.PasswordHash = passwordHash;

            await userManager.UpdateAsync(admin);
            await userManager.AddToRoleAsync(admin, "Admin");

            Console.WriteLine(" Admin başarıyla oluşturuldu (şifre doğrulama atlandı)!");
        }
        else
        {
            Console.WriteLine(" Admin oluşturulamadı:");
            foreach (var err in createUser.Errors)
                Console.WriteLine(err.Description);
        }
    }
    else
    {
        Console.WriteLine(" Admin zaten mevcut.");
    }
}


/*
  Default Admin oluşturma metodu
static async Task CreateDefaultAdminAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Uye>>();

    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    string adminEmail = "B2312100095@hotmail.com";
    string adminPassword = "sau";

    var existingUser = await userManager.FindByEmailAsync(adminEmail);

    if (existingUser == null)
    {
        var adminUser = new Uye
        {
            UserName = adminEmail,
            Email = adminEmail,
            AdSoyad = "Sistem Admini"
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);

        if (result.Succeeded)
            await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}
 */


public class FakeEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        Console.WriteLine($"FAKE EMAIL: {email} - {subject}");
        return Task.CompletedTask;
    }
}
