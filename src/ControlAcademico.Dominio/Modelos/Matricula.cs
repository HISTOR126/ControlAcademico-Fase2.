namespace ControlAcademico.Modelos;
public sealed class Matricula : IRegistrable
{
    public string IdMatricula { get; }
    public Estudiante Alumno { get; }
    public Curso CursoAsignado { get; }
    public DateTime FechaMatricula { get; }
    public bool Activa { get; private set; } = true;
    // Solo el servicio crea matrículas, después de validar el estado completo.
    internal Matricula(string idMatricula, Estudiante alumno, Curso curso)
    {
        IdMatricula = Validacion.Codigo(idMatricula, "ID de matrícula");
        Alumno = alumno ?? throw new ArgumentNullException(nameof(alumno));
        CursoAsignado = curso ?? throw new ArgumentNullException(nameof(curso));
        FechaMatricula = DateTime.UtcNow;
    }
    internal void Cancelar() => Activa = false;
    public string ObtenerDetalle() => $"{IdMatricula} | {Alumno.Carnet}: {Alumno.ObtenerNombreCompleto()} | {CursoAsignado.Codigo} | {FechaMatricula:yyyy-MM-dd} UTC | {(Activa ? "Activa" : "Cancelada")}";
    public override string ToString() => ObtenerDetalle();
}
