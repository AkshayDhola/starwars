using starwars.Models;
using starwars.Design;

namespace starwars;

public class Space
{
    private readonly int Width;
    private readonly int Height;
    private const int RadarSize = 15;
    private readonly int GameAreaWidth;

    private Spaceship _spaceship;
    private List<Enemy> _enemies;
    private List<Laser> _lasers;
    private List<EnemyLaser> _enemyLasers;
    private Random _random;
    private int _score;
    private int _kills;
    private int _health;
    private bool _gameOver;
    private DateTime _lastEnemySpawn;
    private List<Explosion> _explosions;
    private List<Star> _stars = new List<Star>();

    public Space()
    {
        Width = Console.WindowWidth;
        Height = Console.WindowHeight - 3;
        GameAreaWidth = Width - RadarSize - 3;

        _spaceship = new Spaceship(GameAreaWidth / 2, Height / 2);
        _enemies = new List<Enemy>();
        _lasers = new List<Laser>();
        _enemyLasers = new List<EnemyLaser>();
        _explosions = new List<Explosion>();
        _random = new Random();
        _score = 0;
        _kills = 0;
        _health = 100;
        _gameOver = false;
        _lastEnemySpawn = DateTime.Now;
        InitializeStars();
    }

    public async Task RunAsync(CancellationToken token)
    {
        Console.CursorVisible = false;
        Console.Clear();

        // Cross-platform compatible window setup
        try
        {
            if (OperatingSystem.IsWindows())
            {
                Console.SetWindowSize(Math.Min(Width, Console.LargestWindowWidth),
                                     Math.Min(Height + 5, Console.LargestWindowHeight));
            }
        }
        catch
        {
            // If resizing fails, continue anyway
        }

        ShowTitleScreen();
        await Task.Delay(3000);
        Console.Clear();

        DrawBorder();
        ShowInstructions();

        var inputTask = Task.Run(() => HandleInput(token), token);
        var gameTask = Task.Run(() => GameLoop(token), token);

        try
        {
            await Task.WhenAll(inputTask, gameTask);
        }
        catch (OperationCanceledException)
        {
            // Normal shutdown
        }
        finally
        {
            ShowGameOver();
        }
    }

    private void ShowTitleScreen()
    {
        Console.Clear();

        string titleArt = AsciiArtLoader.LoadTitleScreen();
        string[] lines = titleArt.Split('\n');

        int terminalWidth = Console.WindowWidth;
        int terminalHeight = Console.WindowHeight;
        int startY = Math.Max(0, (terminalHeight - lines.Length) / 2 - 5);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            int startX = Math.Max(0, (terminalWidth - line.Length) / 2);

            Console.SetCursorPosition(startX, startY + i);

            if (line.Contains("BATTLE OF THE REBELS") || line.Contains("BATTLE OF YAVIN"))
                Console.ForegroundColor = ConsoleColor.Red;
            else if (line.Contains("TIE Fighters") || line.Contains("Defend") || line.Contains("REBEL"))
                Console.ForegroundColor = ConsoleColor.Cyan;
            else if (line.Contains("May the Force") || line.Contains("long time ago") || line.Contains("Command Center"))
                Console.ForegroundColor = ConsoleColor.DarkGray;
            else if (line.Contains("STAR WARS") || line.Contains("███") || line.Contains("READY") || line.Contains("ARMED"))
                Console.ForegroundColor = ConsoleColor.Yellow;
            else if (line.Contains("X-WING") || line.Contains("SHIELDS") || line.Contains("LASERS"))
                Console.ForegroundColor = ConsoleColor.Cyan;
            else
                Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write(line);
        }

