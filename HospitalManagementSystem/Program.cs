using HospitalManagementSystem;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services.AppointmentServices;
using HospitalManagementSystem.Services.DepartmentServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IDepartmentServices, DepartmentServices>();
builder.Services.AddScoped<IAppointmentServices, AppointmentServices>();

builder.Services.AddDbContext<HMSDbContext>(op =>
{
    op.UseSqlServer("Data Source=(localdb)\\ProjectModels;Initial Catalog=HMSDB;Integrated Security=True;");
});
builder.Services.AddIdentity<ApplicationUser,IdentityRole>()
    .AddEntityFrameworkStores<HMSDbContext>()
    .AddDefaultTokenProviders();

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


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
