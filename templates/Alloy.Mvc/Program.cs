using Alloy.Mvc._1.Extensions;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Data;
using EPiServer.DependencyInjection;
using EPiServer.Scheduler;
using EPiServer.Web.Routing;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureCmsDefaults();

var isDevelopment = builder.Environment.IsDevelopment();

if (isDevelopment)
{
    AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(builder.Environment.ContentRootPath, "App_Data"));

    builder.Services.Configure<SchedulerOptions>(options => options.Enabled = false);
}

builder.Services.Configure<DataAccessOptions>(o => o.UpdateDatabaseCompatibilityLevel = true);

builder.Services
    .AddCmsAspNetIdentity<ApplicationUser>()
    .AddCms()
    .AddAlloy()
    .AddAdminUserRegistration()
    .AddEmbeddedLocalization<Program>();

// Required by Wangkanai.Detection
builder.Services.AddDetection();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (isDevelopment)
{
    app.UseDeveloperExceptionPage();
}

// Required by Wangkanai.Detection
app.UseDetection();
app.UseSession();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapContent();

app.Run();
