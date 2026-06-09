namespace Helios.Domain.Telemetry;

public readonly struct LeituraValor
{
    public LeituraValor(double valor, string unidade)
    {
        Valor = valor;
        Unidade = unidade;
    }

    public double Valor { get; }
    public string Unidade { get; }

    public override string ToString() => $"{Valor:0.##} {Unidade}";
}
