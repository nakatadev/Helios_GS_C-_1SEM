namespace Helios.Domain.Maintenance;

public class TarefaManutencao
{
    public TarefaManutencao(string id, string ativoId, string descricao, DateTime criadaEmUtc)
    {
        Id = id;
        AtivoId = ativoId;
        Descricao = descricao;
        CriadaEmUtc = criadaEmUtc;
        Status = StatusTarefa.Aberta;
    }

    public string Id { get; }
    public string AtivoId { get; }
    public string Descricao { get; }
    public DateTime CriadaEmUtc { get; }
    public DateTime? ConcluidaEmUtc { get; private set; }
    public StatusTarefa Status { get; private set; }

    public void Iniciar() => Status = StatusTarefa.EmExecucao;

    public void Concluir(DateTime instanteUtc)
    {
        Status = StatusTarefa.Concluida;
        ConcluidaEmUtc = instanteUtc;
    }
}
