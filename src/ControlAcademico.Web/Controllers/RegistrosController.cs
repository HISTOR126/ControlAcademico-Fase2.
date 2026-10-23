using ControlAcademico.Negocio;
using ControlAcademico.Persistencia;
using ControlAcademico.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace ControlAcademico.Web.Controllers;
[Route("academico/{modulo}")]
public sealed class RegistrosController(AcademicoService servicio,ILogger<RegistrosController> logger) : Controller
{
 private async Task<PaginaAcademica> Pagina(string modulo,FormularioRegistro? f=null)=>new(){Modulo=modulo,Datos=await servicio.ConsultarAsync(),Formulario=f??new()};
 [HttpGet("")]
 public async Task<IActionResult> Index(string modulo,string? q)
 {if(!AcademicoService.Modulos.Contains(modulo))return NotFound();var p=await Pagina(modulo);p.Buscar=q??"";return View(p);}
 [HttpGet("nuevo")]
 public async Task<IActionResult> Nuevo(string modulo)
 {if(!AcademicoService.Modulos.Contains(modulo))return NotFound();return View("Formulario",await Pagina(modulo));}
 [HttpGet("{id:int}/editar")]
 public async Task<IActionResult> Editar(string modulo,int id)
 {
  if(!AcademicoService.Modulos.Contains(modulo))return NotFound();var p=await Pagina(modulo);
  try {p.Formulario=AcademicoService.Formulario(AcademicoService.Obtener(p.Datos,modulo,id));return View("Formulario",p);}catch(KeyNotFoundException){return NotFound();}
 }
 [HttpGet("{id:int}")]
 public async Task<IActionResult> Detalle(string modulo,int id)
 {if(!AcademicoService.Modulos.Contains(modulo))return NotFound();var p=await Pagina(modulo);try{p.Formulario=AcademicoService.Formulario(AcademicoService.Obtener(p.Datos,modulo,id));return View(p);}catch(KeyNotFoundException){return NotFound();}}
 [HttpPost("guardar")]
 public async Task<IActionResult> Guardar(string modulo,FormularioRegistro f)
 {
  if(!AcademicoService.Modulos.Contains(modulo))return NotFound();
  if(ModelState.IsValid)
  {
   try {var id=await servicio.GuardarAsync(modulo,f);TempData["Exito"]="Registro guardado correctamente.";return RedirectToAction(nameof(Detalle),new{modulo,id});}
   catch(KeyNotFoundException){return NotFound();}
   catch(Exception ex) when(ex is ArgumentException or ValidationException or OperacionAcademicaException)
   {logger.LogWarning("Operación rechazada en {Modulo}: {Mensaje}",modulo,ex.Message);ModelState.AddModelError("",ex.Message);}
  }
  Response.StatusCode=422;return View("Formulario",await Pagina(modulo,f));
 }
 [HttpGet("{id:int}/eliminar")]
 public async Task<IActionResult> Eliminar(string modulo,int id)
 {if(!AcademicoService.Modulos.Contains(modulo))return NotFound();var p=await Pagina(modulo);try{p.Formulario=AcademicoService.Formulario(AcademicoService.Obtener(p.Datos,modulo,id));return View(p);}catch(KeyNotFoundException){return NotFound();}}
 [HttpPost("{id:int}/eliminar")]
 public async Task<IActionResult> ConfirmarEliminar(string modulo,int id,int version)
 {
  if(!AcademicoService.Modulos.Contains(modulo))return NotFound();
  try{await servicio.EliminarAsync(modulo,id,version);TempData["Exito"]="Registro eliminado.";return RedirectToAction(nameof(Index),new{modulo});}
  catch(KeyNotFoundException){return NotFound();}
  catch(Exception ex) when(ex is ArgumentException or OperacionAcademicaException){ModelState.AddModelError("",ex.Message);var p=await Pagina(modulo);p.Formulario=AcademicoService.Formulario(AcademicoService.Obtener(p.Datos,modulo,id));Response.StatusCode=422;return View("Eliminar",p);}
 }
}
