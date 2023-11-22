using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System;
using System.Numerics;
using TreinShop.Areas.Identity.Data;
using TreinShop.Domain.Entities;
using TreinShop.Repositories;
using TreinShop.Repositories.Interfaces;
using TreinShop.Services;
using TreinShop.Services.Interfaces;
using TreinShop.Util.Mail;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddControllersWithViews();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
// Configuration.GetSection("EmailSettings")) zal de instellingen opvragen uit de AppSettings.json file en vervolgens wordt er een emailsettings - object aangemaakt en de waarden worden geïnjecteerd in het object
builder.Services.AddSingleton<IEmailSend, EmailSend>();
//Als in een Constructor een IEmailSender-parameter wordt gevonden, zal een emailSender - object worden aangemaakt.


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API Train",
        Version = "version 1",
        Description = "An API to perform Train operations",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "CDW",
            Email = "christophe.dewaele@vives.be",
            Url = new Uri("https://vives.be"),
        },
        License = new OpenApiLicense
        {
            Name = "Train API LICX",
            Url = new Uri("https://example.com/license"),
        }
    });
});

builder.Services.AddAutoMapper(typeof(Program));
//DI
// your code
builder.Services.AddTransient<IService<Station>, StationService>();
builder.Services.AddTransient<IDAO<Station>, StationDAO>();
builder.Services.AddTransient<IService<Trein>, TreinService>();
builder.Services.AddTransient<IDAO<Trein>, TreinDAO>();
builder.Services.AddTransient<IService<Ticket>, TicketService>();
builder.Services.AddTransient<IDAO<Ticket>, TicketDAO>();
builder.Services.AddTransient<IService<TicketItem>, TicketItemService>();
builder.Services.AddTransient<IDAO<TicketItem>, TicketItemDAO>();
builder.Services.AddTransient<IService<AspNetUser>, UserService>();
builder.Services.AddTransient<IDAO<AspNetUser>, UserDAO>();


builder.Services.AddSession(options =>
{
    options.Cookie.Name = "be.VIVES.Session";

    options.IdleTimeout = TimeSpan.FromMinutes(1);
});

// in welke map zitten de resources
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");


// add View localisation and DataAnnotations localisation.
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder) // vertaling op View
    .AddDataAnnotationsLocalization(); // vertaling op ViewModel

// we need to decide which cultures we support, and which is the default culture.
var supportedCultures = new[] { "nl", "en", "fr" };

builder.Services.Configure<RequestLocalizationOptions>(options => {
    options.SetDefaultCulture(supportedCultures[0])
      .AddSupportedCultures(supportedCultures)  //Culture is used when formatting or parsing culture dependent data like dates, numbers, currencies, etc
      .AddSupportedUICultures(supportedCultures);  //UICulture is used when localizing strings, for example when using resource files.
});



var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var swaggerOptions = new TreinShop.Options.SwaggerOptions();
builder.Configuration.GetSection(nameof(TreinShop.Options.SwaggerOptions)).Bind(swaggerOptions);
// Enable middleware to serve generated Swagger as a JSON endpoint.
//RouteTemplate legt het path vast waar de JSON‐file wordt aangemaakt
app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });

app.UseSwaggerUI(option =>
{
    option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
});
app.UseSwagger();

app.UseHttpsRedirection();
app.UseStaticFiles();

// Culture from the HttpRequest
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseRouting();

app.UseSession();

app.UseAuthentication(); ;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
