using ControlAcademico.Negocio;
using ControlAcademico.Persistencia;
namespace ControlAcademico.Web.Models;
public sealed class PaginaAcademica
{
 public string Modulo { get; set; } = "estudiantes";
 public DatosAcademicos Datos { get; set; } = new();
 public FormularioRegistro Formulario { get; set; } = new();
 public string Buscar { get; set; } = "";
 public string Titulo => Modulo switch {"estudiantes"=>"Estudiantes","docentes"=>"Docentes","cursos"=>"Cursos","matriculas"=>"Matrículas","calificaciones"=>"Calificaciones",_=>"Control Académico"};
 public string Etiqueta(IRegistro r)=>AcademicoService.Etiqueta(r,Datos);
 public IEnumerable<IRegistro> Filas=>AcademicoService.Registros(Datos,Modulo).Where(r=>string.IsNullOrWhiteSpace(Buscar)||Etiqueta(r).Contains(Buscar,StringComparison.OrdinalIgnoreCase));
}
