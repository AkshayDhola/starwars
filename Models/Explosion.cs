namespace starwars.Models;

public class Explosion
{
    private double _x;
    private double _y;
    private int _frame;
    private DateTime _startTime;
    private static readonly string[] Frames = { "░", "▒", "▓", "█", "▓", "▒", "░" };

    public Explosion(double x, double y)
    {
        _x = x;
        _y = y;
        _frame = 0;
        _startTime = DateTime.Now;
    }

    public void Update()
    {
        var elapsed = (DateTime.Now - _startTime).TotalMilliseconds;
        _frame = (int)(elapsed / 100);
    }

    public bool IsFinished()
    {
        return _frame >= Frames.Length;
    }

    public void Draw()
    {
        if (_frame >= Frames.Length) return;

        int cx = (int)Math.Round(_x);
        int cy = (int)Math.Round(_y);

        Console.SetCursorPosition(cx, cy);
        Console.ForegroundColor = _frame < 3 ? ConsoleColor.Yellow : ConsoleColor.Red;
        Console.Write(Frames[_frame]);

        // Draw explosion particles
        if (_frame < 4)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    Console.SetCursorPosition(cx + i, cy + j);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("*");
                }
            }
        }

        Console.ResetColor();
    }
}
