using Helios.Exceptions;

namespace Helios.Domain.Sensors;

public class SensorPoeira : Sensor
{
    public SensorPoeira(string id, string ativoId, IEnumerable<double> amostras)
        : base(id, "poeira", "%", ativoId, amostras)
    {
    }

    protected override void Validar(double valor)
    {
        base.Validar(valor);
        if (valor < 0 || valor > 100)
        {
            throw new LeituraInvalidaException($"Sensor {Id} retornou poeira fora da faixa 0-100%.");
        }
    }

    protected override double ValorPadraoQuandoSemAmostra() => 12.0;
}
