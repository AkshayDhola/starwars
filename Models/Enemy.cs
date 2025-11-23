namespace starwars.Models;

public enum EnemyType
{
    TieFighter,     // Standard
    TieInterceptor, // Fast
    TieBomber       // Slow but tanky
}

public class Enemy
{
    public double X { get; private set; }
    public double Y { get; private set; }
    private DateTime _lastFire;
    private double _fireCooldown;
    private double _speed;
    private Random _random = new Random();
    private EnemyType _type;
    private int _health;

    public Enemy(double x, double y, EnemyType type)
    {
        X = x;
        Y = y;
        _type = type;
        _lastFire = DateTime.Now.AddSeconds(_random.NextDouble() * 2);

        // Set properties based on type
        switch (type)
        {
            case EnemyType.TieInterceptor:
                _speed = 0.8;
                _fireCooldown = 1.5;
                _health = 1;
                break;
            case EnemyType.TieBomber:
                _speed = 0.3;
                _fireCooldown = 3.0;
                _health = 3;
                break;
            default: // TieFighter
                _speed = 0.5;
                _fireCooldown = 2.0;
                _health = 2;
                break;
        }
    }

    public void Update(double targetX, double targetY)
    {
        double dx = targetX - X;
        double dy = targetY - Y;
        double distance = Math.Sqrt(dx * dx + dy * dy);

        if (distance > 0)
        {
            X += (dx / distance) * _speed;
            Y += (dy / distance) * _speed;
        }
    }

    public EnemyLaser? TryFire(double targetX, double targetY)
    {
        if ((DateTime.Now - _lastFire).TotalSeconds < _fireCooldown)
        {
            return null;
        }

        double distance = Math.Sqrt(Math.Pow(X - targetX, 2) + Math.Pow(Y - targetY, 2));
        if (distance > 30)
        {
            return null;
        }

        _lastFire = DateTime.Now;
        double angle = Math.Atan2(targetY - Y, targetX - X);
        return new EnemyLaser(X, Y, angle);
    }

    public bool TakeDamage()
    {
        _health--;
        return _health <= 0;
    }

    public bool IsOutOfBounds(int maxWidth, int maxHeight)
    {
        return X < 0 || X >= maxWidth || Y < 0 || Y >= maxHeight;
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

        if (cx >= 2 && cy >= 1 && cx < 120 - 2 && cy < 35 - 1)
        {
            switch (_type)
            {
                case EnemyType.TieInterceptor:
                    Console.SetCursorPosition(cx - 1, cy);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("◄");
                    Console.SetCursorPosition(cx, cy);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("♦");
                    Console.SetCursorPosition(cx + 1, cy);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("►");
                    break;

                case EnemyType.TieBomber:
                    Console.SetCursorPosition(cx - 1, cy);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("■");
                    Console.SetCursorPosition(cx, cy);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("▓");
                    Console.SetCursorPosition(cx + 1, cy);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("■");
                    break;

                default: // TieFighter
                    Console.SetCursorPosition(cx - 1, cy);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("▌");
                    Console.SetCursorPosition(cx, cy);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("●");
                    Console.SetCursorPosition(cx + 1, cy);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("▐");
                    break;
            }

            Console.ResetColor();
        }
    }
}
