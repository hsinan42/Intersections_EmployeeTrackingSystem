using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using FluentValidation;
using Intersections_EmployeeTrackingSystem.Models;
using Intersections_EmployeeTrackingSystem.ValidationRules;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

//Services
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IIntersectionDal, EfIntersectionRepository>();
builder.Services.AddScoped<IIntersectionService, IntersectionManager>();
builder.Services.AddScoped<IUserDal, EfUserRepository>();
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<IImageDal, EfImageRepository>();
builder.Services.AddScoped<IImageService, ImageManager>();
builder.Services.AddScoped<IReportDal, EfReportRepository>();
builder.Services.AddScoped<IReportService, ReportManager>();
builder.Services.AddScoped<IIntersectionChangeRequestDal, EfIntersectionChangeRequestRepository>();
builder.Services.AddScoped<IIntersectionChangeRequestService, IntersectionChangeRequestManager>();



builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddControllersWithViews()
    .AddViewOptions(options =>
    {
        options.HtmlHelperOptions.ClientValidationEnabled = false;
    });
builder.Services.AddAutoMapper(typeof(Program));

//Validator
builder.Services.AddScoped<IValidator<RegisterViewModel>, RegisterValidator>();
builder.Services.AddScoped<IValidator<LoginViewModel>, LoginValidator>();
builder.Services.AddScoped<IValidator<IntersectionViewModel>, FormValidator>();
builder.Services.AddScoped<IValidator<ReportsViewModel>, ReportViewModelValidator>();
//builder.Services.AddScoped<IValidator<LocationViewModel>, LocationViewModelValidator>();

//Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true;
});

//Authentication
builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.Cookie.Name = "MyCookieAuth";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseMigrationsEndPoint();
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Map}/{id?}");
//app.MapRazorPages();

app.Run();
