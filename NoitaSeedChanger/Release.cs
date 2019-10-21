using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace NoitaSeedChanger
{
    class Release
    {
        private static readonly string hashFile = "/_version_hash.txt";

        private const string final = "c0ba23bc0c325a0dc06604f114ee8217112a23af";
        private const string beta = "cac8fef90391e9409e8be422ec8322bb0b2cde2e";
        private const string old = "3bbb44abfe5f4e08dcff1aba3160cd512f7e756c";

        public static void Set()
        {
            try
            {
                string[] lines = File.ReadAllLines(GetHashFile(), Encoding.UTF8);
                switch (lines[0])
                {
                    case final:
                        Program.release = 0;
                        break;
                    case beta:
                        Program.release = 1;
                        break;
                    case old:
                        Program.release = 2;
                        break;
                    default:
                        Helper.Error("Current game version not supported!");
                        Helper.Error("Closing NSC in 10 seconds.");
                        Thread.Sleep(10000);
                        Process.GetCurrentProcess().Kill();
                        break;
                }
            }
            catch (Exception e)
            {
                Helper.Error(e.Message);
                Helper.Error("Closing NSC in 10 seconds.");
                Thread.Sleep(10000);
                throw;
            }

        }
        private static string GetHashFile()
        {
            try
            {
                return Path.GetDirectoryName(Program.game.MainModule.FileName) + hashFile;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
