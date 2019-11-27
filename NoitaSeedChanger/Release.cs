﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace NoitaSeedChanger
{
    class Release
    {
        private static readonly string hashFile = "/_version_hash.txt";

        // release version hashes
        private const string final = "60081be6ff40a7f54278ec5ac409904e8400a9a7";
        private const string beta = "0c2f5a00b9bc702f55595ac425172e64e1b82273";

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
            return Path.GetDirectoryName(Program.game.MainModule.FileName) + hashFile;
        }

    }
}