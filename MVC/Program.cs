using APP.Authentication.Services;
using APP.Domain;
using APP.Models;
using APP.Services;
using CORE.Authentication.Services;
using CORE.Services;
using CORE.Session.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---------- IoC (Inversion of Control) Container ----------

// Database:
builder.Services.AddDbContext<DbContext, Db>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString(nameof(Db))));

// Users module services (from the guide):
builder.Services.AddScoped<IService<UserRequest, UserResponse>, UserService>();
builder.Services.AddScoped<IService<RoleRequest, RoleResponse>, RoleService>();
builder.Services.AddScoped<IService<StatusRequest, StatusResponse>, StatusService>();

// Patients domain services:
builder.Services.AddScoped<IService<BranchRequest, BranchResponse>, BranchService>();
builder.Services.AddScoped<IService<DoctorRequest, DoctorResponse>, DoctorService>();
builder.Services.AddScoped<IService<PatientRequest, PatientResponse>, PatientService>();

// Authentication:
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICookieAuthService, CookieAuthService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
    });

// Session:
builder.Services.AddSession(config =>
{
    config.IdleTimeout = TimeSpan.FromMinutes(30);
});
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<UserSessionService>();
builder.Services.AddScoped<PatientSessionService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Who are you? Must be before UseAuthorization.
app.UseAuthorization();  // What are you allowed to do?

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();