namespace ControlAcademico.Modelos;
public sealed class Curso : IRegistrable
{
    public string Codigo { get; }
    public string Nombre { get; }
    public int Creditos { get; }
    public Docente DocenteAsignado { get; }
    public int CupoMaximo { get; private set; }
    public int Inscritos { get; private set; }
    public Curso(string codigo, string nombre, int creditos, Docente docenteAsignado, int cupoMaximo)
    {
        Codigo = Validacion.Codigo(codigo, "Código de curso");
        Nombre = Validacion.Texto(nombre, "Nombre de curso");
        Creditos = Validacion.Positivo(creditos, "Créditos");
        DocenteAsignado = docenteAsignado ?? throw new ArgumentNullException(nameof(docenteAsignado));
        CupoMaximo = Validacion.Positivo(cupoMaximo, "Cupo");
    }
    public bool TieneCupoDisponible() => Inscritos < CupoMaximo;
    public void CambiarCupo(int nuevoCupo)
    {
        Validacion.Positivo(nuevoCupo, "Cupo");
        if (nuevoCupo < Inscritos) throw new InvalidOperationException("El cupo no puede ser menor que los inscritos.");
        CupoMaximo = nuevoCupo;
    }
    internal void Matricular()
    {
        if (!TieneCupoDisponible()) throw new InvalidOperationException("El curso no tiene cupo disponible.");
        Inscritos++;
    }
    internal void Retirar() { if (Inscritos > 0) Inscritos--; }
    public string ObtenerDetalle() => $"{Codigo} - {Nombre} | Docente: {DocenteAsignado.ObtenerNombreCompleto()} | Créditos: {Creditos} | Cupo: {Inscritos}/{CupoMaximo}";
    public override string ToString() => ObtenerDetalle();
}
