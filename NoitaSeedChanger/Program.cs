using System;
using System.Diagnostics;
using System.IO;

namespace NoitaSeedChanger
{
    class Program
    {
        private static string gameName = "noita";
        private static Process game = null;
        private static Ini settings;
        private static readonly string settingsFile = AppDomain.CurrentDomain.BaseDirectory + "settings.ini";
        private static readonly string listFile = AppDomain.CurrentDomain.BaseDirectory + "seeds.txt";

        private static int version = 0;
        public static uint seed = 0;        // 0 to 4294967294 +1
        private static bool restart = false;

        private static readonly int[] pointer = new int[] { 
            0x14136D4, 0x1420798, 0x14ABCF0, // release branch
            0x1420798, 0x14ABCF0, 0x1420788, // beta branch
            0x142B898, 0x14B6E20, 0x142B6C8, // mod branch
            0x177712C, 0x1801640, 0x1777AC8, // old version
            0x1423ACC, 0x14AF030, 0x1421208  // GoG version
        };

        static void Main(string[] args)
        {
            // hooked Restart function to CancelKeyPress event
            Console.CancelKeyPress += new ConsoleCancelEventHandler(RestartApp);

            if (!File.Exists(settingsFile)) // check if settings.ini exists
            {
                File.Create(settingsFile).Close();

                settings = new Ini(settingsFile);
                settings.Write("version", "0", "Settings");
                version = Convert.ToInt32(settings.Read("version", "Settings"));
            }
            else // Load settings
            {
                settings = new Ini(settingsFile);
                version = Convert.ToInt32(settings.Read("version", "Settings"));
            }

            if (!File.Exists(listFile)) // check if seedlist.txt exists
            {
                string[] lines = { "1:First Seed" };
                File.Create(listFile).Close();
                File.WriteAllLines(listFile, lines);
            }

            Helper.DrawBanner();

            // get seed from list
            if (!Seedlist.GetList(listFile))
            {
                Console.Write(Environment.NewLine);
                Console.Write("Enter Seed> ");
                Console.ForegroundColor = ConsoleColor.White;
                uint.TryParse(Console.ReadLine(), out seed);
                Console.Write(Environment.NewLine);
            }

            if (seed <= 0)  // game stucks on title screen if the seed is less or equal zero
            {
                seed = Helper.RandomSeed();
                Helper.WriteLine("New randomly generated seed is " + seed);
                Console.Write(Environment.NewLine);
            }

            Restart:

            if (restart)
            {
                Helper.DrawBanner();
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Waiting for noita.exe");

            // checks if noita.exe is running
            while (game == null)
            {
                System.Threading.Thread.Sleep(50);
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
                switch (version)
                {
                    case 0: // release
                        ChangeSeed(pointer[0], pointer[1], pointer[2]);
                        break;
                    case 1: // beta
                        ChangeSeed(pointer[3], pointer[4], pointer[5]);
                        break;
                    case 2: // mod
                        ChangeSeed(pointer[6], pointer[7], pointer[8]);
                        break;
                    case 3: // old
                        ChangeSeed(pointer[9], pointer[10], pointer[11]);
                        break;
                    case 4: // GoG
                        ChangeSeed(pointer[12], pointer[13], pointer[14]);
                        break;
                    default:
                        break;
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

        private static void ChangeSeed(int p1, int p2, int p3)
        {
            while (Memory.Read(game.Handle, p1) != seed && Memory.Read(game.Handle, p2) != seed)
            {
                if (Memory.Read(game.Handle, p1) > 0 || Memory.Read(game.Handle, p2) > 0)
                {
                    Memory.Write(game.Handle, p1, seed);
                    Memory.Write(game.Handle, p2, seed);
                    Memory.Write(game.Handle, p3, seed);
                }
            }
        }

        public static void RestartApp(object sender, EventArgs e)
        {
            Process.Start(AppDomain.CurrentDomain.BaseDirectory + "NoitaSeedChanger.exe");
            Process.GetCurrentProcess().Kill();
        }
    }
}
