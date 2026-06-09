namespace Helios.Domain.Robotics;

public class Comando
{
    public Comando(string comandoId, string robotId, AcaoComando acao, string alvoAtivoId, DateTime timestampUtc)
    {
        ComandoId = comandoId;
        RobotId = robotId;
        Acao = acao;
        AlvoAtivoId = alvoAtivoId;
        TimestampUtc = timestampUtc;
    }

    public string ComandoId { get; }
    public string RobotId { get; }
    public AcaoComando Acao { get; }
    public string AlvoAtivoId { get; }
    public DateTime TimestampUtc { get; }

    public string ToContractJson()
        => $$"""{"comandoId":"{{ComandoId}}","robotId":"{{RobotId}}","acao":"{{Acao.ToString().ToUpperInvariant()}}","alvoAtivoId":"{{AlvoAtivoId}}","timestamp":"{{TimestampUtc:O}}"}""";
}
