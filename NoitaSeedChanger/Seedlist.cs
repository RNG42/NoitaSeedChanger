using NoitaSeedChanger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Seedlist
{
    private static readonly List<string> seedList = new List<string>();
    private static int selection = 1;

    public static bool GetList(string path)
    {
        int lineCount = 1;

        string[] lines = File.ReadAllLines(path, Encoding.UTF8);

        foreach (string line in lines)
        {
            string[] splitString = line.Split(char.Parse(":"));
            if (uint.TryParse(splitString[0], out uint n))
            {
                if (splitString.Length > 1)
                {
                    seedList.Add(n.ToString());
                    splitString[0] = Helper.StringSpaces(splitString[0], 16);
                    Console.WriteLine(" {0}" + splitString[0] + " : " + splitString[1], "[" + Helper.StringSpaces(lineCount.ToString() + "]", 4));
                    lineCount++;
                }
                else
                {
                    seedList.Add(n.ToString());
                    splitString[0] = Helper.StringSpaces(splitString[0], 16);
                    Console.WriteLine( " {0}" + splitString[0] + " : ", "[" + Helper.StringSpaces(lineCount.ToString() + "]", 4));
                    lineCount++;
                }
            }
            else
            {
                if (splitString[0].Length > 0)
                {
                    InvalidSeed(splitString[0]);
                }
            }
        }

        Console.WriteLine(" [{0}]  ENTER NEW SEED", lineCount);
        Console.Write(Environment.NewLine);
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write("Select Seed> ");
        Console.ForegroundColor = ConsoleColor.White;
        int.TryParse(Console.ReadLine(), out selection);
        Console.ForegroundColor = ConsoleColor.DarkCyan;

        if (selection <= lineCount - 1 && selection != 0)
        {
            if (!uint.TryParse(seedList[selection - 1], out Program.seed))
            {
                InvalidSeed(seedList[selection - 1]);
            }
            else
            {
                Helper.WriteLine("Selected: " + seedList[selection - 1]);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(Environment.NewLine);
            }
            return true;
        }
        return false;
    }
    private static void InvalidSeed(string seed)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write(" Seed ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(seed);
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(" Invalid! Make sure it's in a range of 1 to 4294967295.");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
    }
}