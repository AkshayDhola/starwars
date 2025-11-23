namespace starwars;

class Program
{
    static async Task Main(string[] args)
    {
        var game = new Space();
        var cts = new CancellationTokenSource();

        Console.CancelKeyPress += (s, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        await game.RunAsync(cts.Token);
    }
}
