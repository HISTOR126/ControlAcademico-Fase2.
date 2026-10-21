using System.ComponentModel.DataAnnotations;
using ControlAcademico.Modelos;
using ControlAcademico.Persistencia;
namespace ControlAcademico.Negocio;

public sealed class AcademicoService(IRepositorioAcademico repo)
{
 public static readonly string[] Modulos = ["estudiantes","docentes","cursos","matriculas","calificaciones"];
 public Task<DatosAcademicos> ConsultarAsync() => repo.ConsultarAsync();
 public static void ComprobarModulo(string modulo) { if(!Modulos.Contains(modulo)) throw new ArgumentException("El módulo no existe."); }
 public static IRegistro Obtener(DatosAcademicos d,string modulo,int id) => Registros(d,modulo).FirstOrDefault(e=>e.Id==id) ?? throw new KeyNotFoundException("No existe el registro.");
 public static IEnumerable<IRegistro> Registros(DatosAcademicos d,string modulo) => modulo switch {
  "estudiantes"=>d.Estudiantes,"docentes"=>d.Docentes,"cursos"=>d.Cursos,"matriculas"=>d.Matriculas,"calificaciones"=>d.Calificaciones,_=>throw new ArgumentException("Módulo inválido.")};
 public static string Etiqueta(IRegistro registro,DatosAcademicos d) => registro switch {
  EstudianteRegistro e=>$"{e.Carnet} · {e.Nombre} {e.Apellido}",
  DocenteRegistro e=>$"{e.CodigoEmpleado} · {e.Nombre} {e.Apellido}",
  CursoRegistro c=>$"{c.Codigo} · {c.Nombre}",
  MatriculaRegistro m=>$"{m.Codigo} · {d.Estudiantes.FirstOrDefault(e=>e.Id==m.EstudianteId)?.Carnet} · {d.Cursos.FirstOrDefault(e=>e.Id==m.CursoId)?.Codigo}",
  CalificacionRegistro c=>$"{d.Matriculas.FirstOrDefault(m=>m.Id==c.MatriculaId)?.Codigo} · Nota {c.Nota:F2}",_=>""};
 public static FormularioRegistro Formulario(IRegistro r) => r switch {
  EstudianteRegistro e=>new(){Id=e.Id,Version=e.Version,Codigo=e.Carnet,Nombre=e.Nombre,Apellido=e.Apellido,Email=e.Email,Telefono=e.Telefono,FechaNacimiento=e.FechaNacimiento,Carrera=e.Carrera,Facultad=e.Facultad,CicloActual=e.CicloActual,CUM=e.CUM},
  DocenteRegistro e=>new(){Id=e.Id,Version=e.Version,Codigo=e.CodigoEmpleado,Nombre=e.Nombre,Apellido=e.Apellido,Email=e.Email,Telefono=e.Telefono,FechaNacimiento=e.FechaNacimiento,Especialidad=e.Especialidad},
  CursoRegistro c=>new(){Id=c.Id,Version=c.Version,Codigo=c.Codigo,Nombre=c.Nombre,Creditos=c.Creditos,CupoMaximo=c.CupoMaximo,DocenteId=c.DocenteId},
  MatriculaRegistro m=>new(){Id=m.Id,Version=m.Version,Codigo=m.Codigo,EstudianteId=m.EstudianteId,CursoId=m.CursoId,Activa=m.Activa},
  CalificacionRegistro c=>new(){Id=c.Id,Version=c.Version,MatriculaId=c.MatriculaId,Nota=c.Nota},_=>throw new ArgumentException("Registro inválido.")};
 private static void Exigir(bool condicion,string mensaje) {if(!condicion) throw new ArgumentException(mensaje);}
 private static string Codigo(string c) {Exigir(!string.IsNullOrWhiteSpace(c),"El código es obligatorio.");return c.Trim().ToUpperInvariant();}
 private static void Version(IRegistro r,int version) {if(r.Version!=version) throw new OperacionAcademicaException("El registro cambió. Recargue la página y vuelva a intentarlo.");r.Version++;}
 private static Docente DocenteDominio(DocenteRegistro d) => new(Math.Max(1,d.Id),d.Nombre,d.Apellido,d.Email,d.Telefono,d.FechaNacimiento,d.CodigoEmpleado,d.Especialidad);
 public async Task<int> GuardarAsync(string modulo,FormularioRegistro f)
 {
  ComprobarModulo(modulo);Validator.ValidateObject(f,new ValidationContext(f),true);
  IRegistro? resultado = null;
  await repo.ModificarAsync(d=>
  {
   IRegistro? actual = f.Id==0 ? null : Obtener(d,modulo,f.Id);
   if(actual!=null) Version(actual,f.Version);
   switch(modulo)
   {
    case "estudiantes":
     var alumno = new Estudiante(Math.Max(1,f.Id),f.Nombre,f.Apellido,f.Email,f.Telefono,f.FechaNacimiento,f.Codigo,f.Carrera,f.Facultad,f.CicloActual,f.CUM);
     Exigir(!d.Estudiantes.Any(e=>e.Id!=f.Id && e.Carnet==alumno.Carnet),"Ya existe un estudiante con ese carnet.");
     var e = actual as EstudianteRegistro ?? new();
     e.Carnet=alumno.Carnet;e.Nombre=alumno.Nombre;e.Apellido=alumno.Apellido;e.Email=alumno.Email;e.Telefono=alumno.Telefono;e.FechaNacimiento=alumno.FechaNacimiento;e.Carrera=alumno.Carrera;e.Facultad=alumno.Facultad;e.CicloActual=alumno.CicloActual;e.CUM=alumno.CUM;
     if(actual==null)d.Estudiantes.Add(e);resultado=e;break;
    case "docentes":
     var profesor = new Docente(Math.Max(1,f.Id),f.Nombre,f.Apellido,f.Email,f.Telefono,f.FechaNacimiento,f.Codigo,f.Especialidad);
     Exigir(!d.Docentes.Any(x=>x.Id!=f.Id && x.CodigoEmpleado==profesor.CodigoEmpleado),"Ya existe ese código de docente.");
     var t=actual as DocenteRegistro ?? new();t.CodigoEmpleado=profesor.CodigoEmpleado;t.Nombre=profesor.Nombre;t.Apellido=profesor.Apellido;t.Email=profesor.Email;t.Telefono=profesor.Telefono;t.FechaNacimiento=profesor.FechaNacimiento;t.Especialidad=profesor.Especialidad;
     if(actual==null)d.Docentes.Add(t);resultado=t;break;
    case "cursos":
     var doc=d.Docentes.FirstOrDefault(x=>x.Id==f.DocenteId) ?? throw new ArgumentException("Seleccione un docente registrado.");
     var curso = new Curso(f.Codigo,f.Nombre,f.Creditos,DocenteDominio(doc),f.CupoMaximo);
     Exigir(!d.Cursos.Any(x=>x.Id!=f.Id && x.Codigo==curso.Codigo),"Ya existe ese código de curso.");
     Exigir(f.CupoMaximo>=d.Matriculas.Count(x=>x.CursoId==f.Id && x.Activa),"El cupo no puede ser menor que los inscritos.");
     var c=actual as CursoRegistro ?? new();c.Codigo=curso.Codigo;c.Nombre=curso.Nombre;c.Creditos=curso.Creditos;c.CupoMaximo=curso.CupoMaximo;c.DocenteId=doc.Id;
     if(actual==null)d.Cursos.Add(c);resultado=c;break;
    case "matriculas":
     var codigo=Codigo(f.Codigo);
     Exigir(d.Estudiantes.Any(x=>x.Id==f.EstudianteId),"Seleccione un estudiante registrado.");
     var asignatura=d.Cursos.FirstOrDefault(x=>x.Id==f.CursoId) ?? throw new ArgumentException("Seleccione un curso registrado.");
     Exigir(!d.Matriculas.Any(x=>x.Id!=f.Id && x.Codigo==codigo),"Ya existe ese código de matrícula.");
     var m=actual as MatriculaRegistro ?? new();
     if(d.Calificaciones.Any(x=>x.MatriculaId==f.Id)) Exigir(f.Activa && f.EstudianteId==m.EstudianteId && f.CursoId==m.CursoId,"Una matrícula calificada no se puede cancelar ni cambiar de estudiante o curso.");
     if(f.Activa)
     {
      Exigir(!d.Matriculas.Any(x=>x.Id!=f.Id && x.Activa && x.EstudianteId==f.EstudianteId && x.CursoId==f.CursoId),"El estudiante ya tiene una matrícula activa en ese curso.");
      Exigir(d.Matriculas.Count(x=>x.Id!=f.Id && x.Activa && x.CursoId==f.CursoId)<asignatura.CupoMaximo,"El curso no tiene cupos disponibles.");
     }
     m.Codigo=codigo;m.EstudianteId=f.EstudianteId;m.CursoId=f.CursoId;m.Activa=f.Activa;
     if(actual==null)d.Matriculas.Add(m);resultado=m;break;
    case "calificaciones":
     Exigir(d.Matriculas.Any(x=>x.Id==f.MatriculaId && x.Activa),"Seleccione una matrícula activa.");
     Exigir(double.IsFinite(f.Nota) && f.Nota>=0 && f.Nota<=10,"La nota debe estar entre 0 y 10.");
     Exigir(!d.Calificaciones.Any(x=>x.Id!=f.Id && x.MatriculaId==f.MatriculaId),"Esta matrícula ya tiene nota. Edite la existente.");
     var n=actual as CalificacionRegistro ?? new();n.MatriculaId=f.MatriculaId;n.Nota=f.Nota;
     if(actual==null)d.Calificaciones.Add(n);resultado=n;break;
   }
  });
  return resultado!.Id;
 }
 public Task EliminarAsync(string modulo,int id,int version)
 {
  ComprobarModulo(modulo);
  return repo.ModificarAsync(d=>
  {
   var r=Obtener(d,modulo,id);Version(r,version);
   switch(r)
   {
    case EstudianteRegistro e: Exigir(!d.Matriculas.Any(m=>m.EstudianteId==id),"El estudiante tiene matrículas. Elimine primero los registros relacionados.");d.Estudiantes.Remove(e);break;
    case DocenteRegistro e: Exigir(!d.Cursos.Any(c=>c.DocenteId==id),"El docente tiene cursos asignados.");d.Docentes.Remove(e);break;
    case CursoRegistro e: Exigir(!d.Matriculas.Any(m=>m.CursoId==id),"El curso tiene matrículas relacionadas.");d.Cursos.Remove(e);break;
    case MatriculaRegistro e: Exigir(!d.Calificaciones.Any(c=>c.MatriculaId==id),"Elimine primero la calificación de esta matrícula.");d.Matriculas.Remove(e);break;
    case CalificacionRegistro e: d.Calificaciones.Remove(e);break;
   }
  });
 }
 public async Task<object?> ConsultarEstudianteAsync(string carnet)
 {
  var d=await ConsultarAsync();var e=d.Estudiantes.FirstOrDefault(e=>e.Carnet==carnet.Trim().ToUpperInvariant());
  if(e==null)return null;
  return new {e.Carnet,e.Nombre,e.Apellido,Matriculas=d.Matriculas.Where(m=>m.EstudianteId==e.Id).Select(m=>new {m.Codigo,Curso=d.Cursos.Single(c=>c.Id==m.CursoId).Codigo,m.Activa,Nota=d.Calificaciones.FirstOrDefault(n=>n.MatriculaId==m.Id)?.Nota,Estado=d.Calificaciones.FirstOrDefault(n=>n.MatriculaId==m.Id) is {} nota ? nota.Nota>=6 ? "Aprobado":"Reprobado":"Sin nota"})};
 }
}
