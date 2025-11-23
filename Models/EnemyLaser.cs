namespace starwars.Models;

public class EnemyLaser
{
    public double X { get; private set; }
    public double Y { get; private set; }
    private double _dx;
    private double _dy;
    private const double Speed = 2.5;

    public EnemyLaser(double x, double y, double angle)
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

    public bool CollidesWith(double x, double y, double radius)
    {
        double distance = Math.Sqrt(Math.Pow(X - x, 2) + Math.Pow(Y - y, 2));
        return distance < radius;
    }

    public void Draw()
    {
        int cx = (int)Math.Round(X);
        int cy = (int)Math.Round(Y);

        if (cx >= 1 && cy >= 1 && cx < 120 && cy < 35)
        {
            Console.SetCursorPosition(cx, cy);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("▬");
            Console.ResetColor();
        }
    }
}
