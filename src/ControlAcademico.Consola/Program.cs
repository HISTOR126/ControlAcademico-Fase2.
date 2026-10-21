using System.Globalization;
using ControlAcademico.Modelos;
using ControlAcademico.Servicios;

Console.OutputEncoding = System.Text.Encoding.UTF8;
var sistema = new ControlAcademicoService();
var docente = new Docente(1, "Carlos", "Martínez", "carlos@example.edu", "7000-0001", new DateTime(1985,4,10), "DOC-001", "Programación");
sistema.RegistrarPersona(docente);
sistema.RegistrarPersona(new Estudiante(2,"Mario","Pérez","mario@example.edu","7000-0002",new DateTime(2003,5,12),"CT-0001","Sistemas","Ingeniería"));
sistema.RegistrarPersona(new Estudiante(3,"Ana","López","ana@example.edu","7000-0003",new DateTime(2002,11,3),"CT-0002","Sistemas","Ingeniería"));
sistema.RegistrarPersona(new Estudiante(4,"Luis","Gómez","luis@example.edu","7000-0004",new DateTime(2003,3,20),"CT-0003","Sistemas","Ingeniería"));
sistema.RegistrarCurso(new Curso("DSP404","Desarrollo con Software Propietario",4,docente,30));
sistema.Matricular("MAT-0001","CT-0001","DSP404");
sistema.Matricular("MAT-0002","CT-0002","DSP404");
sistema.RegistrarCalificacion(1,"MAT-0001",8.5);
sistema.RegistrarCalificacion(2,"MAT-0002",5);
Console.WriteLine("Control Académico - base para Fase 2 (datos de demostración en memoria)");
while (true)
{
    Console.WriteLine("\n1 Personas | 2 Cursos | 3 Matrículas | 4 Notas | 5 Buscar estudiante");
    Console.WriteLine("6 Registrar matrícula | 7 Registrar nota | 8 Cancelar matrícula | 9 Actualizar nota | 0 Salir");
    Console.Write("Opción: ");
    var opcion = Console.ReadLine();
    if (opcion is null or "0") break;
    try
    {
        switch (opcion.Trim())
        {
            case "1": foreach(var p in sistema.Personas) Console.WriteLine(p.ObtenerDetalle()); break;
            case "2": foreach(var c in sistema.Cursos) Console.WriteLine(c.ObtenerDetalle()); break;
            case "3": foreach(var m in sistema.Matriculas) Console.WriteLine(m.ObtenerDetalle()); break;
            case "4": foreach(var n in sistema.Calificaciones) Console.WriteLine(n.ObtenerDetalle()); break;
            case "5": Console.WriteLine(sistema.BuscarEstudiante(Leer("Carnet"))?.ObtenerDetalle() ?? "No se encontró el estudiante."); break;
            case "6": Console.WriteLine(sistema.Matricular(Leer("ID matrícula nuevo"),Leer("Carnet"),Leer("Código curso")).ObtenerDetalle()); break;
            case "7":
                if (!int.TryParse(Leer("ID de nota"), out var id)) throw new ArgumentException("El ID debe ser entero.");
                Console.WriteLine(sistema.RegistrarCalificacion(id,Leer("ID matrícula"),LeerNota()).ObtenerDetalle()); break;
            case "8": sistema.CancelarMatricula(Leer("ID matrícula")); Console.WriteLine("Matrícula cancelada."); break;
            case "9":
                if (!int.TryParse(Leer("ID de nota"), out var idNota)) throw new ArgumentException("El ID debe ser entero.");
                var nota = sistema.Calificaciones.FirstOrDefault(n => n.Id == idNota) ?? throw new ArgumentException("No existe la nota.");
                nota.ActualizarNota(LeerNota()); Console.WriteLine(nota.ObtenerDetalle()); break;
            default: Console.WriteLine("Opción no válida."); break;
        }
    }
    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
    {
        Console.WriteLine("No se completó la operación: " + ex.Message);
    }
    catch (EndOfStreamException) { break; }
}
static string Leer(string campo)
{
    Console.Write(campo + ": ");
    return Console.ReadLine() ?? throw new EndOfStreamException();
}
static double LeerNota()
{
    var texto = Leer("Nota (0 a 10)").Trim().Replace(',', '.');
    if (!double.TryParse(texto, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
        CultureInfo.InvariantCulture, out var nota)) throw new ArgumentException("Ingrese una nota numérica, por ejemplo 8.5.");
    return nota;
}
