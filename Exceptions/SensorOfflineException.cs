namespace Helios.Exceptions;

public class SensorOfflineException : Exception
{
    public SensorOfflineException(string sensorId)
        : base($"Sensor {sensorId} offline. A leitura foi ignorada para manter o sistema estavel.")
    {
        SensorId = sensorId;
    }

    public string SensorId { get; }
}
