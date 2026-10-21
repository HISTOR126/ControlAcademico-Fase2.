using System.Net.Mail;
namespace ControlAcademico.Modelos;

internal static class Validacion
{
    internal static string Texto(string? valor, string campo) =>
        !string.IsNullOrWhiteSpace(valor) ? valor.Trim() : throw new ArgumentException($"{campo} es obligatorio.");
    internal static string Codigo(string? valor, string campo) => Texto(valor, campo).ToUpperInvariant();
    internal static int Positivo(int valor, string campo) => valor > 0 ? valor : throw new ArgumentException($"{campo} debe ser mayor que cero.");
    internal static double Nota(double valor) => double.IsFinite(valor) && valor >= 0 && valor <= 10
        ? valor : throw new ArgumentException("La nota debe ser un número entre 0 y 10.");
    internal static string Correo(string? valor)
    {
        var texto = Texto(valor, "Correo");
        if (!MailAddress.TryCreate(texto, out var correo) || correo.Address != texto)
            throw new ArgumentException("El correo no tiene un formato válido.");
        return texto;
    }
}
