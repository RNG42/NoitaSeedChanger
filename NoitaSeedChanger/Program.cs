using System;
using System.Diagnostics;

namespace NoitaSeedChanger
{
    class Program
    {
        private static string gameName = "noita";
        private static Process game = null;

        private static readonly int address = 0x177712C;
        private static readonly int address2 = 0x1801640;

        private static int seed = 0;

        private static bool restart = false;

        static void Main(string[] args)
        {
            DrawBanner();

            Console.Write("Enter Seed> ");
            Console.ForegroundColor = ConsoleColor.White;

            int.TryParse(Console.ReadLine(), out seed);

            if (seed <= 0)  // game stucks on title screen if the seed is less or equal zero
            {
                seed = 1;
            }

            Console.WriteLine("");

            Restart:

            if (restart)
            {
                DrawBanner();
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Waiting for noita.exe");

            // checks if noita.exe is running
            while (game == null)
            {
                System.Threading.Thread.Sleep(25);
                if (Process.GetProcessesByName(gameName).Length > 0)
                {
                    game = Process.GetProcessesByName(gameName)[0];
                    Console.WriteLine("noita.exe is running");
                    Console.WriteLine("");
                }
            }

            // injects seed to given memory address
            if (game.WaitForInputIdle())
            {
                while (Read(game.Handle, address) <= 0 && Read(game.Handle, address2) <= 0)
                {
                    System.Threading.Thread.Sleep(250);

                    if (Read(game.Handle, address) > 0)
                    {
                        Write(game.Handle, address, seed);
                    }
                    if (Read(game.Handle, address2) > 0)
                    {
                        Write(game.Handle, address2, seed);
                    }
                }
            }

            Console.WriteLine("Seed changed to: " + seed);
            Console.WriteLine("");
            Console.WriteLine("Waiting for restart.");

            game.WaitForExit();
            game = null;

            Console.Clear();

            restart = true;
            goto Restart;
        }

        // reads memory address
        public static int Read(IntPtr handle, int address)
        {
            // Prepare buffer and pointer
            byte[] dataBuffer = new byte[4];
            int bytesRead = 0;

            // Read
            Imports.ReadProcessMemory((int)handle, address, dataBuffer, dataBuffer.Length, ref bytesRead);

            // Convert the content of your buffer to int
            return BitConverter.ToInt32(dataBuffer, 0);
        }

        // writes memory address
        public static bool Write(IntPtr handle, int address, int value)
        {
            // Create buffer and pointer
            byte[] dataBuffer = BitConverter.GetBytes(value);
            int bytesWritten = 0;

            // Write
            Imports.WriteProcessMemory((int)handle, address, dataBuffer, dataBuffer.Length, ref bytesWritten);

            return true;
        }

        private static void DrawBanner()
        {
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
                @"  _______         __ __              _______                 __      ______ __                                 ",
                @" |    |  |.-----.|__|  |_.---.-.    |     __|.-----.-----.--|  |    |      |  |--.---.-.-----.-----.-----.----.",
                @" |       ||  _  ||  |   _|  _  |    |__     ||  -__|  -__|  _  |    |   ---|     |  _  |     |  _  |  -__|   _|",
                @" |__|____||_____||__|____|___._|    |_______||_____|_____|_____|    |______|__|__|___._|__|__|___  |_____|__|  ",
                @"                                                                                             |_____|           "

            };

            for (int i = 0; i < 5; i++)
            {
                Console.ForegroundColor = gradient[i];
                if (i < 4)
                {
                    Console.Write(banner[i] + Environment.NewLine);
                }
                else
                {
                    Console.Write(banner[i]);
                }

            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(" by RNG42");
            Console.WriteLine("");
            Console.WriteLine("");
        }
    }
}
