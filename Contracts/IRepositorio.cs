namespace Helios.Contracts;

public interface IRepositorio<T>
{
    void Adicionar(T item);
    IReadOnlyList<T> ListarTodos();
}
