using ControlAcademico.Negocio;
using Microsoft.AspNetCore.Mvc;
namespace ControlAcademico.Web.Controllers;
public sealed class InicioController(AcademicoService servicio) : Controller
{
 public async Task<IActionResult> Index()=>View(await servicio.ConsultarAsync());
 [Route("error")][ResponseCache(Duration=0,Location=ResponseCacheLocation.None,NoStore=true)]
 public IActionResult Error(){Response.StatusCode=500;return View();}
}
