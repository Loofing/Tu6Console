using loofer.Forms;
using loofer.Utils;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using XDRPCLib;

namespace loofer.Iw4
{
    public class hideTags
    {
        public static List<savedTags> tagsList = new List<savedTags>(); /*avoiding converttostring calls because of memory limitations*/
        public static void displayHideTags(ref TextBox hideBox)
        {
            byte[] tagBlock = ConsoleIW4.xbCon.GetMemory(ConsoleIW4.hideTagPointer, 0x20);
            string temp = string.Empty;
            for(int i = 0; i < 10; i++)
            {
                ushort currentTag = MemoryManager.toUInt16(tagBlock, i * sizeof(ushort));

                if (currentTag != 0)
                    temp += $"{getString(currentTag)}\\";
            }
            hideBox.Text = ConsoleIW4.hideTagsText = temp;
        }
        public static void saveHideTags(ref TextBox hideBox)
        {
            if(ConsoleIW4.hideTagsText.Equals(hideBox.Text))
                return;

            if(string.IsNullOrEmpty(hideBox.Text))
            {
                ConsoleIW4.xbCon.SetMemory(ConsoleIW4.hideTagPointer, new byte[20]);
                ConsoleIW4.hideTagsText = String.Empty;
                return;
            }

            ConsoleIW4.hideTagsText = hideBox.Text;
            string[] tagCollection = ConsoleIW4.hideTagsText.Split(new char[]{ '\\' },10);
            byte[] tempBlock = new byte[0x20];

            for(int i = 0;i < tagCollection.Length;i++)
            {
                if (string.IsNullOrEmpty(tagCollection[i]))
                    continue;

                ushort value = getValue(tagCollection[i]);
                MemoryManager.getUShortBytes(value).CopyTo(tempBlock, i * sizeof(ushort));
            }
            ConsoleIW4.xbCon.SetMemory(ConsoleIW4.hideTagPointer, tempBlock);
        }
        public static string getString(ushort value)
        {
            string tagName;

            if (isExistingString(value, out tagName))
                return tagName;

            tagName = Iw4Functions.ConvertToString(value);
            tagsList.Add(new savedTags(tagName, value));
            return tagName;
        }
        public static ushort getValue(string tagName)
        {
            ushort? tagValue;

            if (isExistingValue(tagName, out tagValue))
                return (ushort)tagValue;

            tagValue = Iw4Functions.GetString(tagName);
            tagsList.Add(new savedTags(tagName, (ushort)tagValue));
            return (ushort)tagValue;
        }
        public static bool isExistingString(ushort value, out string name)
        {
            name = string.Empty;
            foreach(savedTags tag in tagsList)
            {
                if (tag.value == value)
                {
                    name = tag.name;
                    break;
                }
            }
            return !string.IsNullOrEmpty(name);
        }
        public static bool isExistingValue(string name, out ushort? value)
        {
            value = null;
            foreach (savedTags tag in tagsList)
            {
                if (tag.name == name)
                {
                    value = tag.value;
                    break;
                }
            }
            return value != null;
        }
        public struct savedTags
        {
            public string name;
            public ushort value;

            public savedTags(string name, ushort value)
            {
                this.name = name;
                this.value = value;
            }
        }
    }
}
