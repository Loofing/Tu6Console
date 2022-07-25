using loofer.Forms;
using loofer.Utils;
using System;
using System.Runtime.InteropServices;
using XDRPCLib;


namespace loofer.Iw4
{
    public static class Iw4Functions
    {
        public static void Cbuf(string input) => ConsoleIW4.xbCon.Call(0x82275C60, 0, input);
        public static uint FindXAssetHeader(uint Header, string input) => ConsoleIW4.xbCon.Call(0x821E25B0, Header, input);
        public static uint GetWeaponVariantDef(string weaponName) => FindXAssetHeader((uint)Iw4Structs.assetType.weapon, weaponName);
        public static uint GetAnimation(string animationName) => FindXAssetHeader((uint)Iw4Structs.assetType.xanim, animationName);
        public static uint GetModel(string modelName) => FindXAssetHeader((uint)Iw4Structs.assetType.xmodel, modelName);
        public static int GetWeaponIndexForName(string Weapon) => (int)ConsoleIW4.xbCon.Call(0x822613E8, Weapon);
        public static uint GetWeaponDef(string weaponName) => ConsoleIW4.xbCon.Call(0x821142A8, GetWeaponIndexForName(weaponName));
        public static uint GetPlayerState(int clientIndex)
        {
            return 0x82FF5E80 + (Iw4Structs.playerState.playerSize * (uint)clientIndex);
        }
        public static object _GetWeaponVariantDef(string weaponName, out uint pointer)
        {
            pointer = FindXAssetHeader((uint)Iw4Structs.assetType.weapon, weaponName);
            byte[] memoryBlock = ConsoleIW4.xbCon.GetMemory(pointer, (uint)Marshal.SizeOf<Iw4Structs.weaponVariantDef>());
            return (Iw4Structs.weaponVariantDef)MemoryManager.toStruct<Iw4Structs.weaponVariantDef>(memoryBlock);
        }
        public static object _GetWeaponDef(uint pointer)
        {
            byte[] memoryBlock = ConsoleIW4.xbCon.GetMemory(pointer, (uint)Marshal.SizeOf<Iw4Structs.weaponDef>());
            return (Iw4Structs.weaponDef)MemoryManager.toStruct<Iw4Structs.weaponDef>(memoryBlock);
        }
        public static object _GetTracer(uint pointer)
        {
            byte[] memoryBlock = ConsoleIW4.xbCon.GetMemory(pointer, (uint)Marshal.SizeOf<Iw4Structs.Tracer>());
            return (Iw4Structs.Tracer)MemoryManager.toStruct<Iw4Structs.Tracer>(memoryBlock);
        }
        public static string ConvertToString(UInt16 shortName)
        {
            uint temp = ConsoleIW4.xbCon.Call(0x822A2418, shortName);
            return ConsoleIW4.xbCon.ReadString(temp);
        }
        public static ushort GetString(string nameString)
        {
            uint temp = ConsoleIW4.xbCon.Call(0x822A2E18, nameString,0);
            return ConsoleIW4.xbCon.ReadUInt16(temp);
        }
        public static void SSC(int client,int idk, string text)
        {
            ConsoleIW4.xbCon.Call(0x822BDF00, client,idk, text);
        }
    }
}
