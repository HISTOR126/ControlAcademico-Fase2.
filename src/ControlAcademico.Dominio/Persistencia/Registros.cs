namespace ControlAcademico.Persistencia;
// Modelos de intercambio con persistencia. Las reglas se validan en la capa de negocio.
public interface IRegistro { int Id { get; set; } int Version { get; set; } }
public sealed class EstudianteRegistro : IRegistro
{
 public int Id { get; set; } public int Version { get; set; } = 1;
 public string Carnet { get; set; } = "";
 public string Nombre { get; set; } = ""; public string Apellido { get; set; } = "";
 public string Email { get; set; } = ""; public string Telefono { get; set; } = "";
 public DateTime FechaNacimiento { get; set; }
 public string Carrera { get; set; } = ""; public string Facultad { get; set; } = "";
 public int CicloActual { get; set; } = 1; public double CUM { get; set; }
}
public sealed class DocenteRegistro : IRegistro
{
 public int Id { get; set; } public int Version { get; set; } = 1;
 public string CodigoEmpleado { get; set; } = "";
 public string Nombre { get; set; } = ""; public string Apellido { get; set; } = "";
 public string Email { get; set; } = ""; public string Telefono { get; set; } = "";
 public DateTime FechaNacimiento { get; set; }
 public string Especialidad { get; set; } = "";
}
public sealed class CursoRegistro : IRegistro
{
 public int Id { get; set; } public int Version { get; set; } = 1;
 public string Codigo { get; set; } = ""; public string Nombre { get; set; } = "";
 public int Creditos { get; set; } public int CupoMaximo { get; set; }
 public int DocenteId { get; set; }
}
public sealed class MatriculaRegistro : IRegistro
{
 public int Id { get; set; } public int Version { get; set; } = 1;
 public string Codigo { get; set; } = "";
 public int EstudianteId { get; set; } public int CursoId { get; set; }
 public DateTime FechaMatricula { get; set; } = DateTime.UtcNow;
 public bool Activa { get; set; } = true;
}
public sealed class CalificacionRegistro : IRegistro
{
 public int Id { get; set; } public int Version { get; set; } = 1;
 public int MatriculaId { get; set; } public double Nota { get; set; }
}
public sealed class DatosAcademicos
{
 public List<EstudianteRegistro> Estudiantes { get; set; } = new();
 public List<DocenteRegistro> Docentes { get; set; } = new();
 public List<CursoRegistro> Cursos { get; set; } = new();
 public List<MatriculaRegistro> Matriculas { get; set; } = new();
 public List<CalificacionRegistro> Calificaciones { get; set; } = new();
 public IEnumerable<IRegistro> Todos => Estudiantes.Cast<IRegistro>().Concat(Docentes).Concat(Cursos).Concat(Matriculas).Concat(Calificaciones);
}
public interface IRepositorioAcademico
{
 Task<DatosAcademicos> ConsultarAsync();
 Task ModificarAsync(Action<DatosAcademicos> cambio);
}
public sealed class OperacionAcademicaException(string mensaje) : Exception(mensaje);
