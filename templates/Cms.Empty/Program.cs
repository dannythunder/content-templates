using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.DependencyInjection;
using EPiServer.Scheduler;
using EPiServer.Web.Routing;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureCmsDefaults();

if (builder.Environment.IsDevelopment())
{
    AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(builder.Environment.ContentRootPath, "App_Data"));

    builder.Services.Configure<SchedulerOptions>(options => options.Enabled = false);
}

builder.Services
    .AddCmsAspNetIdentity<ApplicationUser>()
    .AddCms()
    .AddAdminUserRegistration()
    .AddEmbeddedLocalization<Program>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapContent();

app.Run();
