using KuaforYonetim.Data;
using KuaforYonetim.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Veritaban� ba�lant�s�n� yap�land�r
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity servisini ekle
builder.Services.AddIdentity<Kullanici, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Rolleri ekle
builder.Services.AddScoped<RoleManager<IdentityRole>>();  // RoleManager'� DI container'a ekle

builder.Services.AddControllersWithViews();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 1;
    options.Password.RequiredUniqueChars = 0;
});


var app = builder.Build();

// Uygulaman�n hata y�netimi i�in ayar
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // Identity i�in gerekli
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Varsay�lan admin olu�turma i�lemi
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<Kullanici>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // Admin rol� olu�turuluyor
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // Varsay�lan admin kullan�c�
    var adminUser = await userManager.FindByEmailAsync("B211210030@sakarya.edu.tr");
    if (adminUser == null)
    {
        var newAdmin = new Kullanici
        {
            UserName = "B211210030@sakarya.edu.tr",
            Email = "B211210030@sakarya.edu.tr",
            AdSoyad = "Admin Kullan�c�"
        };
        var result = await userManager.CreateAsync(newAdmin, "sau");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdmin, "Admin");
        }
    }
}

    app.Run();





