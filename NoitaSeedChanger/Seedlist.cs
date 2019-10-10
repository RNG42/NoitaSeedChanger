using NoitaSeedChanger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Seedlist
{
    private static readonly List<string> seedList = new List<string>();
    private static int selection;

    public static bool GetList(string path)
    {
        int lineCount = 1;
        string[] array = File.ReadAllLines(path, Encoding.UTF8);
        for (int i = 0; i < array.Length; i++)
        {
            string[] splitString = array[i].Split(char.Parse(":"));
            seedList.Add(splitString[0]);
            splitString[0] = StringSpaces(splitString[0], 15);
            Console.WriteLine("{0}. " + splitString[0] + " : " + splitString[1], lineCount);
            lineCount++;
        }
        Console.WriteLine("{0}. ENTER NEW SEED", lineCount);
        Console.Write(Environment.NewLine);
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write("Select Seed> ");
        Console.ForegroundColor = ConsoleColor.White;
        int.TryParse(Console.ReadLine(), out selection);
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write(Environment.NewLine);
        if (selection <= lineCount - 1 && selection != 0)
        {
            Program.seed = uint.Parse(seedList[selection - 1]);
            return true;
        }
        _ = selection;
        return false;
    }

    private static string StringSpaces(string str, int length)
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
}