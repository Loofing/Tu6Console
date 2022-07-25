using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using loofer.Iw4;
using loofer.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace loofer.Forms
{
    public partial class PresetLoader : Form
    {
        /*not working scrapped this kinda*/
        Iw4Structs.weaponVariantDef currentVariantDef = new Iw4Structs.weaponVariantDef();
        Iw4Structs.weaponDef currentWeaponDef = new Iw4Structs.weaponDef();
        private readonly string presetPath = Directory.GetCurrentDirectory() + @"\presets";
        public static string selectedWeapon = string.Empty;
        public static string currentSelected = "";
        public static string errorMessage = string.Empty;
        public static dynamic parser;
        public PresetLoader()
        {
            InitializeComponent();
            presetBox.ClearSelected();

            if (Directory.Exists(presetPath))
                Directory.CreateDirectory(presetPath);

            refreshList();
        }
        private void refreshList()
        {
            presetBox.ClearSelected();
            presetBox.Items.Clear();

            if (Directory.GetFiles(presetPath, "*.ls").Length < 1)
                return;

            foreach (var file in Directory.GetFiles(presetPath, "*.ls"))
                presetBox.Items.Add(Path.GetFileNameWithoutExtension(file));
        }
        private void displayJsonFile(string jsonParth)
        {
            jsonBox.Clear();
            parser = JObject.Parse(File.ReadAllText(jsonParth));
            Console.Write(File.ReadAllText(jsonParth));
            selectedWeapon = parser.weaponName;
            weaponBox.Enabled = string.IsNullOrEmpty(selectedWeapon);
            weaponBox.Text = selectedWeapon;

            if (parser.Variables != null)
            {
                jsonBox.AppendText("[Variables] : \n");
                foreach(var member in parser.Variables)
                {
                    jsonBox.Text += $"  {member.Name} = {member.Value},\n";
                }
                jsonBox.AppendText("\n");
            }
            if (parser.Animations != null)
            {
                jsonBox.AppendText("[Animations] : \n");
                foreach (var anim in parser.Animations)
                {
                    jsonBox.Text += $"  {anim.Name} = {anim.Value},\n";
                }
                jsonBox.AppendText("\n");
            }
            if (parser.Models != null)
            {
                jsonBox.AppendText("[Models] : \n");
                foreach (var mdl in parser.Models)
                {
                    jsonBox.Text += $"  {mdl.Name} = {mdl.Value},\n";
                }
                jsonBox.AppendText("\n");
            }
        }
        private void setButton_Click(object sender, EventArgs e)
        {
            if (presetBox.SelectedIndex < 0 || !File.Exists(presetBox.SelectedItem.ToString()))
                return;

            if (string.IsNullOrEmpty(selectedWeapon) || !Utility.isValidWeapon(selectedWeapon))
            {
                MessageBox.Show("Please enter a valid weapon.");
                return;
            }

            if(parser.weaponVariables != null)
                setVariables();
        }
        private void setVariables()
        {
            /*foreach(var jsonField in parser.weaponVariables)
            {

            }
            FieldInfo field = currentVariantDef.GetType().GetField(r.Cells[1].Value.ToString());
            */
        }
        private void AppendText(RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }

        private void presetBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentSelected.Equals(presetBox.SelectedItem.ToString()))
                return;

            currentSelected = presetBox.SelectedItem.ToString();
            displayJsonFile(presetPath + @"\" + currentSelected + ".ls");
        }
    }
}
