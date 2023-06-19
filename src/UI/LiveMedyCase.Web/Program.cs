using LiveMedyCase.Business.Services.Base;
using LiveMedyCase.Business.Services;
using LiveMedyCase.Core.Repositories;
using LiveMedyCase.Data.Repositories;
using LiveMedyCase.Data.Repositories.Base;
using LiveMedyCase.Data.Contexts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using LiveMedyCase.Web.Utils.ReCaptcha;
using LiveMedyCase.Web.Configs;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IRecaptchaValidator, RecaptchaValidator>();
builder.Services.AddScoped(typeof(IRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
builder.Services.AddTransient(typeof(IAuthService), typeof(AuthService));
builder.Services.AddTransient(typeof(IUserService), typeof(UserService));

builder.Services.AddDbContext<AppDbContext>(options =>
  options.UseSqlite(builder.Configuration.GetConnectionString("AppDb"))
);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.LoginPath = "/Auth/SignIn";
		options.LogoutPath = "/Auth/SignOut";
		options.Cookie.Name = ".LMCAuthCookies";
		options.Cookie.MaxAge = TimeSpan.FromDays(28);
		options.ExpireTimeSpan = TimeSpan.FromDays(28);
		options.SlidingExpiration = true;
		options.AccessDeniedPath = "/Home/AccessDenied";
	});

builder.Services.AddRateLimiter(options =>
{
	options.AddFixedWindowLimiter("ReferralRate", options =>
	{
		options.AutoReplenishment = true;
		options.PermitLimit = 5;
		options.Window = TimeSpan.FromMinutes(1);
	});
	options.OnRejected = async (context, token) =>
	{
		context.HttpContext.Response.StatusCode = 429;
		await context.HttpContext.Response.WriteAsync("Too many request. Please try again later", cancellationToken: token);
	};
});

builder.Services.Configure<ReCaptchaConfig>(builder.Configuration.GetSection("GoogleSettings"));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
