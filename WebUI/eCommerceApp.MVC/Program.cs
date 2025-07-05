using eCommerceApp.Domain.Entities;
using eCommerceApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//VeriTabaný baðlantýsý
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

//Login ve Register ayarlarý
//Identity servislerini uygulamaya ekler ve AppUser ile IdentityRole modellerini kullanacaðýný container'a belirtmemiz gerekir.
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
	//Þifre ayarlarý   
	opt.Password.RequireDigit = true; //Þifrede en az bir rakam olmalý
	opt.Password.RequireLowercase = true;//Þifrede en az bir küçük harf olmalý
	opt.Password.RequireNonAlphanumeric = true;//Þifrede en az bir alfanümerik karakter olmalý(@,!,#,$ vs)
	opt.Password.RequireUppercase = true; //Þifrede en az bir büyük harf olmalý
	opt.Password.RequiredLength = 6; //Þifre en az 6 karakter uzunluðunda olmalý
	opt.Password.RequiredUniqueChars = 1; //Þifrede en az 1 tane benzersiz karakter olmalý

	//Kilitlenme ayarlarý(Kullanýcý üstüste birkaç kez baþarýsýz giriþten sonra hesap kilitlensin)
	opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //Baþarýsýz giriþten ne kadar zamandan sýnra bir daha giriþ hakký tanýnsýn.
	opt.Lockout.MaxFailedAccessAttempts = 3;//Max baþarýsýz giriþ sayýsý.
	opt.Lockout.AllowedForNewUsers = true; //Yeni kayýt olan kullanýcýlarýn kilitlenebilir olup olmadýðýný belirt.

	//Kullanýcý ayarlarý: Kullanýcý adýnda kullanabilecek karakterleri içerir.
	opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
	opt.User.RequireUniqueEmail = true; //Her kullanýcýnýn benzersiz bir E-posta adresine sahip olmalý.True zorunlu kýlar.

	//Giriþ(SignIn) Ayarlarý: E-posta adresinin onaylanýp olanmamasýnýn gerekip gerekmediði mantýðý eklenir. 
	opt.SignIn.RequireConfirmedEmail = false; //E-posta'nýn onaylanmaýp onaylanmadýðýnýkontrol eder.Geliþtirme(Devolopment) ortamýndayken genellikle false yapýlýr.
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
