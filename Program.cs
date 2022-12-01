using CFDIWEB.Data;
using CFDIWEB.Interfaces;
using CFDIWEB.Services;
using CFDIWEB.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CFDIWEB.Pages;
using Microsoft.AspNetCore.Builder;
using CFDIWEB.Repositorio;

var builder = WebApplication.CreateBuilder(args);

var cns = "Server=tcp:nousfera.database.windows.net,1433;Initial Catalog=nosufera;Persist Security Info=False;User ID=Sqlnousfera;Password=Nousfera2022;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

builder.Services.AddDbContext<MyAppDbContext>(options => {
    options.UseSqlServer(cns);
});
builder.Services.AddSession(options =>{
    options.IdleTimeout = TimeSpan.FromMinutes(120);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();


builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddHttpClient<IHttpSoapClient, HttpSoapClient>();
builder.Services.AddTransient<IAutenticacionService, AutenticacionService>();
builder.Services.AddTransient<IDescargaMasiva, DescargaMasiva>();
builder.Services.AddTransient<ISolicitudService, SolicitudService>();  
builder.Services.AddTransient<IVerificacionService, VerificacionService>();
builder.Services.AddTransient<IDescargaService, DescargaService>();
builder.Services.AddTransient<IPoliza, Poliza>();
builder.Services.AddTransient<IUsuarios, UsuarioService>();
builder.Services.AddTransient<IPdf, PdfService>();
builder.Services.AddTransient<IAzureStorage, AzureStorage>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddMvc().AddRazorPagesOptions(options => options.Conventions.AddPageRoute("/usuarios", ""));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
 

app.UseAuthorization(); 


app.MapControllerRoute(

    name:"default",
    pattern: "{controller=Home}/{action=usuarios}/{id?}"
);

app.UseSession();
    

app.MapRazorPages();


app.Run();


