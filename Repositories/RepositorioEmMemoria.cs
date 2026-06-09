using Helios.Contracts;

namespace Helios.Repositories;

public class RepositorioEmMemoria<T> : IRepositorio<T>
{
    private readonly List<T> _itens = [];

    public void Adicionar(T item) => _itens.Add(item);

    public IReadOnlyList<T> ListarTodos() => _itens.AsReadOnly();
}
