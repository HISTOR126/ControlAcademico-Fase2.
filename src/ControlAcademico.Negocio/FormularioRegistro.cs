using System.ComponentModel.DataAnnotations;
namespace ControlAcademico.Negocio;
public sealed class FormularioRegistro
{
 public int Id { get; set; } public int Version { get; set; } = 1;
 [StringLength(30)] public string Codigo { get; set; } = "";
 [StringLength(100)] public string Nombre { get; set; } = "";
 [StringLength(100)] public string Apellido { get; set; } = "";
 [StringLength(150)] public string Email { get; set; } = "";
 [StringLength(30)] public string Telefono { get; set; } = "";
 public DateTime FechaNacimiento { get; set; } = new(2000,1,1);
 [StringLength(150)] public string Carrera { get; set; } = "";
 [StringLength(150)] public string Facultad { get; set; } = "";
 [StringLength(150)] public string Especialidad { get; set; } = "";
 public int CicloActual { get; set; } = 1; public double CUM { get; set; }
 public int Creditos { get; set; } = 4; public int CupoMaximo { get; set; } = 30;
 public int DocenteId { get; set; } public int EstudianteId { get; set; } public int CursoId { get; set; } public int MatriculaId { get; set; }
 public bool Activa { get; set; } = true; public double Nota { get; set; }
}
