using Helios.Contracts;
using Helios.Domain.Alerts;
using Helios.Domain.Assets;
using Helios.Domain.Maintenance;
using Helios.Domain.Robotics;
using Helios.Domain.Telemetry;
using Helios.Exceptions;

namespace Helios.Services;

public class HeliosRuleEngine
{
    private const double LimitePoeiraAlerta = 40.0;
    private const double LimiteCorrenteBaixa = 3.0;

    private readonly IRepositorio<Leitura> _leituras;
    private readonly IRepositorio<Alerta> _alertas;
    private readonly IRepositorio<Comando> _comandos;
    private readonly IRepositorio<TarefaManutencao> _tarefas;
    private readonly INotificador _notificador;

    public HeliosRuleEngine(
        IRepositorio<Leitura> leituras,
        IRepositorio<Alerta> alertas,
        IRepositorio<Comando> comandos,
        IRepositorio<TarefaManutencao> tarefas,
        INotificador notificador)
    {
        _leituras = leituras;
        _alertas = alertas;
        _comandos = comandos;
        _tarefas = tarefas;
        _notificador = notificador;
    }

    public void ProcessarCiclo(PainelSolar painel, Robo robo, IEnumerable<ISensor> sensores, DateTime instanteUtc)
    {
        _notificador.Informar($"Ciclo iniciado em {instanteUtc:yyyy-MM-dd HH:mm:ss} UTC");

        foreach (var sensor in sensores)
        {
            try
            {
                var leitura = sensor.ColetarLeitura(instanteUtc);
                _leituras.Adicionar(leitura);
                Console.WriteLine($"Leitura: {leitura.SensorId} => {leitura.Valor} | JSON: {leitura.ToContractJson()}");

                AvaliarLeitura(painel, robo, leitura, instanteUtc);
            }
            catch (SensorOfflineException ex)
            {
                RegistrarAlerta(painel.Id, Severidade.Alta, TipoAlerta.SensorOffline, ex.Message, instanteUtc);
            }
            catch (LeituraInvalidaException ex)
            {
                RegistrarAlerta(painel.Id, Severidade.Media, TipoAlerta.LeituraInvalida, ex.Message, instanteUtc);
            }
        }
    }

    private void AvaliarLeitura(PainelSolar painel, Robo robo, Leitura leitura, DateTime instanteUtc)
    {
        if (leitura.Tipo == "poeira")
        {
            painel.AtualizarEficiencia(leitura.Valor.Valor);

            if (leitura.Valor.Valor >= LimitePoeiraAlerta)
            {
                var alerta = RegistrarAlerta(
                    painel.Id,
                    Severidade.Alta,
                    TipoAlerta.SujeiraDetectada,
                    $"Cobertura de poeira em {leitura.Valor.Valor:0.0}%",
                    instanteUtc);

                var tarefa = new TarefaManutencao($"TASK-{instanteUtc:HHmmss}", painel.Id, "Limpeza autonoma do painel solar", instanteUtc);
                tarefa.Iniciar();
                _tarefas.Adicionar(tarefa);

                var comando = robo.CriarComandoLimpeza(painel, instanteUtc.AddSeconds(1));
                _comandos.Adicionar(comando);
                _notificador.Alertar($"Comando emitido: {comando.ToContractJson()}");

                robo.FinalizarLimpeza(painel);
                alerta.Resolver();
                tarefa.Concluir(instanteUtc.AddSeconds(5));
                _notificador.Informar($"Alerta resolvido automaticamente. Eficiencia atual: {painel.EficienciaPercentual:0.0}%");
            }
        }

        if (leitura.Tipo == "corrente" && leitura.Valor.Valor < LimiteCorrenteBaixa)
        {
            RegistrarAlerta(
                painel.Id,
                Severidade.Media,
                TipoAlerta.BaixaGeracao,
                $"Corrente abaixo do esperado: {leitura.Valor}",
                instanteUtc);
        }
    }

    private Alerta RegistrarAlerta(string ativoId, Severidade severidade, TipoAlerta tipo, string mensagem, DateTime instanteUtc)
    {
        var alerta = new Alerta($"ALT-{_alertas.ListarTodos().Count + 1:000}", ativoId, severidade, tipo, mensagem, instanteUtc);
        _alertas.Adicionar(alerta);
        _notificador.Alertar($"{alerta.Tipo} | {alerta.Mensagem} | JSON: {alerta.ToContractJson()}");
        return alerta;
    }
}