        Console.ResetColor();
    }

    private void ShowGameOver()
    {
        Console.Clear();

        string gameOverArt = _health > 0 ? AsciiArtLoader.LoadVictoryScreen() : AsciiArtLoader.LoadGameOverScreen();
        string[] lines = gameOverArt.Split('\n');

        int terminalWidth = Console.WindowWidth;
        int terminalHeight = Console.WindowHeight;
        int startY = Math.Max(0, (terminalHeight - lines.Length - 8) / 2);

        Console.ForegroundColor = _health > 0 ? ConsoleColor.Yellow : ConsoleColor.Red;

        // Display ASCII art centered
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            int startX = Math.Max(0, (terminalWidth - line.Length) / 2);
            Console.SetCursorPosition(startX, startY + i);
            Console.Write(line);
        }

        // Display score and stats centered
        int statsY = startY + lines.Length + 2;

        Console.ForegroundColor = ConsoleColor.White;
        string scoreLine = $"Final Score: {_score}";
        int scoreX = Math.Max(0, (terminalWidth - scoreLine.Length) / 2);
        Console.SetCursorPosition(scoreX, statsY);
        Console.Write(scoreLine);

        string killsLine = $"TIE Fighters Destroyed: {_kills}";
        int killsX = Math.Max(0, (terminalWidth - killsLine.Length) / 2);
        Console.SetCursorPosition(killsX, statsY + 1);
        Console.Write(killsLine);

        Console.ForegroundColor = ConsoleColor.DarkGray;
        string forceLine = "The Force will be with you, always.";
        int forceX = Math.Max(0, (terminalWidth - forceLine.Length) / 2);
        Console.SetCursorPosition(forceX, statsY + 3);
        Console.Write(forceLine);

        Console.ResetColor();
    }

    private void ShowInstructions()
    {
        Console.SetCursorPosition(2, Height + 2);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("Controls: ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("W/S=Move  A/D=Rotate  SPACE=Fire Laser  ESC=Retreat");
        Console.ResetColor();
    }

    private async Task GameLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested && !_gameOver)
        {
            Update();
            Render();
            await Task.Delay(50, token);
        }
    }

    private void HandleInput(CancellationToken token)
    {
        while (!token.IsCancellationRequested && !_gameOver)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.W:
                        _spaceship.MoveForward();
                        break;
                    case ConsoleKey.S:
                        _spaceship.MoveBackward();
                        break;
                    case ConsoleKey.A:
                        _spaceship.RotateLeft();
                        break;
                    case ConsoleKey.D:
                        _spaceship.RotateRight();
                        break;
                    case ConsoleKey.Spacebar:
                        var laser = _spaceship.Fire();
                        if (laser != null)
                            _lasers.Add(laser);
                        break;
                    case ConsoleKey.Escape:
                        _gameOver = true;
                        return;
                }
            }
            Thread.Sleep(16);
        }
    }

    private void Update()
    {
        // Spawn TIE fighters
        var spawnDelay = Math.Max(800, 2000 - (_score / 10) * 100);
        if ((DateTime.Now - _lastEnemySpawn).TotalMilliseconds > spawnDelay)
        {
            SpawnEnemy();
            _lastEnemySpawn = DateTime.Now;
        }

        // Update spaceship
        _spaceship.Update(GameAreaWidth, Height);

        // Update explosions
        for (int i = _explosions.Count - 1; i >= 0; i--)
        {
            _explosions[i].Update();
            if (_explosions[i].IsFinished())
            {
                _explosions.RemoveAt(i);
            }
        }

        // Update lasers
        for (int i = _lasers.Count - 1; i >= 0; i--)
        {
            _lasers[i].Update();
            if (_lasers[i].IsOutOfBounds(GameAreaWidth, Height))
            {
                _lasers.RemoveAt(i);
            }
        }

        // Update enemy lasers
        for (int i = _enemyLasers.Count - 1; i >= 0; i--)
        {
            _enemyLasers[i].Update();
            if (_enemyLasers[i].IsOutOfBounds(GameAreaWidth, Height))
            {
                _enemyLasers.RemoveAt(i);
            }
            else if (_enemyLasers[i].CollidesWith(_spaceship.X, _spaceship.Y, 2))
            {
                _health -= 10;
                _enemyLasers.RemoveAt(i);
                if (_health <= 0)
                {
                    _explosions.Add(new Explosion(_spaceship.X, _spaceship.Y));
                    _gameOver = true;
                    return;
                }
            }
        }

        // Update enemies
        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            _enemies[i].Update(_spaceship.X, _spaceship.Y);

            // Enemy firing
            var enemyLaser = _enemies[i].TryFire(_spaceship.X, _spaceship.Y);
            if (enemyLaser != null)
            {
                _enemyLasers.Add(enemyLaser);
            }

            // Check collision with spaceship
            if (_enemies[i].CollidesWith(_spaceship.X, _spaceship.Y, 3))
            {
                _health -= 20;
                _explosions.Add(new Explosion(_enemies[i].X, _enemies[i].Y));
                _enemies.RemoveAt(i);
                if (_health <= 0)
                {
                    _explosions.Add(new Explosion(_spaceship.X, _spaceship.Y));
                    _gameOver = true;
                    return;
                }
                continue;
            }

            // Check if out of bounds
            if (_enemies[i].IsOutOfBounds(GameAreaWidth, Height))
            {
                _enemies.RemoveAt(i);
            }
        }

        // Check laser-enemy collisions
        for (int i = _lasers.Count - 1; i >= 0; i--)
        {
            for (int j = _enemies.Count - 1; j >= 0; j--)
            {
                if (_enemies[j].CollidesWith(_lasers[i].X, _lasers[i].Y, 2))
                {
                    bool destroyed = _enemies[j].TakeDamage();
                    _lasers.RemoveAt(i);

                    if (destroyed)
                    {
                        _explosions.Add(new Explosion(_enemies[j].X, _enemies[j].Y));
                        _enemies.RemoveAt(j);
                        _score += 50;
                        _kills++;
                    }
                    break;
                }
            }
        }
    }

    private void SpawnEnemy()
    {
        int edge = _random.Next(4);
        double x, y;
        var enemyType = (EnemyType)_random.Next(3);

        switch (edge)
        {
            case 0: // Top
                x = _random.Next(5, GameAreaWidth - 5);
                y = 2;
                break;
            case 1: // Right
                x = GameAreaWidth - 3;
                y = _random.Next(5, Height - 5);
                break;
            case 2: // Bottom
                x = _random.Next(5, GameAreaWidth - 5);
                y = Height - 3;
                break;
            default: // Left
                x = 2;
                y = _random.Next(5, Height - 5);
                break;
        }

        _enemies.Add(new Enemy(x, y, enemyType));
    }

    private void Render()
    {
        // Clear previous frame areas
        ClearPreviousFrame();

        // Draw stars background
        DrawStars();

        // Draw explosions
        foreach (var explosion in _explosions)
        {
            explosion.Draw();
        }

        // Draw spaceship
        if (_health > 0)
        {
            _spaceship.Draw();
        }

        // Draw lasers
        foreach (var laser in _lasers)
        {
            laser.Draw();
        }

        // Draw enemy lasers
        foreach (var laser in _enemyLasers)
        {
            laser.Draw();
        }

        // Draw enemies
        foreach (var enemy in _enemies)
        {
            enemy.Draw();
        }

        // Draw radar
        DrawRadar();

        // Draw stats
        DrawStats();
    }

    private void ClearPreviousFrame()
    {
        // Clear play area (not borders)
        Console.ForegroundColor = ConsoleColor.Black;
        for (int y = 1; y < Height - 1; y++)
        {
            Console.SetCursorPosition(1, y);
            Console.Write(new string(' ', GameAreaWidth - 2));
        }
        Console.ResetColor();
    }

    // private List<Star> _stars = new List<Star>();

    private void InitializeStars()
    {
        for (int i = 0; i < 50; i++)
        {
            _stars.Add(new Star(_random.Next(1, GameAreaWidth - 1),
                               _random.Next(1, Height - 1),
                               _random.Next(3)));
        }
    }

    private void DrawStars()
    {
        if (_stars.Count == 0) InitializeStars();

        foreach (var star in _stars)
        {
            Console.SetCursorPosition((int)star.X, (int)star.Y);
            Console.ForegroundColor = star.Brightness switch
            {
                0 => ConsoleColor.DarkGray,
                1 => ConsoleColor.Gray,
                _ => ConsoleColor.White
            };
            Console.Write(star.Symbol);
        }
    }

    private void DrawBorder()
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;

        // Game area border
        for (int x = 0; x < GameAreaWidth; x++)
        {
            Console.SetCursorPosition(x, 0);
            Console.Write("═");
            Console.SetCursorPosition(x, Height - 1);
            Console.Write("═");
        }
        for (int y = 0; y < Height; y++)
        {
            Console.SetCursorPosition(0, y);
            Console.Write("║");
            Console.SetCursorPosition(GameAreaWidth - 1, y);
            Console.Write("║");
        }

        // Radar border
        int radarX = GameAreaWidth + 1;
        for (int x = radarX; x < Width - 1; x++)
        {
            Console.SetCursorPosition(x, 0);
            Console.Write("═");
            Console.SetCursorPosition(x, Height - 1);
            Console.Write("═");
        }
        for (int y = 0; y < Height; y++)
        {
            Console.SetCursorPosition(Width - 1, y);
            Console.Write("║");
        }

        Console.ResetColor();
    }

    private void DrawRadar()
    {
        int radarX = GameAreaWidth + 2;
        int radarY = 2;

        Console.SetCursorPosition(radarX, radarY - 1);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("◄ RADAR ►");

        // Clear radar area
        for (int y = 0; y < RadarSize - 3; y++)
        {
            Console.SetCursorPosition(radarX, radarY + y);
            Console.Write(new string(' ', RadarSize - 2));
        }

        // Draw radar grid
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        for (int i = 0; i < RadarSize - 3; i += 3)
        {
            for (int j = 0; j < RadarSize - 2; j += 3)
            {
                Console.SetCursorPosition(radarX + j, radarY + i);
                Console.Write("+");
            }
        }

        // Draw spaceship on radar (center)
        int centerX = radarX + (RadarSize - 2) / 2;
        int centerY = radarY + (RadarSize - 3) / 2;
        Console.SetCursorPosition(centerX, centerY);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("◆");

        // Draw enemies on radar
        Console.ForegroundColor = ConsoleColor.Red;
        foreach (var enemy in _enemies)
        {
            int mx = (int)((enemy.X / GameAreaWidth) * (RadarSize - 2));
            int my = (int)((enemy.Y / Height) * (RadarSize - 3));
            if (mx >= 0 && mx < RadarSize - 2 && my >= 0 && my < RadarSize - 3)
            {
                Console.SetCursorPosition(radarX + mx, radarY + my);
                Console.Write("▲");
            }
        }

        Console.ResetColor();
    }

    private void DrawStats()
    {
        int radarX = GameAreaWidth + 2;
        int statsY = Height - 15;

        // Health bar
        Console.SetCursorPosition(radarX, statsY);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("SHIELDS:");
        Console.SetCursorPosition(radarX, statsY + 1);
        Console.ForegroundColor = _health > 50 ? ConsoleColor.Green : _health > 25 ? ConsoleColor.Yellow : ConsoleColor.Red;
        int healthBars = Math.Max(0, Math.Min(10, _health / 10)); // Clamp between 0-10
        int emptyBars = Math.Max(0, 10 - healthBars);
        Console.Write("[" + new string('█', healthBars) + new string('░', emptyBars) + "]");

        // Score
        Console.SetCursorPosition(radarX, statsY + 3);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("SCORE:");
        Console.SetCursorPosition(radarX, statsY + 4);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"{_score,6}");

        // Kills
        Console.SetCursorPosition(radarX, statsY + 6);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("KILLS:");
        Console.SetCursorPosition(radarX, statsY + 7);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"{_kills,6}");

        // Enemies
        Console.SetCursorPosition(radarX, statsY + 9);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("HOSTILE:");
        Console.SetCursorPosition(radarX, statsY + 10);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"{_enemies.Count,6}");

        // Wave info
        Console.SetCursorPosition(radarX, statsY + 12);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("WAVE:");
        Console.SetCursorPosition(radarX, statsY + 13);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"{(_kills / 10) + 1,6}");

        Console.ResetColor();
    }
}
