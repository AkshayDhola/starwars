namespace starwars.Models;

public class Laser
{
    public double X { get; private set; }
    public double Y { get; private set; }
    private double _dx;
    private double _dy;
    private const double Speed = 3.5;

    public Laser(double x, double y, double angle)
    {
        X = x;
        Y = y;
        _dx = Math.Cos(angle) * Speed;
        _dy = Math.Sin(angle) * Speed;
    }

    public void Update()
    {
        X += _dx;
        Y += _dy;
    }

    public bool IsOutOfBounds(int maxWidth, int maxHeight)
    {
        return X < 1 || X >= maxWidth - 1 || Y < 1 || Y >= maxHeight - 1;
    }

    public void Draw()
    {
        int cx = (int)Math.Round(X);
        int cy = (int)Math.Round(Y);

        if (cx >= 1 && cy >= 1 && cx < 120 && cy < 35)
        {
            Console.SetCursorPosition(cx, cy);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("━");
            Console.ResetColor();
        }
    }
}
