namespace Helios.Domain.Assets;

public class PainelSolar : Ativo
{
    private const double EficienciaMinimaOperacional = 65.0;

    public PainelSolar(string id, string nome, Coordenada localizacao, double capacidadeWatts)
        : base(id, nome, localizacao)
    {
        CapacidadeWatts = capacidadeWatts;
        EficienciaPercentual = 100.0;
    }

    public double CapacidadeWatts { get; }
    public double EficienciaPercentual { get; private set; }
    public bool PrecisaLimpeza => EficienciaPercentual < EficienciaMinimaOperacional;

    public void AtualizarEficiencia(double poeiraPercentual)
    {
        EficienciaPercentual = Math.Clamp(100.0 - poeiraPercentual * 0.85, 0.0, 100.0);
    }

    public void RegistrarLimpeza()
    {
        EficienciaPercentual = 96.0;
    }

    public override string Descrever()
        => $"{Nome} ({Id}) - {CapacidadeWatts:0} W - eficiencia {EficienciaPercentual:0.0}%";
}
