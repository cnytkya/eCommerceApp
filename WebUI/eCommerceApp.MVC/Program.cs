using eCommerceApp.Domain.Entities;
using eCommerceApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Veritaban� ba�lant�s�.
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

//login ve register ayarlar�
// Identity servislerini uygulamaya ekler ve AppUser ile IdentityRole modellerini kullanaca��n� container'a belirtmemiz gerekir.
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    //�ifre ayarlar�
    opt.Password.RequireDigit = true; //�ifrede en az bir rakam olmal�.
    opt.Password.RequireLowercase = true;//�ifrede en az bir k���k harf olmal�.
    opt.Password.RequireNonAlphanumeric = true; //�ifrede en az bir alfan�merik karakter olmal�(@,!,#,$ vs)
    opt.Password.RequireUppercase = true; //�ifrede en az bir b�y�k harf olmal�.
    opt.Password.RequiredLength = 6; //�ifre en az 6 karakter uzunlu�unda olsun.
    opt.Password.RequiredUniqueChars = 1;//En az bir tane benzersiz karakter olmal�.

    //Kilitlenme ayarlar�(kullan�c� �st �ste bir ka� kez ba�ar�s�z giri�ten sonra hesap kilitlensin)
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);//ba�ar�s� giri�ten ne kadar zamandan sonra giri� hakk� tan�ns�n.
    opt.Lockout.MaxFailedAccessAttempts = 3; //max ba�ar�s�z giri� say�s� 3't�r
    opt.Lockout.AllowedForNewUsers = true;//Yeni kay�t olan kullan�c�lar�n kilitlenebilir olup olmad���n� belirt.

    //Kullan�c� ayarlar�: Kullan�c� ad�nda kullanabilecek karakterleri i�erir.
    opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    opt.User.RequireUniqueEmail = true; //Her kullan�c� benzersiz bir e-posta adresine sahip olsun mu? true zorunlu k�lar.

    //Giri�(SignIn) Ayarlar�: e-posta adresinin onaylan�p onaylanmamas�n�n gerekip gerekmed�i mant��� eklenir.
    opt.SignIn.RequireConfirmedEmail = false;//e-posta n�n onaylan�p onaylanmad���n� kontrol eder. Geli�tirme(devolopment) ortam�ndayken genellikle false yap�l�r.

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
