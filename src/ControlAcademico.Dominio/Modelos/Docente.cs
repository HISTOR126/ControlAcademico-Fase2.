namespace ControlAcademico.Modelos;
public sealed class Docente : Persona
{
    public string CodigoEmpleado { get; }
    public string Especialidad { get; }
    public Docente(int id, string nombre, string apellido, string email, string telefono,
        DateTime fechaNacimiento, string codigoEmpleado, string especialidad)
        : base(id, nombre, apellido, email, telefono, fechaNacimiento)
    {
        CodigoEmpleado = Validacion.Codigo(codigoEmpleado, "Código de empleado");
        Especialidad = Validacion.Texto(especialidad, "Especialidad");
    }
    public override string ObtenerDetalle() => $"[{CodigoEmpleado}] {ObtenerNombreCompleto()} | {Especialidad} | {Email}";
    public override string ToString() => ObtenerDetalle();
}
