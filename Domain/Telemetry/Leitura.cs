namespace Helios.Domain.Telemetry;

public class Leitura
{
    public Leitura(string sensorId, string tipo, LeituraValor valor, string ativoId, DateTime timestampUtc)
    {
        SensorId = sensorId;
        Tipo = tipo;
        Valor = valor;
        AtivoId = ativoId;
        TimestampUtc = timestampUtc.Kind == DateTimeKind.Utc
            ? timestampUtc
            : DateTime.SpecifyKind(timestampUtc, DateTimeKind.Utc);
    }

    public string SensorId { get; }
    public string Tipo { get; }
    public LeituraValor Valor { get; }
    public string AtivoId { get; }
    public DateTime TimestampUtc { get; }

    public string ToContractJson()
        => $$"""{"sensorId":"{{SensorId}}","tipo":"{{Tipo}}","valor":{{Valor.Valor:0.##}},"unidade":"{{Valor.Unidade}}","ativoId":"{{AtivoId}}","timestamp":"{{TimestampUtc:O}}"}""";
}
