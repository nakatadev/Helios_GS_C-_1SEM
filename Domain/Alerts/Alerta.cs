namespace Helios.Domain.Alerts;

public class Alerta
{
    public Alerta(string alertaId, string ativoId, Severidade severidade, TipoAlerta tipo, string mensagem, DateTime timestampUtc)
    {
        AlertaId = alertaId;
        AtivoId = ativoId;
        Severidade = severidade;
        Tipo = tipo;
        Mensagem = mensagem;
        TimestampUtc = timestampUtc;
    }

    public string AlertaId { get; }
    public string AtivoId { get; }
    public Severidade Severidade { get; }
    public TipoAlerta Tipo { get; }
    public string Mensagem { get; }
    public DateTime TimestampUtc { get; }
    public bool Resolvido { get; private set; }

    public void Resolver() => Resolvido = true;

    public string ToContractJson()
        => $$"""{"alertaId":"{{AlertaId}}","ativoId":"{{AtivoId}}","severidade":"{{Severidade.ToString().ToUpperInvariant()}}","tipo":"{{TipoParaContrato(Tipo)}}","mensagem":"{{Mensagem}}","timestamp":"{{TimestampUtc:O}}","resolvido":{{Resolvido.ToString().ToLowerInvariant()}}}""";

    private static string TipoParaContrato(TipoAlerta tipo)
        => tipo switch
        {
            TipoAlerta.SujeiraDetectada => "SUJEIRA_DETECTADA",
            TipoAlerta.BaixaGeracao => "BAIXA_GERACAO",
            TipoAlerta.SensorOffline => "SENSOR_OFFLINE",
            TipoAlerta.LeituraInvalida => "LEITURA_INVALIDA",
            _ => tipo.ToString().ToUpperInvariant()
        };
}
