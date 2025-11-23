namespace starwars.Models;

public class Spaceship
{
    public double X { get; private set; }
    public double Y { get; private set; }
    public double Angle { get; private set; }
    private DateTime _lastFire;
    private const double FireCooldown = 0.25;

    public Spaceship(double x, double y)
    {
        X = x;
        Y = y;
        Angle = 0;
        _lastFire = DateTime.Now;
    }

    public void RotateLeft()
    {
        Angle -= Math.PI / 8;
    }

    public void RotateRight()
    {
        Angle += Math.PI / 8;
    }

    public void MoveForward()
    {
        X += Math.Cos(Angle) * 2.0;
        Y += Math.Sin(Angle) * 2.0;
    }

    public void MoveBackward()
    {
        X -= Math.Cos(Angle) * 1.5;
        Y -= Math.Sin(Angle) * 1.5;
    }

    public void Update(int maxWidth, int maxHeight)
    {
        X = Math.Clamp(X, 3, maxWidth - 4);
        Y = Math.Clamp(Y, 2, maxHeight - 3);
    }

    public Laser? Fire()
    {
        if ((DateTime.Now - _lastFire).TotalSeconds < FireCooldown)
        {
            return null;
        }

        _lastFire = DateTime.Now;
        return new Laser(X, Y, Angle);
    }

    public void Draw()
    {
        int cx = (int)Math.Round(X);
        int cy = (int)Math.Round(Y);

        // Draw X-Wing with engine trail for better visibility
        Console.SetCursorPosition(cx - 2, cy);
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write("~");

        Console.SetCursorPosition(cx - 1, cy);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("╬");

        Console.SetCursorPosition(cx, cy);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("◆");

        Console.SetCursorPosition(cx + 1, cy);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("╬");

        // Direction indicator (clearer)
        int dx = (int)Math.Round(X + Math.Cos(Angle) * 3);
        int dy = (int)Math.Round(Y + Math.Sin(Angle) * 3);
        if (dx >= 1 && dy >= 1 && dx < 120 && dy < 35)
        {
            Console.SetCursorPosition(dx, dy);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("►");
        }

        Console.ResetColor();
    }
}
