using ControlAcademico.Datos;
using ControlAcademico.Negocio;
using ControlAcademico.Persistencia;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Globalization;
using Microsoft.AspNetCore.DataProtection;

var builder=WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews(o=>o.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute()));
var archivo=builder.Configuration["Academico:Archivo"] ?? Path.Combine(builder.Environment.ContentRootPath,"App_Data","academico.db");
archivo=Path.GetFullPath(archivo);Directory.CreateDirectory(Path.GetDirectoryName(archivo)!);
builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(Path.GetDirectoryName(archivo)!,"keys")));
var conexion=new SqliteConnectionStringBuilder {DataSource=archivo,ForeignKeys=true,DefaultTimeout=10}.ToString();
builder.Services.AddDbContextFactory<AcademicoDbContext>(o=>o.UseSqlite(conexion));
builder.Services.AddScoped<IRepositorioAcademico,RepositorioAcademico>();
builder.Services.AddScoped<AcademicoService>();
var app=builder.Build();
app.UseRequestLocalization(new RequestLocalizationOptions().SetDefaultCulture("es-SV").AddSupportedCultures("es-SV").AddSupportedUICultures("es-SV"));
app.UseExceptionHandler("/error");
app.UseStaticFiles();
app.UseRouting();
using(var scope=app.Services.CreateScope())
{
 var factory=scope.ServiceProvider.GetRequiredService<IDbContextFactory<AcademicoDbContext>>();
 using var db=factory.CreateDbContext();
 db.Database.EnsureCreated();
 if(builder.Configuration.GetValue("Academico:SembrarDatos",true))
  await DatosDemostracion.InicializarAsync(scope.ServiceProvider.GetRequiredService<AcademicoService>());
}
app.MapControllers();
app.MapControllerRoute("default","{controller=Inicio}/{action=Index}/{id?}");
app.Run();
