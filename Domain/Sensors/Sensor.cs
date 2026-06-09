using Helios.Contracts;
using Helios.Domain.Telemetry;
using Helios.Exceptions;

namespace Helios.Domain.Sensors;

public abstract class Sensor : ISensor
{
    private readonly Queue<double> _amostras;

    protected Sensor(string id, string tipo, string unidade, string ativoId, IEnumerable<double> amostras)
    {
        Id = id;
        Tipo = tipo;
        Unidade = unidade;
        AtivoId = ativoId;
        EstaOnline = true;
        _amostras = new Queue<double>(amostras);
    }

    public string Id { get; }
    public string Tipo { get; }
    public string Unidade { get; }
    public string AtivoId { get; }
    public bool EstaOnline { get; private set; }

    public void DefinirOnline(bool online) => EstaOnline = online;

    public Leitura ColetarLeitura(DateTime instanteUtc)
    {
        if (!EstaOnline)
        {
            throw new SensorOfflineException(Id);
        }

        var valor = ProximaAmostra();
        Validar(valor);
        return new Leitura(Id, Tipo, new LeituraValor(valor, Unidade), AtivoId, instanteUtc);
    }

    protected virtual void Validar(double valor)
    {
        if (double.IsNaN(valor) || double.IsInfinity(valor))
        {
            throw new LeituraInvalidaException($"Sensor {Id} retornou leitura numerica invalida.");
        }
    }

    private double ProximaAmostra()
    {
        if (_amostras.Count == 0)
        {
            return ValorPadraoQuandoSemAmostra();
        }

        var valor = _amostras.Dequeue();
        _amostras.Enqueue(valor);
        return valor;
    }

    protected abstract double ValorPadraoQuandoSemAmostra();
}
