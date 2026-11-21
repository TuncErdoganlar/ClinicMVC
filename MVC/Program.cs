using APP.Domain;
using APP.Models;
using APP.Services;
using CORE.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DbContext, Db>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString(nameof(Db))));  //dependency injection !!!
// Add services to the container.

// Users module services (from the guide):
builder.Services.AddScoped<IService<UserRequest, UserResponse>, UserService>();
builder.Services.AddScoped<IService<RoleRequest, RoleResponse>, RoleService>();
builder.Services.AddScoped<IService<StatusRequest, StatusResponse>, StatusService>();

// Patients domain services:
builder.Services.AddScoped<IService<BranchRequest, BranchResponse>, BranchService>();
builder.Services.AddScoped<IService<DoctorRequest, DoctorResponse>, DoctorService>();
builder.Services.AddScoped<IService<PatientRequest, PatientResponse>, PatientService>();


builder.Services.AddControllersWithViews();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
