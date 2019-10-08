using System;
using System.Diagnostics;
using System.IO;

namespace NoitaSeedChanger
{
    class Program
    {
        private static string gameName = "noita";
        private static Process game = null;
        private static bool beta = false;
        private static int seed = 0;
        private static bool restart = false;
        private static IniFile settings;
        private static string settingsFile = AppDomain.CurrentDomain.BaseDirectory + "settings.ini";

        // final
        private static readonly int address = 0x177712C;
        private static readonly int address2 = 0x1777AC8;
        // beta
        private static readonly int address3 = 0x141A75C;
        private static readonly int address4 = 0x14A5CA0;

        static void Main(string[] args)
        {
            // hooked CancelKeyPress to RestartApp function
            Console.CancelKeyPress += new ConsoleCancelEventHandler(RestartApp);

            if (!File.Exists(settingsFile)) // check if settings.ini exists
            {
                File.Create(settingsFile).Close();

                settings = new IniFile(settingsFile);
                settings.Write("beta", "false", "Settings");
                beta = Convert.ToBoolean(settings.Read("beta", "Settings"));
            }
            else // Load settings
            {
                settings = new IniFile(settingsFile);
                beta = Convert.ToBoolean(settings.Read("beta", "Settings"));
            }

            DrawBanner();

            Console.Write("Enter Seed> ");
            Console.ForegroundColor = ConsoleColor.White;

            int.TryParse(Console.ReadLine(), out seed);

            if (seed <= 0)  // game stucks on title screen if the seed is less or equal zero
            {
                seed = 1;
            }

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
                    Console.Write(Environment.NewLine);
                }
            }

            // writes seed to given memory address for the correct version
            if (game.WaitForInputIdle())
            {
                if (!beta)
                {
                    while (Read(game.Handle, address) != seed  && Read(game.Handle, address2) != seed)
                    {
                        if (Read(game.Handle, address) > 0 || Read(game.Handle, address2) > 0)
                        {
                            Write(game.Handle, address, seed);
                            Write(game.Handle, address2, seed);
                        }
                    }
                }
                else if (beta)
                {
                    while (Read(game.Handle, address3) != seed && Read(game.Handle, address4) != seed)
                    {
                        if (Read(game.Handle, address3) > 0 || Read(game.Handle, address4) > 0)
                        {
                            Write(game.Handle, address3, seed);
                            Write(game.Handle, address4, seed);
                        }

                    }
                }
            }

            Console.WriteLine("Seed changed to: " + seed);
            Console.Write(Environment.NewLine);
            Console.WriteLine("Waiting for restart.");

            game.WaitForExit();
            game = null;

            Console.Clear();

            restart = true;
            goto Restart;
        }

        public static void RestartApp(object sender, EventArgs e)
        {
            Process.Start(AppDomain.CurrentDomain.BaseDirectory + "NoitaSeedChanger.exe");
            Process.GetCurrentProcess().Kill();
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
            Console.Write(Environment.NewLine);
            Console.Write(Environment.NewLine);
        }
    }
}
