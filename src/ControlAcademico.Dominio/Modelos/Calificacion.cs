namespace ControlAcademico.Modelos;
public sealed class Calificacion : IEvaluable, IRegistrable
{
    public int Id { get; }
    public Matricula Matricula { get; }
    public Estudiante Alumno => Matricula.Alumno;
    public Curso Asignatura => Matricula.CursoAsignado;
    public double Nota { get; private set; }
    internal Calificacion(int id, Matricula matricula, double nota)
    {
        Id = Validacion.Positivo(id, "ID de calificación");
        Matricula = matricula ?? throw new ArgumentNullException(nameof(matricula));
        ActualizarNota(nota);
    }
    public void ActualizarNota(double nota)
    {
        if (!Matricula.Activa) throw new InvalidOperationException("La matrícula está cancelada.");
        Nota = Validacion.Nota(nota);
    }
    public bool EstaAprobado() => Nota >= 6;
    public string ObtenerEstado() => EstaAprobado() ? "Aprobado" : "Reprobado";
    public string ObtenerDetalle() => $"{Id} | {Alumno.Carnet} | {Asignatura.Codigo} | Nota: {Nota:F2} | {ObtenerEstado()}";
    public override string ToString() => ObtenerDetalle();
}
