using eCommerceApp.Domain.Entities;
using eCommerceApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Veritabaný baðlantýsý.
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

//login ve register ayarlarý
// Identity servislerini uygulamaya ekler ve AppUser ile IdentityRole modellerini kullanacaðýný container'a belirtmemiz gerekir.
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    //Þifre ayarlarý
    opt.Password.RequireDigit = true; //Þifrede en az bir rakam olmalý.
    opt.Password.RequireLowercase = true;//Þifrede en az bir küçük harf olmalý.
    opt.Password.RequireNonAlphanumeric = true; //Þifrede en az bir alfanümerik karakter olmalý(@,!,#,$ vs)
    opt.Password.RequireUppercase = true; //Þifrede en az bir büyük harf olmalý.
    opt.Password.RequiredLength = 6; //Þifre en az 6 karakter uzunluðunda olsun.
    opt.Password.RequiredUniqueChars = 1;//En az bir tane benzersiz karakter olmalý.

    //Kilitlenme ayarlarý(kullanýcý üst üste bir kaç kez baþarýsýz giriþten sonra hesap kilitlensin)
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);//baþarýsý giriþten ne kadar zamandan sonra giriþ hakký tanýnsýn.
    opt.Lockout.MaxFailedAccessAttempts = 3; //max baþarýsýz giriþ sayýsý 3'tür
    opt.Lockout.AllowedForNewUsers = true;//Yeni kayýt olan kullanýcýlarýn kilitlenebilir olup olmadýðýný belirt.

    //Kullanýcý ayarlarý: Kullanýcý adýnda kullanabilecek karakterleri içerir.
    opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    opt.User.RequireUniqueEmail = true; //Her kullanýcý benzersiz bir e-posta adresine sahip olsun mu? true zorunlu kýlar.

    //Giriþ(SignIn) Ayarlarý: e-posta adresinin onaylanýp onaylanmamasýnýn gerekip gerekmedði mantýðý eklenir.
    opt.SignIn.RequireConfirmedEmail = false;//e-posta nýn onaylanýp onaylanmadýðýný kontrol eder. Geliþtirme(devolopment) ortamýndayken genellikle false yapýlýr.

}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
