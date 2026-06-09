using Helios.Exceptions;

namespace Helios.Domain.Sensors;

public class SensorCorrente : Sensor
{
    public SensorCorrente(string id, string ativoId, IEnumerable<double> amostras)
        : base(id, "corrente", "A", ativoId, amostras)
    {
    }

    protected override void Validar(double valor)
    {
        base.Validar(valor);
        if (valor < 0)
        {
            throw new LeituraInvalidaException($"Sensor {Id} retornou corrente negativa.");
        }
    }

    protected override double ValorPadraoQuandoSemAmostra() => 3.8;
}
