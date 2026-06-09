using Helios.Domain.Assets;

namespace Helios.Domain.Robotics;

public partial class Robo
{
    public Comando CriarComandoLimpeza(PainelSolar painel, DateTime instanteUtc)
    {
        Status = $"Limpando {painel.Id}";
        ConsumirBateria(4.5);
        Localizacao = painel.Localizacao;
        return new Comando($"CMD-{instanteUtc:HHmmss}", Id, AcaoComando.Limpar, painel.Id, instanteUtc);
    }

    public void FinalizarLimpeza(PainelSolar painel)
    {
        painel.RegistrarLimpeza();
        Status = $"Limpeza concluida em {painel.Id}";
        ConsumirBateria(1.5);
    }
}
