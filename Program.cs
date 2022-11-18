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

var builder = WebApplication.CreateBuilder(args);

var cns = "Data Source  = .; Initial Catalog = master; Integrated security= True";

builder.Services.AddDbContext<MyAppDbContext>(options => {
    options.UseSqlServer(cns);
});
builder.Services.AddSession(options =>{
    options.IdleTimeout = TimeSpan.FromMinutes(120);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddHttpClient<IHttpSoapClient, HttpSoapClient>();
builder.Services.AddTransient<IAutenticacionService, AutenticacionService>();
builder.Services.AddTransient<IDescargaMasiva, DescargaMasiva>();
builder.Services.AddTransient<ISolicitudService, SolicitudService>();  
builder.Services.AddTransient<IVerificacionService, VerificacionService>();
builder.Services.AddTransient<IDescargaService, DescargaService>();
builder.Services.AddTransient<IPoliza, Poliza>();
builder.Services.AddTransient<IUsuarios, UsuarioService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();


// Add services to the container.
builder.Services.AddRazorPages();


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


