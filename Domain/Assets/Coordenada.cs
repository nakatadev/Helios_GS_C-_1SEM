namespace Helios.Domain.Assets;

public readonly struct Coordenada
{
    public Coordenada(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public double X { get; }
    public double Y { get; }
    public double Z { get; }

    public double DistanciaAte(Coordenada destino)
    {
        var dx = destino.X - X;
        var dy = destino.Y - Y;
        var dz = destino.Z - Z;
        return Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }

    public override string ToString() => $"({X:0.0}, {Y:0.0}, {Z:0.0})";
}
