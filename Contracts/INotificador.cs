namespace Helios.Contracts;

public interface INotificador
{
    void Informar(string mensagem);
    void Alertar(string mensagem);
    void Erro(string mensagem);
}
