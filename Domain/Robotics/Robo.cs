using Helios.Domain.Assets;

namespace Helios.Domain.Robotics;

public partial class Robo
{
    public Robo(string id, string nome, Coordenada localizacaoInicial)
    {
        Id = id;
        Nome = nome;
        Localizacao = localizacaoInicial;
        BateriaPercentual = 88.0;
        Status = "Aguardando";
    }

    public string Id { get; }
    public string Nome { get; }
    public Coordenada Localizacao { get; private set; }
    public double BateriaPercentual { get; private set; }
    public string Status { get; private set; }

    private void ConsumirBateria(double percentual)
    {
        BateriaPercentual = Math.Max(0, BateriaPercentual - percentual);
    }
}
