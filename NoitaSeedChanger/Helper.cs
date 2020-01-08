using System;
using System.Linq;
using System.Text;

namespace NoitaSeedChanger
{
    class Helper
    {
        public static void DrawBanner()
        {
            Console.Clear();
            ConsoleColor[] gradient =
            {
                ConsoleColor.Cyan,
                ConsoleColor.DarkCyan,
                ConsoleColor.Blue,
                ConsoleColor.DarkBlue,
                ConsoleColor.DarkBlue
            };

            string[] banner =
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
        }

        public static string StringSpaces(string str, int length)
        {
            if (str.Length < length)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(str);
                for (int i = 0; i < length - str.Length; i++)
                {
                    builder.Append(" ");
                }
                str = builder.ToString();
            }
            return str;
        }

        public static void WriteLine(string text)   // highlights numbers
        {
            if (text.Any(char.IsDigit))
            {
                string[] snippets = text.Split(char.Parse(" "));

                for (int i = 0; i < snippets.Length; i++)
                {
                    if (snippets[i].Any(char.IsDigit))
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(snippets[i] + " ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write(snippets[i] + " ");
                    }
                }
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(Environment.NewLine);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(text);
            }
        }

        public static void Error(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(text);
            Console.Write(Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
        }

        public static uint RandomSeed()
        {
            Random r = new Random();
            return (uint)(r.Next(0, int.MaxValue) + r.Next(0, int.MaxValue)) + 1;
        }
    }
}
