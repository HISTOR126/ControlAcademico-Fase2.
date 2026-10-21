using ControlAcademico.Persistencia;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
namespace ControlAcademico.Datos;
public sealed class RepositorioAcademico(IDbContextFactory<AcademicoDbContext> factory) : IRepositorioAcademico
{
 private static async Task<DatosAcademicos> Leer(AcademicoDbContext db) => new()
 {
  Estudiantes = await db.Estudiantes.OrderBy(e=>e.Apellido).ThenBy(e=>e.Nombre).ToListAsync(),
  Docentes = await db.Docentes.OrderBy(e=>e.Apellido).ToListAsync(),
  Cursos = await db.Cursos.OrderBy(e=>e.Codigo).ToListAsync(),
  Matriculas = await db.Matriculas.OrderByDescending(e=>e.Id).ToListAsync(),
  Calificaciones = await db.Calificaciones.OrderByDescending(e=>e.Id).ToListAsync()
 };
 public async Task<DatosAcademicos> ConsultarAsync()
 {
  try { await using var db = await factory.CreateDbContextAsync(); return await Leer(db); }
  catch(Exception ex) when(ex is SqliteException or DbUpdateException)
  { throw new OperacionAcademicaException("No fue posible consultar la base de datos. Intente nuevamente."); }
 }
 public async Task ModificarAsync(Action<DatosAcademicos> cambio)
 {
  try
  {
   await using var db = await factory.CreateDbContextAsync();
   await using var transaccion = await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
   var datos = await Leer(db);
   var originales = datos.Todos.ToList();
   cambio(datos);
   var actuales = datos.Todos.ToList();
   foreach(var eliminado in originales.Where(o=>!actuales.Contains(o))) db.Remove(eliminado);
   foreach(var nuevo in actuales.Where(o=>o.Id==0)) db.Add(nuevo);
   await db.SaveChangesAsync();
   await transaccion.CommitAsync();
  }
  catch(DbUpdateConcurrencyException) { throw new OperacionAcademicaException("El registro cambió en otra sesión. Recargue la página antes de guardar."); }
  catch(Exception ex) when(ex is DbUpdateException or SqliteException)
  { throw new OperacionAcademicaException("No se pudo guardar: existe un registro relacionado, un código duplicado o una operación simultánea. Recargue y revise los datos."); }
 }
}
