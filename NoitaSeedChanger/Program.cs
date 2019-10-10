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
        public static uint seed = 0;        // 1 to 4294967295
        private static bool restart = false;
        private static Ini settings;
        private static readonly string settingsFile = AppDomain.CurrentDomain.BaseDirectory + "settings.ini";
        private static readonly string listFile = AppDomain.CurrentDomain.BaseDirectory + "seeds.txt";

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

                settings = new Ini(settingsFile);
                settings.Write("beta", "false", "Settings");
                beta = Convert.ToBoolean(settings.Read("beta", "Settings"));
            }
            else // Load settings
            {
                settings = new Ini(settingsFile);
                beta = Convert.ToBoolean(settings.Read("beta", "Settings"));
            }

            if (!File.Exists(listFile)) // check if seedlist.txt exists
            {
                string[] lines = { "1:First Seed" };
                File.Create(listFile).Close();
                File.WriteAllLines(listFile, lines);
            }

            Banner.Draw();

            // get seed from list
            if (!Seedlist.GetList(listFile))
            {
                Console.Write(Environment.NewLine);
                Console.Write("Enter Seed> ");
                Console.ForegroundColor = ConsoleColor.White;
                uint.TryParse(Console.ReadLine(), out seed);
                Console.Write(Environment.NewLine);

                if (seed <= 0)  // game stucks on title screen if the seed is less or equal zero
                {
                    seed = 1;
                }
            }

            Restart:

            if (restart)
            {
                Banner.Draw();
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
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
                    while (Memory.Read(game.Handle, address) != seed && Memory.Read(game.Handle, address2) != seed)
                    {
                        if (Memory.Read(game.Handle, address) > 0 || Memory.Read(game.Handle, address2) > 0)
                        {
                            Memory.Write(game.Handle, address, seed);
                            Memory.Write(game.Handle, address2, seed);
                        }
                    }
                }
                else if (beta)
                {
                    while (Memory.Read(game.Handle, address3) != seed && Memory.Read(game.Handle, address4) != seed)
                    {
                        if (Memory.Read(game.Handle, address3) > 0 || Memory.Read(game.Handle, address4) > 0)
                        {
                            Memory.Write(game.Handle, address3, seed);
                            Memory.Write(game.Handle, address4, seed);
                        }

                    }
                }
            }

            Console.Write("Seed changed to: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(seed);
            Console.ForegroundColor = ConsoleColor.DarkCyan;

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
    }
}
