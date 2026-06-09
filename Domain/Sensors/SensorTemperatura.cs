using Helios.Exceptions;

namespace Helios.Domain.Sensors;

public class SensorTemperatura : Sensor
{
    public SensorTemperatura(string id, string ativoId, IEnumerable<double> amostras)
        : base(id, "temperatura", "C", ativoId, amostras)
    {
    }

    protected override void Validar(double valor)
    {
        base.Validar(valor);
        if (valor < -180 || valor > 140)
        {
            throw new LeituraInvalidaException($"Sensor {Id} retornou temperatura fora do esperado para a operacao lunar.");
        }
    }

    protected override double ValorPadraoQuandoSemAmostra() => -20.0;
}
