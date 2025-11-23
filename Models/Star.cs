namespace starwars.Models;

public class Star
{
    public double X { get; set; }
    public double Y { get; set; }
    public int Brightness { get; set; }
    public char Symbol { get; set; }

    public Star(double x, double y, int brightness)
    {
        X = x;
        Y = y;
        Brightness = brightness;
        Symbol = brightness == 2 ? '*' : '·';
    }
}
