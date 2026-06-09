using Helios.Domain.Telemetry;

namespace Helios.Contracts;

public interface ISensor
{
    string Id { get; }
    string Tipo { get; }
    string Unidade { get; }
    string AtivoId { get; }
    bool EstaOnline { get; }

    Leitura ColetarLeitura(DateTime instanteUtc);
}
