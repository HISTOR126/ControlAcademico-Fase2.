namespace ControlAcademico.Modelos;

public sealed class Estudiante : Persona
{
    private readonly List<string> materias = new();
    public string Carnet { get; }
    public string Carrera { get; }
    public string Facultad { get; }
    public int CicloActual { get; }
    public double CUM { get; private set; }
    public int CreditosAprobados { get; private set; }
    public string EstadoAcademico => egresado ? "Egresado" : CUM > 0 && CUM < 6 ? "En Riesgo" : "Activo";
    private bool egresado;
    public IReadOnlyList<string> MateriasInscritas => materias.AsReadOnly();

    public Estudiante(int id, string nombres, string apellidos, string email, string telefono,
        DateTime fechaNacimiento, string carnet, string carrera, string facultad, int cicloActual = 1, double cum = 0)
        : base(id, nombres, apellidos, email, telefono, fechaNacimiento)
    {
        Carnet = Validacion.Codigo(carnet, "Carnet");
        Carrera = Validacion.Texto(carrera, "Carrera");
        Facultad = Validacion.Texto(facultad, "Facultad");
        CicloActual = Validacion.Positivo(cicloActual, "Ciclo");
        CUM = Validacion.Nota(cum);
    }
    internal bool TieneMateria(string codigo) => materias.Contains(codigo);
    internal void InscribirMateria(string codigo) => materias.Add(codigo);
    internal void RetirarMateria(string codigo) => materias.Remove(codigo);
    public void ActualizarCUM(double nuevoCum) => CUM = Validacion.Nota(nuevoCum);
    public void ActualizarCreditosAprobados(int creditos)
    {
        if (creditos < 0) throw new ArgumentException("Los créditos no pueden ser negativos.");
        CreditosAprobados = creditos;
        egresado = false;
    }
    public bool VerificarEgresado(int creditosTotalesCarrera = 160)
    {
        egresado = CreditosAprobados >= Validacion.Positivo(creditosTotalesCarrera, "Créditos de carrera");
        return egresado;
    }
    public override string ObtenerDetalle() => $"[{Carnet}] {ObtenerNombreCompleto()} | {Carrera} | CUM: {CUM:F2} | {EstadoAcademico} | Materias: {string.Join(", ", materias)}";
    public override string ToString() => ObtenerDetalle();
}
