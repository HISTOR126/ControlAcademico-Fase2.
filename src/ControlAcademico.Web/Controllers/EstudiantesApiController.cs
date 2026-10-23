using ControlAcademico.Negocio;
using ControlAcademico.Persistencia;
using Microsoft.AspNetCore.Mvc;
namespace ControlAcademico.Web.Controllers;
[ApiController][Route("api/estudiantes")]
public sealed class EstudiantesApiController(AcademicoService servicio) : ControllerBase
{
 [HttpGet("{carnet}/situacion")]
 public async Task<IActionResult> Situacion(string carnet)
 {
  try {var resultado=await servicio.ConsultarEstudianteAsync(carnet);return resultado==null ? NotFound(new {mensaje="No existe el estudiante."}) : Ok(resultado);}
  catch(OperacionAcademicaException){return Problem("No fue posible consultar los registros. Intente nuevamente.",statusCode:503);}
 }
}
