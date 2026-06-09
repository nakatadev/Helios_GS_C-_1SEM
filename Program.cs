using Helios.Contracts;
using Helios.Domain.Alerts;
using Helios.Domain.Assets;
using Helios.Domain.Maintenance;
using Helios.Domain.Robotics;
using Helios.Domain.Sensors;
using Helios.Domain.Telemetry;
using Helios.Repositories;
using Helios.Services;

namespace Helios;

public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("HELIOS - Cerebro de regras para limpeza autonoma de paineis solares lunares");
        Console.WriteLine("Global Solution 2026.1 - C#/.NET\n");

        var painel = new PainelSolar("PAINEL-A", "Painel Solar A", new Coordenada(12, 4, 0), 1200);
        var robo = new Robo("HELIOS-01", "Rover HELIOS", new Coordenada(0, 0, 0));

        var leituras = new RepositorioEmMemoria<Leitura>();
        var alertas = new RepositorioEmMemoria<Alerta>();
        var comandos = new RepositorioEmMemoria<Comando>();
        var tarefas = new RepositorioEmMemoria<TarefaManutencao>();
        INotificador notificador = new ConsoleNotificador();

        var motor = new HeliosRuleEngine(leituras, alertas, comandos, tarefas, notificador);

        var sensores = new List<ISensor>
        {
            new SensorCorrente("PANEL-A-CURRENT", painel.Id, new[] { 4.2, 2.7, 4.0 }),
            new SensorPoeira("PANEL-A-DUST", painel.Id, new[] { 18.0, 47.5, 9.0 }),
            new SensorTemperatura("PANEL-A-TEMP", painel.Id, new[] { -35.0, -32.0, -34.0 }),
            new SensorLuz("PANEL-A-LIGHT", painel.Id, new[] { 98000.0, 76000.0, 97000.0 })
        };

        var inicio = new DateTime(2026, 6, 1, 14, 3, 0, DateTimeKind.Utc);
        for (var ciclo = 0; ciclo < 3; ciclo++)
        {
            Console.WriteLine($"\n--- Ciclo {ciclo + 1} ---");
            motor.ProcessarCiclo(painel, robo, sensores, inicio.AddMinutes(ciclo * 5));
        }

        Console.WriteLine("\n--- Teste de excecao controlada: sensor offline ---");
        var sensorOffline = new SensorPoeira("PANEL-A-DUST-BACKUP", painel.Id, new[] { 22.0 });
        sensorOffline.DefinirOnline(false);
        motor.ProcessarCiclo(painel, robo, new[] { sensorOffline }, inicio.AddMinutes(20));

        ExibirResumo(painel, robo, leituras, alertas, comandos, tarefas);
    }

    private static void ExibirResumo(
        PainelSolar painel,
        Robo robo,
        IRepositorio<Leitura> leituras,
        IRepositorio<Alerta> alertas,
        IRepositorio<Comando> comandos,
        IRepositorio<TarefaManutencao> tarefas)
    {
        Console.WriteLine("\n=== Resumo da execucao ===");
        Console.WriteLine(painel.Descrever());
        Console.WriteLine($"Robo: {robo.Nome} | Status: {robo.Status} | Bateria: {robo.BateriaPercentual:0.0}% | Posicao: {robo.Localizacao}");
        Console.WriteLine($"Leituras registradas: {leituras.ListarTodos().Count}");
        Console.WriteLine($"Alertas registrados: {alertas.ListarTodos().Count}");
        Console.WriteLine($"Comandos enviados: {comandos.ListarTodos().Count}");
        Console.WriteLine($"Tarefas de manutencao: {tarefas.ListarTodos().Count}");
        Console.WriteLine("\nHistorico de alertas:");

        foreach (var alerta in alertas.ListarTodos())
        {
            Console.WriteLine($"- {alerta.TimestampUtc:HH:mm:ss} | {alerta.Severidade} | {alerta.Tipo} | resolvido={alerta.Resolvido}");
        }
    }
}
