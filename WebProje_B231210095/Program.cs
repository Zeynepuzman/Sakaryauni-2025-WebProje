using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using WebProje_B231210095.Data;
using WebProje_B231210095.Models;

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


// ===============================
// ROL + ADMIN SEED
// ===============================
await CreateRolesAndAdminAsync(app);

app.Run();


// ===============================
// METOTLAR
// ===============================
static async Task CreateRolesAndAdminAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Uye>>();

  
    string[] roles = { "Admin", "Uye", "Antrenor" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // 2️⃣ Admin kullanıcı bilgileri (ÖDEVDE VERİLEN)
    string adminEmail = "B231210095@hotmail.com";
    string adminPassword = "sau";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        var admin = new Uye
        {
            UserName = adminEmail,
            Email = adminEmail,
            AdSoyad = "Sistem Admini"
        };

        // ❗ ŞİFRESİZ CREATE (Identity kurallarını bypass etmek için)
        var createResult = await userManager.CreateAsync(admin);

        if (createResult.Succeeded)
        {
            // ❗ ADMIN'E ÖZEL MANUEL HASH (ÖDEV GEREĞİ)
            var passwordHash = userManager.PasswordHasher.HashPassword(admin, adminPassword);
            admin.PasswordHash = passwordHash;

            await userManager.UpdateAsync(admin);

            // Admin rolü ata
            await userManager.AddToRoleAsync(admin, "Admin");
        }
        else
        {
            foreach (var error in createResult.Errors)
            {
                Console.WriteLine(error.Description);
            }
        }
    }
    else
    {
        // Admin varsa ama rolü yoksa garantiye al
        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}


// ===============================
// FAKE EMAIL SENDER
// ===============================
public class FakeEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        Console.WriteLine($"FAKE EMAIL: {email} - {subject}");
        return Task.CompletedTask;
    }
}
