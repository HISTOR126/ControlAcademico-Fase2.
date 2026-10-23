using ControlAcademico.Negocio;
public static class DatosDemostracion
{
 public static async Task InicializarAsync(AcademicoService s)
 {
  // Nunca reconstruir automáticamente los ejemplos sobre datos ya modificados por el usuario.
  var d=await s.ConsultarAsync();if(d.Todos.Any())return;
  var docente=await s.GuardarAsync("docentes",new(){Codigo="DOC-001",Nombre="Carlos",Apellido="Martínez",Email="carlos@example.edu",Telefono="7000-0001",FechaNacimiento=new(1985,4,10),Especialidad="Programación"});
  var alumno=await s.GuardarAsync("estudiantes",new(){Codigo="CT-0001",Nombre="Mario",Apellido="Pérez",Email="mario@example.edu",Telefono="7000-0002",FechaNacimiento=new(2003,5,12),Carrera="Ingeniería en Sistemas",Facultad="Ingeniería"});
  await s.GuardarAsync("estudiantes",new(){Codigo="CT-0002",Nombre="Ana",Apellido="López",Email="ana@example.edu",Telefono="7000-0003",FechaNacimiento=new(2002,11,3),Carrera="Ingeniería en Sistemas",Facultad="Ingeniería"});
  var curso=await s.GuardarAsync("cursos",new(){Codigo="DSP404",Nombre="Desarrollo de Aplicaciones con Software Propietario",DocenteId=docente,Creditos=4,CupoMaximo=30});
  var matricula=await s.GuardarAsync("matriculas",new(){Codigo="MAT-0001",EstudianteId=alumno,CursoId=curso});
  await s.GuardarAsync("calificaciones",new(){MatriculaId=matricula,Nota=8.5});
 }
}
