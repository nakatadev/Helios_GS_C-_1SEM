using Helios.Exceptions;

namespace Helios.Domain.Sensors;

public class SensorLuz : Sensor
{
    public SensorLuz(string id, string ativoId, IEnumerable<double> amostras)
        : base(id, "luz", "lux", ativoId, amostras)
    {
    }

    protected override void Validar(double valor)
    {
        base.Validar(valor);
        if (valor < 0)
        {
            throw new LeituraInvalidaException($"Sensor {Id} retornou luz negativa.");
        }
    }

    protected override double ValorPadraoQuandoSemAmostra() => 95000.0;
}
