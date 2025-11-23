namespace starwars.Design;

public static class AsciiArtLoader
{
    private const string ContentFolder = "Content";

    public static string LoadTitleScreen()
    {
        try
        {
            return File.ReadAllText(Path.Combine(ContentFolder, "title.txt"));
        }
        catch
        {
            return GetDefaultTitleScreen();
        }
    }

    public static string LoadVictoryScreen()
    {
        try
        {
            return File.ReadAllText(Path.Combine(ContentFolder, "victory.txt"));
        }
        catch
        {
            return GetDefaultVictoryScreen();
        }
    }

    public static string LoadGameOverScreen()
    {
        try
        {
            return File.ReadAllText(Path.Combine(ContentFolder, "gameover.txt"));
        }
        catch
        {
            return GetDefaultGameOverScreen();
        }
    }

    private static string GetDefaultTitleScreen()
    {
        return @"

    ███████╗████████╗ █████╗ ██████╗     ██╗    ██╗ █████╗ ██████╗ ███████╗
    ██╔════╝╚══██╔══╝██╔══██╗██╔══██╗    ██║    ██║██╔══██╗██╔══██╗██╔════╝
    ███████╗   ██║   ███████║██████╔╝    ██║ █╗ ██║███████║██████╔╝███████╗
    ╚════██║   ██║   ██╔══██║██╔══██╗    ██║███╗██║██╔══██║██╔══██╗╚════██║
    ███████║   ██║   ██║  ██║██║  ██║    ╚███╔███╔╝██║  ██║██║  ██║███████║
    ╚══════╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝     ╚══╝╚══╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝

                          BATTLE OF THE REBELS

                    TIE Fighters are attacking the Rebel base!
                      Defend your position, Commander!

                              May the Force be with you...";
    }

    private static string GetDefaultVictoryScreen()
    {
        return @"

               ███╗   ███╗██╗███████╗███████╗██╗ ██████╗ ███╗   ██╗
               ████╗ ████║██║██╔════╝██╔════╝██║██╔═══██╗████╗  ██║
               ██╔████╔██║██║███████╗███████╗██║██║   ██║██╔██╗ ██║
               ██║╚██╔╝██║██║╚════██║╚════██║██║██║   ██║██║╚██╗██║
               ██║ ╚═╝ ██║██║███████║███████║██║╚██████╔╝██║ ╚████║
               ╚═╝     ╚═╝╚═╝╚══════╝╚══════╝╚═╝ ╚═════╝ ╚═╝  ╚═══╝

                          ███████╗███╗   ██╗██████╗
                          ██╔════╝████╗  ██║██╔══██╗
                          █████╗  ██╔██╗ ██║██║  ██║
                          ██╔══╝  ██║╚██╗██║██║  ██║
                          ███████╗██║ ╚████║██████╔╝
                          ╚══════╝╚═╝  ╚═══╝╚═════╝";
    }

    private static string GetDefaultGameOverScreen()
    {
        return @"


                  ██████╗   █████╗ ███╗   ███╗███████╗     ██████╗ ██╗   ██╗███████╗██████╗
                 ██╔════╝ ██╔══██╗████╗ ████║██╔════╝    ██╔═══██╗██║   ██║██╔════╝██╔══██╗
                 ██║  ███╗███████║██╔████╔██║█████╗      ██║   ██║██║   ██║█████╗  ██████╔╝
                 ██║   ██║██╔══██║██║╚██╔╝██║██╔══╝      ██║   ██║╚██╗ ██╔╝██╔══╝  ██╔══██╗
                 ╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗    ╚██████╔╝ ╚████╔╝ ███████╗██║  ██║
                  ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝     ╚═════╝   ╚═══╝  ╚══════╝╚═╝  ╚═╝";
    }
}
