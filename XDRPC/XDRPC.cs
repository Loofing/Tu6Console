using Microsoft.Test.Xbox.XDRPC;
using System;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using XDevkit;

namespace XDRPCLib
{
    public static class XDRPC
    {
        private static byte[] memoryBuffer = new byte[0x40];
        private static uint outInt;

        public static byte[] GetMemory(this IXboxConsole xbCon, uint address, uint length)
        {
            byte[] data = new byte[length];
            try
            {
                xbCon.DebugTarget.GetMemory(address, length, data, out outInt);
                xbCon.DebugTarget.InvalidateMemoryCache(true, address, length);
            }
            catch (Exception) { }
            return data;
        }
        public static void SetMemory(this IXboxConsole xbCon, uint address, byte[] data)
        {
            try
            {
                xbCon.DebugTarget.SetMemory(address, (uint)data.Length, data, out outInt);
            }
            catch (Exception) { }
        }
        public static uint Call(this IXboxConsole xbCon, uint offset, params object[] args)
        {
            uint value;
            try
            {
                value = XDRPCMarshaler.ExecuteRPC<uint>(xbCon, new XDRPCExecutionOptions(XDRPCMode.System, offset), args);
            }
            catch (Exception)
            {
                MessageBox.Show("There was an error please try again.");
                value = 0x0;

            }
            return value;
        }
        public static uint CallTitle(this IXboxConsole xbCon, uint offset, params object[] args)
        {
            uint value;
            try
            {
                value = XDRPCMarshaler.ExecuteRPC<uint>(xbCon, new XDRPCExecutionOptions(XDRPCMode.Title, offset), args);
            }
            catch (Exception)
            {
                MessageBox.Show("There was an error please try again.");
                value = 0x0;
            }
            return value;
        }
        public static uint CallString(this IXboxConsole xbCon, uint offset, params object[] args)
        {
            uint value = (uint)XDRPCMarshaler.ExecuteRPC<uint>(xbCon, new XDRPCExecutionOptions(XDRPCMode.Title, offset), args);

            return value;
        }
        public static ulong callReader(this IXboxConsole xbCon, uint offset, params object[] args)
        {
            ulong haha = (ulong)XDRPCMarshaler.ExecuteRPC<ulong>(xbCon, new XDRPCExecutionOptions(XDRPCMode.Title, offset), args);

            return haha;
        }
        public static string CallModuleString(this IXboxConsole xbCon, string moduleName, int ordinal, params object[] args)
        {
            uint address = XDRPCMarshaler.ExecuteRPC<uint>(xbCon, XDRPCMode.System, moduleName, ordinal, args);
            return ReadString(xbCon, address);
        }
        public static byte[] ReadBytes(this IXboxConsole xbCon, uint offset, uint size)
        {
            byte[] temp = new byte[size];
            xbCon.DebugTarget.GetMemory(offset, size, temp, out outInt);
            xbCon.DebugTarget.InvalidateMemoryCache(true, offset, size);
            return temp;
        }
        public static sbyte ReadSByte(this IXboxConsole xbCon, uint offset)
        {
            xbCon.DebugTarget.GetMemory(offset, 1, memoryBuffer, out outInt);
            xbCon.DebugTarget.InvalidateMemoryCache(true, offset, 1);
            return (sbyte)memoryBuffer[0];
        }
        public static bool ReadBool(this IXboxConsole xbCon, uint offset)
        {
            xbCon.DebugTarget.GetMemory(offset, 1, memoryBuffer, out outInt);
            xbCon.DebugTarget.InvalidateMemoryCache(true, offset, 1);
            return memoryBuffer[0] != 0;
        }
        public static short ReadInt16(this IXboxConsole xbCon, uint offset)
        {
            xbCon.DebugTarget.GetMemory(offset, 2, memoryBuffer, out outInt);
            xbCon.DebugTarget.InvalidateMemoryCache(true, offset, 2);
            Array.Reverse(memoryBuffer, 0, 2);
            return BitConverter.ToInt16(memoryBuffer, 0);
        }
        public static int ReadInt32(this IXboxConsole xbCon, uint offset)
        {
            xbCon.DebugTarget.GetMemory(offset, 4, memoryBuffer, out outInt);
            xbCon.DebugTarget.InvalidateMemoryCache(true, offset, 4);
            Array.Reverse(memoryBuffer, 0, 4);
            return BitConverter.ToInt32(memoryBuffer, 0);
        }
        public static long ReadInt64(this IXboxConsole xbCon, uint offset)
        {
            xbCon.DebugTarget.GetMemory(offset, 8, memoryBuffer, out outInt);
            xbCon.DebugTarget.InvalidateMemoryCache(true, offset, 8);
            Array.Reverse(memoryBuffer, 0, 8);
            return BitConverter.ToInt64(memoryBuffer, 0);
        }
        public static byte ReadByte(this IXboxConsole xbCon, uint offset)
        {
            xbCon.DebugTarget.GetMemory(offset, 1, memoryBuffer, out outInt);
            xbCon.DebugTarget.InvalidateMemoryCache(true, offset, 1);
            return memoryBuffer[0];
        }
        public static ushort ReadUInt16(this IXboxConsole xbCon, uint offset)
        {
            xbCon.DebugTarget.GetMemory(offset, 2, memoryBuffer, out outInt);
            xbCon.DebugTarget.InvalidateMemoryCache(true, offset, 2);
            Array.Reverse(memoryBuffer, 0, 2);
            return BitConverter.ToUInt16(memoryBuffer, 0);
        }
        public static uint ReadUInt32(this IXboxConsole xbCon, uint offset)
        {
            xbCon.DebugTarget.GetMemory(offset, 4, memoryBuffer, out outInt);
            xbCon.DebugTarget.InvalidateMemoryCache(true, offset, 4);
            Array.Reverse(memoryBuffer, 0, 4);
            return BitConverter.ToUInt32(memoryBuffer, 0);
        }
        public static ulong ReadUInt64(this IXboxConsole xbCon, uint offset)
        {
            xbCon.DebugTarget.GetMemory(offset, 8, memoryBuffer, out outInt);
            xbCon.DebugTarget.InvalidateMemoryCache(true, offset, 8);
            Array.Reverse(memoryBuffer, 0, 8);
            return BitConverter.ToUInt64(memoryBuffer, 0);
        }
        public static float ReadFloat(this IXboxConsole xbCon, uint offset)
        {
            xbCon.DebugTarget.GetMemory(offset, 4, memoryBuffer, out outInt);
            xbCon.DebugTarget.InvalidateMemoryCache(true, offset, 4);
            Array.Reverse(memoryBuffer, 0, 4);
            return BitConverter.ToSingle(memoryBuffer, 0);
        }
        public static string ReadString(this IXboxConsole xbCon, uint offset, byte[] readBuffer)
        {
            xbCon.DebugTarget.GetMemory(offset, (uint)readBuffer.Length, readBuffer, out outInt);
            xbCon.DebugTarget.InvalidateMemoryCache(true, offset, (uint)readBuffer.Length);
            return new string(System.Text.Encoding.ASCII.GetChars(readBuffer)).Split('\0')[0];
        }
        public static string _ReadStringPtr(this IXboxConsole xbCon, uint Offset )
        {
            uint uTemp32 = ReadUInt32(xbCon, Offset);
            Offset = uTemp32;
            return ReadString(xbCon,Offset);
        }
        public static string ReadStringPtr(this IXboxConsole xbCon, uint Offset)
        {
            return _ReadStringPtr(xbCon, Offset);
        }
        public static string ReadString(this IXboxConsole xbCon, uint offset)
        {
            return ReadString(xbCon, offset, memoryBuffer);
        }
        public static void WriteSByte(this IXboxConsole xbCon, uint offset, sbyte input)
        {
            memoryBuffer[0] = (byte)input;
            xbCon.DebugTarget.SetMemory(offset, 1, memoryBuffer, out outInt);
        }
        public static void WriteInt16(this IXboxConsole xbCon, uint offset, short input)
        {
            BitConverter.GetBytes(input).CopyTo(memoryBuffer, 0);
            Array.Reverse(memoryBuffer, 0, 2);
            xbCon.DebugTarget.SetMemory(offset, 2, memoryBuffer, out outInt);
        }
        public static void WriteInt32(this IXboxConsole xbCon, uint offset, int input)
        {
            BitConverter.GetBytes(input).CopyTo(memoryBuffer, 0);
            Array.Reverse(memoryBuffer, 0, 4);
            xbCon.DebugTarget.SetMemory(offset, 4, memoryBuffer, out outInt);
        }
        public static void WriteInt64(this IXboxConsole xbCon, uint offset, long input)
        {
            BitConverter.GetBytes(input).CopyTo(memoryBuffer, 0);
            Array.Reverse(memoryBuffer, 0, 8);
            xbCon.DebugTarget.SetMemory(offset, 8, memoryBuffer, out outInt);
        }
        public static void WriteByte(this IXboxConsole xbCon, uint offset, byte input)
        {
            memoryBuffer[0] = input;
            xbCon.DebugTarget.SetMemory(offset, 1, memoryBuffer, out outInt);
        }
        public static void WriteUInt16(this IXboxConsole xbCon, uint offset, ushort input)
        {
            BitConverter.GetBytes(input).CopyTo(memoryBuffer, 0);
            Array.Reverse(memoryBuffer, 0, 2);
            xbCon.DebugTarget.SetMemory(offset, 2, memoryBuffer, out outInt);
        }
        public static void WriteUInt32(this IXboxConsole xbCon, uint offset, uint input)
        {
            BitConverter.GetBytes(input).CopyTo(memoryBuffer, 0);
            Array.Reverse(memoryBuffer, 0, 4);
            xbCon.DebugTarget.SetMemory(offset, 4, memoryBuffer, out outInt);
        }
        public static void WriteUInt64(this IXboxConsole xbCon, uint offset, ulong input)
        {
            BitConverter.GetBytes(input).CopyTo(memoryBuffer, 0);
            Array.Reverse(memoryBuffer, 0, 8);
            xbCon.DebugTarget.SetMemory(offset, 8, memoryBuffer, out outInt);
        }
        public static void WriteFloat(this IXboxConsole xbCon, uint offset, float input)
        {
            BitConverter.GetBytes(input).CopyTo(memoryBuffer, 0);
            Array.Reverse(memoryBuffer, 0, 4);
            xbCon.DebugTarget.SetMemory(offset, 4, memoryBuffer, out outInt);
        }
        public static void Push(this byte[] InArray, out byte[] OutArray, byte Value)
        {
            OutArray = new byte[InArray.Length + 1];
            InArray.CopyTo(OutArray, 0);
            OutArray[InArray.Length] = Value;
        }
        public static void WriteString(this IXboxConsole xbCon, uint Address, string String)
        {
            byte[] array = new byte[0];
            for (int i = 0; i < String.Length; i++)
            {
                byte value = (byte)String[i];
                array.Push(out array, value);
            }
            array.Push(out array, 0);
            xbCon.DebugTarget.SetMemory(Address, (uint)array.Length, array, out outInt);
        }
        public static void WriteStringPtr(this IXboxConsole xbCon, uint Address, string String)
        {
            uint Temp = ReadUInt32(xbCon, Address);
            Address = Temp;
            WriteString(xbCon, Address, String);
        }
        public static void _WriteStringPtr(this IXboxConsole xbCon, uint Address, string String)
        {
            WriteStringPtr(xbCon, Address, String);
        }
        public static void WriteBytes(this IXboxConsole xbCon, uint offset, byte[] memory)
        {
            xbCon.DebugTarget.SetMemory(offset, (uint)memory.Length, memory, out outInt);
        }
        public static void Reboot(this IXboxConsole xbCon)
        {
            xbCon.Reboot(null, null, null, XboxRebootFlags.Cold);
        }
        public static void Shutdown(this IXboxConsole xbCon)
        {
            xbCon.ExecuteRPC<uint>(XDRPCMode.System, "xboxkrnl.exe", 40, new object[] { 0x7 });
        }
        public static string GetCPUKey(this IXboxConsole xbCon)
        {
            byte[] RPC = new byte[] { 0x38, 0x00, 0x00, 0x71, 0x44, 0x00, 0x00, 0x02, 0x4E, 0x80, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00 };

            xbCon.DebugTarget.SetMemory(0x81B44688, 16, RPC, out outInt);
            ulong CPU1 = xbCon.ExecuteRPC<ulong>(new XDRPCExecutionOptions(XDRPCMode.Title, 0x81B44688), new object[] { 0x48565050, 3, 0x8000020000020600 });
            ulong CPU2 = xbCon.ExecuteRPC<ulong>(new XDRPCExecutionOptions(XDRPCMode.Title, 0x81B44688), new object[] { 0x48565050, 3, 0x8000020000020800 });
            ulong CPU3 = xbCon.ExecuteRPC<ulong>(new XDRPCExecutionOptions(XDRPCMode.Title, 0x81B44688), new object[] { 0x48565050, 3, 0x8000020000020A00 });
            ulong CPU4 = xbCon.ExecuteRPC<ulong>(new XDRPCExecutionOptions(XDRPCMode.Title, 0x81B44688), new object[] { 0x48565050, 3, 0x8000020000020C00 });
            xbCon.DebugTarget.SetMemory(0x81B44688, 16, new byte[16], out outInt);

            return string.Format("{0}{1}", (CPU1 | CPU2).ToString("X8"), (CPU3 | CPU4).ToString("X8"));
        }
        public static uint GetCurrentTitleId(this IXboxConsole xbCon)
        {
            return xbCon.ExecuteRPC<uint>(XDRPCMode.System, "xam.xex", 463, new object[] { });
        }
        public static string GetConsoleIP(this IXboxConsole xbCon)
        {
            return new System.Net.IPAddress(xbCon.IPAddress).ToString();
        }
        public static void LoadModule(this IXboxConsole xbCon, string module)
        {
            if (module.Contains("\\") == false)
                module = "Hdd:\\" + module;

            xbCon.ExecuteRPC<uint>(XDRPCMode.System, "xboxkrnl.exe", 409, new object[] { module, 8, 0, 0 });
        }
        public static void UnloadModule(this IXboxConsole xbCon, string module)
        {
            uint handle = xbCon.ExecuteRPC<uint>(XDRPCMode.System, "xam.xex", 1102, new object[] { module });
            if (handle != 0u)
            {
                xbCon.WriteInt16(handle + 0x40, 1);
                xbCon.ExecuteRPC<uint>(XDRPCMode.System, "xboxkrnl.exe", 417, new object[] { handle });
            }
        }
    }
}