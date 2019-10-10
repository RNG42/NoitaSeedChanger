using System;

internal class Banner
{
    public static void Draw()
    {
        ConsoleColor[] gradient = new ConsoleColor[5]
        {
            ConsoleColor.Cyan,
            ConsoleColor.DarkCyan,
            ConsoleColor.Blue,
            ConsoleColor.DarkBlue,
            ConsoleColor.DarkBlue
        };
        string[] banner = new string[5]
        {
            "     _______         __ __              _______                 __      ______ __                                 ",
            "    |    |  |.-----.|__|  |_.---.-.    |     __|.-----.-----.--|  |    |      |  |--.---.-.-----.-----.-----.----.",
            "    |       ||  _  ||  |   _|  _  |    |__     ||  -__|  -__|  _  |    |   ---|     |  _  |     |  _  |  -__|   _|",
            "    |__|____||_____||__|____|___._|    |_______||_____|_____|_____|    |______|__|__|___._|__|__|___  |_____|__|  ",
            "                                                                                                |_____|           "
        };
        for (int i = 0; i < gradient.Length; i++)
        {
            Console.ForegroundColor = gradient[i];
            Console.WriteLine(banner[i]);
        }
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write(Environment.NewLine);
        Console.Write(Environment.NewLine);
    }
}