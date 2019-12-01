using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

internal class Memory
{
    [DllImport("kernel32")]
    public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
    [DllImport("kernel32", SetLastError = true)]
    public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

    public static uint Read(IntPtr handle, IntPtr address)
    {
        byte[] dataBuffer = new byte[4];
        int bytesRead = 0;
        ReadProcessMemory((int)handle, (int)address, dataBuffer, dataBuffer.Length, ref bytesRead);
        return BitConverter.ToUInt32(dataBuffer, 0);
    }

    public static bool Write(IntPtr handle, IntPtr address, uint value)
    {
        byte[] dataBuffer = BitConverter.GetBytes(value);
        int bytesWritten = 0;
        WriteProcessMemory((int)handle, (int)address, dataBuffer, dataBuffer.Length, ref bytesWritten);
        return true;
    }

    public static void ChangeSeed(IntPtr game, uint seed, List<IntPtr> pointer)
    {
        while (Read(game, pointer[0]) != seed && Read(game, pointer[1]) != seed)
        {
            if (Read(game, pointer[0]) > 0 || Read(game, pointer[1]) > 0)
            {
                for (int i = 0; i < pointer.Count; i++)
                {
                    Write(game, pointer[i], seed);
                }
            }
        }
    }
}