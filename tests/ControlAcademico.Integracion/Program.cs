using ControlAcademico.Datos;
using ControlAcademico.Negocio;
using ControlAcademico.Persistencia;
using Microsoft.EntityFrameworkCore;

var archivo=Path.Combine(Path.GetTempPath(),"academico-prueba-"+Guid.NewGuid()+".db");
var opciones=new DbContextOptionsBuilder<AcademicoDbContext>().UseSqlite("Data Source="+archivo+";Foreign Keys=True").Options;
var factory=new Factory(opciones);
using(var db=factory.CreateDbContext())db.Database.EnsureCreated();
var servicio=new AcademicoService(new RepositorioAcademico(factory));
int aprobadas=0;
void Bien(bool condicion,string mensaje){if(!condicion)throw new Exception(mensaje);aprobadas++;Console.WriteLine("OK "+mensaje);}
async Task Rechaza(Func<Task> f,string nombre){try{await f();}catch(Exception e) when(e is ArgumentException or OperacionAcademicaException){aprobadas++;Console.WriteLine("OK "+nombre);return;}throw new Exception("Aceptó operación inválida: "+nombre);}
try
{
 var docente=await servicio.GuardarAsync("docentes",new(){Codigo="D1",Nombre="Carlos",Apellido="Martínez",Email="carlos@example.edu",Telefono="7000",FechaNacimiento=new(1980,1,1),Especialidad="Sistemas"});
 var estudiante=await servicio.GuardarAsync("estudiantes",new(){Codigo="E1",Nombre="Ana",Apellido="López",Email="ana@example.edu",Telefono="7000",FechaNacimiento=new(2000,1,1),Carrera="Sistemas",Facultad="Ingeniería"});
 var curso=await servicio.GuardarAsync("cursos",new(){Codigo="C1",Nombre="Curso",Creditos=4,CupoMaximo=1,DocenteId=docente});
 var matricula=await servicio.GuardarAsync("matriculas",new(){Codigo="M1",EstudianteId=estudiante,CursoId=curso});
 var nota=await servicio.GuardarAsync("calificaciones",new(){MatriculaId=matricula,Nota=8.5});
 var d=await servicio.ConsultarAsync();Bien(d.Estudiantes.Count==1 && d.Docentes.Count==1 && d.Cursos.Count==1 && d.Matriculas.Count==1 && d.Calificaciones.Count==1,"Alta de las cinco entidades en SQLite");
 var otraSesion=new AcademicoService(new RepositorioAcademico(factory));
 Bien((await otraSesion.ConsultarAsync()).Calificaciones.Single().Nota==8.5,"Persistencia consultada desde una sesión nueva");
 Bien(await servicio.ConsultarEstudianteAsync(" e1 ")!=null,"API de consulta normaliza carnet");
 Bien(await servicio.ConsultarEstudianteAsync("NO-EXISTE")==null,"Consulta inexistente retorna null");
 await Rechaza(()=>servicio.GuardarAsync("matriculas",new(){Codigo="M2",EstudianteId=estudiante,CursoId=curso}),"Matrícula duplicada rechazada");
 var e2=await servicio.GuardarAsync("estudiantes",new(){Codigo="E2",Nombre="Juan",Apellido="Pérez",Email="juan@example.edu",Telefono="7000",FechaNacimiento=new(2000,1,1),Carrera="Sistemas",Facultad="Ingeniería"});
 await Rechaza(()=>servicio.GuardarAsync("matriculas",new(){Codigo="M2",EstudianteId=e2,CursoId=curso}),"Curso lleno rechaza otro estudiante");
 await Rechaza(()=>servicio.GuardarAsync("calificaciones",new(){Id=nota,Version=1,MatriculaId=matricula,Nota=25}),"Nota fuera de rango rechazada");
 Bien((await servicio.ConsultarAsync()).Calificaciones.Single().Version==1,"Rollback conserva versión y nota tras error");
 await Rechaza(()=>servicio.GuardarAsync("calificaciones",new(){MatriculaId=matricula,Nota=7}),"Segunda nota para misma matrícula rechazada");
 await Rechaza(()=>servicio.EliminarAsync("docentes",docente,1),"No elimina docente con curso");
 await Rechaza(()=>servicio.EliminarAsync("estudiantes",estudiante,1),"No elimina estudiante con matrícula");
 await Rechaza(()=>servicio.EliminarAsync("cursos",curso,1),"No elimina curso con matrícula");
 await Rechaza(()=>servicio.EliminarAsync("matriculas",matricula,1),"No elimina matrícula con nota");
 await Rechaza(()=>servicio.GuardarAsync("matriculas",new(){Id=matricula,Version=1,Codigo="M1",EstudianteId=estudiante,CursoId=curso,Activa=false}),"No cancela matrícula calificada");
 // Modificación de cada entidad, conservando sus identificadores.
 d=await servicio.ConsultarAsync();
 foreach(var modulo in AcademicoService.Modulos)
 {
  var r=AcademicoService.Registros(d,modulo).First();var f=AcademicoService.Formulario(r);
  if(modulo=="estudiantes")f.Nombre="Ana editada";
  if(modulo=="docentes")f.Especialidad="Programación";
  if(modulo=="cursos")f.Nombre="Curso editado";
  if(modulo=="matriculas")f.Codigo="M1-EDITADA";
  if(modulo=="calificaciones")f.Nota=9;
  await servicio.GuardarAsync(modulo,f);
  Bien(AcademicoService.Obtener(await servicio.ConsultarAsync(),modulo,r.Id).Version==2,"Modificación persistida: "+modulo);
 }
 await Rechaza(()=>servicio.GuardarAsync("calificaciones",new(){Id=nota,Version=1,MatriculaId=matricula,Nota=5}),"Edición obsoleta rechazada por versión");
 await servicio.EliminarAsync("calificaciones",nota,2);
 var fm=AcademicoService.Formulario(AcademicoService.Obtener(await servicio.ConsultarAsync(),"matriculas",matricula));fm.Activa=false;await servicio.GuardarAsync("matriculas",fm);
 var nueva=await servicio.GuardarAsync("matriculas",new(){Codigo="M2",EstudianteId=e2,CursoId=curso});Bien(nueva>0,"Cancelación libera cupo");
 await servicio.EliminarAsync("matriculas",nueva,1);await servicio.EliminarAsync("matriculas",matricula,3);await servicio.EliminarAsync("cursos",curso,2);await servicio.EliminarAsync("docentes",docente,2);await servicio.EliminarAsync("estudiantes",estudiante,2);await servicio.EliminarAsync("estudiantes",e2,1);
 Bien(!(await servicio.ConsultarAsync()).Todos.Any(),"Eliminación física de las cinco entidades en orden de dependencias");
 Console.WriteLine($"Resultado: {aprobadas} comprobaciones aprobadas.");
}
finally{Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();File.Delete(archivo);}
sealed class Factory(DbContextOptions<AcademicoDbContext> opciones) : IDbContextFactory<AcademicoDbContext>
{public AcademicoDbContext CreateDbContext()=>new(opciones);}
