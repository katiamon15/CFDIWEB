using CFDIWEB.Data;
using CFDIWEB.Interfaces;
using CFDIWEB.Services;
using CFDIWEB.Models.Entity;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

var cns = "Data Source= .; Initial Catalog = master; Integrated security= True";

builder.Services.AddDbContext<MyAppDbContext>(options => {
    options.UseSqlServer(cns);
});
builder.Services.AddHttpClient<IHttpSoapClient, HttpSoapClient>();
builder.Services.AddTransient<IAutenticacionService, AutenticacionService>();
builder.Services.AddTransient<IDescargaMasiva, DescargaMasiva>();
builder.Services.AddTransient<ISolicitudService, SolicitudService>();


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
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
    

app.MapRazorPages();


app.Run();


