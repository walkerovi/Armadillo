using Armadillo.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configuracion para llamar a la Conexion de la BD en Appsetting.Json.
string ConnectionStrings = builder.Configuration.GetConnectionString("Negocio");
string ConnectionStringsSeguridad = builder.Configuration.GetConnectionString("Seguridad");

builder.Services.AddDbContext<ArmadilloContext>(options =>
           options.UseMySql(ConnectionStrings, ServerVersion.AutoDetect(ConnectionStrings)));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(ConnectionStringsSeguridad, ServerVersion.AutoDetect(ConnectionStringsSeguridad)));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

/*Crea la base de datos cuando no existe*/
using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<ArmadilloContext>();
    context.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
