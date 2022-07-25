using System;
using System.Windows.Forms;
using loofer.Forms;
using loofer.Utils;
using XDRPCLib;

namespace loofer.Iw4
{
    class XModel
    {
        public static string[] modelName = new string[9];
        public static string[] modelNameEx = new string[3];
        public static uint[] modelPtr = new uint[9];
        public static uint[] modelPtrEx = new uint[3];

        public static void displayModels(ref DataGridView modelGrid,uint knifeModel,uint rocketModel,uint projModel)
        {
            modelGrid.Rows.Clear();
            modelPtrEx = new uint[] { knifeModel, rocketModel, projModel };

            byte[] memoryBlock = ConsoleIW4.xbCon.GetMemory(ConsoleIW4.modelPointer, 0x24);

            for (int i = 0; i < 9; i++)
            {
                modelPtr[i] = MemoryManager.toUInt32(memoryBlock, i * 4);

                if (modelPtr[i] == 0x00000000)
                {
                    modelGrid.Rows.Add($"model{i}", modelName[i] = string.Empty);
                    continue;
                }
                modelGrid.Rows.Add($"model{i}", modelName[i] = ConsoleIW4.xbCon.ReadStringPtr(modelPtr[i]));
            }
            for (int i = 0; i < 3; i++)
            {
                if (modelPtrEx[i] == 0x00000000)
                {
                    modelNameEx[i] = string.Empty;
                    continue;
                }
                modelNameEx[i] = ConsoleIW4.xbCon.ReadStringPtr(modelPtrEx[i]);
            }
            modelGrid.Rows.Add("knifeModel", modelNameEx[0]);
            modelGrid.Rows.Add("rocketModel", modelNameEx[1]);
            modelGrid.Rows.Add("projectileModel", modelNameEx[2]);
        }
        public static void saveModels(ref DataGridView modelGrid)
        {
            for (int i = 0; i < 9; i++)
            {
                string cellValue = string.Empty;

                if (modelGrid.Rows[i].Cells[1].Value != null)
                    cellValue = modelGrid.Rows[i].Cells[1].Value.ToString();

                if (cellValue.Equals(modelName[i]))
                    continue;

                modelName[i] = cellValue;

                if (string.IsNullOrEmpty(modelName[i]))
                    modelPtr[i] = 0x00000000;
                else
                    modelPtr[i] = Iw4Functions.GetModel(modelName[i]);

                ConsoleIW4.xbCon.WriteUInt32(ConsoleIW4.modelPointer + ((uint)i * 0x4), modelPtr[i]);
            }
            saveModelEx(ref modelGrid);
        }
        /*need to clean this up*/
        public static void saveModelEx(ref DataGridView modelGrid)
        {
            uint[] offset = new uint[3] { 0x420, 0x1E0, 0x1E4 }; //@weaponDef

            for (int i = 0; i < 3; i++)
            {
                string cellString = Convert.ToString(modelGrid.Rows[modelGrid.Rows.Count - (1 + i)].Cells[1].Value);
                int j = modelNameEx.Length - ( i + 1 );

                if (modelNameEx[j].Equals(cellString))
                    continue;

                modelNameEx[j] = cellString;

                if(string.IsNullOrEmpty(cellString))
                {
                    modelPtrEx[j] = 0x00000000;
                    ConsoleIW4.xbCon.WriteUInt32(ConsoleIW4.weaponDefPointer + offset[i], modelPtrEx[j]);
                    continue;
                }
                modelPtrEx[j] = Iw4Functions.GetModel(modelNameEx[j]);
                ConsoleIW4.xbCon.WriteUInt32(ConsoleIW4.weaponDefPointer + offset[i], modelPtrEx[j]);
            }
        }
    }
}
