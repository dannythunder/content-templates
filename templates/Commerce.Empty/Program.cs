using EPiServer.Cms.Shell;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Mediachase.Commerce.Anonymous;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureCmsDefaults();

var isDevelopment = builder.Environment.IsDevelopment();

if (isDevelopment)
{
    AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(builder.Environment.ContentRootPath, "App_Data"));

    builder.Services.Configure<SchedulerOptions>(options => options.Enabled = false);
}

builder.Services
    .AddCmsAspNetIdentity<ApplicationUser>()
    .AddCommerce()
    .AddAdminUserRegistration()
    .AddEmbeddedLocalization<Program>();

var app = builder.Build();

if (isDevelopment)
{
    app.UseDeveloperExceptionPage();
}

app.UseAnonymousId();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapContent();

app.Run();
