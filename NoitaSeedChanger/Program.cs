using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace NoitaSeedChanger
{
    class Program
    {
        private static string gameName = "noita";
        public static Process game;

        private static readonly string listFile = AppDomain.CurrentDomain.BaseDirectory + "SeedList.txt";

        public static uint seed = 0;        // 1 to 4294967295 
        private static bool restart = false;

        static void Main(string[] args)
        {
            // hooked restart function to CancelKeyPress event
            Console.CancelKeyPress += new ConsoleCancelEventHandler(RestartApp);

            if (!File.Exists(listFile)) // check if seedlist.txt exists
            {
                string[] lines = { "1234567890:Test Seed" };
                File.Create(listFile).Close();
                File.WriteAllLines(listFile, lines);
            }

            Helper.DrawBanner();

            if (!Seedlist.GetList(listFile)) // get seed from list
            {
                Console.Write(Environment.NewLine);
                Console.Write("Enter Seed> ");
                Console.ForegroundColor = ConsoleColor.White;
                uint.TryParse(Console.ReadLine(), out seed);
                Console.Write(Environment.NewLine);
            }

            if (seed <= 0)  // game stucks on title screen if the seed is less or equal zero
            {
                Helper.Error("Seed invalid! Make sure it's in a range of 1 to 4294967295.");
                seed = Helper.RandomSeed();
                Helper.WriteLine("Generated random seed: " + seed);
                Console.Write(Environment.NewLine);
            }

            Restart:

            if (restart)
            {
                Helper.DrawBanner();
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Waiting for noita.exe");

            // checks if noita.exe is running
            while (game == null)
            {
                Thread.Sleep(50);
                if (Process.GetProcessesByName(gameName).Length > 0)
                {
                    game = Process.GetProcessesByName(gameName)[0];
                    Console.WriteLine("noita.exe is running");
                    Console.Write(Environment.NewLine);
                }
            }
            
            // checks current Noita release on the first run and sets 'release' variable
            if (!restart)
            {
                Release.Init();
            }

            // writes seed to given memory address for the correct release version
            if (game.WaitForInputIdle())
            {
                Memory.ChangeSeed(game.Handle, seed, Release.currentTargets);
            }
            Helper.WriteLine("Seed changed to: " + seed);
            Console.WriteLine("Idle until Noita restarts.");

            game.WaitForExit();
            game = null;

            restart = true;
            goto Restart;
        }
        private static void RestartApp(object sender, EventArgs e)
        {
            Process.Start(AppDomain.CurrentDomain.BaseDirectory + "NoitaSeedChanger.exe");
            Process.GetCurrentProcess().Kill();
        }
    }
}
