using Helios.Contracts;

namespace Helios.Services;

public class ConsoleNotificador : INotificador
{
    public void Informar(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"[INFO] {mensagem}");
        Console.ResetColor();
    }

    public void Alertar(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[ALERTA] {mensagem}");
        Console.ResetColor();
    }

    public void Erro(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERRO] {mensagem}");
        Console.ResetColor();
    }
}
