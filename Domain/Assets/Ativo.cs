namespace Helios.Domain.Assets;

public abstract class Ativo
{
    protected Ativo(string id, string nome, Coordenada localizacao)
    {
        Id = id;
        Nome = nome;
        Localizacao = localizacao;
    }

    public string Id { get; }
    public string Nome { get; }
    public Coordenada Localizacao { get; protected set; }

    public abstract string Descrever();
}
