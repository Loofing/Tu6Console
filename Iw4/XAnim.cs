using loofer.Utils;
using loofer.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XDRPCLib;

namespace loofer.Iw4
{
    public class XAnim
    {
        public static int? tabIndex; /* 0 = Default, 1 = Right, 2 = Left*/
        public static string[][] animName = new string[3][] {new string[37], new string[37], new string[37]};
        public static uint[][] animPtr = new uint[3][] {new uint[37], new uint[37], new uint[37]};
        public static bool[] dataLoaded = new bool[3] { false, false, false }; /*to check if animations already got extracted*/
        public static uint emptyAnimation = 0xA6E56A8C;
        public static void displayAnimations(ref DataGridView animGrid,int index = 0)
        {
            animGrid.Rows.Clear();
            if (!dataLoaded[index])
            {
                byte[] memoryBlock = ConsoleIW4.xbCon.GetMemory(ConsoleIW4.animationPointer[index], 0x94);

                for (int i = 0; i < Enum.GetNames(typeof(Iw4Structs.szXAnims)).Length; i++)
                {
                    animPtr[index][i] = MemoryManager.toUInt32(memoryBlock, i * 4);

                    if (animPtr[index][i] == 0x0 || animPtr[index][i] == emptyAnimation)
                    {
                        animGrid.Rows.Add((Iw4Structs.szXAnims)i, animName[index][i] = string.Empty);
                        continue;
                    }
                    animGrid.Rows.Add((Iw4Structs.szXAnims)i, animName[index][i] = ConsoleIW4.xbCon.ReadString(animPtr[index][i]));
                }
                dataLoaded[index] = true;
            }
            else
            {
                for (int i = 0; i < Enum.GetNames(typeof(Iw4Structs.szXAnims)).Length; i++)
                    animGrid.Rows.Add((Iw4Structs.szXAnims)i, animName[index][i]);
            }
            tabIndex = index;
        }
        public static void saveAnimations(ref DataGridView grid)
        {
            if (tabIndex is null)
                return;

            int i = (int)tabIndex;

            foreach(DataGridViewRow dataRow in grid.Rows)
            {
                string cellValue = string.Empty;
                int index = dataRow.Index;

                if (dataRow.Cells[1].Value != null)
                    cellValue = dataRow.Cells[1].Value.ToString();

                if (cellValue.Equals(animName[i][index]))
                    continue;

                if (!Utility.isValidAnimation(cellValue) && !string.IsNullOrEmpty(cellValue))
                {
                    ConsoleIW4.msgBox(cellValue + " is not a valid animation @" + dataRow.Cells[0].Value as string);
                    dataRow.Cells[1].Value = animName[i][index];
                    break;
                }
                animName[i][index] = cellValue;

                if (string.IsNullOrEmpty(animName[i][index]))
                    animPtr[i][index] = emptyAnimation;
                else
                    animPtr[i][index] = ConsoleIW4.xbCon.ReadUInt32(Iw4Functions.GetAnimation(animName[i][index]));

                ConsoleIW4.xbCon.WriteUInt32(ConsoleIW4.animationPointer[(int)tabIndex] + ((uint)index * 0x4), animPtr[i][index]);
            }
        }
    }
}
