using ControlAcademico.Modelos;
namespace ControlAcademico.Servicios;

/// <summary>Prototipo en memoria. En la web las escrituras deberán usar transacciones y restricciones en BD.</summary>
public sealed class ControlAcademicoService
{
    private readonly List<Persona> personas = new();
    private readonly List<Curso> cursos = new();
    private readonly List<Matricula> matriculas = new();
    private readonly List<Calificacion> calificaciones = new();
    public IReadOnlyList<Persona> Personas => personas.AsReadOnly();
    public IReadOnlyList<Curso> Cursos => cursos.AsReadOnly();
    public IReadOnlyList<Matricula> Matriculas => matriculas.AsReadOnly();
    public IReadOnlyList<Calificacion> Calificaciones => calificaciones.AsReadOnly();
    public void RegistrarPersona(Persona persona)
    {
        ArgumentNullException.ThrowIfNull(persona);
        if (personas.Any(p => p.Id == persona.Id)) throw new InvalidOperationException("El ID de persona ya existe.");
        if (persona is Estudiante e && personas.OfType<Estudiante>().Any(p => p.Carnet == e.Carnet))
            throw new InvalidOperationException("El carnet ya existe.");
        if (persona is Docente d && personas.OfType<Docente>().Any(p => p.CodigoEmpleado == d.CodigoEmpleado))
            throw new InvalidOperationException("El código de empleado ya existe.");
        personas.Add(persona);
    }
    public void RegistrarCurso(Curso curso)
    {
        ArgumentNullException.ThrowIfNull(curso);
        if (!personas.Contains(curso.DocenteAsignado)) throw new InvalidOperationException("Registre primero al docente.");
        if (cursos.Any(c => c.Codigo == curso.Codigo)) throw new InvalidOperationException("El código de curso ya existe.");
        cursos.Add(curso);
    }
    public Estudiante? BuscarEstudiante(string? carnet) => string.IsNullOrWhiteSpace(carnet) ? null :
        personas.OfType<Estudiante>().FirstOrDefault(e => e.Carnet == carnet.Trim().ToUpperInvariant());
    public Matricula Matricular(string id, string carnet, string codigoCurso)
    {
        var identificador = Validacion.Codigo(id, "ID de matrícula");
        var estudiante = BuscarEstudiante(carnet) ?? throw new ArgumentException("No existe el estudiante.");
        var codigo = Validacion.Codigo(codigoCurso, "Código de curso");
        var curso = cursos.FirstOrDefault(c => c.Codigo == codigo) ?? throw new ArgumentException("No existe el curso.");
        if (matriculas.Any(m => m.IdMatricula == identificador)) throw new InvalidOperationException("El ID de matrícula ya existe.");
        if (estudiante.TieneMateria(codigo)) throw new InvalidOperationException("El estudiante ya está matriculado en ese curso.");
        if (!curso.TieneCupoDisponible()) throw new InvalidOperationException("El curso no tiene cupo disponible.");
        var matricula = new Matricula(identificador, estudiante, curso);
        curso.Matricular();
        estudiante.InscribirMateria(codigo);
        matriculas.Add(matricula);
        return matricula;
    }
    public void CancelarMatricula(string id)
    {
        var codigo = Validacion.Codigo(id, "ID de matrícula");
        var matricula = matriculas.FirstOrDefault(m => m.IdMatricula == codigo) ?? throw new ArgumentException("No existe la matrícula.");
        if (!matricula.Activa) throw new InvalidOperationException("La matrícula ya está cancelada.");
        if (calificaciones.Any(c => c.Matricula == matricula)) throw new InvalidOperationException("No se puede cancelar una matrícula con calificación.");
        matricula.CursoAsignado.Retirar();
        matricula.Alumno.RetirarMateria(matricula.CursoAsignado.Codigo);
        matricula.Cancelar();
    }
    public Calificacion RegistrarCalificacion(int id, string idMatricula, double nota)
    {
        var codigo = Validacion.Codigo(idMatricula, "ID de matrícula");
        var matricula = matriculas.FirstOrDefault(m => m.IdMatricula == codigo && m.Activa)
            ?? throw new ArgumentException("No existe una matrícula activa con ese ID.");
        if (calificaciones.Any(c => c.Id == id || c.Matricula == matricula))
            throw new InvalidOperationException("El ID de calificación o la nota de esta matrícula ya existe.");
        var calificacion = new Calificacion(id, matricula, nota);
        calificaciones.Add(calificacion);
        return calificacion;
    }
}
