namespace ControlAcademico.Modelos;

public abstract class Persona
{
    public int Id { get; }
    public string Nombre { get; }
    public string Apellido { get; }
    public string Email { get; }
    public string Telefono { get; }
    public DateTime FechaNacimiento { get; }

    protected Persona(int id, string nombre, string apellido, string email, string telefono, DateTime fechaNacimiento)
    {
        Id = Validacion.Positivo(id, "ID");
        Nombre = Validacion.Texto(nombre, "Nombre");
        Apellido = Validacion.Texto(apellido, "Apellido");
        Email = Validacion.Correo(email);
        Telefono = Validacion.Texto(telefono, "Teléfono");
        if (fechaNacimiento.Date > DateTime.Today || fechaNacimiento == default)
            throw new ArgumentException("La fecha de nacimiento es inválida.");
        FechaNacimiento = fechaNacimiento.Date;
    }
    public string ObtenerNombreCompleto() => $"{Nombre} {Apellido}";
    public int ObtenerEdad()
    {
        var hoy = DateTime.Today;
        var edad = hoy.Year - FechaNacimiento.Year;
        return FechaNacimiento > hoy.AddYears(-edad) ? edad - 1 : edad;
    }
    public abstract string ObtenerDetalle();
}
