using loofer.Iw4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace loofer.Utils
{
    public static class MemoryManager
    {
        public static float toFloat(byte[] bytes, int index, bool reverse = true)
        {
            byte[] buff = new byte[4];

            Array.Copy(bytes, index, buff, 0, 4);

            if (reverse)
                Array.Reverse(buff, 0, 4);

            return BitConverter.ToSingle(buff, 0);
        }
        public static short toShort(byte[] bytes, int index, bool reverse = true)
        {
            byte[] buff = new byte[2];

            Array.Copy(bytes, index, buff, 0, 2);

            if (reverse)
                Array.Reverse(buff, 0, 2);

            return BitConverter.ToInt16(buff, 0);
        }
        public static int toInt32(byte[] bytes, int index, bool reverse = true)
        {
            byte[] buff = new byte[4];

            Array.Copy(bytes, index, buff, 0, 4);

            if (reverse)
                Array.Reverse(buff, 0, 4);

            return BitConverter.ToInt32(buff, 0);
        }
        public static uint toUInt32(byte[] bytes, int index, bool reverse = true)
        {
            byte[] buff = new byte[4];

            Array.Copy(bytes, index, buff, 0, 4);

            if (reverse)
                Array.Reverse(buff, 0, 4);

            return BitConverter.ToUInt32(buff, 0);
        }
        public static ushort toUInt16(byte[] bytes, int index, bool reverse = true)
        {
            byte[] buff = new byte[2];

            Array.Copy(bytes, index, buff, 0, 2);

            if (reverse)
                Array.Reverse(buff, 0, 2);

            return BitConverter.ToUInt16(buff, 0);
        }
        public static byte[] getUShortBytes(ushort value,bool reverse = true)
        {
            byte[] buff = new byte[2];

            BitConverter.GetBytes(value).CopyTo(buff, 0);

            if (reverse)
                Array.Reverse(buff, 0, 2);

            return buff;
        }
        public static T toStruct<T>(byte[] rawData, bool endianSwap = true)
        {
            if (endianSwap)
            {
                EndianSwap(typeof(T), rawData);
            }

            IntPtr ptPoit = Marshal.AllocHGlobal(rawData.Length);
            Marshal.Copy(rawData, 0, ptPoit, rawData.Length);
            var newStruct = (T)Marshal.PtrToStructure(ptPoit, typeof(T));
            Marshal.FreeHGlobal(ptPoit);
            return newStruct;
        }
        public static byte[] toBytes<T>(object obj, bool endianSwap = true)
        {
            int length = Marshal.SizeOf(obj);
            byte[] array = new byte[length];
            IntPtr ptr = Marshal.AllocHGlobal(length);
            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, array, 0, length);
            Marshal.FreeHGlobal(ptr);

            if (endianSwap)
            {
                EndianSwap(typeof(T), array);
            }

            return array;
        }
        private static void EndianSwap(Type type, byte[] data, int startOffset = 0)
        {
            foreach (var field in type.GetFields())
            {
                var fieldType = field.FieldType;

                if (field.IsStatic || fieldType == typeof(string) || fieldType == typeof(bool))
                    continue;

                var offset = Marshal.OffsetOf(type, field.Name).ToInt32();

                if (fieldType.IsEnum)
                    fieldType = Enum.GetUnderlyingType(fieldType);

                var subFields = fieldType.GetFields().Where(subField => subField.IsStatic == false).ToArray();
                var effectiveOffset = startOffset + offset;

                if (subFields.Length == 0)
                    Array.Reverse(data, effectiveOffset, Marshal.SizeOf(fieldType));
                else
                    EndianSwap(fieldType, data, effectiveOffset);
            }
        }
    }
}
