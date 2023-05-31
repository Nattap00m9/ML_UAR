using ML_UAR.Services;
using ML_UAR.Controllers;
using ML_UAR.Interfaces;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Set DateTime Alway EN Format
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-Us");
    options.SupportedCultures = new List<CultureInfo> { new CultureInfo("en-Us") };
    options.RequestCultureProviders.Clear();
});

builder.Services.AddSession(option =>
{
    option.IOTimeout = TimeSpan.FromHours(1);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy", builder => builder
        .AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod());
});

builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<HomeController, HomeController>();
builder.Services.AddTransient<SessionController, SessionController>();
builder.Services.AddTransient<SettingsController, SettingsController>();
builder.Services.AddTransient<ApproveMasterController, ApproveMasterController>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Permission}/{id?}");

app.Run();
